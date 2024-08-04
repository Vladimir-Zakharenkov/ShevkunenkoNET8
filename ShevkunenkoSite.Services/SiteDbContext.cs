using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services;

public class SiteDbContext : DbContext
{
    public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options) { }

    public DbSet<PageInfoModel> PageInfo => Set<PageInfoModel>();

    public DbSet<BackgroundFileModel> BackgroundFile => Set<BackgroundFileModel>();

    public DbSet<ImageFileModel> ImageFile => Set<ImageFileModel>();

    public DbSet<IconFileModel> IconFile => Set<IconFileModel>();

    public DbSet<MovieFileModel> MovieFile => Set<MovieFileModel>();

    public DbSet<AccessModel> Access => Set<AccessModel>();

    public DbSet<TopicMovieModel> TopicMovie => Set<TopicMovieModel>();

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

        //modelBuilder.Entity<MovieFileModel>()
        //    .Property(p => p.PageInfoModelIdForSeries)
        //    .HasDefaultValue(null);

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebImageFileNameExtension)
        //    .HasDefaultValue("webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebImageMimeType)
        //    .HasDefaultValue("image/webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebIconFileNameExtension)
        //    .HasDefaultValue("webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebIconMimeType)
        //    .HasDefaultValue("image/webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebIcon200FileNameExtension)
        //    .HasDefaultValue("webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebIcon200MimeType)
        //    .HasDefaultValue("image/webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebIcon100FileNameExtension)
        //    .HasDefaultValue("webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebIcon100MimeType)
        //    .HasDefaultValue("image/webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebImageHDNameExtension)
        //    .HasDefaultValue("webp");

        //modelBuilder.Entity<ImageFileModel>()
        //    .Property(p => p.WebImageHDMimeType)
        //    .HasDefaultValue("image/webp");

        //modelBuilder.Entity<PageInfoModel>()
        //    .Property(p => p.PageLinks)
        //    .HasDefaultValue(false);

        //modelBuilder.Entity<PageInfoModel>()
        //    .Property(p => p.RefPages)
        //    .HasDefaultValue(string.Empty);

        //modelBuilder.Entity<MovieFileModel>()
        //   .Property(p => p.FramesAroundMovie)
        //   .HasDefaultValue(string.Empty);

        //modelBuilder.Entity<MovieFileModel>()
        //   .Property(p => p.SeriesSearchFilter)
        //   .HasDefaultValue(string.Empty);

        //modelBuilder.Entity<PageInfoModel>()
        //   .Property(p => p.PageFilter)
        //   .HasDefaultValue(string.Empty);

        //modelBuilder.Entity<PageInfoModel>()
        //  .Property(p => p.PageFilterOut)
        //  .HasDefaultValue(string.Empty);
    }
}