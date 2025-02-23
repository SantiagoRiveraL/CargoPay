using Microsoft.EntityFrameworkCore;
using CargoPay.Core.Entitites;

namespace CargoPay.Infrastructure.Data
{
    public class CargoPayDbContext : DbContext
    {
        public CargoPayDbContext(DbContextOptions<CargoPayDbContext> options)
            : base(options)
        {
        }

        public CargoPayDbContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email único en User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            // CardNumber único en Card
            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            // Relación User - Card (Uno a Muchos)
            modelBuilder.Entity<Card>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación User - Payment (Uno a Muchos)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Card - Payment (Uno a Muchos)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Card)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único en Payment.Id
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.Id)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Esto es solo para las migraciones, usa el entorno correcto en producción
                //optionsBuilder.UseNpgsql("Host=localhost;Database=CargoPayDB;Username=postgres;Password=postgres");
                optionsBuilder.UseNpgsql("Host=caboose.proxy.rlwy.net;Port=48132;Database=railway;Username=postgres;Password=wGqWCvjDuwQydjIJvXAsumGztAXnOWYa");
            }
        }
    }
}