using Common.Models;

namespace DeezNuts.BLL.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}