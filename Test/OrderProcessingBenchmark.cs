using BenchmarkDotNet.Attributes;
using Hys.Core;
using Hys.Constants;
using Hys.Constants.Enums;
using Hys.Protocols.OutDtos.Order;
using System.Linq;

public class OrderProcessingBenchmark
{
    public PagedList<ClientOrderListOutDto> OrderList { get; set; }

    [Params(1000, 10000, 100000)]
    public int OrderCount { get; set; }

    public OrderProcessingBenchmark()
    {
        OrderList = new PagedList<ClientOrderListOutDto>();
    }

    [GlobalSetup]
    public void Setup()
    {
        var tempList = Enumerable.Range(1, OrderCount)
           .Select(i => new ClientOrderListOutDto
            {
                Id = i,
                OrderStatus = i % 2 == 0? EnumOrderStatus.待付款 : EnumOrderStatus.已完成,
                OrderPayType = i % 3 == 0? EnumOrderPayType.微信 : EnumOrderPayType.支付宝
            })
           .ToList();
        OrderList = new PagedList<ClientOrderListOutDto>(tempList, 1, OrderCount, OrderCount);
    }

    [Benchmark]
    public void ProcessClientOrderExtendInfosWhenAll()
    {
        ProcessClientOrderExtendInfosWhenAll(OrderList).Wait();
    }

    [Benchmark]
    public void ProcessClientOrderExtendInfosAwait()
    {
        ProcessClientOrderExtendInfosAwait(OrderList).Wait();
    }

    private async Task ProcessClientOrderExtendInfosWhenAll(PagedList<ClientOrderListOutDto> orders)
    {
        if (!orders.Any())
            return;

        var orderIds = orders.Select(c => c.Id).ToList();

        // 并行获取破损赔付信息和取消订单时间
        var damageCompensationTasks = Task1(orderIds);
        var cancelTimeTask = Task2();

        // 等待所有任务完成
        await Task.WhenAll(damageCompensationTasks, cancelTimeTask);

        var damageCompensationDic = damageCompensationTasks.Result.ToDictionary(key => key.Id);
        var cancelTime = cancelTimeTask.Result;
    }
    
    /// <summary>
    /// 处理订单扩展信息（订单是否可取消、订单破损赔付信息）
    /// </summary>
    /// <param name="orders">订单信息</param>
    /// <returns>包含扩展信息的订单信息</returns>
    private async Task ProcessClientOrderExtendInfosAwait(PagedList<ClientOrderListOutDto> orders)
    {
        if (!orders.Any())
            return;
        var orderIds = orders.Select(c => c.Id).ToList();

        // 破损赔付信息
        var damageCompensationDic = (await Task1(orderIds))
            .ToDictionary(key => key.Id);

        var cancelTime = await Task2();
    }

    private async Task<List<ClientOrderListOutDto>> Task1(IEnumerable<long> orderIds)
    {
        // 模拟异步操作
        return await Task.Run(() =>
        {
            var result = new List<ClientOrderListOutDto>();
            foreach (var orderId in orderIds)
            {
                var order = OrderList.FirstOrDefault(c => c.Id == orderId);
                if (order!= null)
                {
                    result.Add(order);
                }
            }
            return result;
        });
    }

    private async Task<int> Task2()
    {
        // 模拟异步操作
        return await Task.Run(() =>
        {
           Task.Delay(2000);
           return 2;
        });
    }
 }
