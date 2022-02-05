
using Microsoft.EntityFrameworkCore;

namespace ProjetoVolvo.Models {
    public class DealershipContext : DbContext {
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Accessory> Accessories { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Seller> Sellers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Dealership;User=sa;Password=Volvo@123");
            // optionsBuilder.UseSqlServer("Server=.\;Database=Dealership;Trusted_Connection=True");
        }

    }
}
