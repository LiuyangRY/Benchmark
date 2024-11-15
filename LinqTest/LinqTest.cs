using BenchMark.Model.Enum;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace BenchMark.LinqTest
{
    /// <summary>
    /// 类型转换测试
    /// </summary>
    [MemoryDiagnoser] 
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class LinqTest
    {
        public static void Test()
        {
            var aList = new List<ClassA>()
            {
                new ClassA() { Id = 1 },
                new ClassA() { Id = 2 },
                new ClassA() { Id = 3 },
                new ClassA() { Id = 4 },
            };
            var bList = new List<ClassB>()
            {
                new ClassB { Id = 1, AId = 1, Status = EnumValue.Type1 }
            };
            var result = (from a in aList
                join b in bList on a.Id equals b.AId into tempList
                from temp in tempList.DefaultIfEmpty()
                select new ClassB()
                {
                    Id = temp?.Id ?? 0,
                    AId = temp?.AId ?? 0,
                    Status = temp?.Status ?? EnumValue.Type2,
                }
            ).ToList();
        }
    }    
    
    public class ClassA
    {
        public int Id { get; set; }
    }

    public class ClassB
    {
        public int Id { get; set; }

        public int AId { get; set; }

        public EnumValue Status { get; set; }
    }
}