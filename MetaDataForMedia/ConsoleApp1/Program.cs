using MetadataExtractor;

var directories = ImageMetadataReader.ReadMetadata(@"F:\TEMP\glava01.mp3");

foreach (var directory in directories)
{
    foreach (var tag in directory.Tags)
    {
        Console.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
    }
}