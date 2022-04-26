// ConstructValueSubjectName is still used a an internal implementation detail.
#pragma warning disable CS0618

using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using KafkaDeserPerf.Deserializers;
using NUnit.Framework;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;


namespace Tests
{
    public class ProtobufSerdesTests
    {
        private ISchemaRegistryClient? _schemaRegistryClient;
        private SerializationContext _context;
        private const string TestTopic = "topic";
        private readonly Dictionary<string, int> _store = new();

        [SetUp]
        public void BeforeEachTest()
        {
            var subject = $"{TestTopic}-value";

            _context = new SerializationContext(MessageComponentType.Value, TestTopic);

            var schemaRegistryMock = new Mock<ISchemaRegistryClient>();
            schemaRegistryMock.Setup(x => x.ConstructValueSubjectName(TestTopic, It.IsAny<string>())).Returns(subject);
            schemaRegistryMock.Setup(x => x.RegisterSchemaAsync(subject, It.IsAny<string>())).ReturnsAsync(
                (string _, string schema) => _store.TryGetValue(schema, out int id) ? id : _store[schema] = _store.Count + 1
            );
            schemaRegistryMock.Setup(x => x.GetSchemaAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(
                (int id, string _) => new Schema(_store.First(x => x.Value == id).Key, null, SchemaType.Protobuf)
            );
            _schemaRegistryClient = schemaRegistryMock.Object;
        }

        private (ProtobufSerializer<T> Ser, ProtobufDeserializer<T> Des, ProtobufDeserializer2<T> Des2) GetSerdes<T>() where T : class, IMessage<T>, new() => (
                new ProtobufSerializer<T>(_schemaRegistryClient, new ProtobufSerializerConfig() { SkipKnownTypes = true }),
                new ProtobufDeserializer<T>(),
                new ProtobufDeserializer2<T>()
            );

        [Test]
        public async Task Null_ShouldBeProperlySerialized()
        {
            var (ser, des, des2) = GetSerdes<UInt32Value>();

            var serialized = ser.SerializeAsync(null!, _context).Result;
            Assert.That(serialized, Is.Null);

            var deserialized1 = await des.DeserializeAsync(serialized, true, _context);
            var deserialized2 = await des2.DeserializeAsync(serialized, true, _context);

            Assert.That(deserialized1, Is.Null);
            Assert.That(deserialized2, Is.Null);
        }

        [Test]
        public async Task Simple_UInt32SerDe()
        {
            var (ser, des, des2) = GetSerdes<UInt32Value>();

            var v = new UInt32Value { Value = 1234 };
            var serialized = ser.SerializeAsync(v, _context).Result;
            var deserialized1 = await des.DeserializeAsync(serialized, false, _context);
            var deserialized2 = await des2.DeserializeAsync(serialized, false, _context);


            Assert.That(deserialized1.Value, Is.EqualTo(v.Value));
            Assert.That(deserialized2.Value, Is.EqualTo(v.Value));
        }

        private static readonly IEnumerable<TestCaseData> ChangeNotificationSources = new TestCaseData[]
        {
            new(new ChangeNotification
            {
                Id = 123456789,
                Status = Status.Active,
                Time = Timestamp.FromDateTime(new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
                Currency = new Currency { Amount = 15.6f },
            }),

            new(new ChangeNotification
            {
                Id = 666,
                Status = Status.Suspended,
                Time = Timestamp.FromDateTime(new DateTime(2002, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
                Stock = new Stock {Aliases = {"A", "B", "C"}}
            }),
        };

        [TestCaseSource(nameof(ChangeNotificationSources))]
        public async Task Complex_ChangeNotification(ChangeNotification data)
        {
            var (ser, desConfluent, des2) = GetSerdes<ChangeNotification>();

            var serialized = await ser.SerializeAsync(data, _context);
            var deserializedConfluent = await desConfluent.DeserializeAsync(serialized, false, _context);

            var deserializedAsync = await des2.DeserializeAsync(serialized, false, _context);
            var deserializedSync = des2.Deserialize(serialized, false, _context);

            CheckEquality(deserializedConfluent, data);
            CheckEquality(deserializedAsync, data);
            CheckEquality(deserializedSync, data);

            static void CheckEquality(ChangeNotification actual, ChangeNotification expected)
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Status, Is.EqualTo(expected.Status));
                Assert.That(actual.Time, Is.EqualTo(expected.Time));
                Assert.That(actual.Stock, Is.EqualTo(expected.Stock));
                Assert.That(actual.Currency, Is.EqualTo(expected.Currency));
                Assert.That(actual.InstrumentCase, Is.EqualTo(expected.InstrumentCase));

                Assert.That(actual.Stock?.Aliases, Is.EqualTo(expected.Stock?.Aliases));
                Assert.That(actual.Currency?.Amount, Is.EqualTo(expected.Currency?.Amount));

                Assert.That(actual, Is.Not.SameAs(expected));
                Assert.That(actual, Is.EqualTo(expected));
            }
        }
    }
}
