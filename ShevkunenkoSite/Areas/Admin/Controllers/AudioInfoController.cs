using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NAudio.Wave;

namespace ShevkunenkoSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AudioInfoController(
        IAudioInfoRepository audioFileContext,
        IAudioBookRepository audioBookContext,
        IPageInfoRepository pagesOfSiteContext,
        ITextInfoRepository textsOfSiteContext,
        IWebHostEnvironment hostEnvironment) : Controller
    {
        private readonly string rootPath = hostEnvironment.WebRootPath;

        #region Список аудиофайлов
        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1)
        {
            var allAudioFiles = await audioFileContext.AudioFiles.ToListAsync();

            if (!searchString.IsNullOrEmpty())
            {
                allAudioFiles = [.. allAudioFiles.AudioFileSearch(searchString).OrderBy(audioBook => audioBook.CaptionOfTextInAudioFile)];
            }

            return View(new ItemsListViewModel
            {
                AllAudioFiles = [.. allAudioFiles
                     .Skip((pageNumber - 1) * DataConfig.NumberOfItemsPerPage)
                     .Take(DataConfig.NumberOfItemsPerPage)],

                CurrentPage = pageNumber,
                ItemsPerPage = DataConfig.NumberOfItemsPerPage,
                TotalItems = allAudioFiles.Count(),

                SearchString = searchString ?? string.Empty
            });
        }

        #endregion

        #region Информация об аудиофайле

        public async Task<IActionResult> DetailsAudioFile(Guid? audioFileId)
        {
            if (audioFileId.HasValue &
                await audioFileContext.AudioFiles
                    .Where(audioFile => audioFile.AudioInfoModelId == audioFileId)
                    .AnyAsync())
            {
                var audioInfoModel = await audioFileContext.AudioFiles
                    .Include(a => a.AudioBookModel)
                        .ThenInclude(b => b!.BookForAudioBook)
                            .ThenInclude(p => p!.PageInfoModel)
                                .ThenInclude(i => i!.ImageFileModel)
                    .Include(a => a.PageInfoModel)
                    .Include(a => a.TextInfoModel)
                        .ThenInclude(b => b!.BooksAndArticlesModel)
                    .FirstAsync(audioFile => audioFile.AudioInfoModelId == audioFileId);

                if (audioInfoModel.TextInfoModel != null)
                {
                    using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + audioInfoModel.TextInfoModel.FolderForText + audioInfoModel.TextInfoModel.TxtFileName);

                    audioInfoModel.ClearText = clearText.ReadToEnd();
                }

                return View(audioInfoModel);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Добавить аудиофайл

        [HttpGet]
        public IActionResult AddAudioFile()
        {
            // Список аудиокниг
            ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

            // Список страниц сайта
            ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

            // Список текстовых файлов
            ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

            // Список папок для аудиофайлов
            ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAudioFile(
            [Bind("ChooseAudioFile," +
                    "FolderForAudioFile," +
                    "AuthorOfText," +
                    "CaptionOfTextInAudioFile," +
                    "TextInfoModelId" +
                    ",AudioFileDescription," +
                    "InternetRefToAudioFile," +
                    "AudioFileUploadDate," +
                    "AudioBookModelId," +
                    "SequenceNumber," +
                    "PageInfoModelId")]
            AudioInfoViewModel audioFileForAdding)
        {
            if (ModelState.IsValid)
            {
                #region Автор текста

                _ = audioFileForAdding.AuthorOfText.Trim();

                #endregion

                #region Название текста аудиофайла

                _ = audioFileForAdding.CaptionOfTextInAudioFile.Trim();

                #endregion

                #region Папка аудиофайла

                _ = audioFileForAdding.FolderForAudioFile.Trim();

                #endregion

                #region Transcript - GUID файла с текстом

                _ = audioFileForAdding.TextInfoModelId;

                #endregion

                #region Аудиокнига

                _ = audioFileForAdding.AudioBookModelId;

                #endregion

                #region Номер по порядку - Значение для сортировки

                _ = audioFileForAdding.SequenceNumber;

                audioFileForAdding.SortOfAudioFile = audioFileForAdding.SequenceNumber ?? 0;

                #endregion

                #region Автоматическое определение значений

                #region Продолжительность (время воспроизведения)

                string audioFile = string.Empty;

                //if (audioFileForAdding.ChooseAudioFile != null)
                //{
                //    var tempFile = Path.GetTempFileName();

                //    using FileStream stream = new(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Int16.MaxValue, FileOptions.DeleteOnClose);

                //    await audioFileForAdding.ChooseAudioFile.CopyToAsync(stream);

                //    Mp3FileReader mp3File = new(tempFile);

                //    audioFileForAdding.AudioFileDuration = mp3File.TotalTime;
                //}

                if (audioFileForAdding.ChooseAudioFile != null)
                {
                    audioFile = Path.Combine(audioFileForAdding.FolderForAudioFile, audioFileForAdding.ChooseAudioFile.FileName);

                    Mp3FileReader mp3File = new(audioFile);

                    audioFileForAdding.AudioFileDuration = mp3File.TotalTime;
                }

                #endregion

                IReadOnlyList<MetadataExtractor.Directory> audioDirectories = ImageMetadataReader.ReadMetadata(audioFile);

                foreach (var audioDirectory in audioDirectories)
                {
                    foreach (var tag in audioDirectory.Tags)
                    {
                        #region Битрейт аудиофайла

                        if (audioDirectory.Name == "MP3" && tag.Name == "Bitrate")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("audioFileForAdding.AudioFileBitRate", "Битрейт аудиофайла равен 0");

                                // Список аудиокниг
                                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                                // Список страниц сайта
                                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                                // Список текстовых файлов
                                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                                // Список папок для аудиофайлов
                                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                                return View(audioFileForAdding);
                            }
                            else
                            {
                                audioFileForAdding.AudioFileBitRate = int.Parse(tag.Description);
                            }
                        }

                        #endregion

                        #region Частота аудиофайла

                        if (audioDirectory.Name == "MP3" && tag.Name == "Frequency")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("audioFileForAdding.AudioFileFrequency", "Частота аудиофайла равен 0");

                                // Список аудиокниг
                                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                                // Список страниц сайта
                                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                                // Список текстовых файлов
                                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                                // Список папок для аудиофайлов
                                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                                return View(audioFileForAdding);
                            }
                            else
                            {
                                audioFileForAdding.AudioFileFrequency = int.Parse(tag.Description);
                            }
                        }

                        #endregion

                        #region Размер аудиофайла

                        if (audioDirectory.Name == "File" && tag.Name == "File Size")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("audioFileForAdding.AudioFileFrequency", "Размер аудиофайла равен 0");

                                // Список аудиокниг
                                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                                // Список страниц сайта
                                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                                // Список текстовых файлов
                                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                                // Список папок для аудиофайлов
                                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                                return View(audioFileForAdding);
                            }
                            else
                            {
                                audioFileForAdding.AudioFileSize = int.Parse(tag.Description[..tag.Description.IndexOf(' ')]);
                            }
                        }

                        #endregion

                        #region MIME Type аудиофайла

                        if (audioDirectory.Name == "File Type" && tag.Name == "Detected MIME Type")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("audioFileForAdding.AudioFileMimeType", "MIME Type не определен");

                                // Список аудиокниг
                                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                                // Список страниц сайта
                                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                                // Список текстовых файлов
                                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                                // Список папок для аудиофайлов
                                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                                return View(audioFileForAdding);
                            }
                            else
                            {
                                audioFileForAdding.AudioFileMimeType = tag.Description;
                            }
                        }

                        #endregion

                        #region Расширение аудиофайла

                        if (audioDirectory.Name == "File Type" && tag.Name == "Expected File Name Extension")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("audioFileForAdding.AudioFileType", "Расширение файла не определено");

                                // Список аудиокниг
                                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                                // Список страниц сайта
                                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                                // Список текстовых файлов
                                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                                // Список папок для аудиофайлов
                                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                                return View(audioFileForAdding);
                            }
                            else
                            {
                                audioFileForAdding.AudioFileType = tag.Description;
                            }
                        }

                        #endregion

                        #region Имя аудиофайла

                        if (audioDirectory.Name == "File" && tag.Name == "File Name")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("audioFileForAdding.AudioFileName", "Имя файла не определено");

                                // Список аудиокниг
                                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                                // Список страниц сайта
                                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                                // Список текстовых файлов
                                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                                // Список папок для аудиофайлов
                                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                                return View(audioFileForAdding);
                            }
                            else
                            {
                                audioFileForAdding.AudioFileName = tag.Description;
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                #region Добавить в БД

                await audioFileContext.AddAudioFileAsync(audioFileForAdding);

                #endregion

                #region Открытие параметров добавленного аудиофайла

                var newAudioFile = await audioFileContext
                    .AudioFiles
                    .FirstAsync(audioFile => audioFile.AudioFileName == audioFileForAdding.AudioFileName);

                return RedirectToAction(nameof(DetailsAudioFile), new { audioFileId = newAudioFile.AudioInfoModelId });

                #endregion
            }
            else
            {
                // Список аудиокниг
                ViewData["AudioBooks"] = new SelectList(audioBookContext.AudioBooks, "AudioBookModelId", "CaptionOfAudioBook");

                // Список страниц сайта
                ViewData["PagesOfSite"] = new SelectList(pagesOfSiteContext.PagesInfo.OrderBy(page => page.PageTitle), "PageInfoModelId", "PageTitle");

                // Список текстовых файлов
                ViewData["TextFIles"] = new SelectList(textsOfSiteContext.Texts, "TextInfoModelId", "TxtFileName");

                // Список папок для аудиофайлов
                ViewData["AudioFileFolders"] = new SelectList(System.IO.Directory.GetDirectories(DataConfig.AudioFoldersPath, "*", SearchOption.AllDirectories));

                return View(audioFileForAdding);
            }
        }

        #endregion

        #region Временный код

        //// GET: Admin/AudioInfoModels/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var audioInfoModel = await _context.AudioInfoModel.FindAsync(id);
        //    if (audioInfoModel == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AudioBookModelId"] = new SelectList(_context.AudioBookModel, "AudioBookModelId", "ActorOfAudioBook", audioInfoModel.AudioBookModelId);
        //    ViewData["PageInfoModelId"] = new SelectList(_context.PageInfo, "PageInfoModelId", "Action", audioInfoModel.PageInfoModelId);
        //    ViewData["TextInfoModelId"] = new SelectList(_context.TextFile, "TextInfoModelId", "FolderForText", audioInfoModel.TextInfoModelId);
        //    return View(audioInfoModel);
        //}

        //// POST: Admin/AudioInfoModels/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("AudioInfoModelId,AuthorOfText,CaptionOfTextInAudioFile,TextInfoModelId,AudioFileDescription,AudioFileDuration,AudioFileBitRate,AudioFileFrequency,AudioFileSize,AudioFileMimeType,AudioFileType,AudioFileName,AudioFilePlaybackType,SortOfAudioFile,InternetRefToAudioFile,AudioFileUploadDate,AudioBookModelId,SequenceNumber,PageInfoModelId")] AudioInfoModel audioInfoModel)
        //{
        //    if (id != audioInfoModel.AudioInfoModelId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(audioInfoModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AudioInfoModelExists(audioInfoModel.AudioInfoModelId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AudioBookModelId"] = new SelectList(_context.AudioBookModel, "AudioBookModelId", "ActorOfAudioBook", audioInfoModel.AudioBookModelId);
        //    ViewData["PageInfoModelId"] = new SelectList(_context.PageInfo, "PageInfoModelId", "Action", audioInfoModel.PageInfoModelId);
        //    ViewData["TextInfoModelId"] = new SelectList(_context.TextFile, "TextInfoModelId", "FolderForText", audioInfoModel.TextInfoModelId);
        //    return View(audioInfoModel);
        //}

        //// GET: Admin/AudioInfoModels/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var audioInfoModel = await _context.AudioInfoModel
        //        .Include(a => a.AudioBookModel)
        //        .Include(a => a.PageInfoModel)
        //        .Include(a => a.TextInfoModel)
        //        .FirstOrDefaultAsync(m => m.AudioInfoModelId == id);
        //    if (audioInfoModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(audioInfoModel);
        //}

        //// POST: Admin/AudioInfoModels/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var audioInfoModel = await _context.AudioInfoModel.FindAsync(id);
        //    if (audioInfoModel != null)
        //    {
        //        _context.AudioInfoModel.Remove(audioInfoModel);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AudioInfoModelExists(Guid id)
        //{
        //    return _context.AudioInfoModel.Any(e => e.AudioInfoModelId == id);
        //}

        #endregion
    }
}
