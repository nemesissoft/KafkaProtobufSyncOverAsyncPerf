using System.Linq;

using BenchmarkDotNet.Attributes;

using DotNext.Buffers;

using Memory;

namespace KafkaDeserPerf
{
    /*
|               Method |     Mean |   Error |  StdDev | Ratio | Allocated |
|--------------------- |---------:|--------:|--------:|------:|----------:|
| SpanBinaryReaderTest | 191.7 ns | 1.54 ns | 1.37 ns |  1.00 |         - |
|       SpanReaderTest | 189.9 ns | 1.60 ns | 1.42 ns |  0.99 |         - |
     */
    [MemoryDiagnoser]
    public class SpanReaderBenchmarks
    {
        private const int Iterations = 150;

        private static readonly byte[] _payload = Enumerable.Range(1, 4 * Iterations).Select(i => (byte)i).ToArray();

        [Benchmark(Baseline = true)]
        public int SpanBinaryReaderTest()
        {
            int result = 0;
            var reader = new SpanBinaryReader(_payload);
            for (int i = 0; i < Iterations; i++)
                result = reader.ReadInt32();

            return result;
        }

        [Benchmark]
        public int SpanReaderTest()
        {
            int result = 0;
            var reader = new SpanReader<byte>(_payload);
            for (int i = 0; i < Iterations; i++)
                result = reader.ReadInt32(true);

            return result;
        }
    }
}
