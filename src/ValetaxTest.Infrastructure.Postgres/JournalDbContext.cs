using Microsoft.EntityFrameworkCore;
using ValetaxTest.Domain.ExceptionJournals;

namespace ValetaxTest.Infrastructure.Postgres;

public class JournalDbContext: DbContext
{
    private readonly string _connectionString;

    public JournalDbContext(string connectionString)
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
    
    public DbSet<ExceptionJournal> ExceptionJournals => Set<ExceptionJournal>();

}