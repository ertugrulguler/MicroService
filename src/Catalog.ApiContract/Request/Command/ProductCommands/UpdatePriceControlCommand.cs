using Catalog.ApiContract.Response.Command.ProductCommands;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class UpdatePriceControlCommand : IRequest<ResponseBase<List<UpdatePriceControlResult>>>
    {
        public Guid SellerId { get; set; }
        public List<PriceAndInventory> Items { get; set; }
    }
    public class PriceAndInventory
    {
        public string Code { get; set; }
        public int StockCount { get; set; }
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}