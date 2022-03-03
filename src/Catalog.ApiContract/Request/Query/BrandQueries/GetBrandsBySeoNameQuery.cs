using Catalog.ApiContract.Response.Query.BrandQueries;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BrandQueries
{
    public class GetBrandsBySeoNameQuery : IRequest<ResponseBase<GetBrandIdAndSeoNameQuery>>
    {
        public List<string> BrandNameList { get; set; }
    }
}