using System;

using BenchmarkDotNet.Attributes;

using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

using KafkaDeserPerf.Sync;

using Tutorial;

namespace KafkaDeserPerf
{
    [MemoryDiagnoser]
    public class DeserializerBenchmarks
    {
        private static readonly byte[] _payload = {
            0, 0, 0, 0, 2, 2, 2, 10, 43, 10, 4, 77, 105, 107, 101, 16, 123, 26, 12, 109, 98, 64, 103, 109, 97, 105,
            108, 46, 99, 111, 109, 34, 11, 10, 7, 49, 50, 51, 45, 52, 53, 54, 16, 1, 42, 6, 8, 224, 173, 157, 139, 6
        };

        private static readonly ProtobufDeserializer<AddressBook> _confluent = new ProtobufDeserializer<AddressBook>();
        private static readonly SyncProtobufDeserializer<AddressBook> _sync = new SyncProtobufDeserializer<AddressBook>();

        [Benchmark(Baseline = true)]
        public int Confluent()
        {
            AddressBook ab = _confluent.DeserializeAsync(_payload.AsMemory(), false, SerializationContext.Empty)
                .ConfigureAwait(continueOnCapturedContext: false)
                .GetAwaiter()
                .GetResult();
            return ab.People[0].Id;
        }

        [Benchmark]
        public int Sync()
        {
            AddressBook ab = _sync.Deserialize(_payload.AsSpan(), false, SerializationContext.Empty);
            return ab.People[0].Id;
        }
    }
}
