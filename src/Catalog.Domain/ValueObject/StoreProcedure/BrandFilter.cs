using Microsoft.EntityFrameworkCore;
using System;

namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class BrandFilter
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }

    }
}
