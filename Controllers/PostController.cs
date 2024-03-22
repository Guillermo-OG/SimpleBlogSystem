using Microsoft.AspNetCore.Mvc;
using SimpleBlogSystem.DTOs;
using SimpleBlogSystem.Interfaces;
using SimpleBlogSystem.Models;
using SimpleBlogSystem.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleBlogSystem.Controllers
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly WebSocketNotificationService _notificationService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postService, WebSocketNotificationService notificationService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _notificationService = notificationService; // Ensure this service is injected
            _logger = logger;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();

            // Convertendo entidades para DTOs
            var postsDto = posts.Select(p => new PostDto
            {
                PostId = p.PostId,
                UserId = p.UserId,
                Title = p.Title,
                Content = p.Content,
                CreationDate = p.CreationDate
            });

            return Ok(postsDto);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] PostCreateDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _postService.CreatePost(postDto.UserId, postDto.Title, postDto.Content);

            // Notify clients about the new post
            await _notificationService.NotifyNewPostAsync(createdPost);
            _logger.LogInformation("Notified all connected clients about a new post.");

            return CreatedAtAction("GetPostById", new { id = createdPost.PostId }, createdPost);
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(int id, [FromBody] PostUpdateDto post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != post.PostId)
            {
                return BadRequest();
            }

            var editedPost = await _postService.EditPost(post.PostId, userId, post.Title, post.Content);
            if (editedPost == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
         
            var result = await _postService.DeletePost(id, userIdStr);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}