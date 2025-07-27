using Microsoft.EntityFrameworkCore;


namespace Cartify.DAL.DataBase
{
    public class CartifyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=HASSAN;Database=MVCDay4;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }

    }
}
