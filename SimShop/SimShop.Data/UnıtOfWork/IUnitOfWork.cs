using SimShop.Base;
using SimShop.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    
    IGenericRepository<Entity> Repository<Entity>() where Entity : BaseModel;
    Task CompleteAsync();
    void Complete();
    void CompleteWithTransaction();
}
