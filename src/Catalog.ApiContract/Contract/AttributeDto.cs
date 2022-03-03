using System.Collections.Generic;

namespace Catalog.ApiContract.Contract
{
    public class AttributeDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<AttributeValueDto> AttributeValues { get; set; }
    }
}
