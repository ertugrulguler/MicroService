using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Contract
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }

        public int DisplayOrder { get; set; }

        public bool HasProduct { get; set; }
        public string Description { get; set; }

        public bool Leaf { get; set; }

        public List<AttributeDto> Attributes { get; set; }
    }
}
