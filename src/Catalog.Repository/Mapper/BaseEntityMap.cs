using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public abstract class BaseEntityMap<T> where T : Entity
    {
        protected abstract void Map(EntityTypeBuilder<T> eb);

        public void BaseMap(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>(bi =>
            {
                bi.Property(b => b.CreatedDate).HasColumnType("datetime");
                bi.Property(b => b.ModifiedDate).HasColumnType("datetime");
                bi.Property(b => b.IsActive).HasColumnType("bit");
                bi.HasKey(b => b.Id);
                Map(bi);
            });
        }
    }
}