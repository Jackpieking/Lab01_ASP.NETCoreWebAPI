using System.Collections.Generic;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            assembly: typeof(ApplicationDbContext).Assembly
        );

        DataSeeding(modelBuilder: modelBuilder);
    }

    private static void DataSeeding(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(category =>
        {
            category.HasData(
                new List<Category>()
                {
                    new() { CategoryId = 1, CategoryName = "Beverages" },
                    new() { CategoryId = 2, CategoryName = "Condiments" },
                    new() { CategoryId = 3, CategoryName = "Confections" },
                    new() { CategoryId = 4, CategoryName = "Dairy Products" },
                    new() { CategoryId = 5, CategoryName = "Grains/Cereals" },
                    new() { CategoryId = 6, CategoryName = "Meat/Poultry" },
                    new() { CategoryId = 7, CategoryName = "Produce" },
                    new() { CategoryId = 8, CategoryName = "Seafood" }
                }
            );
        });
    }
}
