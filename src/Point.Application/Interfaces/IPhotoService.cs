namespace Point.Application.Interfaces;

public interface IPhotoService
{
    Task<IEnumerable<string>> GetPhotoUrlsAsync(Guid[] ids, CancellationToken ct);
    Task<string?> GetPhotoUrlByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> AddPhotoAsync(IFormFile photo, CancellationToken ct);
    Task<bool> RemovePhotoAsync(Guid id, CancellationToken ct);
}