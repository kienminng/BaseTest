namespace BaseTest.Models.ReponseModels.Order;

public class GetPageOrderInput : BasePaginationRequestModel
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? PaymentId { get; set; }
    public int? OrderStatus { get; set; } 
}