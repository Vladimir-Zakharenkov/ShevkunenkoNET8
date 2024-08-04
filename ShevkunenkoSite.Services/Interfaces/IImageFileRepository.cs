using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public interface IImageFileRepository
{
    IQueryable<ImageFileModel> ImageFiles { get; }

    Task AddNewImageAsync(ImageFileModel image);

    Task SaveChangesInImageAsync();

    Task DeleteImageAsync(Guid imageId);
}