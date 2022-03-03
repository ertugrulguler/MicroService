using Catalog.ApiContract.Contract;
using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class CreateProductCommand : IRequest<ResponseBase<ProductDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public string Code { get; set; }
        public int PriorityRank { get; set; }
        public int? ProductMainId { get; set; }
        public decimal Desi { get; set; }
        public int VatRate { get; set; }

        public List<ProductAttributeDto> ProductAttributes { get; set; }

        //public List<ProductCategoryDto> ProductCategories { get; set; }
        public List<SimilarProductDto> SimilarProducts { get; set; }
        public List<ProductSellerDto> ProductSellers { get; set; }
        public List<ProductGroupDto> ProductGroups { get; set; }
        public List<ProductImageDto> ProductImages { get; set; }

        public List<ProductDeliveryDto> ProductDeliveries { get; set; }
        public bool IsAdminUser { get; set; }
    }
}