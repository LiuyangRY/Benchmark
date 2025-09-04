using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace BenchMark.Test;

/// <summary>
/// Thread测试
/// </summary>
[MemoryDiagnoser]
[Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ThreadTest
{
    /// <summary>
    /// 内容数量
    /// </summary>
    [Params(10, 100, 1000, 10000)] public int ContentCount;

    private List<int>? Contents { get; set; }

    public ThreadTest()
    {
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Contents = new List<int>(ContentCount);
        for (int i = 0; i < ContentCount; i++)
        {
            Contents.Add(i);
        }
    }

    [Benchmark]
    public async Task<int> TaskTest()
    {
        var totalHeight = 0;
        var tasks = new List<Task>();
        foreach (var content in Contents!)
        {
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(0);
                Interlocked.Add(ref totalHeight, content);
            }));
        }

        await Task.WhenAll(tasks);
        return totalHeight;
    }

    [Benchmark]
    public async Task<int> ParallelForEachAsyncTest()
    {
        var heights = new int[ContentCount];
        await Parallel.ForEachAsync(Contents!.Select((content, index) => (content, index)),
            async (item, token) => { heights[item.index] = await CalculateContentHeightAsync(item.content); });
        return heights.Sum();
    }

    async Task<int> CalculateContentHeightAsync(int content)
    {
        await Task.Delay(0);
        return content;
    }
}