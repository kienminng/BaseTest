using BaseTest.Models.Entities;
using BaseTest.Models.ReponseModels.Order;
using BaseTest.Repository;

namespace BaseTest.Businiss.OrderService;

public class OrderService : IOrderService
{
    private readonly IBaseRepository<Order> _orderRepo;
    private readonly IBaseRepository<OrderDetail> _orderDetailsRepo;

    public OrderService(IBaseRepository<Order> orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public Task Create(OrderCreateFormRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Update(OrderUpdateFormRequest request)
    {
        throw new NotImplementedException();
    }

    public IQueryable<OrderViewsResponse>? GetAllLoginUser()
    {
        throw new NotImplementedException();
    }

    public Task ChangeStatus(int orderId, int orderStatusId)
    {
        throw new NotImplementedException();
    }

    public ListOrderAndMoneyTotalResponse CalculateRevenueByTimePeriod(GetPageOrderInput input)
    {
        throw new NotImplementedException();
    }


    // private async Task Validate(int productId, int paymentId, int userId)
    // {
    //     if ()
    //     {
    //         
    //     }
    // }
}