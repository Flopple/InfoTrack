using System.Diagnostics.CodeAnalysis;
using InfoTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace InfoTrack.Persistence.Context;

[ExcludeFromCodeCoverage]
public class DatabaseContext : DbContext
{
    public DbSet<SearchResult> SearchResults { get; init; }
    public DbSet<SearchResultPosition> SearchResultPositions { get; init; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}