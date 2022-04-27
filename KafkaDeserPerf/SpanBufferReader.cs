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
        /// <summary>
        /// Length of underlying buffer
        /// </summary>
        public int Length => _buffer.Length;

        /// <summary>
        /// Initialize <see cref="SpanBufferReader"/>
        /// </summary>
        /// <param name="buffer">Memory buffer to be used</param>
        /// <param name="position">Starting position</param>
        public SpanBufferReader(ReadOnlySpan<byte> buffer, int position = 0)
        {
            _buffer = buffer;
            _position = position;
        }

        /// <summary>
        /// Reset current position to default (0)
        /// </summary>
        public void Reset() => _position = 0;
        /// <summary>
        /// Advance current position by given amount 
        /// </summary>
        /// <param name="offset">Offset to advance position by</param>
        public void AdvanceBy(int offset) => _position += offset;

        /// <summary>
        /// Determines if end of buffer was reached 
        /// </summary>
        public bool IsEnd => _position >= _buffer.Length;

        /// <summary>
        /// Reads 1 byte from underlying stream
        /// </summary>
        /// <returns>Byte read ot -1 if EOB is reached</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadByte() => _position >= _buffer.Length ? -1 : _buffer[_position++];

        /// <summary>
        /// Reads one little endian 32 bits integer from underlying stream
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32() => BinaryPrimitives.ReadInt32LittleEndian(BufferRead(4));

        /// <summary>
        /// Reads one little endian 64 bits integer from underlying stream
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64() => BinaryPrimitives.ReadInt64LittleEndian(BufferRead(8));
          
        /// <summary>
        /// Reads buffer of given size 
        /// </summary>
        /// <param name="numBytes">Number of bytes to be read</param>
        /// <returns>Buffer read from underlying buffer</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when there is not enough data to read</exception>
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
        /// <summary>
        /// Read an integer stored in variable-length format using signed decoding from <a href="http://code.google.com/apis/protocolbuffers/docs/encoding.html"> Google Protocol Buffers</a>
        /// </summary>
        /// <returns>The integer read</returns>
        /// <exception cref="OverflowException">Thrown if variable-length value does not terminate after 5 bytes have been read</exception>
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
