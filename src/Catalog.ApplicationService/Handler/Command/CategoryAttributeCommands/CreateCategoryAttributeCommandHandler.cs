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
    public class CreateCategoryAttributeCommandHandler : IRequestHandler<AddCategoryAttributeCommand, ResponseBase<List<CreateCategoryAttributeResult>>>
    {
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IDbContextHandler _dbContextHandler;
        public CreateCategoryAttributeCommandHandler(IAttributeRepository attributeRepository,
            ICategoryRepository categoryRepository, ICategoryAttributeRepository categoryAttributeRepository, IDbContextHandler dbContextHandler)
        {
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _dbContextHandler = dbContextHandler;
        }
        public async Task<ResponseBase<List<CreateCategoryAttributeResult>>> Handle(AddCategoryAttributeCommand categoryAttributes, CancellationToken cancellationToken)
        {
            var list = new List<CreateCategoryAttributeResult>();
            foreach (var request in categoryAttributes.CategoryAttribute)
            {
                var catAttributeError = new List<string>();
                var catAttribute = await _categoryAttributeRepository.FindByAsync(x => x.CategoryId == request.CategoryId && x.AttributeId == request.AttributeId);
                if (catAttribute != null)
                {
                    catAttributeError.Add(ApplicationMessage.CategoryAttributeAlreadyExist.UserMessage());
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
                else if (category?.LeafPath == null)
                {
                    catAttributeError.Add(ApplicationMessage.CategoryIsNotLeaf.UserMessage());
                }
                if (catAttribute == null && catAttributeError.Count == 0)
                {
                    var categoryAttribute = new CategoryAttribute(request.CategoryId, request.AttributeId, request.IsRequired, request.IsVariantable,
                        request.IsListed);
                    await _categoryAttributeRepository.SaveAsync(categoryAttribute);
                    await _dbContextHandler.SaveChangesAsync();
                    var query = await _categoryAttributeRepository.FindByAsync(g => g.CategoryId == request.CategoryId && g.AttributeId == request.AttributeId);
                    list.Add(new CreateCategoryAttributeResult
                    {
                        Id = query.Id,
                        IsListed = request.IsListed,
                        AttributeId = request.AttributeId,
                        CategoryId = request.CategoryId,
                        IsRequired = request.IsRequired,
                        IsVariantable = request.IsVariantable,
                        Error = catAttributeError
                    });
                }
                else
                {
                    list.Add(new CreateCategoryAttributeResult
                    {
                        Id = new System.Guid(),
                        IsListed = request.IsListed,
                        AttributeId = request.AttributeId,
                        CategoryId = request.CategoryId,
                        IsRequired = request.IsRequired,
                        IsVariantable = request.IsVariantable,
                        Error = catAttributeError
                    });
                }
            }
            return new ResponseBase<List<CreateCategoryAttributeResult>>()
            {
                Data = list,
                Success = true
            };
        }
    }
}
