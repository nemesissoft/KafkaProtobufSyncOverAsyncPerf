# KafkaProtobufSyncOverAsyncPerf

## No optimizations 

|    Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|---------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|
| Confluent | 1.581 us | 0.0214 us | 0.0179 us |  1.00 |    0.00 | 0.8717 | 0.0134 |      5 KB |
|      Sync | 1.440 us | 0.0249 us | 0.0233 us |  0.91 |    0.02 | 0.8583 | 0.0134 |      5 KB |

## Removed all validations for Sync
|    Method |       Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Allocated |
|---------- |-----------:|---------:|---------:|------:|-------:|-------:|----------:|
| Confluent | 1,184.7 ns | 21.02 ns | 18.63 ns |  1.00 | 0.8278 | 0.0153 |   5,200 B |
|      Sync |   464.3 ns |  5.66 ns |  5.30 ns |  0.39 | 0.0854 |      - |     536 B |

## Optimized reading with all validations for Sync
|    Method |       Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Allocated |
|---------- |-----------:|---------:|---------:|------:|-------:|-------:|----------:|
| Confluent | 1,191.0 ns | 19.56 ns | 18.30 ns |  1.00 | 0.8278 | 0.0153 |   5,200 B |
|      Sync |   470.0 ns |  1.53 ns |  1.28 ns |  0.39 | 0.0854 |      - |     536 B |