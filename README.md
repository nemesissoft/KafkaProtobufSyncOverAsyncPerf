# KafkaProtobufSyncOverAsyncPerf

## Stats

* BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19041.1288 (2004/May2020Update/20H1)
* Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
* .NET SDK=5.0.301
  * [Host]     : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  * DefaultJob : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT


|       Method | Iterations |         Mean |     Error |    StdDev | Ratio |   Gen 0 |  Gen 1 | Allocated |
|------------- |----------- |-------------:|----------:|----------:|------:|--------:|-------:|----------:|
|    Confluent |          1 |   1,166.2 ns |  12.57 ns |  10.50 ns |  1.00 |  0.8278 | 0.0153 |   5,200 B |
|     NonAlloc |          1 |     489.2 ns |   3.41 ns |   3.19 ns |  0.42 |  0.0968 | 0.0014 |     608 B |
| NonAllocSync |          1 |     476.1 ns |   3.07 ns |   2.72 ns |  0.41 |  0.0854 |      - |     536 B |
|              |            |              |           |           |       |         |        |           |
|    Confluent |         10 |  11,622.9 ns |  92.05 ns |  81.60 ns |  1.00 |  8.2779 | 0.1678 |  52,003 B |
|     NonAlloc |         10 |   4,859.8 ns |  39.42 ns |  36.87 ns |  0.42 |  0.9689 |      - |   6,080 B |
| NonAllocSync |         10 |   4,678.5 ns |  43.73 ns |  40.91 ns |  0.40 |  0.8545 |      - |   5,360 B |
|              |            |              |           |           |       |         |        |           |
|    Confluent |        100 | 115,131.1 ns | 236.41 ns | 209.57 ns |  1.00 | 82.7637 | 1.7090 | 520,030 B |
|     NonAlloc |        100 |  48,836.5 ns | 469.76 ns | 416.43 ns |  0.42 |  9.6436 |      - |  60,803 B |
| NonAllocSync |        100 |  46,095.0 ns | 189.30 ns | 167.81 ns |  0.40 |  8.5449 |      - |  53,603 B |