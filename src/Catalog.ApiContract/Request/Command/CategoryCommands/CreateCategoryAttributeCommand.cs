using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class CreateCategoryAttributeCommand : IRequest<ResponseBase<CategoryAttributeDto>>
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public bool IsRequired { get; set; }
    }
}
