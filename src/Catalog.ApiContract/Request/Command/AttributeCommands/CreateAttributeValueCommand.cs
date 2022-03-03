using Catalog.ApiContract.Response.Command.AttributeCommands;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.AttributeCommands
{
    public class CreateAttributeValueCommand : IRequest<ResponseBase<CreateAttributeValue>>
    {
        public Guid AttributeId { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int Order { get; set; }
    }
}
