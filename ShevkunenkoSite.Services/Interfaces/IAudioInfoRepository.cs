namespace ShevkunenkoSite.Services.Interfaces;

public interface IAudioInfoRepository
{
    IQueryable<AudioInfoModel> AudioFilles { get; }

    Task AddAudioFileAsync(AudioInfoModel audioFile);

    Task SaveChangesInAudioFileAsync();

    Task DeleteAudioFileAsync(Guid audioFileId);
}
