using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class DeleteCategoryInstallmentCommandHandler : IRequestHandler<DeleteCategoryInstallmentCommand, ResponseBase<object>>
    {
        private readonly IDbContextHandler _dbContextHandler;
        private readonly ICategoryInstallmentRepository _categoryInstallmentRepository;


        public DeleteCategoryInstallmentCommandHandler(IDbContextHandler dbContextHandler, ICategoryInstallmentRepository categoryInstallmentRepository)
        {
            _dbContextHandler = dbContextHandler;
            _categoryInstallmentRepository = categoryInstallmentRepository;
        }


        public async Task<ResponseBase<object>> Handle(DeleteCategoryInstallmentCommand request, CancellationToken cancellationToken)
        {
            var existing = await _categoryInstallmentRepository.FindByAsync(x => x.CategoryId == request.CategoryId);
            if (existing == null)
            {
                throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                    ApplicationMessage.CategoryNotFound.Message(),
                    ApplicationMessage.CategoryNotFound.UserMessage());
            }

            _categoryInstallmentRepository.Delete(existing);
            await _dbContextHandler.SaveChangesAsync();
            return new ResponseBase<object> { Success = true };
        }
    }
}