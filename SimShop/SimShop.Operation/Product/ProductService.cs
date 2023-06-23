using AutoMapper;
using Serilog;
using SimShop.Base;
using SimShop.Base.Constants;
using SimShop.Data;
using SimShop.Data.UnitOfWork;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public class ProductService : BaseService<Product, ProductRequest, ProductResponse>, IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> InsertAsync(ProductRequest request)
    {
        try
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId);
            if (category == null)
                return new ApiResponse(WarningType.CategoryNotFound);

            var entity = mapper.Map<ProductRequest, Product>(request);
            entity.GetType().GetProperty("CreatedAt").SetValue(entity, DateTime.UtcNow);
            entity.GetType().GetProperty("CreatedBy").SetValue(entity, Environment.MachineName);
            unitOfWork.Repository<Product>().Insert(entity);
            await unitOfWork.CompleteAsync();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.InsertError);
            return new ApiResponse(ex.Message);
        }
    }
    public ApiResponse<List<ProductResponse>> GetProductsByCategory(int categoryId)
    {
        try
        {
            var entityList = unitOfWork.Repository<Product>().Where(x => x.CategoryId==categoryId).ToList();
            var mapped = mapper.Map<List<Product>, List<ProductResponse>>(entityList);
            return new ApiResponse<List<ProductResponse>>(mapped);
            
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.GetError);
            return new ApiResponse<List<ProductResponse>>(ex.Message);
        }
       

    }

}
