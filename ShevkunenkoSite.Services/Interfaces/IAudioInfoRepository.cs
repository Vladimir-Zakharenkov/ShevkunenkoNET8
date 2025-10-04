namespace ShevkunenkoSite.Services.Interfaces;

public interface IAudioInfoRepository
{
    IQueryable<AudioInfoModel> AudioFiles { get; }

    Task AddAudioFileAsync(AudioInfoModel audioFile);

    Task SaveChangesInAudioFileAsync();

    Task DeleteAudioFileAsync(Guid audioFileId);
}
