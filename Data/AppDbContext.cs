using BusinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<IdDateObject>().Property<DateTime>("_createdOn").HasField("_createdOn");
            b.Entity<IdDateObject>().Property<DateTime?>("_modifiedOn").HasField("_modifiedOn");

            b.Entity<Product>().Property<Guid>("_id").HasField("_id");
            b.Entity<Product>().Property<string>("_name").HasField("_name");
            b.Entity<Product>().Property<string>("_description").HasField("_description");
            b.Entity<Product>().Property<decimal>("_price").HasField("_price");
            b.Entity<Product>().Property<int>("_stock").HasField("_stock");

            b.Entity<Order>().Property<Guid>("_id").HasField("_id");
            b.Entity<Order>().Property<string>("_customerName").HasField("_customerName");
            b.Entity<Order>().Property<OrderStatus>("_status").HasField("_status");
            b.Entity<Order>().Property<DateTime>("_orderDate").HasField("_orderDate");
            b.Entity<Order>()
                .OwnsMany(o => o.Items, oi =>
                {
                    oi.WithOwner().HasForeignKey("OrderId");
                    oi.Property<Guid>("ProductId");
                    oi.Property<string>("Name");
                    oi.Property<decimal>("UnitPrice");
                    oi.Property<int>("Quantity");

                    oi.HasOne<Product>()
                      .WithMany()
                      .HasForeignKey("ProductId")
                      .OnDelete(DeleteBehavior.Restrict);

                    oi.HasKey("OrderId", "ProductId");
                });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var now = DateTime.UtcNow;
            foreach (var e in ChangeTracker.Entries<IdDateObject>())
            {
                if (e.State == EntityState.Added)
                {
                    e.Property("_createdOn").CurrentValue = now;
                    e.Property("_modifiedOn").CurrentValue = null;
                }
                else if (e.State == EntityState.Modified)
                {
                    e.Property("_modifiedOn").CurrentValue = now;
                }
            }
        }
    }
}
