using ShevkunenkoSite.Models;

namespace ShevkunenkoSite.Services.Interfaces;

public class ImageFileImplementation(SiteDbContext siteContext) : IImageFileRepository
{
    private readonly SiteDbContext _siteContext = siteContext;

    #region Все файлы картинок в БД

    public IQueryable<ImageFileModel> ImageFiles => _siteContext.ImageFile;

    #endregion

    #region Сохранить данные картинки в БД

    public async Task SaveChangesInImageAsync() => await _siteContext.SaveChangesAsync();

    #endregion

    #region Добавить новую картинку в БД

    public async Task AddNewImageAsync(ImageFileModel image)
    {
        await _siteContext.ImageFile.AddAsync(image);
        await SaveChangesInImageAsync();
    }

    #endregion

    #region  Удалить картинку из БД

    public async Task DeleteImageAsync(Guid imageId)
    {
        if (await _siteContext.ImageFile.Where(i => i.ImageFileModelId == imageId).AnyAsync())
        {
            ImageFileModel imageToDelete = await _siteContext.ImageFile.FirstAsync(i => i.ImageFileModelId == imageId);

            _ = _siteContext.ImageFile.Remove(imageToDelete);
            _ = await _siteContext.SaveChangesAsync();
        }
    }

    #endregion

    #region Получить картинку из БД по GUID

    public ImageFileModel GetImageByGuidOrFileNameAsync(string imageObject)
    {
        // Поиск картинки по GUID
        if (Guid.TryParse(imageObject, out Guid imageIdGuid) & _siteContext.ImageFile.Where(img => img.ImageFileModelId == imageIdGuid).Any())
        {
            return _siteContext.ImageFile.First(img => img.ImageFileModelId == imageIdGuid);
        }
        // Поиск картинки по названию файла
        else if (_siteContext.ImageFile.Where(img => img.WebImageFileName == imageObject || img.ImageFileName == imageObject).Any())
        {
            return _siteContext.ImageFile.First(img => img.WebImageFileName == imageObject || img.ImageFileName == imageObject);
        }
        // Если ничего не найдено, выводим картинку NoImage
        else
        {
            return _siteContext.ImageFile.First(img => img.ImageFileModelId == Guid.Parse(DataConfig.NoImage));
        }
    }

    #endregion
}