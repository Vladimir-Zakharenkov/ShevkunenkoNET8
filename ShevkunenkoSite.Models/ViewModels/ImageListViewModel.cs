﻿namespace ShevkunenkoSite.Models.ViewModels;

public class ImageListViewModel
{
    public ImageFileModel[] AllImages { get; set; } = Array.Empty<ImageFileModel>();

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    public string? ImageSearchString { get; set; }

    public bool IconList { get; set; } = false;

    public Guid? CurrentImageId { get; set; }
}