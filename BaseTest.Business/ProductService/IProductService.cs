using BaseTest.Models.Entities;
using BaseTest.Models.ReponseModels.Product;

namespace BaseTest.Businiss.ProductService;

public interface IProductService
{
    Task Save(ProductCreatingRequest request);
    Task Update(ProductUpdateFormRequest request);
    Task<ProductViewResponse> GetById(int id);
    IQueryable<ProductViewResponse> GetPageProducts(GetPageProductInput input);
    Task Delete(int id);
}