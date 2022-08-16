using Common.Models;

namespace BLL.Interfaces
{
    public interface ITokenManager
    {
        string CreateToken(User user);
    }
}