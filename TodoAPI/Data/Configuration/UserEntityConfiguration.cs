using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Entity.Concrete;
using TodoAPI.Entity;

namespace TodoAPI.Data.Configuration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasIndex(x => x.Email);
        }
    }
}
