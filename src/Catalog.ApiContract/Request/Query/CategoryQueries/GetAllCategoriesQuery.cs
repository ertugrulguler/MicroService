using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetAllCategoriesQuery : IRequest<ResponseBase<List<CategoryIdAndName>>>
    {
        public List<Guid> CategoryIdList { get; set; }

    }


}
