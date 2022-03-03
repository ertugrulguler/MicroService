using Catalog.Domain.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class CategoryMapper : BaseEntityMap<Category>
    {
        protected override void Map(EntityTypeBuilder<Category> eb)
        {
            eb.Property(b => b.ParentId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.DisplayName).HasColumnType("nvarchar(250)");
            eb.Property(b => b.Code).HasColumnType("nvarchar(15)");
            eb.Property(b => b.DisplayOrder).HasColumnType("int");
            eb.Property(b => b.Description).HasColumnType("nvarchar(500)");
            eb.Property(b => b.SuggestedMainId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.Type).HasColumnType("int");
            eb.Property(b => b.HasAll).HasColumnType("bit");
            eb.Property(b => b.IsRequiredIdNumber).HasColumnType("bit");
            eb.Property(b => b.LeafPath).HasColumnType("nvarchar(500)");
            eb.Property(b => b.HasProduct).HasColumnType("bit");
            eb.Property(b => b.ProductFilterOrder).HasColumnType("int");
            eb.Property(b => b.ProductFilterOrder).HasDefaultValue(10000);
            eb.Ignore(b => b.SubCategories);
            eb.HasMany(b => b.CategoryAttributes)
                .WithOne()
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Cascade);

            eb.HasMany(b => b.ProductCategories)
                .WithOne()
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Cascade);

            eb.ToTable("Category");
        }
    }
}