using Catalog.Domain.BrandAggregate;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public class Product : Entity
    {
        public string Name { get; protected set; }
        public string SeoName { get; protected set; }
        public string DisplayName { get; protected set; }
        public string Description { get; protected set; }
        public string Code { get; protected set; } //Ürünleri farklılaştıran EAN13 Kodu
        public Guid BrandId { get; protected set; }
        public Brand Brand { get; protected set; }
        public int PriorityRank { get; protected set; }
        public int? ProductMainId { get; protected set; }
        public decimal Desi { get; protected set; }
        public int VatRate { get; protected set; }

        private readonly List<ProductAttribute> _productAttributes;
        public IReadOnlyCollection<ProductAttribute> ProductAttributes => _productAttributes;

        private readonly List<ProductCategory> _productCategories;
        public IReadOnlyCollection<ProductCategory> ProductCategories => _productCategories;

        private readonly List<ProductSeller> _productSellers;
        public IReadOnlyCollection<ProductSeller> ProductSellers => _productSellers;

        private readonly List<SimilarProduct> _similarProducts;
        public IReadOnlyCollection<SimilarProduct> SimilarProducts => _similarProducts;

        private List<ProductImage> _productImages;
        public IReadOnlyCollection<ProductImage> ProductImages => _productImages;

        private readonly List<ProductGroup> _productGroups;
        public IReadOnlyCollection<ProductGroup> ProductGroups => _productGroups;

        private readonly List<ProductDelivery> _productDeliveries;
        public IReadOnlyCollection<ProductDelivery> ProductDeliveries => _productDeliveries;

        private readonly List<ProductChannel> _productChannels;
        public IReadOnlyCollection<ProductChannel> ProductChannels => _productChannels;

        protected Product()
        {
            _productAttributes = new List<ProductAttribute>();
            _productCategories = new List<ProductCategory>();
            _productSellers = new List<ProductSeller>();
            _similarProducts = new List<SimilarProduct>();
            _productImages = new List<ProductImage>();
            _productGroups = new List<ProductGroup>();
            _productDeliveries = new List<ProductDelivery>();
            _productChannels = new List<ProductChannel>();
        }

        public Product(Guid id, string name, string code, string displayName, Guid brandId, Brand brand, List<ProductAttribute> productAttributes, List<ProductCategory> productCategories, List<ProductSeller> productSellers,
            List<ProductChannel> productChannels, List<ProductImage> productImages)
        {
            Id = id;
            Name = name;
            SeoName = ConvertProductSeoName(name);
            Code = code;
            DisplayName = displayName;
            BrandId = brandId;
            Brand = brand;
            _productAttributes = productAttributes;
            _productCategories = productCategories;
            _productSellers = productSellers;
            _productChannels = productChannels;
            _productImages = productImages;

        }

        public Product(string name, string displayName, string description, string code, Guid brandId, int priorityRank, int? productMainId, decimal desi, int vatRate) : this()
        {
            Name = name;
            SeoName = ConvertProductSeoName(name);
            DisplayName = displayName;
            Description = description;
            Code = code;
            BrandId = brandId;
            PriorityRank = priorityRank;
            ProductMainId = productMainId;
            Desi = desi;
            VatRate = vatRate;
        }

        public void SetProduct(string name, string displayName, string description, string code, Guid brandId, int priorityRank, int? productMainId, decimal desi, int vatRate)
        {
            Name = name;
            SeoName = ConvertProductSeoName(name);
            DisplayName = displayName;
            Description = description;
            Code = code;
            BrandId = brandId;
            PriorityRank = priorityRank;
            ProductMainId = productMainId;
            Desi = desi;
            VatRate = vatRate;
        }

        public void SetProduct(Guid brandId, int priorityRank, int? productMainId, decimal desi, int vatRate)
        {
            BrandId = brandId;
            PriorityRank = priorityRank;
            ProductMainId = productMainId;
            Desi = desi;
            VatRate = vatRate;
        }

        public void SetBrand(Guid brandId)
        {
            BrandId = brandId;
        }

        public void VerifyOrAddProductAttributes(List<ProductAttribute> productAttributes, bool isAdminUser)
        {
            if (!isAdminUser)
            {
                foreach (var productAttribute in productAttributes)
                {
                    var existingProductAttribute = _productAttributes.FirstOrDefault(x => x.AttributeId == productAttribute.AttributeId);

                    if (existingProductAttribute == null)
                    {
                        var newProductAttribute = new ProductAttribute(Id, productAttribute.AttributeId, productAttribute.AttributeValueId, productAttribute.IsVariantable);
                        _productAttributes.Add(newProductAttribute);
                    }
                }
            }
            else
            {
                foreach (var productAttribute in productAttributes)
                {
                    var existingProductAttribute = _productAttributes.FirstOrDefault(x => x.AttributeId == productAttribute.AttributeId);

                    if (existingProductAttribute == null)
                    {
                        var newProductAttribute = new ProductAttribute(Id, productAttribute.AttributeId, productAttribute.AttributeValueId, productAttribute.IsVariantable);
                        _productAttributes.Add(newProductAttribute);
                    }
                    else
                    {
                        existingProductAttribute.SetProductAttribute(Id, productAttribute.AttributeId, productAttribute.AttributeValueId, existingProductAttribute.IsVariantable);
                    }
                }
                var substract = _productAttributes.Where(x => !productAttributes.Select(x => x.AttributeId).Contains(x.AttributeId)).ToList();
                foreach (var item in substract)
                {
                    item.setIsActive(false);
                }
            }
        }

        public void VerifyOrAddProductCategory(Guid categoryId, bool isAdminUser)
        {
            //TODO: Category controls
            if (!_productCategories.Any())
            {
                _productCategories.Add(new ProductCategory(Id, categoryId));
            }
            else
            {
                if (isAdminUser)
                {
                    var existingProductCategory = _productCategories.FirstOrDefault();
                    existingProductCategory.SetProductCategory(Id, categoryId);
                }
            }
        }

        public void VerifyOrAddSimilarProducts(List<SimilarProduct> similarProducts)
        {
            foreach (var similarProduct in similarProducts)
            {
                var existingSimilarProduct = _similarProducts.FirstOrDefault(x => x.SecondProductId == similarProduct.SecondProductId);

                if (existingSimilarProduct != null) //update
                {
                    existingSimilarProduct.SetSimilarProduct(existingSimilarProduct.ProductId, existingSimilarProduct.SecondProductId);
                    continue;
                }

                else
                {
                    var newSimilarProduct = new SimilarProduct(Id, similarProduct.SecondProductId);
                    _similarProducts.Add(similarProduct);
                }

            }

            var substract = _similarProducts.Where(x => !similarProducts.Select(x => x.SecondProductId).Contains(x.SecondProductId)).ToList();
            foreach (var item in substract)
            {
                item.SetSimilarProduct(item.Id, item.SecondProductId);
                item.setIsActive(false);
            }
        }

        public void VerifyOrAddProductSellers(List<ProductSeller> productSellers)
        {
            foreach (var productSeller in productSellers)
            {
                var existingProductSeller = _productSellers.FirstOrDefault(x => x.SellerId == productSeller.SellerId);

                if (existingProductSeller != null)//TODO: Event throw
                {
                    existingProductSeller.SetProductSeller(productSeller.StockCode, productSeller.ListPrice,
                                                           productSeller.StockCount, productSeller.SalePrice);
                    continue;
                }

                else
                {
                    var newProductSeller = new ProductSeller(Id, productSeller.SellerId, productSeller.StockCode, productSeller.ListPrice,
                                                             productSeller.StockCount, productSeller.SalePrice, productSeller.InstallmentCount);
                    _productSellers.Add(newProductSeller);
                }

            }

        }

        public void VerifyOrAddProductImage(List<ProductImage> productImages)
        {
            foreach (var productImage in productImages)
            {
                var existingProductImage = _productImages.FirstOrDefault(x => x.SellerId == productImage.SellerId && x.Url == productImage.Url);

                if (existingProductImage != null) //update
                {
                    existingProductImage.SetProductImage(productImage.Name, productImage.Description, productImage.SortOrder, productImage.IsDefault);
                    continue;
                }
                else //add
                {
                    var newProductImage = new ProductImage(productImage.Name, productImage.Url, productImage.Description, productImage.ProductId,
                                                           productImage.SellerId, productImage.SortOrder, productImage.IsDefault);
                    _productImages.Add(newProductImage);
                }
            }
            var substract = _productImages.Where(x => productImages.FirstOrDefault().SellerId == x.SellerId && !productImages.Select(y => y.Url).Contains(x.Url)).ToList();
            foreach (var item in substract)
            {
                item.setIsActive(false);
            }

        }

        public void VerifyOrAddProductDelivery(List<ProductDelivery> productDeliveries)
        {
            foreach (var productDelivery in productDeliveries)
            {
                var existingProductDelivery = _productDeliveries.FirstOrDefault(x => x.ProductId == Id && x.SellerId == productDelivery.SellerId && x.DeliveryId == productDelivery.DeliveryId && x.CityId == productDelivery.CityId);

                if (existingProductDelivery == null)
                {
                    var newProductDelivery = new ProductDelivery(Id, productDelivery.SellerId, productDelivery.DeliveryId, productDelivery.CityId, productDelivery.DeliveryType);
                    _productDeliveries.Add(newProductDelivery);
                }
            }

            var substract = _productDeliveries.Where(x => productDeliveries.FirstOrDefault().SellerId == x.SellerId && !productDeliveries.Select(x => x.CityId).Contains(x.CityId)).ToList();
            foreach (var productDelivery in substract)
            {
                productDelivery.setIsActive(false);
            }

        }

        public async Task LoadProductImage(ProductImage productImage, IProductImageRepository _imageRepository)
        {
            var image = await _imageRepository.GetByIdAsync(productImage.Id);
            productImage.SetProductImage(image.Name, image.Description, image.SortOrder, image.IsDefault);
        }

        public async Task LoadBrand(Guid brandId, IBrandRepository brandRepository)
        {
            var brand = await brandRepository.GetByIdAsync(brandId);
            brand.SetBrand(brand.Name, brand.LogoUrl, brand.WebSite, brand.SeoName);
        }

        public void ArrangeProductImagesBySellerId(Guid sellerId)
        {
            _productImages = ProductImages.Where(x => x.SellerId == sellerId).Take(5).ToList();
        }

        public string ConvertProductSeoName(string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            text = text.Replace(" ", "-");
            text = text.Replace("'", "");
            text = text.Replace("/", "");
            text = text.Replace("+", "");
            text = text.Replace(".", "-");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("}", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace("&", "");
            text = text.Replace(",", "");
            text = text.Replace("*", "");
            text = text.Replace(":", "");
            text = text.Replace(";", "");
            text = text.Replace("#", "");
            text = text.Replace("%", "");
            text = text.Replace("!", "");
            text = text.Replace("=", "");
            text = text.Replace("?", "");
            text = text.ToLower();
            return text;
        }
    }
}