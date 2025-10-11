namespace ShevkunenkoSite.Models.ViewModels;

public class AudioInfoViewModel : AudioInfoModel
{
    [Required(ErrorMessage = "Выберите аудиофайл")]
    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать аудиофайл:")]
    public IFormFile? ChooseAudioFile { get; set; }
}