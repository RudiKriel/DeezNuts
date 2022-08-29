using Microsoft.EntityFrameworkCore;
using Common.DTOs;
using Common.Models;
using DAL.Context;
using DAL.Interfaces;
using DAL.Extentions;
using DAL.Helpers;

namespace DAL.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly ApplicationDbContext _context;

        public LikesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.FindAsync<UserLike>(sourceUserId, likedUserId);
        }

        public async Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(l => l.SourceUserId == likesParams.UserId);
                users = likes.Select(l => l.LikedUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(l => l.LikedUserId == likesParams.UserId);
                users = likes.Select(l => l.SourceUser);
            }

            var likedUsers = users.Select(u => new LikeDTO()
            {
                Id = u.Id,
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                Age = u.DateOfBirth.CalculateAge(),
                PhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = u.City
            });

            return await PagedList<LikeDTO>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<User> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(u => u.LikedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
