using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetIdNumberControlQuery : IRequest<ResponseBase<GetIdNumberControlQueryResult>>
    {
        public List<GetIdNumberControlInfo> GetIdNumberControlInfos { get; set; }
    }
    public class GetIdNumberControlInfo
    {
        public Guid SellerId { get; set; }
        public Guid CategoryId { get; set; }
        public Price TotalPrice { get; set; }
    }
}
