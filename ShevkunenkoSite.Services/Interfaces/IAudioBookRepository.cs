namespace ShevkunenkoSite.Services.Interfaces

{
    public interface IAudioBookRepository
    {
        IQueryable<AudioBookModel> AudioBooks { get; }

        Task AddAudioBookAsync(AudioBookModel audioBook);

        Task SaveChangesInAudioBookAsync();

        Task DeleteAudioBookAsync(Guid audioBookId);
    }
}
