using Todo.Entity.Abstract;

namespace Todo.Entity.Concrete
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
