using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IProductGroupVariantRepository _productGroupVariantRepository;
        private readonly IExpressionBinding _expressionBinding;

        public ProductVariantService(ICategoryAttributeRepository categoryAttributeRepository,
            IProductRepository productRepository,
            IProductAttributeRepository productAttributeRepository,
            IProductGroupRepository productGroupRepository,
            IProductGroupVariantRepository productGroupVariantRepository,
            IExpressionBinding expressionBinding,
            IProductSellerRepository productSellerRepository)
        {
            _categoryAttributeRepository = categoryAttributeRepository;
            _productRepository = productRepository;
            _productAttributeRepository = productAttributeRepository;
            _productGroupRepository = productGroupRepository;
            _productGroupVariantRepository = productGroupVariantRepository;
            _expressionBinding = expressionBinding;
            _productSellerRepository = productSellerRepository;
        }

        /// <summary>
        /// Gets Product with variants
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>We will see what happens :)</returns>
        public async Task<List<List<VariantGroup>>> GetProductWithVariants(Guid productId, Guid categoryId)
        {
            try
            {
                //Product'ın ProductGroup tablosunda kaydı var mı?
                var productVariantRecord = await _productGroupRepository.FindByAsync(x => x.ProductId == productId);

                if (productVariantRecord == null)
                    return new List<List<VariantGroup>>();

                //Kayıt varsa bağlantılı ProductId'leri çek.
                var productVariants = await _productGroupRepository.FilterByAsync(pg => pg.GroupCode == productVariantRecord.GroupCode);

                //Birbirlerine hangi attribute'larla bağlantılı olduğunu anlamak için ProductGroupVariant tablosundan groupCode'a göre çek.
                var productVariantGroups = await _productGroupVariantRepository.FilterByAsync(pvg => pvg.ProductGroupCode.StartsWith(productVariantRecord.GroupCode));

                //Tüm varyantlı productları ve asıl oğlanı (gönderilen product'ı) çek.
                var productVariantedProducts = await _productRepository.FilterByAsync(p => productVariants.Select(pv => pv.ProductId).Contains(p.Id));

                //Product Attribute tablosundan bütün productlar için birbirleriyle bağlantılı olan attribute'larını ve attributeValue'larını çek.
                var productAttributes = await _productAttributeRepository.FilterByAsync(pa => productVariantGroups.Select(pvg => pvg.AttributeId).Contains(pa.AttributeId)
                && productVariantedProducts.Select(pvp => pvp.Id).Contains(pa.ProductId));

                //Category Attribute tablosuna git öncelik hangi Attribute'da öğren.
                var categoryAttributes = await _categoryAttributeRepository.FilterByAsync(ca => ca.CategoryId == categoryId && productVariantGroups.Select(pvg => pvg.AttributeId).Contains(ca.AttributeId));

                //Category attribute önem sırasına göre order'lanıp dönülüyor. Amaç öncelikli attribute'a göre varyant grupları oluşturmak. Listeleme için de lazım olacak.
                var variantGroups = ProductVariantGroup(productId, productAttributes, categoryAttributes.OrderBy(o => o.Order).ToList());

                return variantGroups;
            }
            catch
            {
                return new List<List<VariantGroup>>();
            }
        }

        /// <summary>
        /// Arrange variant groups with categoryAttribute check
        /// </summary>
        /// <param name="productId">Main product id (esas oğlan)</param>
        /// <param name="productAttributes">esas oğlana bağlı varyant ürünler</param>
        /// <param name="categoryAttributes">category for ordering attributes</param>
        /// <returns></returns>
        private static List<List<VariantGroup>> ProductVariantGroup(Guid productId, List<ProductAttribute> productAttributes, List<CategoryAttribute> categoryAttributes)
        {
            //Geri dönülecek fantastik listemiz.
            var groups = new List<List<VariantGroup>>();
            var groupsSelected = productAttributes.Where(x => x.ProductId == productId).ToList().OrderBy(gs => categoryAttributes.FindIndex(ca => ca.AttributeId == gs.AttributeId));
            Guid? firstAttributeValue = null;
            Guid? secondAttributeValue = null;
            List<Guid> firstGroupProductIds = new();
            List<Guid> secondGroupProductIds = new();
            int index = 1;
            bool twoWayIteration = false;


            foreach (var mainProductGroup in groupsSelected)
            {
                groups.Add(new List<VariantGroup>()
                {
                    new VariantGroup{
                        ProductId = mainProductGroup.ProductId,
                        AttributeId = mainProductGroup.AttributeId,
                        AttributeValueId = mainProductGroup.AttributeValueId,
                        IsOpen = true,
                        IsSelected = true
                    }
                });

                //productAttributes.RemoveAll(x => x.AttributeId == mainProductGroup.AttributeId && x.AttributeValueId == mainProductGroup.AttributeValueId && x.ProductId != productId);
            }

            if (groups.Count > 1)
            {
                firstAttributeValue = groups[0].FirstOrDefault().AttributeValueId;
                secondAttributeValue = groups[1].FirstOrDefault().AttributeValueId;

                firstGroupProductIds = productAttributes.Where(x => x.AttributeValueId == secondAttributeValue && x.ProductId != productId).Select(x => x.ProductId).ToList();
                secondGroupProductIds = productAttributes.Where(x => x.AttributeValueId == firstAttributeValue && x.ProductId != productId).Select(x => x.ProductId).ToList();
                twoWayIteration = true;
            }

            foreach (var group in groups)
            {
                var productGroup = group.FirstOrDefault(x => x.ProductId == productId);

                var groupVariants = productAttributes.Where(x => x.AttributeId == productGroup.AttributeId && x.AttributeValueId != productGroup.AttributeValueId).ToList();

                if (twoWayIteration && index == 1)
                {
                    var swapOrder = groupVariants.Where(x => firstGroupProductIds.Contains(x.ProductId)).ToList();
                    groupVariants.RemoveAll(x => firstGroupProductIds.Contains(x.ProductId));
                    groupVariants.InsertRange(0, swapOrder);
                }
                else if (twoWayIteration && index == 2)
                {
                    var swapOrder = groupVariants.Where(x => secondGroupProductIds.Contains(x.ProductId)).ToList();
                    groupVariants.RemoveAll(x => secondGroupProductIds.Contains(x.ProductId));
                    groupVariants.InsertRange(0, swapOrder);
                }

                foreach (var groupVariant in groupVariants)
                {
                    if (group.FirstOrDefault(x => x.AttributeValueId == groupVariant.AttributeValueId) != null)
                        continue;

                    group.Add(new VariantGroup
                    {
                        ProductId = groupVariant.ProductId,
                        AttributeId = groupVariant.AttributeId,
                        AttributeValueId = groupVariant.AttributeValueId,
                        IsOpen = true,
                        IsSelected = false
                    });
                }

                index++;
            }

            return groups;


            //for (int i = 0; i < orderedCategoryAttributes.Count; i++)
            //{
            //    var groupList = new List<VariantGroup>();

            //    var productAttributeViaCatAttributes = productAttributes.Where(x => x.AttributeId == orderedCategoryAttributes[i].AttributeId).ToList();

            //    var dublicateValue = productAttributeViaCatAttributes.FirstOrDefault(x => x.ProductId == productId && x.AttributeId == orderedCategoryAttributes[i].AttributeId).AttributeValueId;

            //    //esas oğlan'la aynı attribute ve attribute value'ya sahip olan arkadaşları pistin dışına alıyoruz.
            //    if (dublicateValue != Guid.Empty)
            //        productAttributeViaCatAttributes.RemoveAll(x => x.AttributeValueId == dublicateValue && x.ProductId != productId);

            //    if (groups.Count > 0)
            //    {
            //        var firstGroupAttributeValueId = groups.FirstOrDefault().FirstOrDefault(x => x.ProductId == productId).AttributeValueId;
            //        var list = productAttributes.Where(x => x.AttributeId == orderedCategoryAttributes[i].AttributeId);

            //        var productListToBeAdded = productAttributes.Where(x => x.AttributeValueId == firstGroupAttributeValueId
            //        && list.Select(x => x.ProductId).Contains(x.ProductId)).Select(x => x.ProductId);

            //        //TODO : 64 GB Sarı varsa gösterilmeli ve isOpen false olmalı.

            //        productAttributeViaCatAttributes.RemoveAll(x => !productListToBeAdded.Contains(x.ProductId));
            //    }
            //    else if (orderedCategoryAttributes.Count > 1)
            //    {
            //        var secondAttributeValueId = productAttributes.FirstOrDefault(x => x.ProductId == productId && x.AttributeId == orderedCategoryAttributes[i + 1].AttributeId).AttributeValueId;

            //        var list = productAttributes.Where(x => x.AttributeId == orderedCategoryAttributes[i + 1].AttributeId);

            //        var productListToBeAdded = productAttributes.Where(x => x.AttributeValueId == secondAttributeValueId
            //        && list.Select(x => x.ProductId).Contains(x.ProductId)).Select(x => x.ProductId);

            //        productAttributeViaCatAttributes.RemoveAll(x => !productListToBeAdded.Contains(x.ProductId));
            //    }

            //    foreach (var productAttributeViaCatAttribute in productAttributeViaCatAttributes)
            //    {
            //        bool isSelected = false;

            //        //esas oğlan attribute'ları selected true gelmesini sağlıyoruz. Öncelikli bütün attribute'lar open. Reklamlardan sonra onu da ayarlayacağız.
            //        if (productAttributeViaCatAttribute.ProductId == productId)
            //            isSelected = true;

            //        if (groupList.FirstOrDefault(x => x.AttributeValueId == productAttributeViaCatAttribute.AttributeValueId) != null)
            //            continue;

            //        groupList.Add(new VariantGroup
            //        {
            //            ProductId = productAttributeViaCatAttribute.ProductId,
            //            AttributeId = productAttributeViaCatAttribute.AttributeId,
            //            AttributeValueId = productAttributeViaCatAttribute.AttributeValueId,
            //            IsOpen = true,
            //            IsSelected = isSelected
            //        });

            //    }

            //    groups.Add(groupList);

            //}

            //return groups;
        }

        public async Task<List<VariantGroup>> GetProductWithVariantsList(List<Guid> productIdList)
        {
            var groups = new List<VariantGroup>();

            var productVariantRecord = await _productGroupRepository.FilterByAsync(x => productIdList.Contains(x.ProductId));
            if (productVariantRecord == null || productVariantRecord.Count == 0)
                return new List<VariantGroup>();

            var groupCodeList = productVariantRecord.Select(c => { c.SetGroupCode(c.GroupCode); return c; }).ToList();

            var groupList = groupCodeList.Select(x => x.GroupCode).ToList();

            var resp = _productGroupVariantRepository.GetProductVariantListWithinGroupCodeList(groupList);
            var attrResp = await _productAttributeRepository.FilterByAsync(pa => resp.Select(r => r.AttributeId).Contains(pa.AttributeId) && productIdList.Contains(pa.ProductId));

            foreach (var item in productVariantRecord)
            {
                //var productVariantGroups = resp.Where(pvg => pvg.ProductGroupCode.StartsWith(item.GroupCode));

                var productAttributes = attrResp.Where(pa => pa.ProductId == item.ProductId);

                foreach (var attrItem in productAttributes)
                {
                    groups.Add(new VariantGroup()
                    {
                        ProductId = item.ProductId,
                        AttributeId = attrItem.AttributeId,
                        AttributeValueId = attrItem.AttributeValueId,
                        IsOpen = true,
                        IsSelected = true
                    });
                }
            }

            return groups;

        }

        //TODO: Refactor
        public async Task<List<Product>> FilterVariants(List<Product> productList)
        {
            var removingList = new List<Guid>();
            var productVariantGroupList = new List<SanitizingVariantGroup>();
            var filterModelList = new List<FilterModel>();
            var attributeListForOmitting = new List<List<ProductAttribute>>();

            var variantGroups = await _productGroupRepository.FilterByAsync(x => productList.Select(p => p.Id).Contains(x.ProductId));

            if (variantGroups == null || variantGroups.Count == 0)
                return productList;

            foreach (var pg in variantGroups)
            {
                var productVariantGroup = productVariantGroupList.FirstOrDefault(pvg => pvg.GroupCode == pg.GroupCode);
                if (productVariantGroup == null)
                {
                    productVariantGroupList.Add(new SanitizingVariantGroup
                    {
                        GroupCode = pg.GroupCode,
                        ProductIds = new List<Guid> { pg.ProductId },
                        AttributeIds = new List<Guid>(),
                        CategoryIds = productList.FirstOrDefault(x => x.Id == pg.ProductId).ProductCategories.Select(pg => pg.CategoryId).ToList()
                    });
                    filterModelList.Add(new FilterModel
                    {
                        FilterField = "ProductGroupCode",
                        Id = pg.GroupCode
                    });
                }
                else
                    productVariantGroup.ProductIds.Add(pg.ProductId);
            }

            var expression = filterModelList.Select(fml => fml.Id).ToList();
            var variantGroupAttributes = _productGroupVariantRepository.GetProductVariantListWithinGroupCodeList(expression);
            var categoryIds = productVariantGroupList.Select(pvg => pvg.CategoryIds).SelectMany(x => x).ToList();
            var categoryAttributes = await _categoryAttributeRepository.FilterByAsync(x => categoryIds.Contains(x.CategoryId) && x.IsListed);


            foreach (var productVariantGroup in productVariantGroupList)
            {
                productVariantGroup.AttributeIds.AddRange(variantGroupAttributes.Where(vga => vga.ProductGroupCode.StartsWith(productVariantGroup.GroupCode)).Select(a => a.AttributeId).ToList());

                var categoryAttribute = categoryAttributes.FirstOrDefault(x => productVariantGroup.AttributeIds.Contains(x.AttributeId));

                if (categoryAttribute == null)
                    continue;

                productVariantGroup.MainAttributeId = categoryAttribute.AttributeId;

                var productSellers = new List<ProductSeller>();
                var list = productList.Where(h => productVariantGroup.ProductIds.Contains(h.Id)).SelectMany(b => b.ProductSellers).ToList();
                var newList = list.GroupBy(o => o.ProductId, (k, g) => new { Key = k, Value = g.OrderBy(u => u.SalePrice).FirstOrDefault() }).Select(p => p.Value).ToList();
                if (newList.Count <= 1)
                    continue;

                var productAttributesToLook = productList.SelectMany(x => x.ProductAttributes.Where(pa => pa.AttributeId == productVariantGroup.MainAttributeId
                && productVariantGroup.ProductIds.Contains(pa.ProductId)));

                attributeListForOmitting = productAttributesToLook.GroupBy(x => x.AttributeValueId).Select(grp => grp.ToList()).ToList();

                productSellers.AddRange(newList);
                newList.Remove(newList.OrderBy(g => g.SalePrice).FirstOrDefault());
                productSellers = newList;
                removingList.AddRange(productSellers.Select(g => g.ProductId));
            }
            productList.RemoveAll(x => removingList.Contains(x.Id));
            return productList;
        }


        private static List<Guid> RemoveProducts(List<List<ProductAttribute>> omittingProductIds)
        {
            var removeIds = new List<Guid>();

            var isMultiple = omittingProductIds.FirstOrDefault(x => x.Count > 1) != null;

            if (isMultiple)
            {
                foreach (var item in omittingProductIds)
                {
                    if (item.Count > 1)
                        removeIds.AddRange(item.Skip(1).Select(x => x.ProductId));
                }
            }
            else
            {
                removeIds.AddRange(omittingProductIds.Skip(1).SelectMany(x => x.Select(om => om.ProductId)));
            }

            return removeIds;
        }

        public async Task<List<Guid>> VariantableProductIds(List<Product> productList)
        {
            var variantableList = new List<Guid>();
            var variantGroups = await _productGroupRepository.FilterByAsync(x => productList.Select(p => p.Id).Contains(x.ProductId));

            if (variantGroups != null && variantGroups.Count > 0)
                variantableList.AddRange(variantGroups.Select(x => x.ProductId));

            return variantableList;
        }

    }

    public class VariantGroup
    {
        public Guid ProductId { get; set; }
        public Guid AttributeId { get; set; }
        public Guid AttributeValueId { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSelected { get; set; }
    }

    public class SanitizingVariantGroup
    {
        public string GroupCode { get; set; }
        public List<Guid> AttributeIds { get; set; }
        public List<Guid> ProductIds { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public Guid MainAttributeId { get; set; }

    }
}

