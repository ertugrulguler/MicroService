using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class CreateCategoryInstallmentCommandHandler : IRequestHandler<CreateCategoryInstallmentCommand, ResponseBase<CategoryInstallmentDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryInstallmentRepository _categoryInstallmentRepository;

        public CreateCategoryInstallmentCommandHandler(
            ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler,
            ICategoryAssembler categoryAssembler, ICategoryInstallmentRepository categoryInstallmentRepository)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _categoryAssembler = categoryAssembler;
            _categoryInstallmentRepository = categoryInstallmentRepository;
        }

        public async Task<ResponseBase<CategoryInstallmentDto>> Handle(CreateCategoryInstallmentCommand request, CancellationToken cancellationToken)
        {
            if (request.MaxInstallmentCount < 0 || request.MaxInstallmentCount > 18)
                throw new BusinessRuleException(ApplicationMessage.NotMaxInstallmentCount,
                ApplicationMessage.NotMaxInstallmentCount.Message(),
                ApplicationMessage.NotMaxInstallmentCount.UserMessage());

            if (request.MinPrice < 0)
                throw new BusinessRuleException(ApplicationMessage.NotPrice,
                ApplicationMessage.NotPrice.Message(),
                ApplicationMessage.NotPrice.UserMessage());

            var categoryInstallment =
                await _categoryInstallmentRepository.FindByAsync(x => x.CategoryId == request.CategoryId);

            if (categoryInstallment != null)
                throw new BusinessRuleException(ApplicationMessage.CategoryInstallmentAlreadyExist,
                 ApplicationMessage.CategoryInstallmentAlreadyExist.Message(),
                 ApplicationMessage.CategoryInstallmentAlreadyExist.UserMessage());

            var category = await _categoryRepository.FindByAsync(c => c.Id == request.CategoryId);
            if (category == null)
                throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                ApplicationMessage.CategoryNotFound.Message(),
                ApplicationMessage.CategoryNotFound.UserMessage());

            var categoryIsNotLeaf = await _categoryRepository.Exist(c => c.ParentId == request.CategoryId && c.Type == CategoryTypeEnum.MainCategory);
            if (categoryIsNotLeaf)
                throw new BusinessRuleException(ApplicationMessage.CategoryIsNotLeaf,
                ApplicationMessage.CategoryIsNotLeaf.Message(),
                ApplicationMessage.CategoryIsNotLeaf.UserMessage());


            if (request.MinPrice != null && request.MinPrice > 0.GetHashCode())
            {
                var newMaxInstallmentCount = request.MaxInstallmentCount;

                categoryInstallment = new CategoryInstallment(request.CategoryId, 0,
                    request.MinPrice, newMaxInstallmentCount);
            }

            else
                categoryInstallment = new CategoryInstallment(request.CategoryId, request.MaxInstallmentCount,
                    request.MinPrice, null);

            await _categoryInstallmentRepository.SaveAsync(categoryInstallment);
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
