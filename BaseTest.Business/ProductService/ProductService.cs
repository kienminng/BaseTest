using BaseTest.Common;
using BaseTest.Models.Entities;
using BaseTest.Models.ReponseModels.Product;
using BaseTest.Repository;
using Microsoft.IdentityModel.Tokens;

namespace BaseTest.Businiss.ProductService;

public class ProductService : IProductService
{
    private readonly IBaseRepository<Product> _productRepo;

    public ProductService(IBaseRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task Save(ProductCreatingRequest request)
    {
         ValidateProductCreate(request);
         try
         {
             var product = new Product()
             {
                 Name = request.Name,
                 Img = request.Img,
                 Price = request.Price,
                 Discount = request.Discount,
                 Description = request.Description
             };
             
             await _productRepo.CreateAsync(product);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             _productRepo.ClearTrackedChanges();
             throw;
         }
    }

    public async Task Update(ProductUpdateFormRequest request)
    {
        var data = await _productRepo.GetByIDAsync(request.Id);
        if (data is null) throw new Exception(ErrorMessage.ProductErrorMessage.ProductNotExisted);
        if (await ProductNameIsExisted(request.Name)) throw new Exception(ErrorMessage.ProductErrorMessage.ProductNameWasExisted);

        var product = new Product()
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price.Value,
            Discount = request.Discount.Value,
            Description = request.Description,
            Number_Of_View = request.NumberOfViews
        };
        try
        {
            await _productRepo.UpdateAsync(product);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _productRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task<ProductViewResponse> GetById(int id)
    {
        var query = await _productRepo.GetByIDAsync(id);
        var data = new ProductViewResponse()
        {
            Product_Id = query.Id,
            Name_Product = query.Name,
            Price = query.Price,
            Avatar_Image_Product = query.Img,
            Discount = query.Discount,
            Status = query.Status,
            Number_Of_View = query.Number_Of_View,
            PointAvg = query.Reviews.Average(detail => detail.PonitEvaluation)
        };
        return data;
    }

    public IQueryable<ProductViewResponse> GetPageProducts(GetPageProductInput input)
    {
        var query = _productRepo.GetQueryable(record => !record.IsDeleted);
        query =ApplySearchFilter(query, input);
        query = BaseApplyPaging.ApplyPaging(query, input.PageSize, input.PageNo, out var totalItem);
        var data =query.Select(record => new ProductViewResponse()
        {
            Product_Id = record.Id,
            Name_Product = record.Name,
            Price = record.Price,
            Avatar_Image_Product = record.Img,
            Discount = record.Discount,
            Status = record.Status,
            Number_Of_View = record.Number_Of_View,
            PointAvg = record.Reviews.Average(detail => detail.PonitEvaluation)
        });
        return data;
    }

    public async Task Delete(int id)
    {
        var product = await _productRepo.GetByIDAsync(id);
        
        if (product is null)
        {
            throw new Exception(ErrorMessage.ProductErrorMessage.ProductNotExisted);
        }

        product.Status = false;

        await _productRepo.UpdateAsync(product);
    }

    private IQueryable<Product> ApplySearchFilter(IQueryable<Product> query, GetPageProductInput input)
    {
        if (!input.Name.IsNullOrEmpty())
        {
            query = query.Where(record => record.Name.Contains(input.Name));
        }

        if (input.StartPrice > input.EndPrice)
        {
            throw new Exception();
        }

        if (input.StartPrice >= 0 && input.EndPrice >= input.StartPrice)
        {
            query = query.Where(record => record.Price >= input.StartPrice && record.Price <= input.EndPrice);
        }

        if (input.Discount >= 0 )
        {
            query = query.Where(record => record.Discount == input.Discount);
        }

        return query;
    }

    private async Task<bool> ProductNameIsExisted(string name)
    {
        var product = await _productRepo.GetAsync(record => !record.IsDeleted 
                                                            && record.Name.ToLower().Equals(name.ToLower()));
        if (product is null)
        {
            return false;
        }

        return true;
    }

    private async void ValidateProductCreate(ProductCreatingRequest request)
    {
        if (await ProductNameIsExisted(request.Name))
        {
            throw new Exception(ErrorMessage.ProductErrorMessage.ProductNameWasExisted);
        }

        if (request.Price <= 0)
        {
            throw new Exception(ErrorMessage.ProductErrorMessage.PriceError);
        }

        if (request.Discount < 0)
        {
            throw new Exception(ErrorMessage.ProductErrorMessage.Discount);
        }
    }
    
}