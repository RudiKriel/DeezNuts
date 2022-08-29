using Common.DTOs;
using Common.Models;
using DAL.Helpers;

namespace DAL.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<User> GetUserWithLikes(int userId);
        Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}
