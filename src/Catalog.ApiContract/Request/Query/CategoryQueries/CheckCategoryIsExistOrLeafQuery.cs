using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class CheckCategoryIsExistOrLeafQuery : IRequest<ResponseBase<object>>
    {
        public Guid CategoryId { get; set; }
    }
}
