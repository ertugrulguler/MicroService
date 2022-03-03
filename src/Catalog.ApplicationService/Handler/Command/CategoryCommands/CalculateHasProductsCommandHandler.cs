using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class CalculateHasProductsCommandHandler : IRequestHandler<CalculateHasProductsCommand, ResponseBase<object>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IProductRepository _productRepository;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;

        public CalculateHasProductsCommandHandler(ICategoryRepository categoryRepository,
            IProductCategoryRepository productCategoryRepository,
            ICategoryDomainService categoryDomainService,
            IDbContextHandler dbContextHandler,
            IProductRepository productRepository,
            IProductSellerRepository productSellerRepository,
            IBackOfficeCommunicator backOfficeCommunicator)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _productCategoryRepository = productCategoryRepository;
            _categoryDomainService = categoryDomainService;
            _productSellerRepository = productSellerRepository;
            _productRepository = productRepository;
            _backOfficeCommunicator = backOfficeCommunicator;
        }
        public async Task<ResponseBase<object>> Handle(CalculateHasProductsCommand request, CancellationToken cancellationToken)
        {
            var categoryList = await _categoryRepository.FilterByAsync(c => c.Type == CategoryTypeEnum.MainCategory && c.Code != null);
            foreach (var category in categoryList)
            {
                if (category.ParentId == null)
                {
                    category.HasProduct = true;
                    _categoryRepository.Update(category);
                }
                else if (category.LeafPath == null)
                {
                    var leafCategoryList = await _categoryDomainService.GetLeafCategoriesForCalculateHasProduct(category.Id);
                    category.HasProduct = false;
                    foreach (var leafCategory in leafCategoryList)
                    {
                        if (await _productCategoryRepository.Exist(p => p.CategoryId == leafCategory.Id))
                        {
                            var productCategories = await _productCategoryRepository.FilterByAsync(f => f.CategoryId == leafCategory.Id);
                            var products = await _productSellerRepository.FilterByAsync(v => productCategories.Select(g => g.ProductId).ToList().Contains(v.ProductId) && v.StockCount > 0);
                            if (products.Any())
                            {
                                category.HasProduct = true;
                                break;
                            }
                        }
                    }
                    _categoryRepository.Update(category);
                }

                else
                {
                    category.HasProduct = false;
                    if (await _productCategoryRepository.Exist(p => p.CategoryId == category.Id))
                    {
                        var productCategories = await _productCategoryRepository.FilterByAsync(f => f.CategoryId == category.Id);
                        var products = await _productSellerRepository.FilterByAsync(v => productCategories.Select(g => g.ProductId).ToList().Contains(v.ProductId) && v.StockCount > 0);
                        if (products.Any())
                        {
                            category.HasProduct = true;
                        }
                    }
                    _categoryRepository.Update(category);
                }
            }

            await _dbContextHandler.SaveChangesAsync();
            await _backOfficeCommunicator.DeleteCache("GetCategoriesForMobileQuery");
            return new ResponseBase<object>() { Success = true };
        }
    }
}
