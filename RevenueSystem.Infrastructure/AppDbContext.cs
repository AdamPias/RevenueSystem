using Microsoft.EntityFrameworkCore;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Rejestracja tabel w bazie
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; } 
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Client>()
            .HasDiscriminator<string>("ClientType")
            .HasValue<IndividualClient>("Individual")
            .HasValue<CorporateClient>("Corporate");

        modelBuilder.Entity<Client>().Property(c => c.Address).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<Client>().Property(c => c.Email).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Client>().Property(c => c.PhoneNumber).HasMaxLength(20).IsRequired();
        
        modelBuilder.Entity<IndividualClient>().Property(i => i.FirstName).HasMaxLength(50);
        modelBuilder.Entity<IndividualClient>().Property(i => i.LastName).HasMaxLength(50);
        modelBuilder.Entity<IndividualClient>().Property(i => i.Pesel).HasMaxLength(11);
        modelBuilder.Entity<IndividualClient>().HasIndex(i => i.Pesel).IsUnique();
        
        modelBuilder.Entity<CorporateClient>().Property(c => c.CompanyName).HasMaxLength(100);
        modelBuilder.Entity<CorporateClient>().Property(c => c.Krs).HasMaxLength(10);
        modelBuilder.Entity<CorporateClient>().HasIndex(c => c.Krs).IsUnique();
        
        modelBuilder.Entity<Contract>()
            .Property(c => c.TotalPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Subscription>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<SubscriptionPayment>()
            .Property(sp => sp.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Discount>()
            .Property(d => d.ValuePercentage)
            .HasColumnType("decimal(5,2)");


        modelBuilder.Entity<Software>().Property(s => s.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Software>().Property(s => s.Category).HasMaxLength(50);
        modelBuilder.Entity<Software>()
            .Property(s => s.YearlyCost)
            .HasColumnType("decimal(18,2)");
    }
}