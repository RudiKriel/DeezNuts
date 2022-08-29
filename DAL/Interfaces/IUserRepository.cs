using Common.DTOs;
using Common.Models;
using DAL.Helpers;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string name);
        Task<MemberDTO> GetMemberAsync(string username);
        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
    }
}
