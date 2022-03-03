using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public class ProductService : IProductService
    {
        private readonly IExpressionBinding _expressionBinding;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly IFavoriteProductRepository _favoriteProductRepository;
        private readonly IGeneralAssembler _generalAssembler;


        public ProductService(IExpressionBinding expressionBinding,
            ICategoryDomainService categoryDomainService,
            IProductRepository productRepository,
            IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository,
            IProductSellerRepository productSellerRepository,
            IProductAttributeRepository productAttributeRepository,
            IMerhantCommunicator merhantCommunicator, IGeneralAssembler generalAssembler,
            IFavoriteProductRepository favoriteProductRepository)
        {
            _expressionBinding = expressionBinding;
            _categoryDomainService = categoryDomainService;
            _productRepository = productRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _productSellerRepository = productSellerRepository;
            _productAttributeRepository = productAttributeRepository;
            _merhantCommunicator = merhantCommunicator;
            _favoriteProductRepository = favoriteProductRepository;
            _generalAssembler = generalAssembler;

        }

        #region ProductListAndFilter

        public async Task<ProductListWithCount> GetProductList(GetProductList request, List<Guid> bannedSellers, List<Category> categorySubList)
        {
            var getExpressionModel = GetProductListExpressions(request, ProductFilterEnum.Product, categorySubList);

            var products = await GetProductAllRelations(getExpressionModel, request, bannedSellers);
            return products;
        }

        public ExpressionsModel GetProductListExpressions(GetProductList request, ProductFilterEnum productFilterEnum, List<Category> categorySubList)
        {
            List<Expression<Func<Product, bool>>> expressionProductList = new List<Expression<Func<Product, bool>>>();
            List<Expression<Func<ProductSeller, bool>>> expressionProductSellersList = new List<Expression<Func<ProductSeller, bool>>>();
            Expression<Func<Product, bool>> expressionAllProduct = null;
            Expression<Func<ProductSeller, bool>> expressionAllProductSeller = null;
            var attributeAllIdList = new List<List<Guid>>();
            var attributeList = new List<Guid>();


            var listAttribute = request.FilterModel.Where(y => y.FilterField.Split('-')[0] ==
            ProductFilterEnum.Attribute.ToString()).GroupBy(y => y.Type, (k, g) => new
            {
                Key = k,
                Value = g.ToList()

            }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listAttribute)
            {
                foreach (var item1 in item.Value)
                {
                    attributeList.Add(new Guid(item1.Id));
                }
                attributeAllIdList.Add(attributeList);
                attributeList = new List<Guid>();
            }

            var listProduct = request.FilterModel.Where(y => y.Type ==
            ProductFilterEnum.Product.ToString()).GroupBy(y => y.FilterField, (k, g) => new
            {
                Key = k,
                Value = g.ToList()

            }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listProduct)
            {
                Expression<Func<Product, bool>> expressionProduct = null;
                if (productFilterEnum != ProductFilterEnum.Brand)
                {
                    if (item.Key == ProductFilterEnum.BrandId.ToString())
                    {
                        expressionProduct = _expressionBinding.GenericExpressionBinding<Product>(item.Value, Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.IsEqualToNotNullGuid);
                        expressionProductList.Add(expressionProduct);
                    }
                }
                if (item.Key == ProductFilterEnum.Code.ToString())
                {
                    expressionProduct = _expressionBinding.GenericExpressionBinding<Product>(item.Value, Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.IsEqualTo);
                    expressionProductList.Add(expressionProduct);
                }
                if (item.Key == ProductFilterEnum.Search.ToString())
                {
                    expressionProduct = _expressionBinding.GenericExpressionBinding<Product>(item.Value, Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.Contains);
                    expressionProductList.Add(expressionProduct);
                }
            }

            var listSeller = request.FilterModel.Where(y => y.Type == ProductFilterEnum.ProductSeller.ToString())
                .GroupBy(y => y.FilterField, (k, g) => new
                {
                    Key = k,
                    Value = g.ToList()

                }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listSeller)
            {
                Expression<Func<ProductSeller, bool>> expressionProductSeller = null;
                if (productFilterEnum != ProductFilterEnum.SalePrice)
                {
                    if (item.Key == ProductFilterEnum.SalePrice.ToString())
                    {
                        expressionProductSeller = _expressionBinding.GenericExpressionBinding<ProductSeller>(item.Value, Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.Between);
                        expressionProductSellersList.Add(expressionProductSeller);
                    }
                }
                if (productFilterEnum != ProductFilterEnum.ProductSeller)
                {

                    if (item.Key == ProductFilterEnum.SellerId.ToString())
                    {
                        expressionProductSeller = _expressionBinding.GenericExpressionBinding<ProductSeller>(item.Value, Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.IsEqualToNotNullGuid);
                        expressionProductSellersList.Add(expressionProductSeller);
                    }
                }
            }

            expressionAllProduct = _expressionBinding.GenericExpressionBindingListProduct<Product>(expressionProductList, Domain.Enums.ExpressionJoint.And);


            expressionAllProductSeller = _expressionBinding.GenericExpressionBindingListProductSeller<ProductSeller>(expressionProductSellersList, Domain.Enums.ExpressionJoint.And);


            return new ExpressionsModel
            {
                categorySubList = categorySubList,
                expressionAllProduct = expressionAllProduct,
                attributeAllIdList = attributeAllIdList,
                expressionAllProductSeller = expressionAllProductSeller
            };

        }

        public async Task<ProductListWithCount> GetProductAllRelations(ExpressionsModel getExpressionModel, GetProductList request, List<Guid> bannedSellers)
        {
            var sellerList = new List<Guid>();
            foreach (var item in request.FilterModel)
            {
                if (item.FilterField == ProductFilterEnum.SellerId.ToString() && !bannedSellers.Contains(new Guid(item.Id)))
                {
                    sellerList.Add(new Guid(item.Id));
                }
            }
            var products = await _productRepository.GetProductAllRelations(request.PagerInput, getExpressionModel.categorySubList?.Select(u => u.Id).ToList(),
            getExpressionModel.attributeAllIdList, getExpressionModel.expressionAllProduct, getExpressionModel.expressionAllProductSeller, request.OrderBy,
            bannedSellers, request.ProductChannelCode.GetHashCode() == 0 ? 1 : request.ProductChannelCode.GetHashCode(), sellerList);
            return products;

        }

        #endregion

        #region ProductiFilterForSeller

        public async Task<ProductListWithCount> GetProductListBySeller(GetProductList request, List<Guid> bannedSeller)
        {

            var getExpressionModel = GetProductListExpressions(request, ProductFilterEnum.Product, null);
            var products = await GetProductAllRelations(getExpressionModel, request, bannedSeller);
            return products;
        }
        #endregion


        public async Task<List<FavoriteProductsList>> GetFavoriteProductsForCustomerId(Guid CustomerId)
        {
            var result = new List<FavoriteProductsList>();

            var getFavoriteProducts = await _favoriteProductRepository.FilterByAsync(u => u.CustomerId == CustomerId);

            if (getFavoriteProducts.Count > 0)
                getFavoriteProducts.ForEach(y => result.Add(new FavoriteProductsList { ProductId = y.ProductId }));

            return result;
        }
        public List<OrderByListofObject> GetOrderList()
        {
            var listObject = new List<OrderByListofObject>();
            var list = (from action in (OrderBy[])Enum.GetValues(typeof(OrderBy)) select action).ToList();
            var i = 0;

            foreach (var item in list)
            {
                if (item == OrderBy.None)
                    continue;
                var enumDesc = GetEnumDescription(item);
                listObject.Add(new OrderByListofObject { Id = i, Name = enumDesc, SeoValue = _generalAssembler.GetSeoName(enumDesc, SeoNameType.OrderBy) });
                i++;
            }

            return listObject.OrderBy(y => y.Id).ToList();
        }

        public async Task<string> GetProductSeoUrl(Guid id)
        {
            var prod = await _productRepository.FindByAsync(a => a.Id == id);

            return prod.SeoName + "-p-" + prod.Code;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.FirstOrDefault().Description;
            }

            return value.ToString();
        }
    }
}
