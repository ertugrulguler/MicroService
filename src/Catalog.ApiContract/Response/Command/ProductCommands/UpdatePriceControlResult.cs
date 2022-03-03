using Catalog.ApiContract.Request.Command.ProductCommands;

namespace Catalog.ApiContract.Response.Command.ProductCommands
{
    public class UpdatePriceControlResult
    {
        public string Error { get; set; }
        public PriceAndInventory Item { get; set; }
    }
}