using Catalog.ApiContract.Response.Query.AttributeQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.AttributeQueries
{
    public class GetAllAttributeNameWithValuesQuery : IRequest<ResponseBase<GetAllAttributeNameWithValues>>
    {
        public List<Guid> AttributeIdList { get; set; }
    }

}
