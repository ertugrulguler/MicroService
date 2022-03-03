using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.AttributeCommands
{
    public class UpdateAttributeValueCommand : IRequest<ResponseBase<AttributeValueDto>>
    {
        public Guid? Id { get; set; }
        public Guid AttributeId { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int Order { get; set; }
    }
}
