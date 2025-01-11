namespace ShevkunenkoSite.Services.Interfaces;

public interface ITextInfoRepository
{
    IQueryable<TextInfoModel> Texts { get; }

    Task AddNewTextAsync(TextInfoModel text);

    Task SaveChangesInTextAsync();

    Task DeleteTextAsync(Guid textId);
}