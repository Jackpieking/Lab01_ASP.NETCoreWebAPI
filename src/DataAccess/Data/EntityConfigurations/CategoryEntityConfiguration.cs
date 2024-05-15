using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Data.EntityConfigurations;

internal sealed class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(name: "Categories");

        builder.HasKey(keyExpression: category => category.CategoryId);

        builder.Property(propertyExpression: category => category.CategoryId).ValueGeneratedOnAdd();

        builder
            .Property(propertyExpression: category => category.CategoryName)
            .HasColumnType(typeName: "NVARCHAR(40)")
            .IsRequired();

        builder
            .HasMany(navigationExpression: category => category.Products)
            .WithOne(navigationExpression: product => product.Category)
            .HasForeignKey(foreignKeyExpression: product => product.CategoryId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }
}
