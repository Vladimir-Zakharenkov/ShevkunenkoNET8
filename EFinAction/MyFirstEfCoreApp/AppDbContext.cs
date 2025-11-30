using Microsoft.EntityFrameworkCore;

namespace MyFirstEfCoreApp;

public class AppDbContext : DbContext
{
    private const string ConnectionString =
        @"Data Source=PAPA\\VM16433; 
            Database=MyFirstEfCoredb; 
            Persist Security Info=false; 
            User ID='user1'; 
            Password='QwErTy06111968'; 
            Integrated Security=False; 
            Trusted_Connection=False; 
            TrustServerCertificate=True;";

    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
}