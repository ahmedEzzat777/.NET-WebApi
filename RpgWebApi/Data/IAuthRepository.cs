using System.Threading.Tasks;
using RpgWebApi.Models;

namespace RpgWebApi.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
