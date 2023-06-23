using Microsoft.AspNetCore.Mvc;
using SimShop.Data.Context;
using SimShop.Data;
using System.Collections.Generic;
using AutoMapper;
using SimShop.Schema;
using System;
using SimShop.Data.UnitOfWork;
using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using SimShop.Base;
using SimShop.Operation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;
using SimShop.Base.Constants;

namespace SimShop.Service;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryService categoryService,IProductService productService, IMapper mapper)
    {
       _categoryService = categoryService;
      _mapper = mapper;   
    }
    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    public ApiResponse<List<CategoryResponse>> GetAllCategory()
    {
        var list = _categoryService.GetAll();
        return list;
    }

    [HttpPost("AddCategory")]
    [Authorize(Roles = UserRoles.Admin)]
    public ApiResponse PostCategory([FromBody] CategoryRequest request)
    {
        var response =_categoryService.Insert(request);
        return response;
    }
    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public ApiResponse UpdateCategory(int id, [FromBody] CategoryRequest request)
    {
        var response = _categoryService.Update(id, request);
        return response;
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public ApiResponse DeleteCategory(int id)
    {
        var response = _categoryService.Delete(id);
        return response;
    }


}
