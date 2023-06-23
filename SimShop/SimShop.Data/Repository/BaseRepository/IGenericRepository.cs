using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data.Repository;

public interface IGenericRepository<Entity> where Entity : BaseModel
{
    List<Entity> GetAll();
    Task<List<Entity>> GetAllAsync();
    List<Entity> GetAllAsNoTracking();
    void Insert(Entity entity);
    Task InsertAsync(Entity entity);
    Task<Entity> GetByIdAsync(int id);
    void Update(Entity entity);
    Entity GetById(int id);
    Entity GetByIdAsNoTracking(int id);
    void DeleteById(int id);
    void Delete(Entity entity);
    IEnumerable<Entity> Where(Expression<Func<Entity, bool>> expression);
    List<Entity> GetAllWithInclude(params string[] includes);
    void Complete();
    void CompleteWithTransaction();
}
