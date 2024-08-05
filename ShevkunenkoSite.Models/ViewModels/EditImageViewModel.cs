﻿namespace ShevkunenkoSite.Models.ViewModels;

public class EditImageViewModel
{
    public ImageFileModel EditImage { get; set; } = new();

    public List<SelectListItem> ImageDirectories { get; set; } = new(Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\images", "*", SearchOption.AllDirectories)
        .Select(a => new SelectListItem
        {
            Value = a[(Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\images")[0].IndexOf("images") + 7)..].Replace('\\', '/'),
            Text = a[(Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\images")[0].IndexOf("images") + 7)..].Replace('\\', '/')
        })
        .ToList());

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Новый каталог :")]
    public string NewImagePath { get; set; } = string.Empty;

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка :")]
    public IFormFile? ImageFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка HD :")]
    public IFormFile? ImageHDFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Иконка 300 px :")]
    public IFormFile? IconFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Иконка 200 px :")]
    public IFormFile? Icon200FormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Иконка 100 px :")]
    public IFormFile? Icon100FormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка (WebP) :")]
    public IFormFile? WebImageFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка HD (WebP) :")]
    public IFormFile? WebImageHDFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Иконка 300 px (WebP) :")]
    public IFormFile? WebIconFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Иконка 200 px (WebP) :")]
    public IFormFile? WebIcon200FormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Иконка 100 px (WebP) :")]
    public IFormFile? WebIcon100FormFile { get; set; }

    [Display(Name = "Удалить картинку HD")]
    public bool DeleteImageHD { get; set; } = false;

    [Display(Name = "Удалить картинку HD (WebP) :")]
    public bool DeleteWebImageHD { get; set; } = false;

    [Display(Name = "Удалить картинку :")]
    public bool DeleteImage { get; set; } = false;

    [Display(Name = "Удалить иконку 300 px :")]
    public bool DeleteIcon { get; set; } = false;

    [Display(Name = "Удалить иконку 200 px :")]
    public bool DeleteIcon200 { get; set; } = false;

    [Display(Name = "Удалить иконку 100 px :")]
    public bool DeleteIcon100 { get; set; } = false;
}