using System;
using System.ComponentModel.DataAnnotations;


namespace Todo.Entity.Abstract
{
    public class BaseEntity
    {
        [Key]  
        public int Id { get; set; }
        public bool? Deleted { get; set; } = false;
        public DateTimeOffset? Created_at { get; protected set; } = DateTimeOffset.Now;
        public DateTimeOffset? Updated_at { get; set; }
        
    }
}
