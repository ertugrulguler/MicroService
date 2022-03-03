using Catalog.ApiContract.Response.Query.AttributeQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.AttributeQueries
{
    public class GetAttributeAndValueWithCategoryQuery : IRequest<ResponseBase<GetAttributeAndValueQueryResult>>
    {
        public Guid CategoryId { get; set; }
    }
}
