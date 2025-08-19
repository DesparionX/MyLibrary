using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using Namotion.Reflection;
using Newtonsoft.Json;

namespace MyLibrary.Server.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users => Set<User>();
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<BorrowedBooks> BorrowedBooks { get; set; }
        public virtual DbSet<SubscriptionTier> SubscriptionTiers { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.ToTable("Users");
                u.HasKey(u => u.Id);
                u.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            });
             

            builder.Entity<Book>(b =>
            {
                b.ToTable("Books");
                b.HasKey(b => b.Id);
                b.Property(b => b.BasePrice).HasColumnType("decimal(18,2)");
                b.Property(b => b.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
                b.Property(b => b.IsAvailable)
                .HasDefaultValue(true);
            });

            builder.Entity<BorrowedBooks>(b =>
            {
                b.ToTable("BorrowedBooks");
                b.HasKey(b => b.Id);
                b.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            });

            builder.Entity<Warehouse>(w =>
            {
                w.ToTable("Warehouse");
                w.HasKey(w => w.Id);
                w.Property(w => w.Id)
                .ValueGeneratedOnAdd();
            });

            builder.Entity<Operation>(o =>
            {
                o.ToTable("Operations");
                o.HasKey(o => o.Id);
                o.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
                o.Property(o => o.Id)
                .ValueGeneratedOnAdd();
                o.Property(o => o.OrderListInternal)
                .HasColumnName("OrderList")
                .HasConversion(
                        v => JsonConvert.SerializeObject(v, Formatting.None),
                        v => JsonConvert.DeserializeObject<List<Order>>(v)
                    );
            });

            builder.Entity<SubscriptionTier>(s =>
            {
                s.ToTable("SubscriptionTiers");
                s.HasKey(s => s.Id);
                s.Property(s => s.Id).ValueGeneratedOnAdd();
                s.Property(s => s.Cost).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Subscription>(s =>
            {
                s.ToTable("Subscriptions");
                s.HasKey(s => s.Id);
                s.Property(s => s.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NEWID()");
            });

            // Renaming Identity tables
            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });
        }
    }
}
