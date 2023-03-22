namespace Point.Infrastructure.Repositories;

public class AzurePhotoService : IPhotoService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly string _accountName;
    private readonly string _accountKey;

    public AzurePhotoService()
    {
        // TODO: options
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=point2132;AccountKey=lF9wp40L07J0+lZ0xdE1s2kNO6Q0CgbDiEvJmYVQGaclcIATqNfxnOOjEAPn3BFBaUklFcJM3caL+AStSh09gA==;EndpointSuffix=core.windows.net";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerName = "scripts";
        _accountName = "point2132";
        _accountKey = "lF9wp40L07J0+lZ0xdE1s2kNO6Q0CgbDiEvJmYVQGaclcIATqNfxnOOjEAPn3BFBaUklFcJM3caL+AStSh09gA==";
    }

    public async Task<IEnumerable<string>> GetPhotoUrlsAsync(Guid[] ids, CancellationToken ct)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var tasks = new List<Task<string>>();
        foreach (var id in ids)
        {
            var blobClient = containerClient.GetBlobClient($"{id}.jpg");

            if (await blobClient.ExistsAsync(ct))
            {
                tasks.Add(Task.FromResult(GetBlobUrlWithSasToken(blobClient)));
            }
        }

        return await Task.WhenAll(tasks);
    }

    public async Task<string?> GetPhotoUrlByIdAsync(Guid id, CancellationToken ct)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient($"{id}.jpg");

        if (!await blobClient.ExistsAsync(ct))
        {
            return null;
        }

        return GetBlobUrlWithSasToken(blobClient);
    }

    public async Task<Guid> AddPhotoAsync(IFormFile photo, CancellationToken ct)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var photoId = Guid.NewGuid();
        var blobClient = containerClient.GetBlobClient($"{photoId}.jpg");

        await blobClient.UploadAsync(photo.OpenReadStream(), new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = photo.ContentType
            }
        }, ct);

        return photoId;
    }

    public async Task<bool> RemovePhotoAsync(Guid id, CancellationToken ct)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobName = $"{id}.jpg";
        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: ct);

        return response.Value;
    }

    private string GetBlobUrlWithSasToken(BlobClient blobClient)
    {
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddMinutes(10)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasQueryParameters = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_accountName, _accountKey));

        return blobClient.Uri + "?" + sasQueryParameters;
    }
}