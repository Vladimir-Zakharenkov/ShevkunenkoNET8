namespace ShevkunenkoSite.Services;

public interface IBackgroundFotoRepository
{
    IQueryable<BackgroundFileModel> BackgroundFiles { get; }
}