﻿// Ignore Spelling: Msapplication

namespace ShevkunenkoSite.Models;

public class DataConfig
{
    public static string ImageForMsapplication { get; set; } = null!;

    public static string IconItem { get; set; } = null!;

    public static string IconsFolder { get; set; } = null!;

    public static string BrowserconfigIconsFolder { get; set; } = null!;

    public static string BrowserconfigPath { get; set; } = null!;

    public static string ManifestPath { get; set; } = null!;

    public static string Test { get; set; } = string.Empty;


    // GUID картинки NoImage
    public static string NoImage { get; set; } = null!;

    // каталог с каталогами иконок для страниц
    public static string IconFoldersPath { get; set; } = null!;

    // каталог с файлами фильмов
    public static string MovieFoldersPath { get; set; } = null!;

    // каталог с файлами текстов
    public static string TextsFolderPath { get; set; } = null!;

    // каталог для архива файлов текстов
    public static string ArchiveTextsFolderPath { get; set; } = null!;

    // колличество видео на странице для постраничного представления
    public static int NumberOfVideoPerPage { get; set; }
}