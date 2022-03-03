using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Command.CategoryAttributeCommands
{
    public class UpdateCategoryAttributeResult
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVariantable { get; set; }
        public bool IsListed { get; set; }
        public List<string> Error { get; set; }
    }
}
