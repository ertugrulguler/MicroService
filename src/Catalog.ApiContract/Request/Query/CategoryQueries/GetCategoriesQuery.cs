using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoriesQuery : CachedQuery, IRequest<ResponseBase<GetCategoriesResult>>
    {
        public Guid CategoryId { get; set; }
        public override string CacheKey => nameof(GetCategoriesQuery);
    }
}