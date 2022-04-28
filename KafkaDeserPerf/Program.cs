using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using BenchmarkDotNet.Running;


namespace KafkaDeserPerf
{
    class Program
    {
        static void Main(string[] args)
        {
            //new DeserializerBenchmarks().NonAlloc();
            //new DeserializerBenchmarks().NonAllocSync();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }

        /*private static void Read()
        {
            var payload = new byte[]
            {
                10, 43, 10, 4, 77, 105, 107, 101, 16, 123, 26, 12, 109, 98, 64, 103, 109, 97, 105, 108, 46, 99, 111,
                109, 34, 11, 10, 7, 49, 50, 51, 45, 52, 53, 54, 16, 1, 42, 6, 8, 224, 173, 157, 139, 6
            };
            using var ms = new MemoryStream(payload);
            var ab2 = AddressBook.Parser.ParseFrom(ms);

        }

        private static void Write()
        {
            var p = new Person
            {
                Email = "mb@gmail.com",
                Id = 123,
                LastUpdated = Timestamp.FromDateTime(DateTime.Today.ToUniversalTime()),
                Name = "Mike",
                Phones = { new Person.Types.PhoneNumber { Number = "123-456", Type = Person.Types.PhoneType.Home } }
            };
            var ab = new AddressBook { People = { p } };
            using var ms = new MemoryStream();
            ab.WriteTo(ms);
            ms.Flush();

            var array = "new byte[]{" + string.Join(", ", ms.ToArray()) + "}";
        }

        private static void WriteConfluent()
        {
            var p = new Person
            {
                Email = "mb@gmail.com",
                Id = 123,
                LastUpdated = Timestamp.FromDateTime(DateTime.Today.ToUniversalTime()),
                Name = "Mike",
                Phones = { new Person.Types.PhoneNumber { Number = "123-456", Type = Person.Types.PhoneType.Home } }
            };
            var ab = new AddressBook { People = { p } };

            var ser = new Confluent.SchemaRegistry.Serdes.ProtobufSerializer<AddressBook>(
                new CachedSchemaRegistryClient(new SchemaRegistryConfig
                {
                    Url = "localhost:8081"
                }));

            var serialized = ser.SerializeAsync(ab, SerializationContext.Empty).GetAwaiter().GetResult();


            var array = "new byte[]{" + string.Join(", ", serialized) + "}";
        }*/
    }
}
