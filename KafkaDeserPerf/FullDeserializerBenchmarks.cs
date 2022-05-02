using System;

using BenchmarkDotNet.Attributes;

using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

using Deser = Confluent.Kafka.Deserializers;


using Tutorial;
using KafkaDeserPerf.Deserializers;
using System.Collections.Generic;

namespace KafkaDeserPerf
{
    /*
    |         Method | Iterations |        Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Allocated |
    |--------------- |----------- |------------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|
    |         Create |          1 |    105.3 ns |   0.69 ns |   0.61 ns |  1.00 |    0.00 |  0.0994 | 0.0001 |     624 B |
    |      Confluent |          1 |    825.7 ns |   4.99 ns |   4.42 ns |  7.84 |    0.06 |  0.8669 | 0.0105 |   5,440 B |
    | EfficientAsync |          1 |    527.1 ns |   9.16 ns |   8.12 ns |  5.01 |    0.09 |  0.1345 |      - |     848 B |
    |  EfficientSync |          1 |    513.9 ns |   9.55 ns |   9.81 ns |  4.88 |    0.11 |  0.1230 |      - |     776 B |
    |                |            |             |           |           |       |         |         |        |           |
    |         Create |         10 |  1,049.5 ns |  18.40 ns |  26.97 ns |  1.00 |    0.00 |  0.9937 | 0.0019 |   6,240 B |
    |      Confluent |         10 |  8,116.3 ns |  92.92 ns |  82.37 ns |  7.65 |    0.26 |  8.6670 | 0.1221 |  54,403 B |
    | EfficientAsync |         10 |  5,137.8 ns |  96.98 ns |  99.59 ns |  4.85 |    0.20 |  1.3504 |      - |   8,480 B |
    |  EfficientSync |         10 |  4,993.0 ns |  34.52 ns |  32.29 ns |  4.71 |    0.15 |  1.2360 |      - |   7,760 B |
    |                |            |             |           |           |       |         |         |        |           |
    |         Create |        100 | 10,128.9 ns |  74.44 ns |  69.63 ns |  1.00 |    0.00 |  9.9335 | 0.0153 |  62,400 B |
    |      Confluent |        100 | 80,308.4 ns | 553.22 ns | 517.48 ns |  7.93 |    0.05 | 86.6699 | 1.2207 | 544,031 B |
    | EfficientAsync |        100 | 50,320.2 ns | 442.89 ns | 414.28 ns |  4.97 |    0.05 | 13.4888 |      - |  84,805 B |
    |  EfficientSync |        100 | 49,219.6 ns | 302.91 ns | 283.35 ns |  4.86 |    0.04 | 12.3291 |      - |  77,604 B |*/
    [MemoryDiagnoser]
    public class FullDeserializerBenchmarks
    {
        [Params(1, 10, 100)] public int Iterations { get; set; } = 1;

        private static readonly byte[] _payload = {
            0, 0, 0, 0, 2, 2, 2, 10, 43, 10, 4, 77, 105, 107, 101, 16, 123, 26, 12, 109, 98, 64, 103, 109, 97, 105,
            108, 46, 99, 111, 109, 34, 11, 10, 7, 49, 50, 51, 45, 52, 53, 54, 16, 1, 42, 6, 8, 224, 173, 157, 139, 6
        };

        private static readonly ProtobufDeserializer<AddressBook> _confluent = new();
        private static readonly EfficientProtobufDeserializer<AddressBook> _efficient = new();
        private static readonly List<byte[]> _keys = new()
        {
            new byte[] { 75, 101, 121, 48 },
            new byte[] { 75, 101, 121, 49 },
            new byte[] { 75, 101, 121, 50 }
        };

        private static readonly List<string> _keysString = new() { "Key0", "Key1", "Key2", };

        [Benchmark(Baseline = true)]
        public int Create()
        {
            int id = 0;

            var keys = _keysString;
            var keysCount = keys.Count;
            for (int i = 0; i < Iterations; i++)
            {
                var p = new Person
                {
                    Email = "mb@gmail.com",
                    Id = 123,
                    LastUpdated = new Google.Protobuf.WellKnownTypes.Timestamp() { Seconds = 1634162400, Nanos = 0 },
                    Name = "Mike",
                    Phones = { new Person.Types.PhoneNumber { Number = "123-456", Type = Person.Types.PhoneType.Home } }
                };
                var val = new AddressBook { People = { p } };

                var key = keys[i % keysCount];

                var cr = new ConsumeResult<string, AddressBook>()
                {
                    TopicPartitionOffset = new TopicPartitionOffset("Topic", new Partition(i % 3), new Offset(i)),
                    Message = new Message<string, AddressBook>
                    {
                        Timestamp = new Timestamp(10000 * i, TimestampType.CreateTime),
                        Headers = new Headers(),
                        Key = key,
                        Value = val
                    },
                    IsPartitionEOF = false
                };


                id = cr.Message.Value.People[0].Id;
            }
            return id;
        }

        [Benchmark]
        public int Confluent()
        {
            int id = 0;
            var keys = _keys;
            var keysCount = keys.Count;
            for (int i = 0; i < Iterations; i++)
            {
                var val = _confluent.DeserializeAsync(_payload.AsMemory(), false, SerializationContext.Empty)
                    .ConfigureAwait(continueOnCapturedContext: false)
                    .GetAwaiter()
                    .GetResult();
                var key = Deser.Utf8.Deserialize(keys[i % keysCount], false, SerializationContext.Empty);


                var cr = new ConsumeResult<string, AddressBook>()
                {
                    TopicPartitionOffset = new TopicPartitionOffset("Topic", new Partition(i % 3), new Offset(i)),
                    Message = new Message<string, AddressBook>
                    {
                        Timestamp = new Timestamp(10000 * i, TimestampType.CreateTime),
                        Headers = new Headers(),
                        Key = key,
                        Value = val
                    },
                    IsPartitionEOF = false
                };

                id = cr.Message.Value.People[0].Id;
            }
            return id;
        }


        [Benchmark]
        public int EfficientAsync()
        {
            int id = 0;
            var keys = _keys;
            var keysCount = keys.Count;
            for (int i = 0; i < Iterations; i++)
            {
                var val = _efficient.DeserializeAsync(_payload.AsMemory(), false, SerializationContext.Empty)
                    .ConfigureAwait(continueOnCapturedContext: false)
                    .GetAwaiter()
                    .GetResult();
                var key = Deser.Utf8.Deserialize(keys[i % keysCount], false, SerializationContext.Empty);


                var cr = new ConsumeResult<string, AddressBook>()
                {
                    TopicPartitionOffset = new TopicPartitionOffset("Topic", new Partition(i % 3), new Offset(i)),
                    Message = new Message<string, AddressBook>
                    {
                        Timestamp = new Timestamp(10000 * i, TimestampType.CreateTime),
                        Headers = new Headers(),
                        Key = key,
                        Value = val
                    },
                    IsPartitionEOF = false
                };

                id = cr.Message.Value?.People[0]?.Id ?? 0;
            }
            return id;
        }

        [Benchmark]
        public int EfficientSync()
        {
            int id = 0;
            var keys = _keys;
            var keysCount = keys.Count;
            for (int i = 0; i < Iterations; i++)
            {
                var val = _efficient.Deserialize(_payload.AsSpan(), false, SerializationContext.Empty);
                var key = Deser.Utf8.Deserialize(keys[i % keysCount], false, SerializationContext.Empty);


                var cr = new ConsumeResult<string, AddressBook>()
                {
                    TopicPartitionOffset = new TopicPartitionOffset("Topic", new Partition(i % 3), new Offset(i)),
                    Message = new Message<string, AddressBook>
                    {
                        Timestamp = new Timestamp(10000 * i, TimestampType.CreateTime),
                        Headers = new Headers(),
                        Key = key,
                        Value = val
                    },
                    IsPartitionEOF = false
                };

                id = cr.Message.Value?.People[0]?.Id ?? 0;
            }
            return id;
        }
    }
}
