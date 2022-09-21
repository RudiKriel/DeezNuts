using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.DTOs;
using Common.Models;
using DAL.Context;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public PhotoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PhotoForApprovalDTO>> GetUnapprovedPhotos()
        {
            return await _context.Photos.Where(p => !p.IsApproved).IgnoreQueryFilters().Select(p => new PhotoForApprovalDTO() {
                Id = p.Id,
                Url = p.Url,
                Username = p.User.UserName,
                IsApproved = p.IsApproved
            }).ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }
    }
}
