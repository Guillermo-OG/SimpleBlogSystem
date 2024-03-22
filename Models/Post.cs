using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogSystem.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [ForeignKey("User")] 
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
    }
}
