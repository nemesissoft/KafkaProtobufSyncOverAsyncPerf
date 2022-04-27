using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

using Google.Protobuf;


namespace KafkaDeserPerf.Deserializers
{
    /// <summary>Protobuf deserializer.</summary>
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
    public class ProtobufDeserializer2<T> : IAsyncDeserializer<T>, IDeserializer<T>
        where T : class, IMessage<T>, new()
    {
        private const byte MagicByte = 0; //Magic byte that identifies a message with Confluent Platform framing.

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
            => Task.FromResult(Deserialize(data.Span, isNull, context));

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) return null!;//TODO add proper nullability support once IDeserializer<T> would support it 

            if (data.Length < 6)
                throw new InvalidDataException($"Expecting data framing of length 6 bytes or more but total data size is {data.Length} bytes");

            var spanReader = new SpanBufferReader(data);

            var magicByte = spanReader.ReadByte();
            if (magicByte != MagicByte)
                throw new InvalidDataException($"Expecting message {context.Component} with Confluent Schema Registry framing. Magic byte was {magicByte}, expecting {MagicByte}");

            // A schema is not required to deserialize protobuf messages since the serialized data includes tag and type information, which is enough for
            // the IMessage<T> implementation to deserialize the data (even if the schema has evolved). Schema Id is thus unused
            // EDIT: so just advancing by 4 bytes is enough
            spanReader.AdvanceBy(4); //var _schemaId = IPAddress.NetworkToHostOrder(spanReader.ReadInt32());

            // Read the index array length, then all of the indices. These are not needed, but parsing them is the easiest way to seek to the start of the serialized data because they are varints.
            var indicesLength = _useDeprecatedFormat ? (int)spanReader.ReadUnsignedVarint() : spanReader.ReadVarint();
            for (int i = 0; i < indicesLength; ++i)
                if (_useDeprecatedFormat)
                    spanReader.ReadUnsignedVarint();
                else
                    spanReader.ReadVarint();

            return _parser.ParseFrom(spanReader.Tail());
        }
    }
}
