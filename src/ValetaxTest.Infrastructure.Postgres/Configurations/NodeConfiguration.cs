using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValetaxTest.Domain.Trees;

namespace ValetaxTest.Infrastructure.Postgres.Configurations;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("nodes");

        builder.HasKey(n => n.Id)
            .HasName("pk_nodes");

        builder.Property(n => n.Name)
            .IsRequired()
            .HasColumnName("name");
        
        builder.HasOne(n => n.Tree)
            .WithMany(t => t.Nodes)
            .HasForeignKey(n => n.TreeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(n => n.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(n => n.Name).IsUnique();
    }
}