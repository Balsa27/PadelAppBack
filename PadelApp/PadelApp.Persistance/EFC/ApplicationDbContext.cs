using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.Outbox;

namespace PadelApp.Persistance.EFC;

public class ApplicationDbContext : DbContext
{ 
    private readonly IConfiguration _configuration;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<Court> Courts { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;
    
    public ApplicationDbContext() 
    {
    }
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connstring =
            "Host=dbaas-db-5110007-do-user-14547597-0.b.db.ondigitalocean.com; Port=25060; Database=defaultdb; Username=doadmin; Password=AVNS_9l1PkdtBRCQAEm1WbOu;";
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(connstring,
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