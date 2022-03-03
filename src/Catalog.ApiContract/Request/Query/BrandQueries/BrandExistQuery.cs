using Catalog.ApiContract.Response.Query.BrandQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.BrandQueries
{
    public class BrandExistQuery : IRequest<ResponseBase<BrandExist>>
    {
        public Guid Id { get; set; }
    }
}
