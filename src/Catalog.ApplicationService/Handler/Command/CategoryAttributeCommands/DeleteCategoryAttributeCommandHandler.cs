using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BrandCommands
{
    public class DeleteCategoryAttributeCommandHandler : IRequestHandler<DeleteCategoryAttributeCommand, ResponseBase<List<DeleteCategoryAttributeResult>>>
    {
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IDbContextHandler _dbContextHandler;
        public DeleteCategoryAttributeCommandHandler(IAttributeRepository attributeRepository,
            ICategoryRepository categoryRepository, ICategoryAttributeRepository categoryAttributeRepository, IDbContextHandler dbContextHandler)
        {
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _dbContextHandler = dbContextHandler;
        }

        public async Task<ResponseBase<List<DeleteCategoryAttributeResult>>> Handle(DeleteCategoryAttributeCommand deletedCategoryAttributes, CancellationToken cancellationToken)
        {
            var list = new List<DeleteCategoryAttributeResult>();
            foreach (var request in deletedCategoryAttributes.DeletedCategoryAttribute)
            {
                var catAttributeError = new List<string>();
                var catAttribute = await _categoryAttributeRepository.FindByAsync(x => x.CategoryId == request.CategoryId && x.AttributeId == request.AttributeId);
                if (catAttribute == null)
                {
                    catAttributeError.Add(ApplicationMessage.CategoryAttributeNotFound.UserMessage());
                }
                var attribute = await _attributeRepository.FindByAsync(f => f.Id == request.AttributeId);
                if (attribute == null)
                {
                    catAttributeError.Add(ApplicationMessage.AttributeNotFound.UserMessage());
                }
                var category = await _categoryRepository.FindByAsync(f => f.Id == request.CategoryId);
                if (category == null)
                {
                    catAttributeError.Add(ApplicationMessage.CategoryNotFound.UserMessage());
                }

                if (catAttribute != null && catAttributeError.Count == 0)
                {
                    catAttribute.SetCategoryAttributeIsActive(false);
                    _categoryAttributeRepository.Update(catAttribute);
                    await _dbContextHandler.SaveChangesAsync();
                    catAttributeError.Add(ApplicationMessage.CategoryAttributeDeleted.UserMessage());
                    list.Add(new DeleteCategoryAttributeResult
                    {
                        AttributeId = request.AttributeId,
                        CategoryId = request.CategoryId,
                        Result = catAttributeError
                    });
                }
                else
                {
                    list.Add(new DeleteCategoryAttributeResult
                    {
                        AttributeId = request.AttributeId,
                        CategoryId = request.CategoryId,
                        Result = catAttributeError
                    });
                }
            }
            return new ResponseBase<List<DeleteCategoryAttributeResult>>()
            {
                Data = list,
                Success = true
            };
        }
    }

}
