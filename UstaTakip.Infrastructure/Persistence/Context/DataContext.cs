//using DataAccess.Migrations;

//using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Infrastructure.Persistence.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        /* public DataContext()
         {
         }*/


        public DbSet<User> Users => Set<User>();
        public DbSet<OperationClaim> OperationClaims => Set<OperationClaim>();
        public DbSet<UserOperationClaim> UserOperationClaims => Set<UserOperationClaim>();

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<RepairJob> RepairJobs => Set<RepairJob>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        // public DbSet<AuditLog> AuditLogs =>Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key
         

            modelBuilder.Entity<UserOperationClaim>()
        .HasOne(uoc => uoc.User)
        .WithMany(u => u.UserOperationClaims) // 👈 navigation property kullanılıyor
        .HasForeignKey(uoc => uoc.UserId);

            modelBuilder.Entity<UserOperationClaim>()
                .HasOne(uoc => uoc.OperationClaim)
                .WithMany(oc => oc.UserOperationClaims) // 👈 navigation property kullanılıyor
                .HasForeignKey(uoc => uoc.OperationClaimId);

            // (Opsiyonel) Kullanıcı e-mail unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // (Opsiyonel) Varsayılan roller
            modelBuilder.Entity<OperationClaim>().HasData(
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "Garson" }
            );
            // Customer - Vehicle ilişkisi (1 - N)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Customer)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // müşteri silinince araçları da silinir

            // Vehicle - RepairJob ilişkisi (1 - N)
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.RepairJobs)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Cascade); // araç silinince işlemleri de silinsin

            // Ek örnek: Primary key ve tablo adları belirleme (isteğe bağlı)
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
            modelBuilder.Entity<RepairJob>().ToTable("RepairJobs");



        }

    }
}
//Add-Migration InitialCreate -StartupProject Cafe.WebAPI -Project Cafe.Infrastructure


//Update-Database -StartupProject Cafe.WebAPI -Project Cafe.Infrastructure

//Add-Migration InitialCreate

//update-database "InitialCreate"