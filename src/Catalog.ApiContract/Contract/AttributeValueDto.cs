using System;

namespace Catalog.ApiContract.Contract
{
    public class AttributeValueDto
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public Guid Id { get; set; }
    }
}
