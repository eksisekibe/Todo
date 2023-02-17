using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Abstract;
using Todo.Data.Concrete;
using Todo.Entity.Concrete;

namespace NotesApp.Data.Concrete
{
    public class UsersRepository : PgGenericRepository<User>, IUsersRepository
    {
        public UsersRepository(DatabaseContexts context) : base(context) { }

    }
}
