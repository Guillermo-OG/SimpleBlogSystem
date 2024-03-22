using SimpleBlogSystem.Interfaces;
using SimpleBlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleBlogSystem.Data;

namespace SimpleBlogSystem.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _context;

        public PostService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePost(string userId, string title, string content)
        {
            var post = new Post
            {
                UserId = userId,
                Title = title,
                Content = content,
                CreationDate = DateTime.Now
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> EditPost(int postId, string userId, string title, string content)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId && p.UserId == userId);
            if (post == null)
            {
                return null;
            }

            post.Title = title;
            post.Content = content;

            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePost(int postId, string userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId && p.UserId == userId);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostById(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            return post;
        }
    }
}
