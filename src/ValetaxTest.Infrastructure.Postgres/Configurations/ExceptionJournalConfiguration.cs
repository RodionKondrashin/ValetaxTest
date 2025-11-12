using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValetaxTest.Domain.ExceptionJournals;

namespace ValetaxTest.Infrastructure.Postgres.Configurations;

public class ExceptionJournalConfiguration : IEntityTypeConfiguration<ExceptionJournal>
{
    public void Configure(EntityTypeBuilder<ExceptionJournal> builder)
    {
        builder.ToTable("exception_journal");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_exception_journal");
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(e => e.EventId)
            .HasColumnName("event_id");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .HasColumnName("created_at");
        
        builder.Property(e => e.Text)
            .HasColumnName("text");
        
        builder.Property(e => e.QueryParameters)
            .HasColumnName("query_parameters");
        
        builder.Property(e => e.BodyParameters)
            .HasColumnName("body_parameters");
        
        builder.Property(e => e.StackTrace)
            .HasColumnName("stack_trace");

        builder.HasIndex(e => e.EventId);
        builder.HasIndex(e => e.CreatedAt);
    }
}