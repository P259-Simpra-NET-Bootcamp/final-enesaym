using AutoMapper;
using Serilog;
using SimShop.Base;
using SimShop.Base.Constants;
using SimShop.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public class BaseService<TEntity, TRequest, TResponse> : IBaseService<TEntity, TRequest, TResponse> where TEntity : BaseModel
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public virtual ApiResponse Delete(int Id)
    {
        try
        {
            var entity = unitOfWork.Repository<TEntity>().GetByIdAsNoTracking(Id);
            if (entity is null)
            {
                Log.Warning(LogType.RequestNull);
                return new ApiResponse(WarningType.NotFound);
            }

            unitOfWork.Repository<TEntity>().DeleteById(Id);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.DeleteError);
            return new ApiResponse(ex.Message);
        }
    }

    public virtual ApiResponse<List<TResponse>> GetAll()
    {
        try
        {
            var entityList = unitOfWork.Repository<TEntity>().GetAllAsNoTracking();
            var mapped = mapper.Map<List<TEntity>, List<TResponse>>(entityList);
            return new ApiResponse<List<TResponse>>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex,LogType.GetError);
            return new ApiResponse<List<TResponse>>(ex.Message);
        }
    }

    public virtual ApiResponse<TResponse> GetById(int id)
    {
        try
        {
            var entity = unitOfWork.Repository<TEntity>().GetByIdAsNoTracking(id);
            if (entity is null)
            {
                Log.Warning(LogType.RequestNull);
                return new ApiResponse<TResponse>(WarningType.NotFound);
            }

            var mapped = mapper.Map<TEntity, TResponse>(entity);
            return new ApiResponse<TResponse>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.GetError);
            return new ApiResponse<TResponse>(ex.Message);
        }
    }  

    public virtual  ApiResponse Insert(TRequest request)
    {
        try
        {
            var entity = mapper.Map<TRequest, TEntity>(request);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = Environment.MachineName;

            unitOfWork.Repository<TEntity>().Insert(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.InsertError);
            return new ApiResponse(ex.Message);
        }
    }

    public virtual ApiResponse Update(int Id, TRequest request)
    {
        try
        {
            var entity = mapper.Map<TRequest, TEntity>(request);

            var exist = unitOfWork.Repository<TEntity>().GetByIdAsNoTracking(Id);
            if (exist is null)
            {
                Log.Warning(LogType.RequestNull);
                return new ApiResponse(WarningType.NotFound);
            }

            entity.Id = Id;
            entity.CreatedBy = exist.CreatedBy;
            entity.CreatedAt = exist.CreatedAt;
            unitOfWork.Repository<TEntity>().Update(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.UpdateError);
            return new ApiResponse(ex.Message);
        }
    }
}
