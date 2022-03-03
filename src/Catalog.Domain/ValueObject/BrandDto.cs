using System;

namespace Catalog.Domain.ValueObject
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }
        public bool Status { get; set; }
    }
}
