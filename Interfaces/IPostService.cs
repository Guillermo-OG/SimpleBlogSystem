using SimpleBlogSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogSystem.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePost(string userId, string title, string content);
        Task<Post> EditPost(int postId, string userId, string title, string content);
        Task<bool> DeletePost(int postId, string userId);
        Task<Post> GetPostById(int postId);
        Task<IEnumerable<Post>> GetAllPosts();
    }
}
