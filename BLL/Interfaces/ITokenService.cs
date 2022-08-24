using Common.Models;

namespace BLL.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}