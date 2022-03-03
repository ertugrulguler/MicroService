using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BrandQueries
{
    public class GetBrandQuery : IRequest<ResponseBase<List<BrandDto>>>

    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
    }
}
