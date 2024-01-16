namespace BaseTest.Models.ReponseModels.Product;

public class ProductUpdateFormRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Img { get; set; }
    public float? Price { get; set; }
    public int? Discount { get; set; }
    public string? Description { get; set; }
    public int NumberOfViews { get; set; }
}