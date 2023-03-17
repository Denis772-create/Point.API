namespace Point.Application.Interfaces;

public interface IPhotoService
{
    Task<IEnumerable<string>> GetPhotoUrlsAsync(Guid[] ids);
    Task<string> GetPhotoUrlByIdAsync(Guid id);
    Task<Guid> AddPhotoAsync(IFormFile photo);
    Task<bool> RemovePhotoAsync(Guid id);
}