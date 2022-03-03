using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetActiveCategoryByCategoryId : IRequest<ResponseBase<GetActiveCategoryByCategoryIdResult>>
    {
        public Guid CategoryId { get; set; }

    }

}