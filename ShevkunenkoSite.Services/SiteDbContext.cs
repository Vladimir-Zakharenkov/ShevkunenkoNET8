﻿namespace ShevkunenkoSite.Services;

public class SiteDbContext(DbContextOptions<SiteDbContext> options) : DbContext(options)
{
    public DbSet<PageInfoModel> PageInfo => Set<PageInfoModel>();

    public DbSet<BackgroundFileModel> BackgroundFile => Set<BackgroundFileModel>();

    public DbSet<ImageFileModel> ImageFile => Set<ImageFileModel>();

    public DbSet<IconFileModel> IconFile => Set<IconFileModel>();

    public DbSet<MovieFileModel> MovieFile => Set<MovieFileModel>();

    public DbSet<AccessModel> Access => Set<AccessModel>();

    public DbSet<TopicMovieModel> TopicMovie => Set<TopicMovieModel>();

    public DbSet<TextInfoModel> TextFile => Set<TextInfoModel>();

    public DbSet<BooksAndArticlesModel> BooksAndArticles => Set<BooksAndArticlesModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PageInfoModel>()
            .Property(p => p.PageFullPath)
            .HasComputedColumnSql("[PageArea] + [Controller] + [PageLoc]");

        modelBuilder.Entity<PageInfoModel>()
            .Property(p => p.PageFullPathWithData)
            .HasComputedColumnSql("[PageArea] + [Controller] + [PageLoc] + [RoutData]");

        modelBuilder.Entity<PageInfoModel>()
            .Property(p => p.PagePathNickNameWithData)
            .HasComputedColumnSql("[PagePathNickName] + [RoutData]");

        modelBuilder.Entity<MovieFileModel>()
            .HasOne(o => o.PageForMovieSeries)
            .WithMany()
            .HasForeignKey(o => o.PageForMovieSeriesId);
    }
}