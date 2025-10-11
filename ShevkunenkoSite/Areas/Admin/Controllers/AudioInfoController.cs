using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using NAudio.Wave;

namespace ShevkunenkoSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AudioInfoController(
        IAudioInfoRepository audioFileContext,
        IAudioBookRepository audioBookContext,
        IPageInfoRepository pagesOfSiteContext,
        ITextInfoRepository textsOfSiteContext) : Controller
    {
        // Список аудиофайлов
        public async Task<IActionResult> Index()
        {
            var audioFiles = audioFileContext
                .AudioFiles
                .Include(a => a.AudioBookModel)
                .Include(a => a.PageInfoModel)
                .Include(a => a.TextInfoModel);

            return View(await audioFiles.ToListAsync());
        }

        #region Информация об аудиофайле

        public async Task<IActionResult> DetailsAudioFile(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audioInfoModel = await audioFileContext.AudioFiles
                .Include(a => a.AudioBookModel)
                .Include(a => a.PageInfoModel)
                .Include(a => a.TextInfoModel)
                .FirstOrDefaultAsync(m => m.AudioInfoModelId == id);
            if (audioInfoModel == null)
            {
                return NotFound();
            }

            return View(audioInfoModel);
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

            return View();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAudioFile(
            [Bind("ChooseAudioFile," +
            "AudioInfoModelId," +
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
            if (!ModelState.IsValid)
            {
                #region Автор текста

                _ = audioFileForAdding.AuthorOfText.Trim();

                #endregion

                #region Название текста аудиофайла

                _ = audioFileForAdding.CaptionOfTextInAudioFile.Trim();

                #endregion

                #region Transcript - GUID файла с текстом

                _ = audioFileForAdding.TextInfoModelId;

                #endregion

                #region Продолжительность (время воспроизведения)

                if (audioFileForAdding.ChooseAudioFile != null)
                {
                    var tempFile = Path.GetTempFileName();

                    using FileStream stream = new(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None, Int16.MaxValue, FileOptions.DeleteOnClose);

                    await audioFileForAdding.ChooseAudioFile.CopyToAsync(stream);

                    Mp3FileReader mp3File = new(tempFile);

                    audioFileForAdding.AudioFileDuration = mp3File.TotalTime;
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

                return View(audioFileForAdding);
            }
        }


        #endregion

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
    }
}
