using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class CategoryIsExistQuery : IRequest<ResponseBase<CategoryIsExistQueryResult>>
    {
        public Guid CategoryId { get; set; }
    }
}
