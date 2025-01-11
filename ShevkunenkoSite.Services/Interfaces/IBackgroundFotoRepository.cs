namespace ShevkunenkoSite.Services.Interfaces;

public interface IBackgroundFotoRepository
{
    IQueryable<BackgroundFileModel> BackgroundFiles { get; }
}