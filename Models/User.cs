using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SimpleBlogSystem.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
