#region Listing 5.48 The contents of the IProductSelection.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public interface IProductSelection
//    {
//        IEnumerable<Product>? Products { get; }
//    }
//}

#endregion

#region Listing 5.51 Adding a feature in the IProductSelection.cs file in the Models folder

namespace LanguageFeatures.Models
{
    public interface IProductSelection
    {
        IEnumerable<Product>? Products { get; }

        IEnumerable<string>? Names => Products?.Select(p => p.Name);
    }
}

#endregion

