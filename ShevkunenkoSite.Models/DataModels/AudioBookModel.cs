namespace ShevkunenkoSite.Models.DataModels;

public class AudioBookModel
{
    #region Guid аудиокниги

    [Key]
    [Display(Name = "Идентификатор аудиокниги :")]
    [Column("AudioBookId")]
    public Guid AudioBookModelId { get; set; } = Guid.Empty;

    #endregion

    #region Название аудиокниги

    [DataType(DataType.Text)]
    [Display(Name = "Заголовок :")]
    public string CaptionOfAudioBook { get; set; } = string.Empty;

    #endregion

    #region Описание аудиокниги

    [Required(ErrorMessage = "Добавте описание")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание аудиокниги :")]
    public string AudioBookDescription { get; set; } = string.Empty;

    #endregion

    #region Исполнитель текста

    [Required(ErrorMessage = "Введите имя исполнителя", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Имя исполнителя :")]
    public string ActorOfAudioBook { get; set; } = string.Empty;

    #endregion

    #region Количество файлов (глав)

    [Display(Name = "Количество файлов (глав) :")]
    public int NumberOfFiles { get; set; } = 1;

    #endregion

    #region Книга связанная с аудиокнигой

    [Display(Name = "Книга для аудиокниги :")]
    public Guid? BookForAudioBookId { get; set; }
    public BooksAndArticlesModel? BookForAudioBook { get; set; }

    #endregion

    #region Страница аудиокниги (первый файл)

    public Guid? PageInfoModelId { get; set; }
    public PageInfoModel? PageInfoModel { get; set; }

    #endregion
}
