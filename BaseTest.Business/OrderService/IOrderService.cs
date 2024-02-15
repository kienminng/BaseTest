using BaseTest.Models.ReponseModels.Order;

namespace BaseTest.Businiss.OrderService;

public interface IOrderService
{
    Task Create(OrderCreateFormRequest request);
    Task Update(OrderUpdateFormRequest request);
    IQueryable<OrderViewsResponse>? GetAllLoginUser();
    Task ChangeStatus(int orderId, int orderStatusId);
    ListOrderAndMoneyTotalResponse CalculateRevenueByTimePeriod(GetPageOrderInput input);
}