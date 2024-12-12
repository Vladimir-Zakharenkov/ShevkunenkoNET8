#region Listing 6.15 The contents of the IDataSource.cs file in the SimpleApp/Models folder

namespace SimpleApp.Models
{
    public interface IDataSource
    {
        IEnumerable<Product> Products { get; }
    }
}

#endregion