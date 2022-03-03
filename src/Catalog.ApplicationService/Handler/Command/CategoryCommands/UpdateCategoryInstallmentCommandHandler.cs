using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class UpdateCategoryInstallmentCommandHandler : IRequestHandler<UpdateCategoryInstallmentCommand, ResponseBase<CategoryInstallmentDto>>
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly ICategoryInstallmentRepository _categoryInstallmentRepository;

        public UpdateCategoryInstallmentCommandHandler(
            ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler,
            ICategoryInstallmentRepository categoryInstallmentRepository)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _categoryInstallmentRepository = categoryInstallmentRepository;
        }

        public async Task<ResponseBase<CategoryInstallmentDto>> Handle(UpdateCategoryInstallmentCommand request, CancellationToken cancellationToken)
        {
            var categoryInstallment = await _categoryInstallmentRepository.FindByAsync(x => x.CategoryId == request.CategoryId);
            if (categoryInstallment == null)
                throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                 ApplicationMessage.CategoryNotFound.Message(),
                 ApplicationMessage.CategoryNotFound.UserMessage());


            if (request.MinPrice != null && request.MinPrice > 0.GetHashCode())
            {
                var newMaxInstallmentCount = request.MaxInstallmentCount;
                categoryInstallment.SetCategoryInstallment(0, request.MinPrice, newMaxInstallmentCount);
            }

            else
                categoryInstallment.SetCategoryInstallment(request.MaxInstallmentCount, request.MinPrice, null);


            _categoryInstallmentRepository.Update(categoryInstallment);
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<CategoryInstallmentDto>()
            {
                Data = new CategoryInstallmentDto()
                {
                    CategoryId = categoryInstallment.CategoryId,
                    MaxInstallmentCount = categoryInstallment.NewMaxInstallmentCount == null ? categoryInstallment.MaxInstallmentCount : categoryInstallment.NewMaxInstallmentCount.Value,
                    MinPrice = categoryInstallment.MinPrice,
                }
            };
        }

    }
}
