using Common.Models;

namespace DeezNuts.BLL.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}