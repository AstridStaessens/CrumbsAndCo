using Crumbs.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crumbs.Persistence
{
    public class CrumbsDbContext : IdentityDbContext<IdentityUser>
    {
        public CrumbsDbContext(DbContextOptions<CrumbsDbContext> options) : base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderLineEntity> OrderLines { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntity>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderLineEntity>()
                .Property(ol => ol.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderEntity>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PaymentEntity>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderEntity>()
            .Property(o => o.UserId)
            .HasColumnName("UserId");

            modelBuilder.Entity<OrderEntity>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .IsRequired(false);
        }
    }
}