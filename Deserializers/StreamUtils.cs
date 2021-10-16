// Copyright 2020 Confluent Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Refer to LICENSE for more information.

using System;


namespace KafkaDeserPerf.Deserializers
{
    internal static class StreamUtils
    {
        public static int ReadVarint(this ReadOnlySpan<byte> buffer, ref int position)
        {
            var value = ReadUnsignedVarint(buffer, ref position);
            return (int)((value >> 1) ^ -(value & 1));
        }

        public static uint ReadUnsignedVarint(this ReadOnlySpan<byte> buffer, ref int position)
        {
            int value = 0;
            int i = 0;
            int b;
            while (true)
            {
                b = buffer[position++];
                if (b == -1) throw new InvalidOperationException("Unexpected end of stream reading varint.");
                if ((b & 0x80) == 0) { break; }
                value |= (b & 0x7f) << i;
                i += 7;
                if (i > 28)
                {
                    throw new OverflowException($"Encoded varint is larger than uint.MaxValue");
                }
            }
            value |= b << i;
            return (uint)value;
        }
    }
}
