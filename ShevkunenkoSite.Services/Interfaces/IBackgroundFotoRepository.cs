using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public interface IBackgroundFotoRepository
{
    IQueryable<BackgroundFileModel> BackgroundFiles { get; }
}