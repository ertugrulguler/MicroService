using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseBase<CategoryDto>>
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly ICategoryAssembler _categoryAssembler;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler,
            ICategoryAssembler categoryAssembler
            )
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _categoryAssembler = categoryAssembler;
        }

        public async Task<ResponseBase<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _categoryRepository.GetCategoryWithAllRelations(request.Id);
            if (existing == null)
                throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                 ApplicationMessage.CategoryNotFound.Message(),
                 ApplicationMessage.CategoryNotFound.UserMessage());
            //şimdilik parentid kısmı kapatıldı.parentid güncellenemeyecek şekilde revize edildi
            existing.setCategoryWithoutParentId(request.Name, request.DisplayName, request.Code, request.DisplayOrder,
                                  request.Description, request.IsActive, request.HasProduct);
            //şimdilik kapatalım silinmesin
            //if (request.CategoryAttributeIds == null)
            //{
            //    existing.DeleteCategoryAttributes(request.Id);
            //    _categoryRepository.Update(existing);
            //}
            //else
            //{
            if (request.CategoryAttributeIds.Count > 1)
            {
                existing.UpdateCategoryAttributes(request.CategoryAttributeIds, request.Id);
            }
            _categoryRepository.Update(existing);
            //}

            await _dbContextHandler.SaveChangesAsync();

            //durumu pasif olan attributeler response da görünmesin diye tekrar aktif attribute olan kategoriler çekildi
            var existingRes = await _categoryRepository.GetCategoryWithAllRelations(existing.Id);
            existingRes.GetActiveCategoryAttributes();

            return _categoryAssembler.MapToUpdateCategoryCommandResult(existingRes);
        }

    }
}
