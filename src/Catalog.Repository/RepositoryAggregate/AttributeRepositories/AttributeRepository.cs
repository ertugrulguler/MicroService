using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.ValueObject.StoreProcedure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.AttributeRepositories
{
    public class AttributeRepository : GenericRepository<Domain.AttributeAggregate.Attribute>, IAttributeRepository
    {
        public AttributeRepository(CatalogDbContext context) : base(context)
        {
        }
        public async Task<List<Guid>> GetAttributeMapList(List<Guid> attIdList)
        {
            var list = new List<Guid>();
            if (attIdList.Count > 0)
            {
                list = await _entities.AsQueryable()
                             .Where(p => attIdList.Contains(p.Id) && p.Code != null)
                             .Where(f => f.IsActive)
                             .Select(t => t.Id)
                             .ToListAsync();
            }
            return list;
        }

        public async Task<List<AttributeFilter>> GetProductAttributeFilter(List<Guid> categoryId,
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

            var attributeFilterList = await _dbContext.Set<AttributeFilter>().FromSqlRaw("exec SP_ProductAttributeFilterV2 @CategoryIds,@AttributeIds,@SalePriceList,@BrandIdList,@CodeList,@SearchList,@BannedSellers,@ProductChannel,@SellerList ",
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
            return attributeFilterList;
        }
        public async Task<List<AttributeFilter>> GetProductAttributeFilter(string attributeFilterQuery)
        {
            var attributeFilterList = await _dbContext.Set<AttributeFilter>().FromSqlRaw("exec SP_GetFilterGenericList @FilterGenericListQuery ",
                    new SqlParameter { Value = attributeFilterQuery, SqlDbType = SqlDbType.NVarChar, ParameterName = "FilterGenericListQuery" }
                )
                .ToListAsync();
            return attributeFilterList;
        }
    }
}