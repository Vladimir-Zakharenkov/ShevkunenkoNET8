namespace ShevkunenkoSite.Services;

public interface IImageFileRepository
{
    IQueryable<ImageFileModel> ImageFiles { get; }

    Task AddNewImageAsync(ImageFileModel image);

    Task SaveChangesInImageAsync();

    Task DeleteImageAsync(Guid imageId);
}