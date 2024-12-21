#region Listing 8.22 The contents of the Cart.cs file in the SportsStore/Models folder

//namespace SportsStore.Models
//{
//    public class Cart
//    {
//        public List<CartLine> Lines { get; set; } = new List<CartLine>();

//        public void AddItem(Product product, int quantity)
//        {
//            CartLine? line = Lines
//                .Where(p => p.Product.ProductID == product.ProductID)
//                .FirstOrDefault();

//            if (line == null)
//            {
//                Lines.Add(new CartLine
//                {
//                    Product = product,
//                    Quantity = quantity
//                });
//            }
//            else
//            {
//                line.Quantity += quantity;
//            }
//        }

//        public void RemoveLine(Product product) =>
//            Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);

//        public decimal ComputeTotalValue() => Lines.Sum(e => e.Product.Price * e.Quantity);

//        public void Clear() => Lines.Clear();
//    }

//    public class CartLine
//    {
//        public int CartLineID { get; set; }

//        public Product Product { get; set; } = new();

//        public int Quantity { get; set; }
//    }
//}

#endregion

#region Listing 9.1 Applying the keyword in the Cart.cs file in the SportsStore/Models folder

namespace SportsStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = Lines
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public decimal ComputeTotalValue() => Lines.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineID { get; set; }

        public Product Product { get; set; } = new();

        public int Quantity { get; set; }
    }
}

#endregion