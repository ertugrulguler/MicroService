using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductDetailToCreateQueryHandler : IRequestHandler<GetProductDetailToCreateQuery, ResponseBase<GetProductDetailToCreate>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IProductSellerRepository _productSellerRepository;

        public GetProductDetailToCreateQueryHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IProductAssembler productAssembler,
            IBrandRepository brandRepository,
            IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository, IProductSellerRepository productSellerRepository)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _productSellerRepository = productSellerRepository;
        }

        public async Task<ResponseBase<GetProductDetailToCreate>> Handle(GetProductDetailToCreateQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductDetailToCreate(request.ProductId);

            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                ApplicationMessage.EmptyList.Message(),
                ApplicationMessage.EmptyList.UserMessage());

            var productSeller = await _productSellerRepository.FindByAsync(ps => ps.ProductId == product.Id && ps.SellerId == request.NewSellerId);
            if (productSeller != null)
                throw new BusinessRuleException(ApplicationMessage.ProductSellerAlreadyExist,
                ApplicationMessage.ProductSellerAlreadyExist.Message(),
                ApplicationMessage.ProductSellerAlreadyExist.UserMessage());


            var attributes = await _attributeRepository.FilterByAsync(z => product.ProductAttributes
                .Select(xx => xx.AttributeId).Contains(z.Id));
            var attributeValues = await _attributeValueRepository.FilterByAsync(x => product.ProductAttributes
                .Select(pa => pa.AttributeValueId).Contains(x.Id));
            var categories = await _categoryRepository.FilterByAsync(z => product.ProductCategories
                .Select(xx => xx.CategoryId).Contains(z.Id));


            return _productAssembler.MapToGetProductDetailToCreateQueryResult(product, categories, attributes, attributeValues);

        }
    }

}
