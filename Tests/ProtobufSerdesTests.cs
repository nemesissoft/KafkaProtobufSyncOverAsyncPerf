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
using NUnit.Framework;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;


namespace Tests
{
    public class ProtobufSerdesTests
    {
        private ISchemaRegistryClient? _schemaRegistryClient;
        private const string TestTopic = "topic";
        private readonly Dictionary<string, int> _store = new();

        [SetUp]
        public void BeforeEachTest()
        {
            var subject = $"{TestTopic}-value";

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

        private (ProtobufSerializer<T> Ser, ProtobufDeserializer<T> Des) GetSerdes<T>() where T : class, IMessage<T>, new() => (
                new ProtobufSerializer<T>(_schemaRegistryClient, new ProtobufSerializerConfig() { SkipKnownTypes = true }),
                new ProtobufDeserializer<T>()
            );

        [Test]
        public void Null_ShouldBeProperlySerialized()
        {
            var (ser, des) = GetSerdes<UInt32Value>();

            var serialized = ser.SerializeAsync(null, new SerializationContext(MessageComponentType.Value, TestTopic)).Result;
            Assert.That(serialized, Is.Null);

            var deserialized = des.DeserializeAsync(serialized, true, new SerializationContext(MessageComponentType.Value, TestTopic)).Result;
            Assert.That(deserialized, Is.Null);
        }

        [Test]
        public void Simple_UInt32SerDe()
        {
            var (ser, des) = GetSerdes<UInt32Value>();

            var v = new UInt32Value { Value = 1234 };
            var serialized = ser.SerializeAsync(v, new SerializationContext(MessageComponentType.Value, TestTopic)).Result;
            var deserialized = des.DeserializeAsync(serialized, false, new SerializationContext(MessageComponentType.Value, TestTopic)).Result;


            Assert.That(deserialized.Value, Is.EqualTo(v.Value));
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
            var (ser, des) = GetSerdes<ChangeNotification>();

            var context = new SerializationContext(MessageComponentType.Value, TestTopic);

            var serialized = await ser.SerializeAsync(data, context);
            var deserialized = await des.DeserializeAsync(serialized, false, context);

            CheckEquality(deserialized, data);

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
