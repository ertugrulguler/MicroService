using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryQueries
{
    public class GetCategoryInstallmentQueryHandler : IRequestHandler<GetCategoryInstallmentQuery, ResponseBase<List<CategoryInstallmentDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryInstallmentRepository _categoryInstallmentRepository;

        public GetCategoryInstallmentQueryHandler(
            ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler,
            ICategoryAssembler categoryAssembler, ICategoryInstallmentRepository categoryInstallmentRepository)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _categoryAssembler = categoryAssembler;
            _categoryInstallmentRepository = categoryInstallmentRepository;
        }

        public async Task<ResponseBase<List<CategoryInstallmentDto>>> Handle(GetCategoryInstallmentQuery request, CancellationToken cancellationToken)
        {
            var categoryInstallmentList =
                await _categoryInstallmentRepository.AllAsync();

            if (categoryInstallmentList == null)
                throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                 ApplicationMessage.CategoryNotFound.Message(),
                 ApplicationMessage.CategoryNotFound.UserMessage());

            var response = new List<CategoryInstallmentDto>();

            foreach (var item in categoryInstallmentList)
            {
                var categoryName = string.Empty;

                var category = await _categoryRepository.FindByAsync(x => x.Id == item.CategoryId);
                if (category != null)
                {
                    categoryName = category.Name;
                }

                response.Add(new CategoryInstallmentDto
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    CategoryName = categoryName,
                    MaxInstallmentCount = item.MaxInstallmentCount
                });
            }

            return new ResponseBase<List<CategoryInstallmentDto>> { Data = response, Success = true };

        }
    }
}
