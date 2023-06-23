using SimShop.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface IBaseService<TEntity, TRequest, TResponse>
{
    ApiResponse<List<TResponse>> GetAll();
    ApiResponse<TResponse> GetById(int id);
    ApiResponse Insert(TRequest request);
    ApiResponse Update(int Id, TRequest request);
    ApiResponse Delete(int Id);
}
