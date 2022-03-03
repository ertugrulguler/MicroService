using Catalog.Domain.CategoryAggregate;
using System;

namespace Catalog.ApiContract.Response.Command.CategoryCommands
{
    public class CreateCategory
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public CategoryImage CategoryImage { get; set; }

    }
}
