using SimpleBlogSystem.Models;
using System.Threading.Tasks;

namespace SimpleBlogSystem.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
