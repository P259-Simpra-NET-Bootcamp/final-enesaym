using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using SimShop.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data.Repository;

public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseModel
{
    protected readonly SimShopDbContext dbContext;
    private bool disposed;

    public GenericRepository(SimShopDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void Delete(Entity entity)
    {
        dbContext.Set<Entity>().Remove(entity);
    }

    public void DeleteById(int id)
    {
        var entity = dbContext.Set<Entity>().Find(id);
        dbContext.Set<Entity>().Remove(entity);
    }

    public List<Entity> GetAll()
    {
        return dbContext.Set<Entity>().ToList();
    }

    public async Task<List<Entity>> GetAllAsync()
    {
        return await dbContext.Set<Entity>().ToListAsync();
    }

    public List<Entity> GetAllAsNoTracking()
    {
        return dbContext.Set<Entity>().AsNoTracking().ToList();
    }
    public List<Entity> GetAllWithInclude(params string[] includes)
    {
        var query = dbContext.Set<Entity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        return query.ToList();
    }

    public Entity GetById(int id)
    {
        return dbContext.Set<Entity>().Find(id);
    }
    public async Task<Entity> GetByIdAsync(int id)
    {
        return await dbContext.Set<Entity>().FindAsync(id);
    }

    public Entity GetByIdAsNoTracking(int id)
    {
        return dbContext.Set<Entity>().AsNoTracking().FirstOrDefault(x => x.Id == id);
    }

    public void Insert(Entity entity)
    {
        dbContext.Set<Entity>().Add(entity);
    }

    public async Task InsertAsync(Entity entity)
    {
        
        await dbContext.Set<Entity>().AddAsync(entity);
    }

    public void Update(Entity entity)
    {
        dbContext.Set<Entity>().Update(entity);
    }

    public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> expression)
    {
        return dbContext.Set<Entity>().Where(expression).AsQueryable();
    }
    

    public void Complete()
    {
        dbContext.SaveChanges();
    }

    public void CompleteWithTransaction()
    {
        using (var dbDcontextTransaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                dbContext.SaveChanges();
                dbDcontextTransaction.Commit();
            }
            catch (Exception ex)
            {
                // logging
                dbDcontextTransaction.Rollback();
            }
        }
    }


    private void Clean(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }

        disposed = true;
        GC.SuppressFinalize(this);
    }
    public void Dispose()
    {
        Clean(true);
    }


}
