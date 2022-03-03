using Catalog.ApiContract.Response.Command.CategoryCommands;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class DeleteCategoryAttributeCommand : IRequest<ResponseBase<List<DeleteCategoryAttributeResult>>>
    {
        public List<DeletedCategoryAttribute> DeletedCategoryAttribute { get; set; }
    }

    public class DeletedCategoryAttribute
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
    }
}
