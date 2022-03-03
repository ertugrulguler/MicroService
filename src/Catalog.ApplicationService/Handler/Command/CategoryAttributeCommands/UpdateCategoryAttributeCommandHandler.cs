using Catalog.ApiContract.Request.Command.CategoryAttributeCommands;
using Catalog.ApiContract.Response.Command.CategoryAttributeCommands;
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
    public class UpdateCategoryAttributeCommandHandler : IRequestHandler<UpdateCategoryAttributeCommand, ResponseBase<List<UpdateCategoryAttributeResult>>>
    {
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IDbContextHandler _dbContextHandler;
        public UpdateCategoryAttributeCommandHandler(IAttributeRepository attributeRepository,
            ICategoryRepository categoryRepository, ICategoryAttributeRepository categoryAttributeRepository, IDbContextHandler dbContextHandler)
        {
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _dbContextHandler = dbContextHandler;
        }

        public async Task<ResponseBase<List<UpdateCategoryAttributeResult>>> Handle(UpdateCategoryAttributeCommand updatedCategoryAttributes, CancellationToken cancellationToken)
        {
            var list = new List<UpdateCategoryAttributeResult>();
            foreach (var request in updatedCategoryAttributes.UpdateCategoryAttribute)
            {
                var catAttributeError = new List<string>();
                var catAttribute = await _categoryAttributeRepository.FindByAsync(x => x.CategoryId == request.CategoryId && x.AttributeId == request.AttributeId
               );
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
                    catAttribute.UpdateCategoryAttribute(request.IsRequired, request.IsVariantable, request.IsListed, request.IsActive);
                    _categoryAttributeRepository.Update(catAttribute);
                    await _dbContextHandler.SaveChangesAsync();
                    catAttributeError.Add(ApplicationMessage.CategoryAttributeUpdated.UserMessage());
                    var query = await _categoryAttributeRepository.FindByAsync(g => g.CategoryId == request.CategoryId && g.AttributeId == request.AttributeId);
                    list.Add(new UpdateCategoryAttributeResult
                    {
                        Id = query.Id,
                        IsListed = request.IsListed,
                        IsRequired = request.IsRequired,
                        IsVariantable = request.IsVariantable,
                        AttributeId = request.AttributeId,
                        CategoryId = request.CategoryId,
                        Error = catAttributeError
                    });
                }
                else
                {
                    list.Add(new UpdateCategoryAttributeResult
                    {
                        Id = new System.Guid(),
                        IsListed = request.IsListed,
                        IsRequired = request.IsRequired,
                        IsVariantable = request.IsVariantable,
                        AttributeId = request.AttributeId,
                        CategoryId = request.CategoryId,
                        Error = catAttributeError
                    });
                }
            }
            return new ResponseBase<List<UpdateCategoryAttributeResult>>()
            {
                Data = list,
                Success = true
            };
        }
    }

}
