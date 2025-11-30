namespace MyFirstEfCoreApp;

public class Book
{
    public int BookId { get; set; } //#A

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; }
    public int AuthorId { get; set; } //#B


    public required Author Author { get; set; } //#C
}
