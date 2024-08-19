namespace ShevkunenkoSite.Models.DataModels;

public class IconFileModel
{
    [Key]
    [Display(Name = "Идентификатор иконки :")]
    [Column("IconFileId")]
    public Guid IconFileModelId { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Каталог иконки :")]
    public string IconPath { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    [Display(Name = "Файл иконки :")]
    public string IconFileName { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    [Display(Name = "Расширение файла иконки :")]
    [Length(2, 5, ErrorMessage = "Расширение от 2 до 5 символов")]
    public string IconFileNameExtension { get; set; } = "png";

    [DataType(DataType.Text)]
    [Display(Name = "MimeType файла иконки :")]
    [Length(6, 11, ErrorMessage = "MimeType от 6 до 11 символов")]
    public string IconMimeType { get; set; } = "image/png";

    [DataType(DataType.Text)]
    [Display(Name = "Размер файла иконки :")]
    [Range(1, 100000, ErrorMessage = "Размер файла от 1 до 100 000 байт")]
    public int IconFileSize { get; set; } = 1;

    [DataType(DataType.Text)]
    [Display(Name = "Ширина иконки в px :")]
    [Range(1, 512, ErrorMessage = "Ширина иконки от 1 до 512 px")]
    public int IconWidth { get; set; } = 1;

    [DataType(DataType.Text)]
    [Display(Name = "Высота иконки в px :")]
    [Range(1, 512, ErrorMessage = "Высота иконки от 1 до 512 px")]
    public int IconHeight { get; set; } = 1;

    [DataType(DataType.Text)]
    [Display(Name = "Параметр rel в метатеге link :")]
    public string IconRel { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    [Display(Name = "Параметр type в метатеге link :")]
    public string IconType { get; set; } = string.Empty;
}