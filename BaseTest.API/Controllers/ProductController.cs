using BaseTest.Businiss.ProductService;
using BaseTest.Common;
using BaseTest.Models.Entities;
using BaseTest.Models.ReponseModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseTest.API.Controllers;

[ApiController]
[Route(Constant.DefaultRouter)]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize(Roles = Constant.Roles.RoleAdmin)]
    public async Task<IActionResult> Create([FromForm] ProductCreatingRequest request)
    {
        await _productService.Save(request);
        return Ok();
    }
    
    [HttpPost]
    [Authorize(Roles = Constant.Roles.RoleAdmin)]
    public async Task<IActionResult> Update([FromForm] ProductUpdateFormRequest product)
    {
        await _productService.Update(product);
        return Ok();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetPageProduct([FromQuery] GetPageProductInput input)
    {
        var result = _productService.GetPageProducts(input);
        return Ok(result);
    }

    [HttpGet("{Id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int productId)
    {
        var result = await _productService.GetById(productId);
        return Ok(result);
    }
}