# KafkaProtobufSyncOverAsyncPerf

## Stats

* BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19041.1288 (2004/May2020Update/20H1)
* Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
* .NET SDK=5.0.301
  * [Host]     : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  * DefaultJob : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT


|         Method | Iterations |         Mean |       Error |      StdDev | Ratio |   Gen 0 |  Gen 1 | Allocated |
|--------------- |----------- |-------------:|------------:|------------:|------:|--------:|-------:|----------:|
|      Confluent |          1 |   1,228.1 ns |    19.94 ns |    16.65 ns |  1.00 |  0.8278 | 0.0153 |   5,200 B |
|       NonAlloc |          1 |     512.7 ns |     9.49 ns |     8.88 ns |  0.42 |  0.0963 | 0.0010 |     608 B |
|   NonAllocSync |          1 |     482.6 ns |     3.32 ns |     2.94 ns |  0.39 |  0.0854 |      - |     536 B |
| EfficientAsync |          1 |     506.1 ns |     5.49 ns |     5.14 ns |  0.41 |  0.0968 | 0.0014 |     608 B |
|  EfficientSync |          1 |     487.4 ns |     3.96 ns |     3.70 ns |  0.40 |  0.0854 |      - |     536 B |
|                |            |              |             |             |       |         |        |           |
|      Confluent |         10 |  12,087.8 ns |   117.93 ns |   110.31 ns |  1.00 |  8.2779 | 0.1678 |  52,003 B |
|       NonAlloc |         10 |   5,378.6 ns |    94.74 ns |    88.62 ns |  0.45 |  0.9689 |      - |   6,080 B |
|   NonAllocSync |         10 |   4,738.9 ns |    36.41 ns |    34.06 ns |  0.39 |  0.8545 |      - |   5,360 B |
| EfficientAsync |         10 |   4,995.7 ns |    52.32 ns |    48.94 ns |  0.41 |  0.9689 |      - |   6,080 B |
|  EfficientSync |         10 |   4,838.4 ns |    80.18 ns |    75.00 ns |  0.40 |  0.8545 |      - |   5,360 B |
|                |            |              |             |             |       |         |        |           |
|      Confluent |        100 | 122,244.2 ns | 1,952.18 ns | 1,730.56 ns |  1.00 | 82.7637 | 1.7090 | 520,030 B |
|       NonAlloc |        100 |  49,873.6 ns |   535.68 ns |   501.08 ns |  0.41 |  9.6436 |      - |  60,804 B |
|   NonAllocSync |        100 |  47,310.8 ns |   442.90 ns |   414.29 ns |  0.39 |  8.5449 |      - |  53,603 B |
| EfficientAsync |        100 |  49,830.6 ns |   443.76 ns |   415.10 ns |  0.41 |  9.6436 |      - |  60,803 B |
|  EfficientSync |        100 |  47,873.2 ns |   571.38 ns |   534.47 ns |  0.39 |  8.5449 |      - |  53,603 B |