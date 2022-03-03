using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class PriceFilter
    {
        public decimal SalePrice { get; set; }
        // public string Name { get; set; }

    }
}
