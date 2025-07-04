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
        public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
        public DbSet<RepairJob> RepairJobs => Set<RepairJob>();

        public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();
        public DbSet<InsurancePayment> InsurancePayments=> Set<InsurancePayment>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        // public DbSet<AuditLog> AuditLogs =>Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UserOperationClaim ilişkileri
            modelBuilder.Entity<UserOperationClaim>()
                .HasOne(uoc => uoc.User)
                .WithMany(u => u.UserOperationClaims)
                .HasForeignKey(uoc => uoc.UserId);

            modelBuilder.Entity<UserOperationClaim>()
                .HasOne(uoc => uoc.OperationClaim)
                .WithMany(oc => oc.UserOperationClaims)
                .HasForeignKey(uoc => uoc.OperationClaimId);

            // Kullanıcı e-mail unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Varsayılan roller
            modelBuilder.Entity<OperationClaim>().HasData(
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "Garson" }
            );

            // Customer - Vehicle ilişkisi
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Customer)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vehicle - RepairJob ilişkisi
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.RepairJobs)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Vehicle - VehicleImage ilişkisi (1 - N)
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.VehicleImages)
                .WithOne(i => i.Vehicle)
                .HasForeignKey(i => i.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RepairJob>()
                 .Property(r => r.Price)
                 .HasPrecision(18, 2); // 18 basamak, 2 ondalık

            // Vehicle - InsurancePolicy (1:N)
            modelBuilder.Entity<InsurancePolicy>()
                .HasOne(p => p.Vehicle)
                .WithMany(v => v.InsurancePolicies)
                .HasForeignKey(p => p.VehicleId)
                .OnDelete(DeleteBehavior.Cascade); // Bir araç silinirse poliçeleri de silinir

            // InsurancePolicy - InsurancePayment (1:N)
            modelBuilder.Entity<InsurancePayment>()
                .HasOne(ip => ip.InsurancePolicy)
                .WithMany(p => p.InsurancePayments)
                .HasForeignKey(ip => ip.InsurancePolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            // RepairJob - InsurancePayment (1:1)
            modelBuilder.Entity<InsurancePayment>()
                .HasOne(ip => ip.RepairJob)
                .WithOne(rj => rj.InsurancePayment)
                .HasForeignKey<InsurancePayment>(ip => ip.RepairJobId)
                .OnDelete(DeleteBehavior.Restrict); // İsteğe bağlı: onarım silinirse ödeme kalabilir

            // Opsiyonel: RepairJob içinde Navigation Property varsa
            // public InsurancePayment InsurancePayment { get; set; }

            // Diğer yapıların fluent configleri varsa onlar da burada olmalı
            // Tablo adları
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
            modelBuilder.Entity<RepairJob>().ToTable("RepairJobs");
            modelBuilder.Entity<VehicleImage>().ToTable("VehicleImages"); // ✅ Yeni tablo


        }

    }
}
//Add-Migration InitialCreate -StartupProject UstaTakip.Web -Project UstaTakip.Infrastructure
//Add-Migration InitialCreate -StartupProject UstaTakip.WebAPI -Project UstaTakip.Infrastructure


//Update-Database -StartupProject UstaTakip.WebAPI -Project UstaTakip.Infrastructure

//Add-Migration InitialCreate

//update-database "InitialCreate"