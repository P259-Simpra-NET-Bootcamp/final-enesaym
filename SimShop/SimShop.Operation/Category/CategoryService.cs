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
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public class CategoryService:BaseService<Category, CategoryRequest, CategoryResponse>, ICategoryService
{

    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public override ApiResponse Delete(int Id)
    {
        try
        {
            var category = unitOfWork.Repository<Category>().GetById(Id);
            if (category is null)
            {
                Log.Warning(LogType.RequestNull);
                return new ApiResponse(WarningType.NotFound);
            }

            var products = unitOfWork.Repository<Product>().Where(p => p.CategoryId == Id).ToList();
            if (products.Any())
            {
                return new ApiResponse(WarningType.CategoryHasProducts);
            }

            unitOfWork.Repository<Category>().DeleteById(Id);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.DeleteError);
            return new ApiResponse(ex.Message);
        }
    }


    public override ApiResponse<List<CategoryResponse>> GetAll()
    {
        try
        {
            var list = unitOfWork.Repository<Category>().GetAllWithInclude("Products");
            var mapped = mapper.Map<List<Category>, List<CategoryResponse>>(list);
            return new ApiResponse<List<CategoryResponse>>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CategoryResponse>>(ex.Message);
        }
    }

    public override ApiResponse Insert(CategoryRequest request)
    {
        try
        {
            var entity = mapper.Map<CategoryRequest, Category>(request);
            entity.GetType().GetProperty("CreatedAt").SetValue(entity, DateTime.UtcNow);
            entity.GetType().GetProperty("CreatedBy").SetValue(entity, Environment.MachineName);
            unitOfWork.Repository<Category>().Insert(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
       catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }
}
