using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoriesForMobileQuery : CachedQuery, IRequest<ResponseBase<CategoryResultForMobile>>
    {
        public override string CacheKey => nameof(GetCategoriesForMobileQuery);
    }
}