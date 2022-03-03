using AutoMapper;
using Catalog.ApiContract.Contract;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;

namespace Catalog.ApplicationService.AutoMapper
{
    public class MapperProfile : Profile
    {
        private readonly int depth = 5;

        public MapperProfile()
        {
            CreateMap<Product, ProductDto>().MaxDepth(depth).ReverseMap();
            CreateMap<ProductAttribute, ProductAttributeDto>().MaxDepth(depth).ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().MaxDepth(depth).ReverseMap();
            CreateMap<SimilarProduct, SimilarProductDto>().MaxDepth(depth).ReverseMap();
            CreateMap<ProductSeller, ProductSellerDto>().MaxDepth(depth).ReverseMap();
            CreateMap<ProductGroup, ProductGroupDto>().MaxDepth(depth).ReverseMap();
            CreateMap<ProductGroupVariant, ProductGroupVariantDto>().MaxDepth(depth).ReverseMap();

            CreateMap<Category, CategoryDto>().MaxDepth(depth).ReverseMap();

            CreateMap<ProductImage, ProductImageDto>().MaxDepth(depth).ReverseMap();
            CreateMap<ProductDelivery, ProductDeliveryDto>().MaxDepth(depth).ReverseMap();
        }

    }
}
