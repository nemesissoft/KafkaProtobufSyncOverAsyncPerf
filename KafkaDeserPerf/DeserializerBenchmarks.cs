using System;

using BenchmarkDotNet.Attributes;

using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

using KafkaDeserPerf.Deserializers;


using Tutorial;

namespace KafkaDeserPerf
{
    /*
|         Method | Iterations |         Mean |       Error |      StdDev | Ratio |   Gen 0 |  Gen 1 | Allocated |
|--------------- |----------- |-------------:|------------:|------------:|------:|--------:|-------:|----------:|
|      Confluent |          1 |   1,228.1 ns |    19.94 ns |    16.65 ns |  1.00 |  0.8278 | 0.0153 |   5,200 B |
|       NonAlloc |          1 |     512.7 ns |     9.49 ns |     8.88 ns |  0.42 |  0.0963 | 0.0010 |     608 B |
|   NonAllocSync |          1 |     482.6 ns |     3.32 ns |     2.94 ns |  0.39 |  0.0854 |      - |     536 B |
| EfficientAsync |          1 |     506.1 ns |     5.49 ns |     5.14 ns |  0.41 |  0.0968 | 0.0014 |     608 B |
|  EfficientSync |          1 |     487.4 ns |     3.96 ns |     3.70 ns |  0.40 |  0.0854 |      - |     536 B |
|                |            |              |             |             |       |         |        |           |
|      Confluent |         10 |  12,087.8 ns |   117.93 ns |   110.31 ns |  1.00 |  8.2779 | 0.1678 |  52,003 B |
|       NonAlloc |         10 |   5,378.6 ns |    94.74 ns |    88.62 ns |  0.45 |  0.9689 |      - |   6,080 B |
|   NonAllocSync |         10 |   4,738.9 ns |    36.41 ns |    34.06 ns |  0.39 |  0.8545 |      - |   5,360 B |
| EfficientAsync |         10 |   4,995.7 ns |    52.32 ns |    48.94 ns |  0.41 |  0.9689 |      - |   6,080 B |
|  EfficientSync |         10 |   4,838.4 ns |    80.18 ns |    75.00 ns |  0.40 |  0.8545 |      - |   5,360 B |
|                |            |              |             |             |       |         |        |           |
|      Confluent |        100 | 122,244.2 ns | 1,952.18 ns | 1,730.56 ns |  1.00 | 82.7637 | 1.7090 | 520,030 B |
|       NonAlloc |        100 |  49,873.6 ns |   535.68 ns |   501.08 ns |  0.41 |  9.6436 |      - |  60,804 B |
|   NonAllocSync |        100 |  47,310.8 ns |   442.90 ns |   414.29 ns |  0.39 |  8.5449 |      - |  53,603 B |
| EfficientAsync |        100 |  49,830.6 ns |   443.76 ns |   415.10 ns |  0.41 |  9.6436 |      - |  60,803 B |
|  EfficientSync |        100 |  47,873.2 ns |   571.38 ns |   534.47 ns |  0.39 |  8.5449 |      - |  53,603 B |*/
    [MemoryDiagnoser]
    public class DeserializerBenchmarks
    {
        [Params(1, 10, 100)] public int Iterations { get; set; } = 1;

        private static readonly byte[] _payload = {
            0, 0, 0, 0, 2, 2, 2, 10, 43, 10, 4, 77, 105, 107, 101, 16, 123, 26, 12, 109, 98, 64, 103, 109, 97, 105,
            108, 46, 99, 111, 109, 34, 11, 10, 7, 49, 50, 51, 45, 52, 53, 54, 16, 1, 42, 6, 8, 224, 173, 157, 139, 6
        };

        private static readonly ProtobufDeserializer<AddressBook> _confluent = new();
        private static readonly NonAllocProtobufDeserializer<AddressBook> _nonAlloc = new();
        private static readonly EfficientProtobufDeserializer<AddressBook> _efficient = new();

        [Benchmark(Baseline = true)]
        public int Confluent()
        {
            int id = 0;
            for (int i = 0; i < Iterations; i++)
            {
                AddressBook ab = _confluent.DeserializeAsync(_payload.AsMemory(), false, SerializationContext.Empty)
                .ConfigureAwait(continueOnCapturedContext: false)
                .GetAwaiter()
                .GetResult();
                id = ab.People[0].Id;
            }
            return id;
        }

        [Benchmark]
        public int NonAlloc()
        {
            int id = 0;
            for (int i = 0; i < Iterations; i++)
            {
                AddressBook ab = _nonAlloc.DeserializeAsync(_payload.AsMemory(), false, SerializationContext.Empty)
                .ConfigureAwait(continueOnCapturedContext: false)
                .GetAwaiter()
                .GetResult();
                id = ab.People[0].Id;
            }
            return id;
        }

        [Benchmark]
        public int NonAllocSync()
        {
            int id = 0;
            for (int i = 0; i < Iterations; i++)
            {
                AddressBook ab = _nonAlloc.Deserialize(_payload.AsSpan(), false, SerializationContext.Empty);
                id = ab.People[0].Id;
            }
            return id;
        }

        [Benchmark]
        public int EfficientAsync()
        {
            int id = 0;
            for (int i = 0; i < Iterations; i++)
            {
                var ab = _efficient.DeserializeAsync(_payload.AsMemory(), false, SerializationContext.Empty)
                    .ConfigureAwait(continueOnCapturedContext: false)
                    .GetAwaiter()
                    .GetResult();
                id = ab.People[0].Id;
            }
            return id;
        }

        [Benchmark]
        public int EfficientSync()
        {
            int id = 0;
            for (int i = 0; i < Iterations; i++)
            {
                var ab = _efficient.Deserialize(_payload.AsSpan(), false, SerializationContext.Empty);
                id = ab.People[0].Id;
            }
            return id;
        }
    }
}
