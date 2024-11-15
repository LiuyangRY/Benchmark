using BenchMark.Model.Enum;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace BenchMark.EnumTest
{
    /// <summary>
    /// 类型转换测试
    /// </summary>
    [MemoryDiagnoser] 
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ConvertTest
    {
        [Params(EnumValue.Type1, EnumValue.Type2, EnumValue.Type3)]
        public EnumValue enumValue;

        [Benchmark]
        public int ForceConvert()
        {
            return (int)enumValue;
        }

        [Benchmark]
        public int ToInt32()
        {
            return enumValue.GetHashCode();
        }
    }    
}