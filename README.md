# KafkaProtobufSyncOverAsyncPerf

|    Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|---------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|
| Confluent | 1.581 us | 0.0214 us | 0.0179 us |  1.00 |    0.00 | 0.8717 | 0.0134 |      5 KB |
|      Sync | 1.440 us | 0.0249 us | 0.0233 us |  0.91 |    0.02 | 0.8583 | 0.0134 |      5 KB |