using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApplicationService.Handler.Command.AttributeCommands
{
    public class DeleteAttributeValueCommand : IRequest<ResponseBase<AttributeValueDto>>
    {
        public Guid Id { get; set; }
    }
}
