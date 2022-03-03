using Catalog.Domain.Entities;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Catalog.Domain.ValueObject.StoreProcedure;
using Catalog.Repository.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Repository
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<AttributeFilter> AttributeFilter { get; set; }
        public virtual DbSet<RelatedCategories> CategoryFilter { get; set; }
        public virtual DbSet<ProductFilterWithCount> ProductFilterWithCount { get; set; }
        public virtual DbSet<PriceFilter> PriceFilter { get; set; }
        public virtual DbSet<BrandFilter> BrandFilter { get; set; }
        public virtual DbSet<SellerFilter> SellerFilter { get; set; }
        public virtual DbSet<ProductCountForBackoffice> ProductCountForBackoffice { get; set; }
        public virtual DbSet<XmlProduct> XmlProduct { get; set; }
        public virtual DbSet<XmlAttribute> XmlAttribute { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AttributeMapper().BaseMap(modelBuilder);
            new AttributeValueMapper().BaseMap(modelBuilder);
            new AttributeMapMapper().BaseMap(modelBuilder);
            new BannerMapper().BaseMap(modelBuilder);
            new BrandMapper().BaseMap(modelBuilder);
            new CategoryMapper().BaseMap(modelBuilder);
            new CategoryAttributeMapper().BaseMap(modelBuilder);
            new CategoryAttributeValueMapMapper().BaseMap(modelBuilder);
            new CategoryImageMapper().BaseMap(modelBuilder);
            new ProductMapper().BaseMap(modelBuilder);
            new ProductAttributeMapper().BaseMap(modelBuilder);
            new ProductCategoryMapper().BaseMap(modelBuilder);
            new ProductGroupMapper().BaseMap(modelBuilder);
            new ProductGroupVariantMapper().BaseMap(modelBuilder);
            new ProductImageMapper().BaseMap(modelBuilder);
            new ProductSellerMapper().BaseMap(modelBuilder);
            new SimilarProductMapper().BaseMap(modelBuilder);
            new FavoriteProductMapper().BaseMap(modelBuilder);
            new ProductDeliveryMapper().BaseMap(modelBuilder);

            new CategoryInstallmentMapper().BaseMap(modelBuilder);
            new ProductChannelMapper().BaseMap(modelBuilder);

            new BannerActionTypeMapper().BaseMap(modelBuilder);
            new BannerFilterTypeMapper().BaseMap(modelBuilder);
            new BannerFiltersMapper().BaseMap(modelBuilder);
            new BannerTypeMapper().BaseMap(modelBuilder);
            new BannerLocationMapper().BaseMap(modelBuilder);
            new BannerMapper().BaseMap(modelBuilder);


            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();
            var added = this.ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in added)
            {
                if (entity is Entity track)
                {
                    track.CreatedDate = DateTime.Now;
                    track.ModifiedDate = track.CreatedDate; //searchOptimization
                    track.setIsActive(true);
                }
            }

            var modified = this.ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in modified)
            {
                if (entity is Entity track)
                {
                    track.ModifiedDate = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}