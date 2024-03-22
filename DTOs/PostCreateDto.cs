namespace SimpleBlogSystem.DTOs
{
    public class PostUpdateDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
