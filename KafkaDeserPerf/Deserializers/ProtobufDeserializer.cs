using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

using Google.Protobuf;


namespace KafkaDeserPerf.Deserializers
{
    /// <summary>
    ///     (async) Protobuf deserializer.
    /// </summary>
    /// <remarks>
    ///     Serialization format:
    ///       byte 0:           A magic byte that identifies this as a message with
    ///                         Confluent Platform framing.
    ///       bytes 1-4:        Unique global id of the Protobuf schema that was used
    ///                         for encoding (as registered in Confluent Schema Registry),
    ///                         big endian.
    ///       following bytes:  1. A size-prefixed array of indices that identify the
    ///                            specific message type in the schema (a given schema
    ///                            can contain many message types and they can be nested).
    ///                            Size and indices are unsigned varints. The common case
    ///                            where the message type is the first message in the
    ///                            schema (i.e. index data would be [1,0]) is encoded as
    ///                            a single 0 byte as an optimization.
    ///                         2. The protobuf serialized data.
    /// </remarks>
    public class ProtobufDeserializer2<T> : IAsyncDeserializer<T> where T : class, IMessage<T>, new()
    {
        /// <summary>
        ///     Magic byte that identifies a message with Confluent Platform framing.
        /// </summary>
        public const byte MagicByte = 0;

        private readonly bool _useDeprecatedFormat;

        private readonly MessageParser<T> _parser;

        /// <summary>Initialize a new ProtobufDeserializer instance.</summary>
        /// <param name="config">Deserializer configuration properties (refer to <see cref="ProtobufDeserializerConfig" />).</param>
        public ProtobufDeserializer2(IReadOnlyCollection<KeyValuePair<string, string>>? config = null)
        {
            _parser = new MessageParser<T>(() => new T());

            if (config == null) { return; }

            var nonProtobufConfig = config.Where(item => !item.Key.StartsWith("protobuf.")).ToList();
            if (nonProtobufConfig.Any())
                throw new ArgumentException($"ProtobufDeserializer: unknown configuration parameters: {string.Join(", ", nonProtobufConfig.Select(c => c.Key))}");

            var protobufConfig = new ProtobufDeserializerConfig(config);
            if (protobufConfig.UseDeprecatedFormat != null)
                _useDeprecatedFormat = protobufConfig.UseDeprecatedFormat.Value;
        }

        /// <summary>Deserialize an object of type <typeparamref name="T"/> from a byte array.</summary>
        /// <param name="data">The raw byte data to deserialize.</param>
        /// <param name="isNull">True if this is a null value.</param>
        /// <param name="context">Context relevant to the deserialize operation.</param>
        /// <returns>A <see cref="Task" /> that completes with the deserialized value.</returns>
        public Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) { return Task.FromResult<T>(null); }

            var array = data.ToArray();
            if (array.Length < 6)
                throw new InvalidDataException($"Expecting data framing of length 6 bytes or more but total data size is {array.Length} bytes");

            try
            {
                using (var stream = new MemoryStream(array))
                using (var reader = new BinaryReader(stream))
                {
                    var magicByte = reader.ReadByte();
                    if (magicByte != MagicByte)
                    {
                        throw new InvalidDataException($"Expecting message {context.Component.ToString()} with Confluent Schema Registry framing. Magic byte was {array[0]}, expecting {MagicByte}");
                    }

                    // A schema is not required to deserialize protobuf messages since the serialized data includes tag and type information, which is enough for
                    // the IMessage<T> implementation to deserialize the data (even if the schema has evolved). Schema Id is thus unused
                    IPAddress.NetworkToHostOrder(reader.ReadInt32());

                    // Read the index array length, then all of the indices. These are not needed, but parsing them is the easiest way to seek to the start of the serialized data because they are varints.
                    var indicesLength = _useDeprecatedFormat ? (int)stream.ReadUnsignedVarint() : stream.ReadVarint();
                    for (int i = 0; i < indicesLength; ++i)
                    {
                        if (_useDeprecatedFormat)
                            stream.ReadUnsignedVarint();
                        else
                            stream.ReadVarint();
                    }
                    return Task.FromResult(_parser.ParseFrom(stream));
                }
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
        }
    }

    internal static class Utils
    {
        public static int ReadVarint(this Stream stream)
        {
            var value = ReadUnsignedVarint(stream);
            return (int)((value >> 1) ^ -(value & 1));
        }

        /// <remarks>
        ///     Inspired by: https://github.com/apache/kafka/blob/2.5/clients/src/main/java/org/apache/kafka/common/utils/ByteUtils.java#L142
        /// </remarks>
        public static uint ReadUnsignedVarint(this Stream stream)
        {
            int value = 0;
            int i = 0;
            int b;
            while (true)
            {
                b = stream.ReadByte();
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
