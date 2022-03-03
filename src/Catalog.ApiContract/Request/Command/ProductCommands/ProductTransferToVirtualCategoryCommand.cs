using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class ProductTransferToVirtualCategoryCommand : IRequest<ResponseBase<object>>
    {
        public Guid CategoryId { get; set; }
        public List<Guid> ProductIdList { get; set; }
    }
}