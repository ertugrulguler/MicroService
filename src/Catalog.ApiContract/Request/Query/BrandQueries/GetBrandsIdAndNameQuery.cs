using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BrandQueries
{
    public class GetBrandsIdAndNameQuery : IRequest<ResponseBase<List<BrandDto>>>
    {
        public List<Guid> BrandIdList { get; set; }

    }
}
