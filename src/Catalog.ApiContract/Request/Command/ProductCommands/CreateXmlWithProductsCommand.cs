using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class CreateXmlWithProductsCommand : IRequest<ResponseBase<string>>
    {
        public List<FeedProductDto> FeedProductDtos { get; set; }

    }

    public class FeedProductDto
    {
        public string ProductSeller { get; set; }
        public string SellerDelivery { get; set; }
    }

}
