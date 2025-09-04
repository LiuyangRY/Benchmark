using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace BenchMark.Test;

/// <summary>
/// 配伍禁忌优化（废案：查询条件较多时，组合的可能性指数上升，查询效率降低。查询条件在6个以下时，最多会生成127种主键，查询性能较优）
/// </summary>
[MemoryDiagnoser]
[Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class TabooOptimizeTest
{
    private readonly int SearchCommodityIdCount = 10;

    private readonly int TabooCount = 10;

    private readonly int CommodityCount = 100;

    public required List<Commodity> Commodities { get; set; }

    public required List<Taboo> Taboos { get; set; }

    public required List<TabooCommodity> TabooCommodities { get; set; }

    public required List<long> SearchedCommodityIds { get; set; }

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        Commodities = await InitCommodityData();
        Taboos = await InitTabooData();
        TabooCommodities = await AssociateTabooAndCommodity(Taboos, Commodities);
        SearchedCommodityIds = Commodities.Select(commodity => commodity.CommodityId).Take(SearchCommodityIdCount).ToList();
    }

    [Benchmark]
    public async Task Test()
    {
        var searchIdList = GetPermutations(SearchedCommodityIds);
        var searchIdMD5List = new List<string>();
        foreach (var searchId in searchIdList)
        {
            var searchIdMD5 = await CommodityIdMD5(searchId);
            searchIdMD5List.Add(searchIdMD5);
        }
        var result = Taboos.FindAll(taboo => searchIdMD5List.Contains(taboo.CommodityIdMD5))
            .Select(taboo => taboo.TabooId)
            .ToList();
        var commodityIds = TabooCommodities.FindAll(tabooCommodity => result.Contains(tabooCommodity.TabooId))
            .Select(tabooCommodity => tabooCommodity.CommodityId)
            .ToHashSet();
    }

    /// <summary>
    /// 初始化商品数据
    /// </summary>
    public async Task<List<Commodity>> InitCommodityData()
    {
        var commodities = new List<Commodity>();
        await Task.Run(() =>
        {
            for (int i = 0; i < CommodityCount; i++)
            {
                var buffer = new byte[8];
                using (var random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(buffer);
                }
                long commodityId = BitConverter.ToInt64(buffer, 0);
                if (commodityId < 0)
                    commodityId = Math.Abs(commodityId);
                commodities.Add(new Commodity() { CommodityId = commodityId, CommodityName = commodityId.ToString() });
            }
        });
        return commodities;
    }

    /// <summary>
    /// 初始化配伍禁忌数据
    /// </summary>
    public async Task<List<Taboo>> InitTabooData()
    {
        var taboos = new List<Taboo>();
        await Task.Run(() =>
        {
            for (int i = 0; i < TabooCount; i++)
            {
                var buffer = new byte[8];
                using (var random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(buffer);
                }
                var tabooId = BitConverter.ToInt64(buffer, 0);
                if (tabooId < 0)
                    tabooId = Math.Abs(tabooId);
                taboos.Add(new Taboo() { TabooId = tabooId, TabooName = tabooId.ToString(), CommodityIdMD5 = string.Empty });
            }
        });
        return taboos;
    }

    /// <summary>
    /// 关联配伍禁忌和商品
    /// </summary>
    /// <param name="tabooList">配伍禁忌列表</param>
    /// <param name="commodityList">商品列表</param>
    /// <returns>配伍禁忌商品列表</returns>
    public async Task<List<TabooCommodity>> AssociateTabooAndCommodity(List<Taboo> tabooList, List<Commodity> commodityList)
    {
        return await Task.Run(async () =>
        {
            var result = new List<TabooCommodity>();
            foreach (var taboo in tabooList)
            {
                var random = new Random();
                var associatedCommodityIds = new List<long>();
                var commodityCount = random.Next(2, commodityList.Count);
                var indexSet = new HashSet<long>();
                // 确保至少取2个商品与当前禁忌关联
                while (associatedCommodityIds.Count < commodityCount)
                {
                    int index = random.Next(0, commodityList.Count);
                    var commodity = commodityList[index];
                    if (indexSet.Add(commodity.CommodityId))
                        associatedCommodityIds.Add(commodity.CommodityId);
                }
                taboo.CommodityIdMD5 = await CommodityIdMD5(associatedCommodityIds);
                foreach (var commodityId in associatedCommodityIds)
                {
                    var buffer = new byte[8];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(buffer);
                    }
                    var tabooCommodityId = BitConverter.ToInt64(buffer, 0);
                    if (tabooCommodityId < 0)
                        tabooCommodityId = Math.Abs(tabooCommodityId);
                    result.Add(new TabooCommodity() { Id = tabooCommodityId, TabooId = taboo.TabooId, CommodityId = commodityId });
                }
            }
            return result;
        });

    }

    /// <summary>
    /// 获取商品编号排列结果
    /// </summary>
    /// <param name="commodityIds">商品编号列表</param>
    /// <param name="needSort">是否需要对商品编号列表排序</param>
    /// <returns>商品编号排列结果</returns>
    private List<List<long>> GetPermutations(List<long> commodityIds, bool needSort = true)
    {
        if (commodityIds?.Any() != true)
            return [[]];
        if (commodityIds.Count == 1)
            return [commodityIds];
        if (needSort)
        {
            commodityIds.Sort();
        }
        var result = new Dictionary<string, List<long>>();
        for (int lastIndex = commodityIds.Count; lastIndex > 0; lastIndex--)
        {
            for (int i = 0; i < lastIndex; i++)
            {
                var remainingList = commodityIds[(i + 1)..lastIndex];
                var subPermutations = GetPermutations(remainingList, false);
                foreach (var subPermutation in subPermutations)
                {
                    var permutations = new List<long> { commodityIds[i] };
                    permutations.AddRange(subPermutation);
                    result[string.Join('-', permutations)] = permutations;
                }
            }
        }
        return [.. result.Values];
    }

    /// <summary>
    /// 对商品编号列表进行MD5加密
    /// </summary>
    /// <param name="commodityIds">商品编号列表</param>
    /// <param name="needSort">是否需要排序</param> 
    /// <returns>商品编号排列MD5</returns>
    public async Task<string> CommodityIdMD5(List<long> commodityIds, bool needSort = true)
    {
        if (needSort)
            commodityIds.Sort();
        return await Task.Run(() =>
        {
            // 对商品编号列表进行MD5加密
            string permutationString = string.Join(",", commodityIds);
            byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(permutationString));
            return BitConverter.ToString(hashBytes).Replace("-", "");
        });
    }

    public class Commodity
    {
        public long CommodityId { get; set; }

        public string? CommodityName { get; set; }
    }

    public class Taboo
    {
        public long TabooId { get; set; }

        public string? TabooName { get; set; }

        public required string CommodityIdMD5 { get; set; }
    }

    public class TabooCommodity
    {
        public long Id { get; set; }

        public long CommodityId { get; set; }

        public long TabooId { get; set; }
    }
}