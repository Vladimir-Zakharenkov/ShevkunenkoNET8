namespace ShevkunenkoSite.Services.Extensions;

public static class AudioBookModelExtension
{
    public static IEnumerable<AudioBookModel> AudioBookSearch(this IEnumerable<AudioBookModel> audioBookModel, string? audioBookSearchString)
    {
        if (audioBookModel.Any())
        {
            foreach (var foundAudioBook in audioBookModel)
            {
                if (foundAudioBook.CaptionOfAudioBook.Contains((audioBookSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    || foundAudioBook.AudioBookDescription.Contains((audioBookSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    || foundAudioBook.ActorOfAudioBook.Contains((audioBookSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    )
                {
                    yield return foundAudioBook;
                }
            }
        }
    }
}