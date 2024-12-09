#region Listing 6.10 The contents of the ProductTests.cs file in the SimpleApp.Tests folder

//using SimpleApp.Models;
//using Xunit;

//namespace SimpleApp.Tests
//{
//    public class ProductTests
//    {
//        [Fact]
//        public void CanChangeProductName()
//        {
//            // Arrange
//            var p = new Product { Name = "Test", Price = 100M };
//            // Act
//            p.Name = "New Name";
//            //Assert
//            Assert.Equal("New Name", p.Name);
//        }

//        [Fact]
//        public void CanChangeProductPrice()
//        {
//            // Arrange
//            var p = new Product { Name = "Test", Price = 100M };
//            // Act
//            p.Price = 200M;
//            //Assert
//            Assert.Equal(100M, p.Price);
//        }
//    }
//}

#endregion

#region Listing 6.12 Correcting a test in the ProductTests.cs file in the SimpleApp.Tests folder

using SimpleApp.Models;
using Xunit;

namespace SimpleApp.Tests
{
    public class ProductTests
    {
        [Fact]
        public void CanChangeProductName()
        {
            // Arrange
            var p = new Product { Name = "Test", Price = 100M };
            // Act
            p.Name = "New Name";
            //Assert
            Assert.Equal("New Name", p.Name);
        }

        [Fact]
        public void CanChangeProductPrice()
        {
            // Arrange
            var p = new Product { Name = "Test", Price = 100M };
            // Act
            p.Price = 200M;
            //Assert
            Assert.Equal(200M, p.Price);
        }
    }
}

#endregion

