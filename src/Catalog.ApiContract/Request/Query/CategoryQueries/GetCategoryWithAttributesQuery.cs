using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoryWithAttributesQuery : IRequest<ResponseBase<GetCategoryWithAttributes>>
    {
        public Guid Id { get; set; }
        public bool OnlyRequiredFields { get; set; }
    }
}
