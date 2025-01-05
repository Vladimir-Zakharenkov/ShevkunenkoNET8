namespace MyFirstEfCoreApp8;

public class Book
{
    public int BookId { get; set; }

    public string Title { get; set; }=string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}