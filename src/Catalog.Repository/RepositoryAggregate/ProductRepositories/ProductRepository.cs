using Catalog.Domain.Enums;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Catalog.Domain.ValueObject.StoreProcedure;
using Framework.Core.Model;
using Framework.Core.Model.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(CatalogDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsByChannel(ChannelCode channel,Guid? categoryId,Guid? brandId)
        {
            IQueryable<Product> query = _entities.AsQueryable()
                .Include(i=>i.ProductCategories)
                .Include(pi=>pi.ProductImages)
                .Where(pc => pc.ProductChannels.Where(i=>i.IsActive && i.ChannelCode==(int)channel).ToList().Count > 0);

            if (categoryId != null)
                query = query.Where(cat =>
                    cat.ProductCategories.Where(i => i.IsActive && i.CategoryId == categoryId).ToList().Count>0);

            if (brandId != null)
                query = query.Where(i => i.BrandId == brandId);

            return await query.ToListAsync();

        }
        public async Task<Product> GetProductWithAllRelations(Guid Id)
        {
            return await _entities.AsQueryable()
                .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Include(sp => sp.SimilarProducts.Where(t => t.IsActive))
                .Include(ps => ps.ProductSellers.Where(t => t.IsActive))
                .Include(ps => ps.ProductImages.Where(t => t.IsActive))
                .Include(pg => pg.ProductGroups.Where(t => t.IsActive))
                .Include(pd => pd.ProductDeliveries.Where(t => t.IsActive))
                .Where(p => p.Id == Id)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductWithAllRelations(string code)
        {
            return await _entities.AsQueryable()
                .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Include(sp => sp.SimilarProducts.Where(t => t.IsActive))
                .Include(ps => ps.ProductSellers.Where(t => t.IsActive))
                .Include(ps => ps.ProductImages.Where(t => t.IsActive))
                .Include(pg => pg.ProductGroups.Where(t => t.IsActive))
                .Include(pd => pd.ProductDeliveries.Where(t => t.IsActive))
                .Where(p => p.Code == code && p.IsActive)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductByCategoryIdWithAllRelations(Guid Id)
        {
            return await _entities.AsQueryable()
                .Include(pi => pi.ProductImages.Where(t => t.IsActive))
                .Include(ps => ps.ProductSellers.Where(t => t.IsActive))
                .Where(p => p.Id == Id)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductByProductSellerId(Guid productId, Guid sellerId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Include(ps => ps.ProductSellers.Where(t => t.SellerId == sellerId && t.IsActive))
                .Include(pd => pd.ProductDeliveries.Where(t => t.SellerId == sellerId && t.IsActive))
                .Include(pi => pi.ProductImages.Where(t => t.SellerId == sellerId && t.IsActive))
                .Where(p => p.Id == productId && p.IsActive)
                .Where(ps => ps.ProductSellers.Any(t => t.SellerId == sellerId && t.IsActive))
                .Where(pd => pd.ProductDeliveries.Any(t => t.SellerId == sellerId && t.IsActive))
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductWithCategory(Guid productId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Where(p => p.Id == productId && p.IsActive)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductWithCategoryByCode(string code)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Where(p => p.Code == code && p.IsActive)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductByProductCodeSellerId(string code, Guid sellerId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Include(sp => sp.SimilarProducts.Where(t => t.IsActive))
                .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                .Include(pg => pg.ProductGroups.Where(t => t.IsActive))
                .Include(pi => pi.ProductImages.Where(t => t.IsActive))
                .Where(p => p.Code == code)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Product>> GetProductsDetailBySellerId(Guid sellerId, PagerInput pagerInput)
        {
            return await _entities.AsQueryable()
                .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Include(sp => sp.SimilarProducts.Where(t => t.IsActive))
                .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                .Include(pg => pg.ProductGroups.Where(t => t.IsActive))
                .Include(pi => pi.ProductImages.Where(t => t.IsActive))
                .Include(pc => pc.ProductDeliveries.Where(t => t.IsActive))
                .Where(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive).ToList().Count > 0)
                .Where(p => p.IsActive)
                .OrderByDescending(c => c.CreatedDate)
                .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                .Take(pagerInput.PageSize)
                .ToListAsync();
        }

        public async Task<PagedList<Product>> GetProductsSearchOptimizationByPagingAsync(PagerInput pagerInput, DateTime createdDate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))

            {
                var itemsCount = await _entities.AsQueryable()
                    .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                    .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Include(x => x.ProductGroups.Where(w => w.IsActive))
                    .Where(p => p.ModifiedDate >= createdDate && p.IsActive).OrderBy(product => product.ModifiedDate)
                    .CountAsync();

                var list = await _entities.AsQueryable()
                    .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                    .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Include(x => x.ProductGroups.Where(w => w.IsActive))
                    .Where(p => p.ModifiedDate >= createdDate && p.IsActive).OrderBy(product => product.ModifiedDate)
                    .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                    .Take(pagerInput.PageSize)
                    .ToListAsync();
                scope.Complete();
                return new PagedList<Product>(list, itemsCount, pagerInput);
            }
        }

        public async Task<ProductBrandList> GetProductsBrandSearchOptimization(DateTime createdDate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))

            {
                var itemsCount = await _entities.AsQueryable()
                    .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                    .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Where(p => p.ModifiedDate >= createdDate && p.IsActive).OrderBy(product => product.ModifiedDate)
                    .GroupBy(p => new { p.Brand.Name, p.BrandId })
                    .CountAsync();

                var brandList = await _entities.AsQueryable()
                    .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                    .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Include(b => b.Brand)
                    .Where(p => p.ModifiedDate >= createdDate && p.IsActive).OrderBy(product => product.ModifiedDate)
                    .GroupBy(p => new { p.Brand.Name, p.BrandId })
                    .Select(g => new BrandList { BrandId = g.Key.BrandId, BrandName = g.Key.Name })
                    .Take(1000)
                    .ToListAsync();

                var lastDateTime = await _entities.AsQueryable()
                    .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                    .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Include(b => b.Brand)
                    .Where(p => p.ModifiedDate >= createdDate && p.IsActive).OrderBy(product => product.ModifiedDate)
                    .GroupBy(p => new { p.Brand.Name, p.BrandId })
                    .Select(p => p.Max(p => p.ModifiedDate))
                    .Take(1000).ToListAsync();

                var result = new ProductBrandList
                {
                    LastDateTime = lastDateTime.LastOrDefault().Value,
                    Count = itemsCount,
                    BrandList = brandList
                };

                scope.Complete();
                return result;
            }
        }

        public async Task<List<Product>> GetProductsSearchOptimization(DateTime createdDate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _entities
                    .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                    .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Where(p => p.ModifiedDate >= createdDate && p.IsActive).OrderBy(product => product.ModifiedDate)
                    .ToListAsync().ConfigureAwait(false);
                //scope.Complete();
                return result;
            }

        }

        public async Task<ProductListWithCount> GetProductAllRelations(PagerInput pagerInput, List<Guid> categoryId, List<List<Guid>> attributeIds, Expression<Func<Product, bool>> expressionProduct,
            Expression<Func<ProductSeller, bool>> expressionProductSeller, OrderBy orderBy, List<Guid> bannedSellers, int productChannel, List<Guid> sellerList)
        {
            var resultProduct = new List<Product>();
            var sortedList = new List<ProductSeller>();
            var list = new List<Product>();
            Expression<Func<Product, bool>> categoryExpression3 = null;
            Expression<Func<Product, bool>> productSellerExpression3 = null;

            Expression<Func<Product, bool>> productSellerExpression = x => x.ProductSellers.All(ps => ps.IsActive == true);
            Expression<Func<Product, bool>> productSellerExpression2 = x => x.ProductSellers.AsQueryable().Any(expressionProductSeller);
            if (bannedSellers != null)
            {
                productSellerExpression = x => x.ProductSellers.Any(ps => !bannedSellers.Contains(ps.SellerId));
            }

            productSellerExpression3 = ExpressionExtensions.And(productSellerExpression, productSellerExpression2);


            Expression<Func<Product, bool>> categoryExpression1 = x => x.ProductCategories.All(pc => pc.IsActive == true);
            if (categoryId == null || categoryId.Count == 0)
            {
                categoryExpression3 = categoryExpression1;
            }
            else
            {
                Expression<Func<Product, bool>> categoryExpression = x => x.ProductCategories.Any(pc => categoryId.Contains(pc.CategoryId));
                categoryExpression3 = ExpressionExtensions.And(categoryExpression, categoryExpression1);
            }

            var count2 = _entities.AsQueryable().AsNoTracking()
                   .Include(b => b.Brand)
                   .Include(pi => pi.ProductImages)
                   .Include(pc => pc.ProductChannels)
                   .Include(pa => pa.ProductAttributes)
                   .Include(pc => pc.ProductCategories)
                   .Include(ps => ps.ProductSellers)
                   .Where(expressionProduct)
                   .Where(categoryExpression3)
                   .Where(productSellerExpression3);

            foreach (var item in attributeIds)
            {
                count2 = count2.Where(pa => pa.ProductAttributes.AsQueryable().Any(x => item.Contains(x.AttributeValueId)));
            }


            var count = await count2.Where(o => o.IsActive == true).Select(x => new Product(x.Id, x.Name, x.Code, x.DisplayName, x.BrandId, x.Brand, x.ProductAttributes.Where(y => y.IsActive == true).ToList(), x.ProductCategories.Where(y => y.IsActive == true).ToList(), x.ProductSellers.Where(p => sellerList.Count() > 0 ? sellerList.Contains(p.SellerId) && p.StockCount > 0 && p.SalePrice >= 1 : p.IsActive == true && p.StockCount > 0 && p.SalePrice >= 1).ToList(), x.ProductChannels.Where(t => t.IsActive == true && t.ChannelCode == productChannel && t.IsActive == true).ToList(), x.ProductImages.Where(y => y.IsActive == true).ToList())).ToListAsync();

            if (attributeIds.Count() > 0)
                list = count.Where(y => y.ProductCategories.Count > 0 && y.ProductSellers.Count > 0 && y.ProductChannels.Count > 0 && y.ProductAttributes.Count() > 0).ToList();
            else
                list = count.Where(y => y.ProductCategories.Count > 0 && y.ProductSellers.Count > 0 && y.ProductChannels.Count > 0).ToList();



            if (orderBy == OrderBy.AscPrice || orderBy == OrderBy.Suggession)
                sortedList = list.Select(c => c.ProductSellers.OrderBy(c => c.SalePrice).FirstOrDefault()).ToList().OrderBy(v => v.SalePrice).ToList();
            else if (orderBy == OrderBy.DescPrice)
                sortedList = list.Select(c => c.ProductSellers.OrderByDescending(c => c.SalePrice).FirstOrDefault()).ToList().OrderByDescending(v => v.SalePrice).ToList();
            else
                sortedList = list.Select(c => c.ProductSellers.OrderByDescending(c => c.CreatedDate).FirstOrDefault()).ToList().OrderByDescending(v => v.CreatedDate).ToList();

            if (sortedList.Count > 0)
            {
                foreach (var item in sortedList)
                {
                    var product = list.Where(t => t.ProductSellers.Where(u => u.ProductId == item.ProductId).FirstOrDefault() != null).FirstOrDefault();
                    if (product != null)
                    {
                        product.ArrangeProductImagesBySellerId(item.SellerId);
                        resultProduct.Add(product);
                    }
                }
            }

            var result = new ProductListWithCount
            {
                ProductList = resultProduct.Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize).Take(pagerInput.PageSize).ToList(),
                TotalCount = attributeIds.Count() > 0 ? count.Where(y => y.ProductCategories.Count() > 0 && y.ProductSellers.Count() > 0 && y.ProductChannels.Count() > 0 && y.ProductAttributes.Count() > 0).ToList().Count() : count.Where(y => y.ProductCategories.Count() > 0 && y.ProductSellers.Count() > 0 && y.ProductChannels.Count() > 0).Count(),
                AllProductList = attributeIds.Count() > 0 ? resultProduct.Where(y => y.ProductCategories.Count() > 0 && y.ProductSellers.Count() > 0 && y.ProductChannels.Count() > 0 && y.ProductAttributes.Count() > 0).ToList() : resultProduct.Where(y => y.ProductCategories.Count() > 0 && y.ProductSellers.Count() > 0 && y.ProductChannels.Count() > 0).ToList()
            };
            return result;
        }

        public async Task<List<ProductFilter>> GetProductListGroupByBrand(List<Guid> categoryId, List<List<Guid>> attritubeIds, Expression<Func<Product,
                                                                   bool>> expressionProduct, Expression<Func<ProductSeller, bool>> expressionProductSeller, List<Guid> bannedSellers, int productChannel)
        {
            var resultProduct = new List<Product>();
            var sortedList = new List<ProductSeller>();
            var count = new List<ProductFilter>();
            IQueryable<Product> counts;

            Expression<Func<Product, bool>> categoryExpression3 = null;
            Expression<Func<Product, bool>> productSellerExpression3 = null;

            Expression<Func<Product, bool>> productSellerExpression = x => x.ProductSellers.All(ps => ps.IsActive == true);
            Expression<Func<Product, bool>> productSellerExpression2 = x => x.ProductSellers.AsQueryable().Any(expressionProductSeller);
            if (bannedSellers != null)
            {
                productSellerExpression = x => x.ProductSellers.Any(ps => !bannedSellers.Contains(ps.SellerId));
            }

            productSellerExpression3 = ExpressionExtensions.And(productSellerExpression, productSellerExpression2);

            if (categoryId.Count > 0)
            {
                Expression<Func<Product, bool>> categoryExpression1 = x => x.ProductCategories.All(pc => pc.IsActive == true);
                if (categoryId == null || categoryId.Count == 0)
                {
                    categoryExpression3 = categoryExpression1;
                }
                else
                {
                    Expression<Func<Product, bool>> categoryExpression = x => x.ProductCategories.Any(pc => categoryId.Contains(pc.CategoryId));
                    categoryExpression3 = ExpressionExtensions.And(categoryExpression, categoryExpression1);
                }
            }

            counts = _entities.AsQueryable().AsNoTracking()
           .Include(b => b.Brand)
           .Include(pi => pi.ProductImages)
           .Include(pc => pc.ProductChannels.Where(c => c.ChannelCode == productChannel))
           .Include(pa => pa.ProductAttributes)
           .Include(pc => pc.ProductCategories)
           .Include(ps => ps.ProductSellers)
           .Where(expressionProduct)
           .Where(productSellerExpression3)
           .AsQueryable();

            if (categoryExpression3 != null)
            {
                counts = counts.Where(categoryExpression3).AsQueryable();
            }
            foreach (var item in attritubeIds)
            {
                counts = counts.Where(pa => pa.ProductAttributes.AsQueryable().Any(x => item.Contains(x.AttributeValueId)));
            }

            //count = await counts.Where(u => u.Brand.IsActive == true && u.IsActive == true).GroupBy(p => new { p.Brand.Name, p.BrandId })
            //.Select(g => new ProductFilter { Id = g.Key.BrandId.ToString(), Value = g.Key.Name, FilterField = ProductFilterEnum.BrandId.ToString(), Type = ProductFilterEnum.Product.ToString() })
            //.ToListAsync();
            return count;
        }


        public async Task<List<decimal>> GetProductListMaxPrice(List<Guid> categoryId, List<List<Guid>> attributeIds, Expression<Func<Product,
                                                                   bool>> expressionProduct, Expression<Func<ProductSeller, bool>> expressionProductSeller, List<Guid> bannedSellers, int productChannel, List<Guid> sellerList)
        {
            var resultProduct = new List<Product>();
            var sortedList = new List<ProductSeller>();
            var list = new List<Product>();

            Expression<Func<Product, bool>> categoryExpression3 = null;
            Expression<Func<Product, bool>> productSellerExpression3 = null;

            Expression<Func<Product, bool>> productSellerExpression = x => x.ProductSellers.All(ps => ps.IsActive == true);
            Expression<Func<Product, bool>> productSellerExpression2 = x => x.ProductSellers.AsQueryable().Any(expressionProductSeller);
            if (bannedSellers != null)
            {
                productSellerExpression = x => x.ProductSellers.Any(ps => !bannedSellers.Contains(ps.SellerId));
            }

            productSellerExpression3 = ExpressionExtensions.And(productSellerExpression, productSellerExpression2);


            Expression<Func<Product, bool>> categoryExpression1 = x => x.ProductCategories.All(pc => pc.IsActive == true);
            if (categoryId == null || categoryId.Count == 0)
            {
                categoryExpression3 = categoryExpression1;
            }
            else
            {
                Expression<Func<Product, bool>> categoryExpression = x => x.ProductCategories.Any(pc => categoryId.Contains(pc.CategoryId));
                categoryExpression3 = ExpressionExtensions.And(categoryExpression, categoryExpression1);
            }

            var price = _entities.AsQueryable().AsNoTracking()
                 .Include(b => b.Brand)
                 .Include(pi => pi.ProductImages)
                 .Include(pc => pc.ProductChannels)
                 .Include(pa => pa.ProductAttributes)
                 .Include(pc => pc.ProductCategories)
                 .Include(ps => ps.ProductSellers)
                 .Where(expressionProduct)
                 .Where(categoryExpression3)
                 .Where(productSellerExpression3)
                 .AsQueryable();

            foreach (var item in attributeIds)
            {
                price = price.Where(pa => pa.ProductAttributes.AsQueryable().Any(x => item.Contains(x.AttributeValueId)));
            }
            var result = new List<decimal>();
            //var result = await price.Where(o => o.IsActive == true).Select(p => p.ProductSellers.Where(p =>
            //    sellerList.Count() > 0 ? sellerList.Contains(p.SellerId) : p.IsActive == true).Select(u => u.SalePrice)).Where(o =>
            //    o.Count() > 0).Select(u => u.OrderBy(p => p).FirstOrDefault()).ToListAsync();

            return result;
        }

        public async Task<List<Guid>> GetProductListGroupBySeller(List<Guid> categoryId, List<List<Guid>> attributeIds, Expression<Func<Product,
                                                                   bool>> expressionProduct, Expression<Func<ProductSeller, bool>> expressionProductSeller, List<Guid> bannedSellers, int productChannel)
        {
            var resultProduct = new List<Product>();
            var sortedList = new List<ProductSeller>();

            Expression<Func<Product, bool>> categoryExpression3 = null;
            Expression<Func<Product, bool>> productSellerExpression3 = null;

            Expression<Func<Product, bool>> productSellerExpression = x => x.ProductSellers.All(ps => ps.IsActive == true);
            Expression<Func<Product, bool>> productSellerExpression2 = x => x.ProductSellers.AsQueryable().Any(expressionProductSeller);
            if (bannedSellers != null)
            {
                productSellerExpression = x => x.ProductSellers.Any(ps => !bannedSellers.Contains(ps.SellerId));
            }

            productSellerExpression3 = ExpressionExtensions.And(productSellerExpression, productSellerExpression2);


            Expression<Func<Product, bool>> categoryExpression1 = x => x.ProductCategories.All(pc => pc.IsActive == true);
            if (categoryId == null || categoryId.Count == 0)
            {
                categoryExpression3 = categoryExpression1;
            }
            else
            {
                Expression<Func<Product, bool>> categoryExpression = x => x.ProductCategories.Any(pc => categoryId.Contains(pc.CategoryId));
                categoryExpression3 = ExpressionExtensions.And(categoryExpression, categoryExpression1);
            }

            var count = _entities.AsQueryable().AsNoTracking()
               .Include(b => b.Brand)
               .Include(pi => pi.ProductImages)
               .Include(pc => pc.ProductChannels)
               .Include(pa => pa.ProductAttributes)
               .Include(pc => pc.ProductCategories)
               .Include(ps => ps.ProductSellers)
               .Where(expressionProduct)
               .Where(categoryExpression3)
               .Where(productSellerExpression3)
               .AsQueryable();


            foreach (var item in attributeIds)
            {
                count = count.Where(pa => pa.ProductAttributes.AsQueryable().Any(x => item.Contains(x.AttributeValueId)));
            }

            var list = new List<Guid>();
            //list = await count.Where(o => o.IsActive == true).SelectMany(p => p.ProductSellers.Where(u => u.IsActive == true).Select(s => s.SellerId))
            //   .GroupBy(y => y).Select(x => x.Key)
            //   .ToListAsync();

            return list;
        }

        public async Task<List<Product>> GetProductListForSeller(List<Guid> productList, Guid sellerId, string barcode, string productName, string brandName, OrderByDate orderBy, string groupCode)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(b => b.Brand)
                .Include(pa => pa.ProductCategories)
                .Include(pa => pa.ProductAttributes)
                .Include(pa => pa.ProductGroups.Where(p => !string.IsNullOrEmpty(groupCode) ? (p.GroupCode.Contains(groupCode) && !string.IsNullOrEmpty(p.GroupCode)) : p.IsActive))
                .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                .Where(ps => ps.ProductSellers.Any(t => t.SellerId == sellerId && t.IsActive))
                .Where(x => productList.Contains(x.Id))
                .Where(p => !string.IsNullOrEmpty(barcode) ? p.Code.Contains(barcode) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(productName) ? p.Name.Contains(productName) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(brandName) ? p.Brand.Name.Contains(brandName) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(groupCode) ? p.ProductGroups.Where(x => x.GroupCode.Contains(groupCode) && !string.IsNullOrEmpty(x.GroupCode)).ToList().Count > 0 : 1 == 1)
                //.OrderByDescending(u => orderBy == OrderByDate.UpdateDate ? u.ModifiedDate : u.CreatedDate)
                //.Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                //.Take(pagerInput.PageSize)
                .ToListAsync();
        }
        public async Task<List<Product>> GetProductListForSellerWithPaging(List<Guid> productList, Guid sellerId, string barcode, string productName, string brandName, OrderByDate orderBy, string groupCode, PagerInput pagerInput)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(b => b.Brand)
                .Include(pa => pa.ProductCategories)
                .Include(pa => pa.ProductAttributes)
                .Include(pa => pa.ProductGroups.Where(p => !string.IsNullOrEmpty(groupCode) ? (p.GroupCode.Contains(groupCode) && !string.IsNullOrEmpty(p.GroupCode)) : p.IsActive))
                .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                .Where(ps => ps.ProductSellers.Any(t => t.SellerId == sellerId && t.IsActive))
                .Where(x => productList.Contains(x.Id))
                .Where(p => !string.IsNullOrEmpty(barcode) ? p.Code.Contains(barcode) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(productName) ? p.Name.Contains(productName) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(brandName) ? p.Brand.Name.Contains(brandName) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(groupCode) ? p.ProductGroups.Where(x => x.GroupCode.Contains(groupCode) && !string.IsNullOrEmpty(x.GroupCode)).ToList().Count > 0 : 1 == 1)
                .OrderByDescending(u => orderBy == OrderByDate.UpdateDate ? u.ModifiedDate : u.CreatedDate)
                .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                .Take(pagerInput.PageSize)
                .ToListAsync();
        }
        public async Task<List<Product>> GetProductBySellerId(Guid sellerId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(g => g.ProductCategories)
                .Include(f => f.ProductSellers.Where(c => c.SellerId == sellerId && c.IsActive)).ToListAsync();
        }
        public async Task<List<Product>> GetProductSearchNameOrCode(string code, string productName, PagerInput pagerInput)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(b => b.Brand)
                .Include(pa => pa.ProductCategories)
                .Include(pa => pa.ProductAttributes)
                .Include(ps => ps.ProductSellers)
                .Include(pi => pi.ProductImages)
                .Where(p => !string.IsNullOrEmpty(code) ? p.Code.Contains(code) : p.IsActive)
                .Where(p => !string.IsNullOrEmpty(productName) ? p.Name.Contains(productName) : p.IsActive)
                .OrderBy(p => p.DisplayName).ThenByDescending(p => p.CreatedDate)
                .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                .Take(pagerInput.PageSize)
                .ToListAsync();
        }
        public async Task<ProductFavorite> GetFavoriteProductsByProductId(Guid ProductId, Guid Id)
        {
            var product = await _entities.AsQueryable()
                   .Include(b => b.Brand)
                   .Include(pa => pa.ProductImages.Where(t => t.IsActive))
                   .Include(pc => pc.ProductCategories.Where(p => p.IsActive))
                   .Include(pc => pc.ProductSellers)
                   .Where(p => p.Id == ProductId)
                   .FirstOrDefaultAsync();

            return new ProductFavorite { Product = product, FavoriteProductId = Id };
        }
        public async Task<Product> GetProductDetailToCreate(Guid productId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(pa => pa.ProductAttributes.Where(t => t.IsActive))
                .Include(pc => pc.ProductCategories.Where(t => t.IsActive))
                .Include(sp => sp.SimilarProducts.Where(t => t.IsActive))
                .Include(pd => pd.ProductDeliveries.Where(t => t.IsActive))
                .Include(pg => pg.ProductGroups.Where(t => t.IsActive))
                .Include(pi => pi.ProductImages.Where(t => t.IsActive))
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductIsBanners(List<Guid?> productSellerId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(g => g.ProductSellers.Where(c => productSellerId.Contains(c.Id)))
                .Include(f => f.ProductImages).Where(p => p.IsActive).ToListAsync();
        }

        public async Task<List<Product>> GetProductListWithCodes(List<string> codeList, List<Guid> sellerIdList)
        {
            var result = new List<Product>();
            if (sellerIdList.Any())
            {
                using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled);
                result = await _entities
                    .Include(pa => pa.ProductImages.Where(t => t.IsActive))
                    .Include(pa => pa.ProductSellers.Where(t => t.IsActive && sellerIdList.Contains(t.SellerId) && t.StockCount > 0))
                     .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Include(pa => pa.Brand)
                    .Where(p => codeList.Contains(p.Code) && p.IsActive)
                    .ToListAsync().ConfigureAwait(false);
            }
            else
            {
                using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled);
                result = await _entities
                    .Include(pa => pa.ProductImages.Where(t => t.IsActive))
                    .Include(pa => pa.ProductSellers.Where(t => t.IsActive && t.StockCount > 0))
                     .Include(pa => pa.ProductCategories.Where(t => t.IsActive))
                    .Include(pa => pa.Brand)
                    .Where(p => codeList.Contains(p.Code) && p.IsActive)
                    .ToListAsync().ConfigureAwait(false);
            }
            return result;
        }

        #region ProductFilter_SP

        public async Task<ProductListWithCountV2> GetProductAllRelationsSP(PagerInput pagerInput, List<Guid> categoryId, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList,
            List<Guid> searchList, OrderBy orderBy, List<Guid> bannedSellers, int productChannel,
            List<Guid> sellerList)
        {


            //var categoryList = String.Join(",", categoryId);
            //var attributeList = String.Join(",", attributeIds);
            //var salePrices = String.Join(",", salePriceList);
            //var brandIdsList = String.Join(",", brandIdList);
            //var codesList = String.Join(",", codeList);
            var searchsList = String.Join(",", searchList);
            //var bannedSellerss = String.Join(",", bannedSellers);
            //var sellersList = String.Join(",", sellerList);

            DataTable categoryTable = new DataTable();
            categoryTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in categoryId)
            {
                var row = categoryTable.NewRow();
                row["ID"] = item;
                categoryTable.Rows.Add(row);
            }

            DataTable attributeTable = new DataTable();
            attributeTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in attributeIds)
            {
                foreach (var item1 in item)
                {
                    var row = attributeTable.NewRow();
                    row["ID"] = item1;
                    attributeTable.Rows.Add(row);
                }
            }

            var salePriceStr = new StringBuilder("");
            if (salePriceList.Any())
            {
                salePriceStr = new StringBuilder("(");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        salePriceStr.Append(" OR ");
                    }
                    salePriceStr.AppendFormat("(p.SalePrice BETWEEN {0} AND {1})", item.Split(",")[0], item.Split(",")[1]);
                }
                salePriceStr.Append(")");
            }

            DataTable codeListTable = new DataTable();
            codeListTable.Columns.Add("ID", typeof(string));
            foreach (var item in codeList)
            {
                var row = codeListTable.NewRow();
                row["ID"] = item;
                codeListTable.Rows.Add(row);
            }

            DataTable brandIdListTable = new DataTable();
            brandIdListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in brandIdList)
            {
                var row = brandIdListTable.NewRow();
                row["ID"] = item;
                brandIdListTable.Rows.Add(row);
            }

            DataTable bannedSellerTable = new DataTable();
            bannedSellerTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in bannedSellers)
            {
                var row = bannedSellerTable.NewRow();
                row["ID"] = item;
                bannedSellerTable.Rows.Add(row);
            }

            DataTable sellerListTable = new DataTable();
            sellerListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in sellerList)
            {
                var row = sellerListTable.NewRow();
                row["ID"] = item;
                sellerListTable.Rows.Add(row);
            }

            var rowCount = new SqlParameter
            {
                ParameterName = "rowcount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,

            };


            var productList = await _entities.FromSqlRaw("exec SP_ProductFilterV9 @PageIndex,@PageSize,@CategoryIds,@AttributeIds,@SalePriceList,@BrandIdList,@CodeList,@SearchList,@OrderBy,@BannedSellers,@ProductChannel,@SellerList,@rowcount OUTPUT ",
                    new SqlParameter("@PageIndex", (pagerInput.PageIndex - 1) * pagerInput.PageSize),
                    new SqlParameter("@PageSize", pagerInput.PageSize),
                    new SqlParameter { Value = categoryTable, SqlDbType = SqlDbType.Structured, ParameterName = "CategoryIds", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter { Value = attributeTable, SqlDbType = SqlDbType.Structured, ParameterName = "AttributeIds", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter { Value = salePriceStr.ToString(), SqlDbType = SqlDbType.NVarChar, ParameterName = "SalePriceList" },
                    new SqlParameter { Value = brandIdListTable, SqlDbType = SqlDbType.Structured, ParameterName = "BrandIdList", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter { Value = codeListTable, SqlDbType = SqlDbType.Structured, ParameterName = "CodeList", TypeName = "[dbo].[VarcharTableType]" },
                    new SqlParameter { Value = "", SqlDbType = SqlDbType.NVarChar, ParameterName = "SearchList" },
                    // new SqlParameter("@SearchList", String.IsNullOrEmpty(searchsList) ? DBNull.Value : searchsList).SqlDbType == SqlDbType.NVarChar,
                    new SqlParameter("@OrderBy", (int)orderBy),
                    new SqlParameter { Value = bannedSellerTable, SqlDbType = SqlDbType.Structured, ParameterName = "BannedSellers", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter("@ProductChannel", productChannel),
                    new SqlParameter { Value = sellerListTable, SqlDbType = SqlDbType.Structured, ParameterName = "SellerList", TypeName = "[dbo].[IdTableType]" },
                    rowCount
                )
                .ToListAsync();

            return new ProductListWithCountV2()
            {
                ProductList = productList,
                TotalCount = (int)rowCount?.Value
            };



        }

        public async Task<ProductListWithCountV2> GetProductAllRelationsSP(string productFilterQuery, string productFilterCountQuery)
        {

            var rowCount = new SqlParameter
            {
                ParameterName = "rowcount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,

            };

            var productList = await _entities.FromSqlRaw("exec SP_FilterProducts @ProductFilterCountQuery,@ProductFilterQuery,@rowcount OUTPUT ",
                    new SqlParameter { Value = productFilterCountQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "ProductFilterCountQuery" },
                    new SqlParameter { Value = productFilterQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "ProductFilterQuery" },
                    rowCount
                )
                .ToListAsync();

            return new ProductListWithCountV2()
            {
                ProductList = productList,
                TotalCount = (int)rowCount?.Value
            };
        }

        #endregion

        public async Task<List<BrandFilter>> GetProductBrandFilter(string brandFilterQuery)
        {
            var attributeFilterList = await _dbContext.Set<BrandFilter>().FromSqlRaw("exec SP_GetFilterGenericList @FilterGenericListQuery ",
                    new SqlParameter { Value = brandFilterQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterGenericListQuery" }
                )
                .ToListAsync();
            return attributeFilterList;
        }

        public async Task<List<RelatedCategories>> GetProductCategoryFilter(string categoryFilterQuery)
        {
            var attributeFilterList = await _dbContext.Set<RelatedCategories>().FromSqlRaw("exec SP_GetFilterGenericList @FilterGenericListQuery ",
                    new SqlParameter { Value = categoryFilterQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterGenericListQuery" }
                )
                .ToListAsync();
            return attributeFilterList;
        }

        public async Task<List<PriceFilter>> GetProductPriceFilter(string priceFilterQuery)
        {
            var priceFilterList = await _dbContext.Set<PriceFilter>().FromSqlRaw("exec SP_GetFilterGenericList @FilterGenericListQuery ",
                    new SqlParameter { Value = priceFilterQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterGenericListQuery" }
                )
                .ToListAsync();
            return priceFilterList;
        }

        public async Task<List<SellerFilter>> GetProductSellerFilter(string sellerFilterQuery)
        {
            var sellerFilterList = await _dbContext.Set<SellerFilter>().FromSqlRaw("exec SP_GetFilterGenericList @FilterGenericListQuery ",
                    new SqlParameter { Value = sellerFilterQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterGenericListQuery" }
                )
                .ToListAsync();
            return sellerFilterList;
        }

        public async Task<List<XmlProduct>> GetXmlProducts(string productSellerIds, string sellerDeliveryIds)
        {
            return await _dbContext.Set<XmlProduct>().FromSqlRaw("exec GetXmlProducts @FilterProductSellers,@FilterSellerDelivery",
                    new SqlParameter { Value = productSellerIds, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterProductSellers" },
                    new SqlParameter { Value = sellerDeliveryIds, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterSellerDelivery" }).ToListAsync();
        }

        public async Task<List<XmlAttribute>> GetXmlAttributes(string productIds)
        {
            return await _dbContext.Set<XmlAttribute>().FromSqlRaw("exec GetXmlAttributes @FilterAttributes",
                    new SqlParameter { Value = productIds, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterAttributes" }).ToListAsync();
        }

        public async Task<List<BrandFilter>> GetProductListGroupByBrandSP(List<Guid> categoryId, List<List<Guid>> attritubeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, List<Guid> bannedSellers, int productChannel, List<Guid> sellerIdList)
        {
            DataTable categoryTable = new DataTable();
            categoryTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in categoryId)
            {
                var row = categoryTable.NewRow();
                row["ID"] = item;
                categoryTable.Rows.Add(row);
            }

            DataTable attributeTable = new DataTable();
            attributeTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in attritubeIds)
            {
                foreach (var item1 in item)
                {
                    var row = attributeTable.NewRow();
                    row["ID"] = item1;
                    attributeTable.Rows.Add(row);
                }
            }

            var salePriceStr = new StringBuilder("");
            if (salePriceList.Any())
            {
                salePriceStr = new StringBuilder("(");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        salePriceStr.Append(" OR ");
                    }
                    salePriceStr.AppendFormat("(p.SalePrice BETWEEN {0} AND {1})", item.Split(",")[0], item.Split(",")[1]);
                }
                salePriceStr.Append(")");
            }

            DataTable codeListTable = new DataTable();
            codeListTable.Columns.Add("ID", typeof(string));
            foreach (var item in codeList)
            {
                var row = codeListTable.NewRow();
                row["ID"] = item;
                codeListTable.Rows.Add(row);
            }

            DataTable brandIdListTable = new DataTable();
            brandIdListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in brandIdList)
            {
                var row = brandIdListTable.NewRow();
                row["ID"] = item;
                brandIdListTable.Rows.Add(row);
            }

            DataTable bannedSellerTable = new DataTable();
            bannedSellerTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in bannedSellers)
            {
                var row = bannedSellerTable.NewRow();
                row["ID"] = item;
                bannedSellerTable.Rows.Add(row);
            }

            DataTable sellerListTable = new DataTable();
            sellerListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in sellerIdList)
            {
                var row = sellerListTable.NewRow();
                row["ID"] = item;
                sellerListTable.Rows.Add(row);
            }



            var brandList = await _dbContext.Set<BrandFilter>().FromSqlRaw("exec SP_ProductBrandFilterV2  @CategoryIds,@AttributeIds,@SalePriceList,@BrandIdList,@CodeList,@SearchList,@BannedSellers,@ProductChannel,@SellerList ",
                new SqlParameter { Value = categoryTable, SqlDbType = SqlDbType.Structured, ParameterName = "CategoryIds", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter { Value = attributeTable, SqlDbType = SqlDbType.Structured, ParameterName = "AttributeIds", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter { Value = salePriceStr.ToString(), SqlDbType = SqlDbType.NVarChar, ParameterName = "SalePriceList" },
                new SqlParameter { Value = brandIdListTable, SqlDbType = SqlDbType.Structured, ParameterName = "BrandIdList", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter { Value = codeListTable, SqlDbType = SqlDbType.Structured, ParameterName = "CodeList", TypeName = "[dbo].[VarcharTableType]" },
                new SqlParameter { Value = "", SqlDbType = SqlDbType.NVarChar, ParameterName = "SearchList" },
                new SqlParameter { Value = bannedSellerTable, SqlDbType = SqlDbType.Structured, ParameterName = "BannedSellers", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter("@ProductChannel", productChannel),
                new SqlParameter { Value = sellerListTable, SqlDbType = SqlDbType.Structured, ParameterName = "SellerList", TypeName = "[dbo].[IdTableType]" }).ToListAsync();

            return brandList;
        }


        public async Task<List<SellerFilter>> GetProductListGroupBySellerSP(List<Guid> categoryId, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, List<Guid> bannedSellers, int productChannel, List<Guid> sellerIdList)
        {

            DataTable categoryTable = new DataTable();
            categoryTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in categoryId)
            {
                var row = categoryTable.NewRow();
                row["ID"] = item;
                categoryTable.Rows.Add(row);
            }

            DataTable attributeTable = new DataTable();
            attributeTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in attributeIds)
            {
                foreach (var item1 in item)
                {
                    var row = attributeTable.NewRow();
                    row["ID"] = item1;
                    attributeTable.Rows.Add(row);
                }
            }

            var salePriceStr = new StringBuilder("");
            if (salePriceList.Any())
            {
                salePriceStr = new StringBuilder("(");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        salePriceStr.Append(" OR ");
                    }
                    salePriceStr.AppendFormat("(p.SalePrice BETWEEN {0} AND {1})", item.Split(",")[0], item.Split(",")[1]);
                }
                salePriceStr.Append(")");
            }

            DataTable codeListTable = new DataTable();
            codeListTable.Columns.Add("ID", typeof(string));
            foreach (var item in codeList)
            {
                var row = codeListTable.NewRow();
                row["ID"] = item;
                codeListTable.Rows.Add(row);
            }

            DataTable brandIdListTable = new DataTable();
            brandIdListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in brandIdList)
            {
                var row = brandIdListTable.NewRow();
                row["ID"] = item;
                brandIdListTable.Rows.Add(row);
            }

            DataTable bannedSellerTable = new DataTable();
            bannedSellerTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in bannedSellers)
            {
                var row = bannedSellerTable.NewRow();
                row["ID"] = item;
                bannedSellerTable.Rows.Add(row);
            }

            DataTable sellerListTable = new DataTable();
            sellerListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in sellerIdList)
            {
                var row = sellerListTable.NewRow();
                row["ID"] = item;
                sellerListTable.Rows.Add(row);
            }

            var sellerList = await _dbContext.Set<SellerFilter>().FromSqlRaw("exec SP_ProductSellerFilterV2 @CategoryIds,@AttributeIds,@SalePriceList,@BrandIdList,@CodeList,@SearchList,@BannedSellers,@ProductChannel,@SellerList ",
                new SqlParameter { Value = categoryTable, SqlDbType = SqlDbType.Structured, ParameterName = "CategoryIds", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter { Value = attributeTable, SqlDbType = SqlDbType.Structured, ParameterName = "AttributeIds", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter { Value = salePriceStr.ToString(), SqlDbType = SqlDbType.NVarChar, ParameterName = "SalePriceList" },
                new SqlParameter { Value = brandIdListTable, SqlDbType = SqlDbType.Structured, ParameterName = "BrandIdList", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter { Value = codeListTable, SqlDbType = SqlDbType.Structured, ParameterName = "CodeList", TypeName = "[dbo].[VarcharTableType]" },
                new SqlParameter { Value = "", SqlDbType = SqlDbType.NVarChar, ParameterName = "SearchList" },
                new SqlParameter { Value = bannedSellerTable, SqlDbType = SqlDbType.Structured, ParameterName = "BannedSellers", TypeName = "[dbo].[IdTableType]" },
                new SqlParameter("@ProductChannel", productChannel),
                new SqlParameter { Value = sellerListTable, SqlDbType = SqlDbType.Structured, ParameterName = "SellerList", TypeName = "[dbo].[IdTableType]" }).ToListAsync();

            return sellerList;
        }

        public async Task<List<PriceFilter>> GetProductListMaxPriceSP(List<Guid> categoryId, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, List<Guid> bannedSellers, int productChannel, List<Guid> sellerIdList)
        {

            DataTable categoryTable = new DataTable();
            categoryTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in categoryId)
            {
                var row = categoryTable.NewRow();
                row["ID"] = item;
                categoryTable.Rows.Add(row);
            }

            DataTable attributeTable = new DataTable();
            attributeTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in attributeIds)
            {
                foreach (var item1 in item)
                {
                    var row = attributeTable.NewRow();
                    row["ID"] = item1;
                    attributeTable.Rows.Add(row);
                }
            }

            var salePriceStr = new StringBuilder("");
            if (salePriceList.Any())
            {
                salePriceStr = new StringBuilder("(");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        salePriceStr.Append(" OR ");
                    }
                    salePriceStr.AppendFormat("(p.SalePrice BETWEEN {0} AND {1})", item.Split(",")[0], item.Split(",")[1]);
                }
                salePriceStr.Append(")");
            }

            DataTable codeListTable = new DataTable();
            codeListTable.Columns.Add("ID", typeof(string));
            foreach (var item in codeList)
            {
                var row = codeListTable.NewRow();
                row["ID"] = item;
                codeListTable.Rows.Add(row);
            }

            DataTable brandIdListTable = new DataTable();
            brandIdListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in brandIdList)
            {
                var row = brandIdListTable.NewRow();
                row["ID"] = item;
                brandIdListTable.Rows.Add(row);
            }

            DataTable bannedSellerTable = new DataTable();
            bannedSellerTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in bannedSellers)
            {
                var row = bannedSellerTable.NewRow();
                row["ID"] = item;
                bannedSellerTable.Rows.Add(row);
            }

            DataTable sellerListTable = new DataTable();
            sellerListTable.Columns.Add("ID", typeof(Guid));
            foreach (var item in sellerIdList)
            {
                var row = sellerListTable.NewRow();
                row["ID"] = item;
                sellerListTable.Rows.Add(row);
            }

            var priceFilterList = await _dbContext.Set<PriceFilter>().FromSqlRaw("exec SP_ProductSalePriceFilterV2 @CategoryIds,@AttributeIds,@SalePriceList,@BrandIdList,@CodeList,@SearchList,@BannedSellers,@ProductChannel,@SellerList ",
                    new SqlParameter { Value = categoryTable, SqlDbType = SqlDbType.Structured, ParameterName = "CategoryIds", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter { Value = attributeTable, SqlDbType = SqlDbType.Structured, ParameterName = "AttributeIds", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter { Value = salePriceStr.ToString(), SqlDbType = SqlDbType.NVarChar, ParameterName = "SalePriceList" },
                    new SqlParameter { Value = brandIdListTable, SqlDbType = SqlDbType.Structured, ParameterName = "BrandIdList", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter { Value = codeListTable, SqlDbType = SqlDbType.Structured, ParameterName = "CodeList", TypeName = "[dbo].[VarcharTableType]" },
                    new SqlParameter { Value = "", SqlDbType = SqlDbType.NVarChar, ParameterName = "SearchList" },
                    new SqlParameter { Value = bannedSellerTable, SqlDbType = SqlDbType.Structured, ParameterName = "BannedSellers", TypeName = "[dbo].[IdTableType]" },
                    new SqlParameter("@ProductChannel", productChannel),
                    new SqlParameter { Value = sellerListTable, SqlDbType = SqlDbType.Structured, ParameterName = "SellerList", TypeName = "[dbo].[IdTableType]" }
                )
                .ToListAsync();
            return priceFilterList;
        }
        public async Task<List<Product>> GetProductListForSellerAndFilterByCode(List<Guid> productList, Guid sellerId, string code, PagerInput pagerInput)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(b => b.Brand)
                .Include(pa => pa.ProductCategories)
                .Include(pa => pa.ProductAttributes)
                .Include(pa => pa.ProductGroups.Where(p => p.IsActive))
                .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                .Where(x => productList.Contains(x.Id))
                .Where(p => !string.IsNullOrEmpty(code) ? p.Code.Contains(code) : p.IsActive)
                .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
                .Take(pagerInput.PageSize)
                .ToListAsync();

        }
        public async Task<int> GetProductListForSellerAndFilterByCodeTotalCount(List<Guid> productList, Guid sellerId, string code)
        {
            return _entities.AsQueryable().AsNoTracking()
                .Include(b => b.Brand)
                .Include(pa => pa.ProductCategories)
                .Include(pa => pa.ProductAttributes)
                .Include(pa => pa.ProductGroups.Where(p => p.IsActive))
                .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                .Where(x => productList.Contains(x.Id))
                .Where(p => !string.IsNullOrEmpty(code) ? p.Code.Contains(code) : p.IsActive).Count();

        }
        public async Task<Product> GetProductWithProductSellerId(Guid productSellerId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(j => j.ProductSellers)
                .Where(h => h.IsActive)
                .Where(ps => ps.ProductSellers.Where(p => p.Id == productSellerId && p.IsActive).ToList().Count > 0)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductListForSellerForCount(List<Guid> productList, Guid sellerId)
        {
            return await _entities.AsQueryable().AsNoTracking()
                   .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                   .Where(ps => ps.ProductSellers.Any(t => t.SellerId == sellerId && t.IsActive))
                   .Where(x => productList.Contains(x.Id))
                   .Where(p => p.IsActive)
                   .ToListAsync();
        }
        public async Task<Product> GetProductSellerInfo(Guid sellerId, string code)
        {
            return await _entities.AsQueryable().AsNoTracking()
                    .Include(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive))
                    .Where(p => p.Code == code && p.IsActive)
                    .Where(ps => ps.ProductSellers.Where(p => p.SellerId == sellerId && p.IsActive).ToList().Count > 0)
                    .FirstOrDefaultAsync();

        }
        public async Task<List<ProductCountForBackoffice>> GetProductListForSellerForCountSP(Guid sellerId)
        {
            var productList = await _dbContext.Set<ProductCountForBackoffice>().FromSqlRaw("exec SP_GetProductCountListForSeller @SellerId",
                   new SqlParameter { Value = sellerId, SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "SellerId" }
               )
               .ToListAsync();
            return productList;
        }

    }
}
