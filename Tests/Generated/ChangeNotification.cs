// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: ChangeNotification.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from ChangeNotification.proto</summary>
public static partial class ChangeNotificationReflection {

  #region Descriptor
  /// <summary>File descriptor for ChangeNotification.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static ChangeNotificationReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChhDaGFuZ2VOb3RpZmljYXRpb24ucHJvdG8aH2dvb2dsZS9wcm90b2J1Zi90",
          "aW1lc3RhbXAucHJvdG8iGAoFU3RvY2sSDwoHYWxpYXNlcxgBIAMoCSIaCghD",
          "dXJyZW5jeRIOCgZhbW91bnQYASABKAIiqQEKEkNoYW5nZU5vdGlmaWNhdGlv",
          "bhIKCgJpZBgBIAEoBRIXCgZzdGF0dXMYAiABKA4yBy5TdGF0dXMSKAoEdGlt",
          "ZRgDIAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5UaW1lc3RhbXASFwoFc3RvY2sY",
          "BSABKAsyBi5TdG9ja0gAEh0KCGN1cnJlbmN5GAYgASgLMgkuQ3VycmVuY3lI",
          "AEIMCgppbnN0cnVtZW50KkkKBlN0YXR1cxILCgdVTktOT1dOEAASCwoHUEVO",
          "RElORxABEgoKBkFDVElWRRACEg0KCVNVU1BFTkRFRBADEgoKBkNMT1NFRBAE",
          "YgZwcm90bzM="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
        new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Status), }, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::Stock), global::Stock.Parser, new[]{ "Aliases" }, null, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::Currency), global::Currency.Parser, new[]{ "Amount" }, null, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::ChangeNotification), global::ChangeNotification.Parser, new[]{ "Id", "Status", "Time", "Stock", "Currency" }, new[]{ "Instrument" }, null, null, null)
        }));
  }
  #endregion

}
#region Enums
public enum Status {
  [pbr::OriginalName("UNKNOWN")] Unknown = 0,
  [pbr::OriginalName("PENDING")] Pending = 1,
  [pbr::OriginalName("ACTIVE")] Active = 2,
  [pbr::OriginalName("SUSPENDED")] Suspended = 3,
  [pbr::OriginalName("CLOSED")] Closed = 4,
}

#endregion

#region Messages
public sealed partial class Stock : pb::IMessage<Stock>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<Stock> _parser = new pb::MessageParser<Stock>(() => new Stock());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<Stock> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::ChangeNotificationReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public Stock() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public Stock(Stock other) : this() {
    aliases_ = other.aliases_.Clone();
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public Stock Clone() {
    return new Stock(this);
  }

  /// <summary>Field number for the "aliases" field.</summary>
  public const int AliasesFieldNumber = 1;
  private static readonly pb::FieldCodec<string> _repeated_aliases_codec
      = pb::FieldCodec.ForString(10);
  private readonly pbc::RepeatedField<string> aliases_ = new pbc::RepeatedField<string>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public pbc::RepeatedField<string> Aliases {
    get { return aliases_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as Stock);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(Stock other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if(!aliases_.Equals(other.aliases_)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    hash ^= aliases_.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    aliases_.WriteTo(output, _repeated_aliases_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    aliases_.WriteTo(ref output, _repeated_aliases_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    size += aliases_.CalculateSize(_repeated_aliases_codec);
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(Stock other) {
    if (other == null) {
      return;
    }
    aliases_.Add(other.aliases_);
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          aliases_.AddEntriesFrom(input, _repeated_aliases_codec);
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 10: {
          aliases_.AddEntriesFrom(ref input, _repeated_aliases_codec);
          break;
        }
      }
    }
  }
  #endif

}

public sealed partial class Currency : pb::IMessage<Currency>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<Currency> _parser = new pb::MessageParser<Currency>(() => new Currency());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<Currency> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::ChangeNotificationReflection.Descriptor.MessageTypes[1]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public Currency() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public Currency(Currency other) : this() {
    amount_ = other.amount_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public Currency Clone() {
    return new Currency(this);
  }

  /// <summary>Field number for the "amount" field.</summary>
  public const int AmountFieldNumber = 1;
  private float amount_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public float Amount {
    get { return amount_; }
    set {
      amount_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as Currency);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(Currency other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Amount, other.Amount)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (Amount != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Amount);
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (Amount != 0F) {
      output.WriteRawTag(13);
      output.WriteFloat(Amount);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (Amount != 0F) {
      output.WriteRawTag(13);
      output.WriteFloat(Amount);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (Amount != 0F) {
      size += 1 + 4;
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(Currency other) {
    if (other == null) {
      return;
    }
    if (other.Amount != 0F) {
      Amount = other.Amount;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 13: {
          Amount = input.ReadFloat();
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 13: {
          Amount = input.ReadFloat();
          break;
        }
      }
    }
  }
  #endif

}

public sealed partial class ChangeNotification : pb::IMessage<ChangeNotification>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<ChangeNotification> _parser = new pb::MessageParser<ChangeNotification>(() => new ChangeNotification());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<ChangeNotification> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::ChangeNotificationReflection.Descriptor.MessageTypes[2]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ChangeNotification() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ChangeNotification(ChangeNotification other) : this() {
    id_ = other.id_;
    status_ = other.status_;
    time_ = other.time_ != null ? other.time_.Clone() : null;
    switch (other.InstrumentCase) {
      case InstrumentOneofCase.Stock:
        Stock = other.Stock.Clone();
        break;
      case InstrumentOneofCase.Currency:
        Currency = other.Currency.Clone();
        break;
    }

    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ChangeNotification Clone() {
    return new ChangeNotification(this);
  }

  /// <summary>Field number for the "id" field.</summary>
  public const int IdFieldNumber = 1;
  private int id_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int Id {
    get { return id_; }
    set {
      id_ = value;
    }
  }

  /// <summary>Field number for the "status" field.</summary>
  public const int StatusFieldNumber = 2;
  private global::Status status_ = global::Status.Unknown;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Status Status {
    get { return status_; }
    set {
      status_ = value;
    }
  }

  /// <summary>Field number for the "time" field.</summary>
  public const int TimeFieldNumber = 3;
  private global::Google.Protobuf.WellKnownTypes.Timestamp time_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Google.Protobuf.WellKnownTypes.Timestamp Time {
    get { return time_; }
    set {
      time_ = value;
    }
  }

  /// <summary>Field number for the "stock" field.</summary>
  public const int StockFieldNumber = 5;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Stock Stock {
    get { return instrumentCase_ == InstrumentOneofCase.Stock ? (global::Stock) instrument_ : null; }
    set {
      instrument_ = value;
      instrumentCase_ = value == null ? InstrumentOneofCase.None : InstrumentOneofCase.Stock;
    }
  }

  /// <summary>Field number for the "currency" field.</summary>
  public const int CurrencyFieldNumber = 6;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Currency Currency {
    get { return instrumentCase_ == InstrumentOneofCase.Currency ? (global::Currency) instrument_ : null; }
    set {
      instrument_ = value;
      instrumentCase_ = value == null ? InstrumentOneofCase.None : InstrumentOneofCase.Currency;
    }
  }

  private object instrument_;
  /// <summary>Enum of possible cases for the "instrument" oneof.</summary>
  public enum InstrumentOneofCase {
    None = 0,
    Stock = 5,
    Currency = 6,
  }
  private InstrumentOneofCase instrumentCase_ = InstrumentOneofCase.None;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public InstrumentOneofCase InstrumentCase {
    get { return instrumentCase_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearInstrument() {
    instrumentCase_ = InstrumentOneofCase.None;
    instrument_ = null;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as ChangeNotification);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(ChangeNotification other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Id != other.Id) return false;
    if (Status != other.Status) return false;
    if (!object.Equals(Time, other.Time)) return false;
    if (!object.Equals(Stock, other.Stock)) return false;
    if (!object.Equals(Currency, other.Currency)) return false;
    if (InstrumentCase != other.InstrumentCase) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (Id != 0) hash ^= Id.GetHashCode();
    if (Status != global::Status.Unknown) hash ^= Status.GetHashCode();
    if (time_ != null) hash ^= Time.GetHashCode();
    if (instrumentCase_ == InstrumentOneofCase.Stock) hash ^= Stock.GetHashCode();
    if (instrumentCase_ == InstrumentOneofCase.Currency) hash ^= Currency.GetHashCode();
    hash ^= (int) instrumentCase_;
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (Id != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Id);
    }
    if (Status != global::Status.Unknown) {
      output.WriteRawTag(16);
      output.WriteEnum((int) Status);
    }
    if (time_ != null) {
      output.WriteRawTag(26);
      output.WriteMessage(Time);
    }
    if (instrumentCase_ == InstrumentOneofCase.Stock) {
      output.WriteRawTag(42);
      output.WriteMessage(Stock);
    }
    if (instrumentCase_ == InstrumentOneofCase.Currency) {
      output.WriteRawTag(50);
      output.WriteMessage(Currency);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (Id != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Id);
    }
    if (Status != global::Status.Unknown) {
      output.WriteRawTag(16);
      output.WriteEnum((int) Status);
    }
    if (time_ != null) {
      output.WriteRawTag(26);
      output.WriteMessage(Time);
    }
    if (instrumentCase_ == InstrumentOneofCase.Stock) {
      output.WriteRawTag(42);
      output.WriteMessage(Stock);
    }
    if (instrumentCase_ == InstrumentOneofCase.Currency) {
      output.WriteRawTag(50);
      output.WriteMessage(Currency);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (Id != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
    }
    if (Status != global::Status.Unknown) {
      size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
    }
    if (time_ != null) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Time);
    }
    if (instrumentCase_ == InstrumentOneofCase.Stock) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Stock);
    }
    if (instrumentCase_ == InstrumentOneofCase.Currency) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Currency);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(ChangeNotification other) {
    if (other == null) {
      return;
    }
    if (other.Id != 0) {
      Id = other.Id;
    }
    if (other.Status != global::Status.Unknown) {
      Status = other.Status;
    }
    if (other.time_ != null) {
      if (time_ == null) {
        Time = new global::Google.Protobuf.WellKnownTypes.Timestamp();
      }
      Time.MergeFrom(other.Time);
    }
    switch (other.InstrumentCase) {
      case InstrumentOneofCase.Stock:
        if (Stock == null) {
          Stock = new global::Stock();
        }
        Stock.MergeFrom(other.Stock);
        break;
      case InstrumentOneofCase.Currency:
        if (Currency == null) {
          Currency = new global::Currency();
        }
        Currency.MergeFrom(other.Currency);
        break;
    }

    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Id = input.ReadInt32();
          break;
        }
        case 16: {
          Status = (global::Status) input.ReadEnum();
          break;
        }
        case 26: {
          if (time_ == null) {
            Time = new global::Google.Protobuf.WellKnownTypes.Timestamp();
          }
          input.ReadMessage(Time);
          break;
        }
        case 42: {
          global::Stock subBuilder = new global::Stock();
          if (instrumentCase_ == InstrumentOneofCase.Stock) {
            subBuilder.MergeFrom(Stock);
          }
          input.ReadMessage(subBuilder);
          Stock = subBuilder;
          break;
        }
        case 50: {
          global::Currency subBuilder = new global::Currency();
          if (instrumentCase_ == InstrumentOneofCase.Currency) {
            subBuilder.MergeFrom(Currency);
          }
          input.ReadMessage(subBuilder);
          Currency = subBuilder;
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 8: {
          Id = input.ReadInt32();
          break;
        }
        case 16: {
          Status = (global::Status) input.ReadEnum();
          break;
        }
        case 26: {
          if (time_ == null) {
            Time = new global::Google.Protobuf.WellKnownTypes.Timestamp();
          }
          input.ReadMessage(Time);
          break;
        }
        case 42: {
          global::Stock subBuilder = new global::Stock();
          if (instrumentCase_ == InstrumentOneofCase.Stock) {
            subBuilder.MergeFrom(Stock);
          }
          input.ReadMessage(subBuilder);
          Stock = subBuilder;
          break;
        }
        case 50: {
          global::Currency subBuilder = new global::Currency();
          if (instrumentCase_ == InstrumentOneofCase.Currency) {
            subBuilder.MergeFrom(Currency);
          }
          input.ReadMessage(subBuilder);
          Currency = subBuilder;
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
