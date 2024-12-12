using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Tests;

public class ImageModelTests
{
    [Fact]
    public void CanChangeImageName()
    {
        // Arrange
        var p = new ImageFileModel { ImageCaption = "Test" };

        // Act
        p.ImageCaption = "New Name";

        //Assert
        Assert.Equal("New Name", p.ImageCaption);
    }
}
