using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValetaxTest.Domain.Trees;

namespace ValetaxTest.Infrastructure.Postgres.Configurations;

public class TreeConfiguration : IEntityTypeConfiguration<Tree>
{
    public void Configure(EntityTypeBuilder<Tree> builder)
    {
        builder.ToTable("trees");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_trees");
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}