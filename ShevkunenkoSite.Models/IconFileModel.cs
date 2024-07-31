namespace ShevkunenkoSite.Models;

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
    public string IconFileNameExtension { get; set; } = "png";

    [DataType(DataType.Text)]
    [Display(Name = "MimeType файла иконки :")]
    public string IconMimeType { get; set; } = "image/png";

    [DataType(DataType.Text)]
    [Display(Name = "Размер файла иконки :")]
    public uint IconFileSize { get; set; } = 1;

    [Display(Name = "Ширина иконки в px :")]
    public uint IconWidth { get; set; } = 1;

    [Display(Name = "Высота картинки в px :")]
    public uint IconHeight { get; set; } = 1;

    [Display(Name = "Параметр rel в метатеге link :")]
    public string IconRel { get; set; } = string.Empty;

    [Display(Name = "Параметр type в метатеге link :")]
    public string IconType { get; set; } = string.Empty;
}