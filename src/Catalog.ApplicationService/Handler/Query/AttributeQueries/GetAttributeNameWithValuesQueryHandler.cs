using Catalog.ApiContract.Request.Query.AttributeQueries;
using Catalog.ApiContract.Response.Query.AttributeQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.AttributeAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.AttributeQueries
{
    public class GetAllAttributeNameWithValuesQueryHandler : IRequestHandler<GetAllAttributeNameWithValuesQuery,
        ResponseBase<GetAllAttributeNameWithValues>>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;
        private readonly IAttributeAssembler _attributeAssembler;

        public GetAllAttributeNameWithValuesQueryHandler(IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository, IAttributeAssembler attributeAssembler, IAttributeMapRepository attributeMapRepository)
        {
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _attributeAssembler = attributeAssembler;
            _attributeMapRepository = attributeMapRepository;
        }

        public async Task<ResponseBase<GetAllAttributeNameWithValues>> Handle(
            GetAllAttributeNameWithValuesQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _attributeRepository.FilterByAsync(a => request.AttributeIdList.Contains(a.Id));

            var attributeValues = new List<AttributeValue>();

            bool newAtt = attributes.FirstOrDefault().Code != null ? true : false;

            if (newAtt)
            {
                var attributeValueIds = _attributeMapRepository.FilterByAsync(x => request.AttributeIdList.Contains(x.AttributeId)).Result.Select(av => av.AttributeValueId);
                attributeValues = await _attributeValueRepository.FilterByAsync(x => attributeValueIds.Contains(x.Id));
            }
            else
                attributeValues = await _attributeValueRepository.FilterByAsync(v => request.AttributeIdList.Contains(v.AttributeId.Value));

            return _attributeAssembler.MapToGetAllAttributeNameWithValuesQueryResult(attributes, attributeValues);
        }
    }
}