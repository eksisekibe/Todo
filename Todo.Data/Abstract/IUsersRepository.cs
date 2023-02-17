using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Entity.Concrete;

namespace Todo.Data.Abstract
{
    public interface IUsersRepository:IRepository<User>
    {
    }
}
