using System;

namespace Catalog.ApiContract.Response.Command.AttributeCommands
{
    public class CreateAttributeResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
