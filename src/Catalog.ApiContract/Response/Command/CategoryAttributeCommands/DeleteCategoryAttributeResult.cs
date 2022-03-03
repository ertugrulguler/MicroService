using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Command.CategoryCommands
{
    public class DeleteCategoryAttributeResult
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public List<string> Result { get; set; }

    }
}
