using Microsoft.EntityFrameworkCore;
using Todo.Entity.Concrete;
using TodoAPI.Entity;

namespace Todo.Data
{
    public class DatabaseContexts : DbContext
    {
        public DatabaseContexts(DbContextOptions<DatabaseContexts> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public virtual DbSet<TodoItem> TodoItems { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //User
            modelBuilder.Entity<User>().HasData
            (
                new User() { FirstName = "Aditya", LastName = "Oberai", Email = "adityaoberai1@gmail.com", Password = "pass123", IsActive =false, Role = "Admin", Token = "" }

            );

            modelBuilder.Entity<TodoItem>().HasData
            (
                new TodoItem() {Id = 1, TaskToDo = "Test Task" }

            );
            base.OnModelCreating(modelBuilder);

        }
    }
}
