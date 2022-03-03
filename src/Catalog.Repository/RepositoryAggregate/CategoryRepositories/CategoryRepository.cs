using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.CategoryRepositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogDbContext context) : base(context)
        {
        }
        public async Task<Category> GetCategoryWithAllRelations(Guid id)
        {
            return await _entities.AsQueryable()
                 .Include(c => c.CategoryAttributes.Where(t => t.IsActive))
                 .ThenInclude(ca => ca.Attribute)
                 .ThenInclude(am => am.AttributeMaps)
                 .Where(d => d.Id == id)
                 .FirstOrDefaultAsync();
        }
        public async Task<List<Category>> GetCategorySearchOptimization(DateTime createdDate)
        {
            return await _entities.AsQueryable()
                .Where(p => p.ModifiedDate > createdDate && p.IsActive && p.Type == CategoryTypeEnum.MainCategory & p.Type == CategoryTypeEnum.Virtual)
                .OrderByDescending(k => k.ModifiedDate)
                .ToListAsync();
        }
        public async Task<List<Category>> AllCategoryListWithRelations()
        {
            return await _entities.AsQueryable()
                 .Include(c => c.CategoryAttributes.Where(t => t.IsActive))
                 .ThenInclude(ca => ca.Attribute).ToListAsync();
        }

        public async Task<Category> GetCategoryWithAttributeValues(Guid id)
        {
            return await _entities.AsQueryable()
                 .Include(c => c.CategoryAttributes.Where(t => t.IsActive))
                 .ThenInclude(ca => ca.Attribute)
                 .ThenInclude(av => av.AttributeValues.Where(t => t.IsActive))
                 .Where(d => d.Id == id)
                 .FirstOrDefaultAsync();
        }

        public async Task<List<Category>> GetCategoryAndImageBySellerId(List<Guid> categoryIds)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Include(ff => ff.CategoryImage)
                .Where(o => categoryIds.Contains(o.Id) && o.IsActive).ToListAsync();
        }
        public async Task<Category> GetCategoryWithAttributeMap(Guid id)
        {
            return await _entities.AsQueryable()
                .Include(c => c.CategoryAttributes.Where(t => t.IsActive))
                .ThenInclude(ca => ca.Attribute)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Guid>> NotExistingCategories(List<Guid> categoryIdList)
        {
            var notExistList = new List<Guid>();
            var exist = await _entities.AsQueryable().AsNoTracking()
                       .Where(y => categoryIdList.Contains(y.Id))
                       .Where(l => l.IsActive == true)
                       .Select(p => p.Id)
                       .ToListAsync();

            foreach (var item in categoryIdList)
            {
                if (!exist.Contains(item))
                {
                    notExistList.Add(item);
                }
            }
            return notExistList;
        }

        public async Task<List<RelatedCategories>> GetProductCategoryFilter(List<Guid> categoryId,
            List<Guid> sellerIdList, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList,
            List<Guid> searchList, List<Guid> bannedSellers, int productChannel,
            List<Guid> sellerList)
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
            foreach (var item in sellerList)
            {
                var row = sellerListTable.NewRow();
                row["ID"] = item;
                sellerListTable.Rows.Add(row);
            }


            var categoryFilterList = await _dbContext.Set<RelatedCategories>().FromSqlRaw("exec SP_ProductCategoryFilterV2 @CategoryIds,@AttributeIds,@SalePriceList,@BrandIdList,@CodeList,@SearchList,@BannedSellers,@ProductChannel,@SellerList ",
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


            return categoryFilterList;
        }
        public async Task<List<string>> GetCategoryNamesByCategoryId(List<Guid> categoryIds)
        {
            return await _entities.AsQueryable().AsNoTracking()
                .Where(o => categoryIds.Contains(o.Id) && o.IsActive).Select(j => j.Name).Reverse().ToListAsync();
        }
        public async Task<Dictionary<string, Guid>> GetContainsCategories(string name)
        {
            var categoryNameDic = new Dictionary<string, Guid>();

            var i = await _entities.Where(s => s.SeoName == name && s.IsActive).FirstOrDefaultAsync();
            if (i != null)
            {
                categoryNameDic.Add(i.Code, i.Id);
            }

            return categoryNameDic;
        }
    }
}