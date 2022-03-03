using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoryParentsNamesQuery : IRequest<ResponseBase<Dictionary<Guid, string>>>
    {
        public List<Guid> CategoryIds { get; set; }
    }
}
