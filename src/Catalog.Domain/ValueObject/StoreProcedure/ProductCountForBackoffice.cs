using Microsoft.EntityFrameworkCore;
namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class ProductCountForBackoffice
    {
        public int OnSaleStockCount { get; set; }
        public int OnSaleProductCount { get; set; }
        public int OnSoldProductCount { get; set; }
    }
}