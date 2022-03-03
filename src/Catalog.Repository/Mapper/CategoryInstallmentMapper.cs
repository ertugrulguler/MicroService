using Catalog.Domain.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class CategoryInstallmentMapper : BaseEntityMap<CategoryInstallment>
    {
        protected override void Map(EntityTypeBuilder<CategoryInstallment> ci)
        {
            ci.Property(b => b.CategoryId).HasColumnType("uniqueidentifier");
            ci.Property(b => b.MaxInstallmentCount).HasColumnType("int");
            ci.Property(b => b.MinPrice).HasColumnType("decimal(18,2)");
            ci.Property(b => b.NewMaxInstallmentCount).HasColumnType("int");



            ci.ToTable("CategoryInstallment");
        }
    }
}