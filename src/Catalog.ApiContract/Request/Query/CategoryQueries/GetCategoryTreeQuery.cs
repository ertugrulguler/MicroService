using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoryTreeQuery : CachedQuery, IRequest<ResponseBase<List<CategoryTree>>>
    {
        public override string CacheKey => nameof(GetCategoryTreeQuery);
    }
}