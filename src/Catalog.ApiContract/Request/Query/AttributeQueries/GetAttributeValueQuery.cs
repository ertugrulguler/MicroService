using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.AttributeQueries
{
    public class GetAttributeValueQuery : IRequest<ResponseBase<List<AttributeValueDto>>>
    {
        public Guid? AttributeId { get; set; }
    }
}
