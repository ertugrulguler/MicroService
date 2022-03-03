using Microsoft.EntityFrameworkCore;
using System;

namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class SellerFilter
    {
        public Guid SellerId { get; set; }
        // public string Name { get; set; }

    }
}
