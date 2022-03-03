using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoryWithAttributesQueryHandler : IRequestHandler<GetCategoryWithAttributesQuery, ResponseBase<GetCategoryWithAttributes>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryAttributeValueMapRepository _categoryAttributeValueMapRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;

        public GetCategoryWithAttributesQueryHandler(
            ICategoryRepository categoryRepository,
            IAttributeRepository attributeRepository,
            ICategoryAssembler categoryAssembler,
            IAttributeValueRepository attributeValueRepository,
            ICategoryAttributeValueMapRepository categoryAttributeValueMapRepository,
            IAttributeMapRepository attributeMapRepository
            )
        {
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _categoryAssembler = categoryAssembler;
            _attributeValueRepository = attributeValueRepository;
            _categoryAttributeValueMapRepository = categoryAttributeValueMapRepository;
            _attributeMapRepository = attributeMapRepository;
        }

        public async Task<ResponseBase<GetCategoryWithAttributes>> Handle(GetCategoryWithAttributesQuery request, CancellationToken cancellationToken)
        {

            Category category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new BusinessRuleException(ApplicationMessage.EmptyCategoryList,
                 ApplicationMessage.EmptyCategoryList.Message(),
                 ApplicationMessage.EmptyCategoryList.UserMessage());
            }
            if (string.IsNullOrWhiteSpace(category.Code))
            {
                category = await _categoryRepository.GetCategoryWithAttributeValues(request.Id);
            }
            else
            {
                category = await _categoryRepository.GetCategoryWithAttributeMap(request.Id);

                var attributes = await _attributeRepository.FilterByAsync(x => category.CategoryAttributes.Select(x => x.AttributeId).Contains(x.Id));
                var attributeValueByCategoryAttributes = await _categoryAttributeValueMapRepository.FilterByAsync(x => category.CategoryAttributes.Select(a => a.Id).Contains(x.CategoryAttributeId));
                var attributeValues = await _attributeValueRepository.FilterByAsync(x => attributeValueByCategoryAttributes.Select(av => av.AttributeValueId).Contains(x.Id));
                var attributeMaps = await _attributeMapRepository.FilterByAsync(x => attributeValues.Select(av => av.Id).Contains(x.AttributeValueId));

                return _categoryAssembler.MapToGetCategoryWithAttributesCodeResult(category, attributeMaps, attributes, attributeValues, attributeValueByCategoryAttributes, request.OnlyRequiredFields);
            }
            return _categoryAssembler.MapToGetCategoryWithAttributesQueryResult(category, request.OnlyRequiredFields);
        }
    }
}