using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Data.EntityConfigurations;

internal sealed class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(name: "Products");

        builder.HasKey(keyExpression: product => product.ProductId);

        builder.Property(propertyExpression: product => product.ProductId).ValueGeneratedOnAdd();

        builder
            .Property(propertyExpression: product => product.ProductName)
            .HasColumnType(typeName: "NVARCHAR(40)")
            .IsRequired();

        builder.Property(propertyExpression: product => product.CategoryId).IsRequired();

        builder.Property(propertyExpression: product => product.UnitsInStock).IsRequired();

        builder
            .Property(propertyExpression: product => product.UnitPrice)
            .HasColumnType(typeName: "DECIMAL(12, 2)")
            .IsRequired();
    }
}
