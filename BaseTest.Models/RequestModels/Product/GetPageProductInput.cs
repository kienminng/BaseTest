namespace BaseTest.Models.ReponseModels.Product;

public class GetPageProductInput : BasePaginationRequestModel
{
    public string Name { get; set; } = string.Empty;
    public double StartPrice { get; set; } = 0;
    public double EndPrice { get; set; } = 0;
    public int Discount { get; set; } = 0;
}