using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data.Entities;
using Namotion.Reflection;

namespace MyLibrary.Server.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users => Set<User>();
        public DbSet<Book> Books { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Operation> Operations { get; set; }

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
            });

            builder.Entity<Book>(b =>
            {
                b.ToTable("Books");
                b.HasKey(b => b.Id);
                b.Property(b => b.BasePrice).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Warehouse>(w =>
            {
                w.ToTable("StockDTO");
                w.HasKey(w => w.Id);
            });

            builder.Entity<Operation>(o =>
            {
                o.ToTable("Operations");
                o.HasKey(o => o.Id);
                o.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
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
