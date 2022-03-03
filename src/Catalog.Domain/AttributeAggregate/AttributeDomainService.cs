using Catalog.Domain.AttributeAggregate.ServiceModels;
using Catalog.Domain.CategoryAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Catalog.Domain.AttributeAggregate
{
    public class AttributeDomainService : IAttributeDomainService
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;
        public AttributeDomainService(IAttributeRepository attributeRepository,
            IAttributeMapRepository attributeMapRepository,
            IAttributeValueRepository attributeValueRepository)
        {
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _attributeMapRepository = attributeMapRepository;
        }
        public async Task<Dictionary<string, AttributeIdAndRequiredList>> GetAttributeName(List<AttributeIdAndRequiredList> list)
        {
            var attNameDic = new Dictionary<string, AttributeIdAndRequiredList>();
            var attributeList = await _attributeRepository.FilterByAsync(a => list.Select(p => p.Id).ToList().Contains(a.Id));
            if (attributeList != null)
            {
                foreach (var x in attributeList)
                {
                    if (!attNameDic.ContainsKey(x.DisplayName))
                        attNameDic.Add(x.DisplayName, new AttributeIdAndRequiredList { Id = x.Id, IsRequired = list.Where(p => p.Id == x.Id).FirstOrDefault().IsRequired });
                }
            }
            return attNameDic;
        }
        public async Task<Dictionary<string, Guid>> GetAttributeValueNameWithMap(List<Guid> list, List<CategoryAttribute> attList)
        {
            var attValueNameDic = new Dictionary<string, Guid>();
            var attributeMapValues = await _attributeMapRepository.FilterByAsync(x => list.Contains(x.AttributeValueId));
            var attValues = await _attributeValueRepository.FilterByAsync(x => list.Contains(x.Id));
            if (attributeMapValues != null)
            {
                foreach (var x in attributeMapValues)
                {
                    var attValue = attValues.Where(j => j.Id == x.AttributeValueId).FirstOrDefault();
                    var attMap = attributeMapValues.Where(f => f.AttributeValueId == x.AttributeValueId);
                    var attIds = attList.Select(j => j.AttributeId).ToList();
                    var listMapValues = attMap.Where(h => attIds.Contains(h.AttributeId));
                    foreach (var f in listMapValues)
                    {
                        if (!attValueNameDic.ContainsKey(attValue.Value + "$&#@" + f.AttributeId))
                            attValueNameDic.Add(attValue.Value + "$&#@" + f.AttributeId, x.AttributeValueId);

                    }
                }
            }
            return attValueNameDic;
        }
        public async Task<Dictionary<string, Guid>> GetAttributeValueName(List<Guid> list)
        {
            var attValueNameDic = new Dictionary<string, Guid>();
            var attributeValues = await _attributeValueRepository.FilterByAsync(x => list.Contains(x.Id));
            if (attributeValues != null)
            {
                foreach (var x in attributeValues)
                {
                    if (!attValueNameDic.ContainsKey(x.Value + "$&#@" + x.AttributeId))
                        attValueNameDic.Add(x.Value + "$&#@" + x.AttributeId, x.Id);
                }
            }
            return attValueNameDic;
        }
    }
}
