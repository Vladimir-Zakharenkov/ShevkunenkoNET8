using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces
{
    public interface IIconFileRepository
    {
        IQueryable<IconFileModel> IconFiles { get; }

        Task SaveChangesInIconAsync();

        Task AddNewIconAsync(IconFileModel icon);
    }
}