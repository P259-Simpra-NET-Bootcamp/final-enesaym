using SimShop.Base;
using SimShop.Data.Context;
using SimShop.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SimShopDbContext dbContext;
    private bool disposed;

    public UnitOfWork(SimShopDbContext dbContext)
    {
        this.dbContext = dbContext;
       
    }

    public IGenericRepository<Entity> Repository<Entity>() where Entity : BaseModel
    {
        return new GenericRepository<Entity>(dbContext);
    }
    public void Complete()
    {
        dbContext.SaveChanges();
    }
    public async Task CompleteAsync()
    {
        await dbContext.SaveChangesAsync();
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
            if (disposing && dbContext is not null)
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
