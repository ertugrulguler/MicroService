using System;

namespace Catalog.ApiContract.Response.Command.CategoryCommands
{
    public class CreateVirtualCategoryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
