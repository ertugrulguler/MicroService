using System;

namespace Catalog.Domain.AttributeAggregate.ServiceModels
{
    public class AttributeIdAndRequiredList
    {
        public Guid Id { get; set; }
        public bool IsRequired { get; set; }
    }
}

