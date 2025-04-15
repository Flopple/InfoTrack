using System.Diagnostics.CodeAnalysis;
using InfoTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfoTrack.Persistence.Configuration;

[ExcludeFromCodeCoverage]
public class SearchResultConfiguration : IEntityTypeConfiguration<SearchResult>
{
    public void Configure(EntityTypeBuilder<SearchResult> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Keywords)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.SearchTarget)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.SearchEngine)
            .IsRequired();

        builder.Property(p => p.SearchDate)
            .IsRequired();

        builder.HasMany(p => p.Positions)
            .WithOne()
            .HasForeignKey(p => p.SearchResultId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}