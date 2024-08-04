using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public interface IMovieFileRepository
{
    IQueryable<MovieFileModel> MovieFiles { get; }

    Task AddNewMovieAsync(MovieFileModel movie);

    Task SaveChangesInMovieAsync();

    Task DeleteMovieAsync(Guid movieId);
}
