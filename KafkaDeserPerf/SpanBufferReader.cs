using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KafkaDeserPerf
{
    /// <summary>Lightweight reader wrapper for <see cref="ReadOnlySpan{T}"/> that follows <see cref="System.IO.StreamReader"/> logic</summary>
    public ref struct SpanBufferReader
    {
        private readonly ReadOnlySpan<byte> _buffer;
        private int _position;
        public int Length => _buffer.Length;

        public SpanBufferReader(ReadOnlySpan<byte> buffer, int position = 0)
        {
            _buffer = buffer;
            _position = position;
        }

        public void Reset() => _position = 0;
        public void AdvanceBy(int offset) => _position += offset;
        public bool IsEnd => _position >= _buffer.Length;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadByte() => _position >= _buffer.Length ? -1 : _buffer[_position++];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32() => BinaryPrimitives.ReadInt32LittleEndian(BufferRead(4));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64() => BinaryPrimitives.ReadInt64LittleEndian(BufferRead(8));
          

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> BufferRead(int numBytes)
        {
            Debug.Assert(numBytes is >= 2 and <= 16, "value of 1 should use ReadByte. For value > 16 implement more efficient read method");
            int origPos = _position;
            int newPos = origPos + numBytes;

            if ((uint)newPos > (uint)_buffer.Length)
            {
                _position = _buffer.Length;
                throw new ArgumentOutOfRangeException(nameof(numBytes), $"Not enough data to read {numBytes} bytes from underlying buffer");
            }

            var span = _buffer.Slice(origPos, numBytes);
            _position = newPos;
            return span;
        }

        public ReadOnlySpan<byte> Tail() => _buffer.Slice(_position, _buffer.Length - _position);
    }

    public static class SpanBufferReaderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadVarint(this ref SpanBufferReader reader)
        {
            var value = ReadUnsignedVarint(ref reader);
            return (int)((value >> 1) ^ -(value & 1));
        }

        /// <summary>
        /// Read an integer stored in variable-length format using unsigned decoding from <a href="http://code.google.com/apis/protocolbuffers/docs/encoding.html"> Google Protocol Buffers</a>
        /// </summary>
        /// /// <returns>The integer read</returns>
        /// <remarks>
        ///     Inspired by: https://github.com/apache/kafka/blob/2.5/clients/src/main/java/org/apache/kafka/common/utils/ByteUtils.java#L142
        /// </remarks>
        /// <exception cref="OverflowException">Thrown if variable-length value does not terminate after 5 bytes have been read</exception>
        public static uint ReadUnsignedVarint(this ref SpanBufferReader reader)
        {
            int value = 0;
            int i = 0;
            int b;
            while (true)
            {
                b = reader.ReadByte();
                if (b == -1) throw new InvalidOperationException("Unexpected end of stream reading varint.");
                if ((b & 0x80) == 0) { break; }
                value |= (b & 0x7f) << i;
                i += 7;
                if (i > 28)
                    throw new OverflowException("Encoded varint is larger than uint.MaxValue");
            }
            value |= b << i;
            return (uint)value;
        }
    }
}
