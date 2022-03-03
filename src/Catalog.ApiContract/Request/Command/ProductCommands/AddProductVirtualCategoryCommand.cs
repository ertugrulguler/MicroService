using Catalog.ApiContract.Response.Command.ProductCommands;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class AddProductVirtualCategoryCommand : IRequest<ResponseBase<AddProductVirtualCategoryResult>>
    {
        public List<string> Code { get; set; }
        public Guid VirtualCategoryId { get; set; }
        public VirtualCategoryActionType VirtualCategoryActionType { get; set; }
    }
}