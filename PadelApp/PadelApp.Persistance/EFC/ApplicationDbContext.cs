using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;

namespace PadelApp.Persistance.EFC;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Organization> Organizations { get; set; } = null!;
    
    public ApplicationDbContext() 
    {
    }
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration) 
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(_configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8,  0)), mysql =>
                {
                    mysql.EnableRetryOnFailure();
                });
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}