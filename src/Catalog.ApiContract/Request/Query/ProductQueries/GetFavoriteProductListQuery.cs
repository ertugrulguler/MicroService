using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetFavoriteProductListQuery : IRequest<ResponseBase<FavoriteProductList>>
    {
        public PagerInput PagerInput { get; set; }
    }
}
