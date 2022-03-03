using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public class AttributeValueService : IAttributeValueService
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        public AttributeValueService(IDbContextHandler dbContextHandler, IAttributeValueRepository attributeValueRepository)
        {
            _attributeValueRepository = attributeValueRepository;
        }
        public async Task<Guid?> GetAttributeValueId(string value)
        {
            var attValue = await _attributeValueRepository.FilterByAsync(h => h.SeoName == value && !string.IsNullOrEmpty(h.Code) && h.AttributeId == null);
            return attValue?.FirstOrDefault()?.Id;
        }
    }
}

