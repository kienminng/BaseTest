namespace BaseTest.Models.ReponseModels.Order;

public class ListOrderAndMoneyTotalResponse
{
    public IQueryable<OrderViewsResponse>? List { get; set; }
    public double Money { get; set; }
}