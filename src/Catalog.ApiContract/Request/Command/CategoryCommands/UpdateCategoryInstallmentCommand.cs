using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class UpdateCategoryInstallmentCommand : IRequest<ResponseBase<CategoryInstallmentDto>>
    {
        public Guid CategoryId { get; set; }
        public int MaxInstallmentCount { get; set; }
        public decimal? MinPrice { get; set; }
    }
}
