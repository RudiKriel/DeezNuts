using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;

namespace DeezNuts.BLL.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
