using CloudinaryDotNet.Actions;

namespace API.ServiceContracts;

public interface IPhotoService
{
    //CRUD on photos
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}
