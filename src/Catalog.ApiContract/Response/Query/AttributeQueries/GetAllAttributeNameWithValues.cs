using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.AttributeQueries
{
    public class GetAllAttributeNameWithValues
    {
        public List<AttributeNames> AttributeNames { get; set; }
        public List<AttributeValueNames> AttributeValueNames { get; set; }
    }
    public class AttributeNames
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
    public class AttributeValueNames
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
