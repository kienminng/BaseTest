namespace BaseTest.Models.ReponseModels.Product;

public class ProductViewResponse
{
    public int Product_Id { get; set; } 
    
    public string Name_Product { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Avatar_Image_Product { get; set; } 
    public string Title { get; set; } = string.Empty;
    public int? Discount { get; set; }
    public bool Status { get; set; }
    public int? Number_Of_View { get; set; }    
    public double? PointAvg { get; set; }
    public DateTime? Create_At { get; set; }
    public DateTime? Update_At { get;set; }
}