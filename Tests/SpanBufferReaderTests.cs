using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TCD = NUnit.Framework.TestCaseData;
using System;
using KafkaDeserPerf;

namespace Tests
{
    [TestFixture]
    public class SpanBufferReaderTests
    {
        [Test]
        public void IsEnd_ReturnsTrueWhenEndIsReached()
        {
            var sut = new SpanBufferReader(new byte[] { 1, 2, 3, 4, 5 });
            Assert.That(sut.Length, Is.EqualTo(5));
            for (int i = 0; i < sut.Length; i++)
            {
                Assert.That(sut.IsEnd, Is.False);
                sut.AdvanceBy(1);
            }
            Assert.That(sut.IsEnd, Is.True);
        }

        [Test]
        public void ReadByte_ReturnsAppropriateValues()
        {
            var source = Enumerable.Range(1, 5).Select(i => (byte)(i * 11)).ToArray();
            var sut = new SpanBufferReader(source);
            Assert.That(sut.Length, Is.EqualTo(5));

            for (int i = 0; i < sut.Length; i++)
                Assert.That(sut.ReadByte(), Is.EqualTo(source[i]), "Error at " + i);

            Assert.That(sut.IsEnd, Is.True);
        }

        [Test]
        public void ReadInt32_ReturnsAppropriateValues()
        {
            var sourceInt = new List<int>();
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            foreach (var @int in Enumerable.Range(1, 5).Select(i => i * 11111))
            {
                sourceInt.Add(@int);
                bw.Write(@int);
            }
            bw.Flush();


            var sut = new SpanBufferReader(ms.ToArray());
            Assert.That(sut.Length, Is.EqualTo(5 * 4));
            Assert.That(sourceInt.Count, Is.EqualTo(5));

            for (int i = 0; i < sourceInt.Count; i++)
                Assert.That(sut.ReadInt32(), Is.EqualTo(sourceInt[i]), "Error at " + i);

            Assert.That(sut.IsEnd, Is.True);
        }

        [Test]
        public void ReadInt64_ReturnsAppropriateValues()
        {
            var sourceLong = new List<long>();
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            foreach (var @long in Enumerable.Range(1, 5).Select(i => i * 1111111111111))
            {
                sourceLong.Add(@long);
                bw.Write(@long);
            }
            bw.Flush();


            var sut = new SpanBufferReader(ms.ToArray());
            Assert.That(sut.Length, Is.EqualTo(5 * 8));
            Assert.That(sourceLong.Count, Is.EqualTo(5));

            for (int i = 0; i < sourceLong.Count; i++)
                Assert.That(sut.ReadInt64(), Is.EqualTo(sourceLong[i]), "Error at " + i);

            Assert.That(sut.IsEnd, Is.True);
        }

        private static IEnumerable<TCD> UnsignedVarintExamples = new (uint Number, byte[] Bytes)[]
        {
            (        0, new[]{ (byte)0b_00000000} ),
            (      127, new[]{ (byte)0b_01111111} ),
            (      128, new[]{ (byte)0b_10000000, (byte)0b_00000001} ),
            (      300, new[]{ (byte)0b_10101100, (byte)0b_00000010} ),
            (     8192, new[]{ (byte)0b_10000000, (byte)0b_01000000} ),
            (    16383, new[]{ (byte)0b_11111111, (byte)0b_01111111} ),
            (    16384, new[]{ (byte)0b_10000000, (byte)0b_10000000, (byte)0b_00000001} ),
            (  2097151, new[]{ (byte)0b_11111111, (byte)0b_11111111, (byte)0b_01111111} ),
            (  2097152, new[]{ (byte)0b_10000000, (byte)0b_10000000, (byte)0b_10000000, (byte)0b_00000001 }),
            (134217728, new[]{ (byte)0b_10000000, (byte)0b_10000000, (byte)0b_10000000, (byte)0b_01000000 }),
            (268435455, new[]{ (byte)0b_11111111, (byte)0b_11111111, (byte)0b_11111111, (byte)0b_01111111 }),
        }.Select(p => new TCD(p.Bytes, p.Number).SetName($"UnsignedVarint_{p.Number:000000000}"));

        [TestCaseSource(nameof(UnsignedVarintExamples))]
        public void ReadUnsignedVarint_ReturnsAppropriateValues(byte[] bytes, uint expectedNumber)
        {
            /*static void WriteUnsignedVarint(uint value, Stream stream)
            {
                while ((value & 0xffffff80) != 0L)
                {
                    byte b = (byte)((value & 0x7f) | 0x80);
                    stream.WriteByte(b);
                    value >>= 7;
                }
                stream.WriteByte((byte)value);
            }
            using var ms2 = new MemoryStream();
            WriteUnsignedVarint(expectedNumber, ms2);
            foreach (var b in ms2.ToArray())
                Console.Write($"(byte)0b_{int.Parse(Convert.ToString(b, 2)):00000000}, ");*/


            using var ms = new MemoryStream();
            foreach (var b in bytes)
                ms.WriteByte(b);
            var buffer = ms.ToArray();
            var sut = new SpanBufferReader(buffer);

            Assert.That(sut.ReadUnsignedVarint(), Is.EqualTo(expectedNumber));

            static string SpanToString(ReadOnlySpan<byte> span) =>
                string.Join(", ", span.ToArray().Select(b => b.ToString("X2")));


            Assert.That(sut.IsEnd, Is.True, $"End not reached. Remaining [{SpanToString(sut.Tail())}] out of [{SpanToString(buffer)}]");
        }
    }
}