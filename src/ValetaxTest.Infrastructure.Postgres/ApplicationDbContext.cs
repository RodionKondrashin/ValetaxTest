using Microsoft.EntityFrameworkCore;
using ValetaxTest.Domain.Trees;

namespace ValetaxTest.Infrastructure.Postgres;

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    public DbSet<Tree> Trees => Set<Tree>();
    public DbSet<Node> Nodes => Set<Node>();
}