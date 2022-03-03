using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ResponseBase<ProductDetail>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IBrandRepository _brandRepository;

        public GetProductQueryHandler(IProductRepository productRepository, IProductAssembler productAssembler,
             IProductImageRepository productImageRepository,
            IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _productImageRepository = productImageRepository;
            _brandRepository = brandRepository;
        }

        public async Task<ResponseBase<ProductDetail>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByCategoryIdWithAllRelations(request.Id);
            if (product == null)
            {
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                    ApplicationMessage.ProductNotFound.Message(),
                    ApplicationMessage.ProductNotFound.UserMessage());
            }

            await product.LoadBrand(product.BrandId, _brandRepository);

            if (product.ProductImages.Where(y => y.IsDefault == true).FirstOrDefault() != null)
                await product.LoadProductImage(product.ProductImages.Where(y => y.IsDefault == true).FirstOrDefault(), _productImageRepository);

            return _productAssembler.MapToGetProductQueryResult(product);

        }
    }
}
