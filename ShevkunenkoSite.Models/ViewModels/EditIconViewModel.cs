namespace ShevkunenkoSite.Models.ViewModels;

public class EditIconViewModel
{
    public IconFileModel EditIcon { get; set; } = new();

    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать иконку :")]
    public IFormFile? IconFormFile { get; set; }
}