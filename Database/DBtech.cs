using Microsoft.EntityFrameworkCore;

namespace E_commerce_website_Techcareer.Database
{
    public class DBtech : DbContext
    {
        public DbSet<Users> User { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Middle> Fiddle { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DEMONSLAYER\\SQLEXPRESS;Database=CommerceSite_DB;Trusted_Connection=True;Encrypt=False");
        }
    }
}
