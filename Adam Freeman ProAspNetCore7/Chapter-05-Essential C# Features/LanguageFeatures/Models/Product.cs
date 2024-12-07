#region Listing 5.3 The contents of the Product.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class Product
//    {
//        public string Name { get; set; }
//        public decimal? Price { get; set; }

//        public static Product[] GetProducts()
//        {
//            Product kayak = new Product
//            {
//                Name = "Kayak",
//                Price = 275M
//            };
//            Product lifejacket = new Product
//            {
//                Name = "Lifejacket",
//                Price = 48.95M
//            };

//            return new Product[] { kayak, lifejacket, null };
//        }
//    }
//}

#endregion

#region Listing 5.11 Using the required keyword in the Product.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class Product
//    {
//        public required string Name { get; set; }
//        public decimal? Price { get; set; }
//        public static Product[] GetProducts()
//        {
//            Product kayak = new Product
//            {
//                Name = "Kayak",
//                Price = 275M
//            };
//            Product lifejacket = new Product
//            {
//                Name = "Lifejacket",
//                Price = 48.95M
//            };

//            return new Product[] { kayak, lifejacket, null };
//        }
//    }
//}

#endregion

#region Listing 5.12 Omitting a value in the Product.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class Product
//    {
//        public required string Name { get; set; }
//        public decimal? Price { get; set; }
//        public static Product[] GetProducts()
//        {
//            Product kayak = new Product
//            {
//                Name = "Kayak",
//                Price = 275M
//            };
//            Product lifejacket = new Product
//            {
//                //Name = "Lifejacket",
//                Price = 48.95M
//            };
//            return new Product[] { kayak, lifejacket, null };
//        }
//    }
//}

#endregion

#region Listing 5.13 Providing a default value in the Product.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class Product
//    {
//        public string Name { get; set; } = string.Empty;
//        public decimal? Price { get; set; }
//        public static Product[] GetProducts()
//        {
//            Product kayak = new Product
//            {
//                Name = "Kayak",
//                Price = 275M
//            };
//            Product lifejacket = new Product
//            {
//                //Name = "Lifejacket",
//                Price = 48.95M
//            };

//            return new Product[] { kayak, lifejacket, null };
//        }
//    }
//}

#endregion

#region Listing 5.14 Using a nullable type in the Product.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class Product
//    {
//        public string Name { get; set; } = string.Empty;
//        public decimal? Price { get; set; }

//        public static Product?[] GetProducts()
//        {
//            Product kayak = new Product
//            {
//                Name = "Kayak",
//                Price = 275M
//            };
//            Product lifejacket = new Product
//            {
//                //Name = "Lifejacket",
//                Price = 48.95M
//            };

//            return new Product?[] { kayak, lifejacket, null };
//        }
//    }
//}

#endregion

#region Listing 5.44 A lambda property in the Product.cs file in the Models folder

namespace LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;

        public decimal? Price { get; set; }

        public bool NameBeginsWithS => Name.Length > 0 && Name[0] == 'S';

        public static Product?[] GetProducts()
        {
            Product kayak = new Product
            {
                Name = "Kayak",
                Price = 275M
            };

            Product lifejacket = new Product
            {
                //Name = "Lifejacket",
                Price = 48.95M
            };

            return new Product?[] { kayak, lifejacket, null };
        }
    }
}

#endregion