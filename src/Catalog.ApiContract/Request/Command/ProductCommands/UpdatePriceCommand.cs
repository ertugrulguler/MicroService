using Catalog.ApiContract.Response.Command.ProductCommands;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class UpdatePriceCommand : IRequest<ResponseBase<List<UpdatePriceControlResult>>>
    {
        public Guid SellerId { get; set; }
        public List<PriceAndInventory> Items { get; set; }
    }
}