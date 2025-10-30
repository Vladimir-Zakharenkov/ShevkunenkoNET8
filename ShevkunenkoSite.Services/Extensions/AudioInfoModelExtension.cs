namespace ShevkunenkoSite.Services.Extensions;

public static class AudioInfoModelExtension
{
    public static IEnumerable<AudioInfoModel> AudioFileSearch(this IEnumerable<AudioInfoModel> audioInfoModel, string? audioFileSearchString)
    {
        if (audioInfoModel.Any())
        {
            foreach (var foundAudioFile in audioInfoModel)
            {
                if (foundAudioFile.AuthorOfText.Contains((audioFileSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    || foundAudioFile.CaptionOfTextInAudioFile.Contains((audioFileSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    || foundAudioFile.AudioFileDescription.Contains((audioFileSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    || foundAudioFile.AudioFileName.Contains((audioFileSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                    )
                {
                    yield return foundAudioFile;
                }
            }
        }
    }
}