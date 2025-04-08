using System.Diagnostics.CodeAnalysis;
using InfoTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfoTrack.Persistence.Configuration;

[ExcludeFromCodeCoverage]
public class SearchResultPositionConfiguration : IEntityTypeConfiguration<SearchResultPosition>
{
    public void Configure(EntityTypeBuilder<SearchResultPosition> builder)
    {
        builder.HasKey(k => new { k.SearchResultId, k.Position });

        builder.Property(p => p.Position)
            .IsRequired();
    }
}