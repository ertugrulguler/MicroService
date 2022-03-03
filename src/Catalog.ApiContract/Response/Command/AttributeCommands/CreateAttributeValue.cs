using System;

namespace Catalog.ApiContract.Response.Command.AttributeCommands
{
    public class CreateAttributeValue
    {
        public Guid AttributeId { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
