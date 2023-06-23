using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimShop.Base;
using SimShop.Base.Constants;
using SimShop.Operation;
using SimShop.Schema;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SimShop.Service;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{

    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public ApiResponse<List<ProductResponse>> GetAll()
    {
        var response = _productService.GetAll();
        return response;
    }

    [HttpGet("product/{id}")]
    public ApiResponse<ProductResponse> GetByProductId(int id)
    {
        var response = _productService.GetById(id);
        return response;
    }

    [HttpGet("productByCategoryId/{id}")]
    public ApiResponse<List<ProductResponse>> GetByCategoryId(int id)
    {
        var response = _productService.GetProductsByCategory(id);
        return response;
    }

    [HttpPost("AddProduct")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ApiResponse> PostProduct([FromBody] ProductRequest request)
    {
        var response = await _productService.InsertAsync(request);
        return response;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public ApiResponse UpdateProduct(int id, [FromBody] ProductRequest request)
    {
        var response = _productService.Update(id, request);
        return response;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public ApiResponse DeleteProduct(int id)
    {
        var response = _productService.Delete(id);
        return response;
    }
   


}
