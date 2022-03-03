using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class DeleteCategoryInstallmentCommand : IRequest<ResponseBase<object>>
    {
        public Guid CategoryId { get; set; }
    }
}
