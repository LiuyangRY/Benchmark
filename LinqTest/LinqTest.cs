using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Hys.Protocols.Dtos;

namespace BenchMark.LinqTest
{
    /// <summary>
    /// Linq测试
    /// </summary>
    [MemoryDiagnoser] 
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class LinqTest
    {
        /// <summary>
        /// 100个元素的情况下：使用字典比遍历查询快约30倍，内存占用少分配约4倍内存； 
        /// 1000个元素的情况下：使用字典比遍历查询快约220倍，内存占用少分配约4倍内存；
        /// 10000个元素的情况下：使用字典比遍历查询快约1300倍，内存占用少分配约4.5倍内存；
        /// </summary>
        [Params(100, 1000, 10000)]
        public int DoctorIdCount = 10;

        public List<long> DoctorIds { get; set; }

        public List<DoctorInfoDto> DoctorInfos { get; set; }

        public LinqTest()
        {
            DoctorIds = new List<long>();
            DoctorInfos = new List<DoctorInfoDto>();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            DoctorIds = new List<long>(DoctorIdCount);
            DoctorInfos = new List<DoctorInfoDto>(DoctorIdCount);
            foreach(var id in Enumerable.Range(1, DoctorIdCount))
            {
                DoctorIds.Add(id);
                DoctorInfos.Add(new DoctorInfoDto() { Id = id, Name = $"医生{id}" });
            };
        }
        
        [Benchmark]
        public void FirstOrDefaultTest()
        {
            foreach (var id in DoctorIds)
            {
                var doctorInfo = DoctorInfos.FirstOrDefault(doctor => doctor.Id == id);
            }
        }

        [Benchmark]
        public void DictionaryFindTest()
        {
            var doctorInfoDictionary = DoctorInfos.ToDictionary(doctor => doctor.Id);
            foreach (var id in DoctorIds)
            {
                doctorInfoDictionary.TryGetValue(id, out var doctorInfo);
            }
        }
    }
}