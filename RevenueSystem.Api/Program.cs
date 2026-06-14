using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Application.Services;
using RevenueSystem.Infrastructure;
using RevenueSystem.Infrastructure.Repositories;
using RevenueSystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ISoftwareRepository, SoftwareRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExchangeRateService, NbpExchangeRateService>();
builder.Services.AddScoped<IRevenueService, RevenueService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Wpisz token JWT w formacie: Bearer {twój_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); // Musi wskazywać na plik wygenerowany przez AddSwaggerGen
        s.RoutePrefix = string.Empty; // Twój pusty prefix!
    });
}

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    if (!context.Softwares.Any())
    {
        context.Softwares.Add(new RevenueSystem.Domain.Entities.Software { Name = "System ERP Finanse", Description = "Zarządzanie finansami firmy", CurrentVersion = "1.0", Category = "Finanse", YearlyCost = 5000m });
        context.Discounts.Add(new RevenueSystem.Domain.Entities.Discount { Name = "Zniżka na start", ValuePercentage = 10m, DateFrom = DateTime.UtcNow.AddDays(-5), DateTo = DateTime.UtcNow.AddDays(15) });
        context.SaveChanges();
    }

    if (!context.Users.Any())
    {
        context.Users.Add(new RevenueSystem.Domain.Entities.User { Login = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 12), Role = "Admin" });
        context.Users.Add(new RevenueSystem.Domain.Entities.User { Login = "pracownik", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pracownik123", workFactor: 12), Role = "User" });
        context.SaveChanges();
    }
}

app.Run();