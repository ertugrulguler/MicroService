using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductMapper : BaseEntityMap<Product>
    {
        protected override void Map(EntityTypeBuilder<Product> eb)
        {
            eb.Property(b => b.BrandId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.Name).HasColumnType("nvarchar(250)");
            eb.Property(b => b.SeoName).HasColumnType("nvarchar(250)");
            eb.Property(b => b.DisplayName).HasColumnType("nvarchar(250)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(Max)");
            eb.Property(b => b.Desi).HasColumnType("decimal(18,2)");
            eb.Property(b => b.VatRate).HasColumnType("int");
            eb.Property(b => b.Code).HasColumnType("nvarchar(500)");
            eb.Property(b => b.PriorityRank).HasColumnType("int");
            eb.Property(b => b.ProductMainId).HasColumnType("int");

            eb.HasMany(b => b.ProductAttributes)
               .WithOne()
               .HasForeignKey("ProductId")
               .OnDelete(DeleteBehavior.Cascade);

            eb.HasMany(b => b.ProductCategories)
                .WithOne()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);

            eb.HasMany(b => b.ProductSellers)
                .WithOne()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);

            eb.HasMany(b => b.SimilarProducts)
               .WithOne()
               .HasForeignKey("ProductId")
               .OnDelete(DeleteBehavior.ClientCascade);

            eb.ToTable("Product");
        }
    }

}
