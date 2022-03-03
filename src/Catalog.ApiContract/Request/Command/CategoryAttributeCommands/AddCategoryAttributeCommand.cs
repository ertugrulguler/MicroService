
using Catalog.ApiContract.Response.Command.CategoryAttributeCommands;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.CategoryAttributeCommands
{
    public class AddCategoryAttributeCommand : IRequest<ResponseBase<List<CreateCategoryAttributeResult>>>
    {
        public List<CreateCategoryAttribute> CategoryAttribute { get; set; }

    }
    public class CreateCategoryAttribute
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVariantable { get; set; }
        public bool IsListed { get; set; }
    }
}
