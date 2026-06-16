using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Infrastructure;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Softwares.Any())
        {
            context.Softwares.AddRange(
                new Software { Name = "System ERP Finanse", Description = "Pełne zarządzanie finansami", CurrentVersion = "2.1", Category = "Finanse", YearlyCost = 5000m },
                new Software { Name = "HR Manager Pro", Description = "Zarządzanie urlopami i pracownikami", CurrentVersion = "1.5", Category = "HR", YearlyCost = 3500m },
                new Software { Name = "CRM Sales Boost", Description = "Śledzenie lejków sprzedażowych", CurrentVersion = "3.0", Category = "Sprzedaż", YearlyCost = 4200m },
                new Software { Name = "Data Analytics Core", Description = "Analiza Big Data dla korporacji", CurrentVersion = "1.0", Category = "Analityka", YearlyCost = 12000m }
            );
            context.SaveChanges();
        }
        
        if (!context.Discounts.Any())
        {
            context.Discounts.AddRange(
                new Discount { Name = "Zniżka na start", ValuePercentage = 10m, DateFrom = DateTime.UtcNow.AddDays(-30), DateTo = DateTime.UtcNow.AddDays(30) },
                new Discount { Name = "Black Friday", ValuePercentage = 25m, DateFrom = DateTime.UtcNow.AddMonths(4), DateTo = DateTime.UtcNow.AddMonths(5) },
                new Discount { Name = "Lojalnościowa", ValuePercentage = 15m, DateFrom = DateTime.UtcNow.AddYears(-1), DateTo = DateTime.UtcNow.AddYears(1) }
            );
            context.SaveChanges();
        }
        
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Login = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 12), Role = "Admin" },
                new User { Login = "pracownik", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pracownik123", 12), Role = "User" },
                new User { Login = "ksiegowosc", PasswordHash = BCrypt.Net.BCrypt.HashPassword("ksiegowosc123", 12), Role = "User" }
            );
            context.SaveChanges();
        }
        
        if (!context.Clients.Any())
        {
            context.Clients.AddRange(
                new IndividualClient { FirstName = "Jan", LastName = "Kowalski", Pesel = "90010112345", Email = "jan.kowalski@test.pl", PhoneNumber = "111-222-333", Address = "ul. Prosta 1, 00-001 Warszawa" },
                new IndividualClient { FirstName = "Anna", LastName = "Nowak", Pesel = "85020298765", Email = "anna.nowak@test.pl", PhoneNumber = "444-555-666", Address = "ul. Krzywa 2, 30-002 Kraków" },
                new IndividualClient { FirstName = "Piotr", LastName = "Zieliński", Pesel = "95030311223", Email = "piotr.z@test.pl", PhoneNumber = "777-888-999", Address = "ul. Morska 3, 80-003 Gdańsk" },
                
                new CorporateClient { CompanyName = "TechPol Sp. z o.o.", Krs = "0000123456", Email = "kontakt@techpol.pl", PhoneNumber = "22-111-22-33", Address = "Al. Jerozolimskie 100, 02-001 Warszawa" },
                new CorporateClient { CompanyName = "Bud-Max S.A.", Krs = "0000987654", Email = "biuro@budmax.com.pl", PhoneNumber = "12-333-44-55", Address = "ul. Przemysłowa 10, 40-004 Katowice" }
            );
            context.SaveChanges();
        }
        
        if (!context.Contracts.Any())
        {
            context.Contracts.AddRange(
                new Contract 
                { 
                    ClientId = 1, 
                    SoftwareId = 1, 
                    SoftwareVersion = "2.1",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddYears(1),
                    TotalPrice = 4500m, 
                    ExtraSupportYears = 1,
                    IsSigned = true
                },
                new Contract 
                { 
                    ClientId = 4, 
                    SoftwareId = 4, 
                    SoftwareVersion = "1.0",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddYears(2),
                    TotalPrice = 13000m, 
                    ExtraSupportYears = 2,
                    IsSigned = true
                },
                new Contract 
                { 
                    ClientId = 5, 
                    SoftwareId = 3, 
                    SoftwareVersion = "3.0",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date.AddYears(1),
                    TotalPrice = 4200m, 
                    ExtraSupportYears = 0,
                    IsSigned = false
                }
            );
            context.SaveChanges();
        }
    }
}