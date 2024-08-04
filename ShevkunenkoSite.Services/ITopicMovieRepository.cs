﻿namespace ShevkunenkoSite.Services;

public interface ITopicMovieRepository
{
    IQueryable<TopicMovieModel> TopicMovies { get; }

    Task AddNewTopicMovieAsync(TopicMovieModel topic);

    Task SaveChangesInTopicMovieAsync();

    Task DeleteTopicMovieAsync(Guid topicId);
}