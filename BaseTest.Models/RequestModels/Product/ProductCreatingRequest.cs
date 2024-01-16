namespace BaseTest.Models.ReponseModels.Product;

public class ProductCreatingRequest
{
    public string Name { get; set; }
    public string Img { get; set; }
    public double Price { get; set; }
    public int Discount { get; set; }
    public string Description { get; set; }
}