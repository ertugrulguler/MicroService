using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class DeleteCategoryCommand : IRequest<ResponseBase<object>>
    {
        public Guid Id { get; set; }
    }
}
