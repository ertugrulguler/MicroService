using Catalog.ApiContract.Contract;
using Framework.Core.Model;

using MediatR;

using System;

namespace Catalog.ApiContract.Request.Command.AttributeCommands
{
    public class UpdateAttributeCommand : IRequest<ResponseBase<AttributeDto>>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
