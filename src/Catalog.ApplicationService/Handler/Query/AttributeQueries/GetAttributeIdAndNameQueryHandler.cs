using Catalog.ApiContract.Request.Query.AttributeQueries;
using Catalog.ApiContract.Response.Query.AttributeQueries;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.AttributeAggregate.ServiceModels;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.AttributeQueries
{
    public class GetAttributeIdAndNameQueryHandler : IRequestHandler<GetAttributeAndValueWithCategoryQuery,
        ResponseBase<GetAttributeAndValueQueryResult>>
    {
        private readonly IAttributeDomainService _attributeDomainService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;
        public GetAttributeIdAndNameQueryHandler(IAttributeDomainService attributeDomainService,
            ICategoryAttributeRepository categoryAttributeRepository,
            IAttributeValueRepository attributeValueRepository,
            IAttributeMapRepository attributeMapRepository,
            ICategoryRepository categoryRepository)
        {
            _attributeDomainService = attributeDomainService;
            _categoryAttributeRepository = categoryAttributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _categoryRepository = categoryRepository;
            _attributeMapRepository = attributeMapRepository;
        }

        public async Task<ResponseBase<GetAttributeAndValueQueryResult>> Handle(
            GetAttributeAndValueWithCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.FindByAsync(j => j.Id == request.CategoryId);
            var categoryAttributes =
                await _categoryAttributeRepository.FilterByAsync(x => x.CategoryId == request.CategoryId);
            var attributeValueList = new Dictionary<string, Guid>();
            if (categoryAttributes.Count == 0)
                return new ResponseBase<GetAttributeAndValueQueryResult>()
                {
                    Data = new GetAttributeAndValueQueryResult { AttributeNameDic = new Dictionary<string, AttributeIdAndRequiredList> { }, AttributeValueNamesDic = new Dictionary<string, Guid> { } },
                    Success = true
                };

            var attributeIds = categoryAttributes.ToList();
            var attributeList = await _attributeDomainService.GetAttributeName(attributeIds.Select(x => new AttributeIdAndRequiredList { Id = x.AttributeId, IsRequired = x.IsRequired }).ToList());

            if (attributeList == null)
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                    ApplicationMessage.EmptyList.Message(),
                    ApplicationMessage.EmptyList.UserMessage());
            if (!string.IsNullOrEmpty(category.Code))
            {
                var datas = await _attributeMapRepository.FilterByAsync(x => attributeIds.Select(u => u.AttributeId).ToList().Contains(x.AttributeId));
                attributeValueList = await _attributeDomainService.GetAttributeValueNameWithMap(datas.Select(x => x.AttributeValueId).ToList(), attributeIds);
            }
            else
            {
                var values = await _attributeValueRepository.FilterByAsync(x => attributeIds.Select(u => u.AttributeId).ToList().Contains(x.AttributeId.Value));
                attributeValueList =
                    await _attributeDomainService.GetAttributeValueName(values.Select(x => x.Id).ToList());
            }

            if (attributeValueList == null)
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                    ApplicationMessage.EmptyList.Message(),
                    ApplicationMessage.EmptyList.UserMessage());

            return new ResponseBase<GetAttributeAndValueQueryResult>()
            {
                Data = new GetAttributeAndValueQueryResult
                { AttributeNameDic = attributeList, AttributeValueNamesDic = attributeValueList },
                Success = true
            };
        }


    }
}