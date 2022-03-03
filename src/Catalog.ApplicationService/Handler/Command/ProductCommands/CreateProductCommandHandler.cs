using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApplicationService.AutoMapper;
using Catalog.ApplicationService.Communicator.Parameter;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Core.Model.Enums;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseBase<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IProductGroupVariantRepository _productGroupVariantRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAutoMapperConfiguration _autoMapper;
        private readonly IProductDomainService _productDomainService;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IParameterCommunicator _parameterCommunicator;
        private readonly IProductChannelRepository _productChannelRepository;

        public CreateProductCommandHandler(IProductRepository productRepository,
            ICategoryAttributeRepository categoryAttributeRepository,
            IProductGroupVariantRepository productGroupVariantRepository,
            IDbContextHandler dbContextHandler,
            IAutoMapperConfiguration autoMapper,
            IProductDomainService productDomainService, IProductCategoryRepository productCategoryRepository, IProductGroupRepository productGroupRepository, IParameterCommunicator parameterCommunicator, IProductChannelRepository productChannelRepository)
        {
            _productRepository = productRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _productGroupVariantRepository = productGroupVariantRepository;
            _dbContextHandler = dbContextHandler;
            _autoMapper = autoMapper;
            _productDomainService = productDomainService;
            _productCategoryRepository = productCategoryRepository;
            _productGroupRepository = productGroupRepository;
            _parameterCommunicator = parameterCommunicator;
            _productChannelRepository = productChannelRepository;
        }

        public async Task<ResponseBase<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            ProductAddOrUpdate addOrUpdate = ProductAddOrUpdate.Update;

            var product = await _productRepository.GetProductWithAllRelations(request.Code);

            if (product == null)
            {
                addOrUpdate = ProductAddOrUpdate.Add;

                product = new Product(request.Name, request.DisplayName, request.Description,
                    request.Code, request.BrandId, request.PriorityRank, request.ProductMainId, request.Desi, request.VatRate);
            }
            if (addOrUpdate == ProductAddOrUpdate.Update && !product.ProductCategories.Where(h => h.CategoryId == request.CategoryId && h.IsActive).Any())
            {
                throw new BusinessRuleException(ApplicationMessage.SameCodeDiffrentCategory,
                    ApplicationMessage.SameCodeDiffrentCategory.Message(),
                    ApplicationMessage.SameCodeDiffrentCategory.UserMessage());
            }
            product.VerifyOrAddProductAttributes(_autoMapper.MapCollection<ProductAttributeDto, ProductAttribute>(request.ProductAttributes), request.IsAdminUser);
            product.VerifyOrAddProductCategory(request.CategoryId, request.IsAdminUser);
            product.VerifyOrAddProductSellers(_autoMapper.MapCollection<ProductSellerDto, ProductSeller>(request.ProductSellers));
            product.VerifyOrAddProductImage(_autoMapper.MapCollection<ProductImageDto, ProductImage>(request.ProductImages));
            product.VerifyOrAddSimilarProducts(_autoMapper.MapCollection<SimilarProductDto, SimilarProduct>(request.SimilarProducts));
            product.VerifyOrAddProductDelivery(_autoMapper.MapCollection<ProductDeliveryDto, ProductDelivery>(request.ProductDeliveries));

            if (addOrUpdate == ProductAddOrUpdate.Update)
            {
                //if(request.IsAdminUser)
                //    product.SetProduct(request.Name,request.DisplayName,request.Description,request.Code, request.BrandId, request.PriorityRank, request.ProductMainId, request.Desi, request.VatRate);
                //else
                product.SetProduct(request.BrandId, request.PriorityRank, request.ProductMainId, request.Desi, request.VatRate);
                _productRepository.Update(product);
            }
            else
                await _productRepository.SaveAsync(product);

            #region Variant Operations
            if (request.ProductGroups != null && request.ProductGroups.Count > 0)
            {
                var productGroups = await _productDomainService.SaveProductGroup(_autoMapper.MapCollection<ProductGroupDto, ProductGroup>(request.ProductGroups), product.Id);
                foreach (var productGroup in productGroups)
                {
                    await _productGroupRepository.SaveAsync(productGroup);
                }

                var productGroupVariantList = await _productDomainService.SaveProductGroupVariant(request.ProductGroups.FirstOrDefault().GroupCode, request.CategoryId, product.ProductAttributes);
                foreach (var productGroupVariant in productGroupVariantList)
                {
                    await _productGroupVariantRepository.SaveAsync(productGroupVariant);
                }
            }
            #endregion

            var productChannelCodes = (await _parameterCommunicator.GetProductChannelList()).Data.Select(p => p.Code).ToList();

            var productChannel = await _productChannelRepository.GetProductChannelAll(product.Id);
            if (productChannel == null)
            {
                foreach (var channelCode in productChannelCodes)
                {
                    if(channelCode==(int)ChannelCode.IsCep) //channel code işcepi her ürüne eklememeli. işcep için iş birimi elle güncelleme yapmalı
                        continue;
                    productChannel = new ProductChannel(product.Id, channelCode);
                    await _productChannelRepository.SaveAsync(productChannel);
                }
            }

            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<ProductDto> { Success = true };

        }
    }
}
