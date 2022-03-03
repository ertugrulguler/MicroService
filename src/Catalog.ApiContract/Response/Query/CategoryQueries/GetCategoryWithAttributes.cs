using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class GetCategoryWithAttributes
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public List<AttributeQueryResult> Attributes { get; set; }
    }

    public class AttributeQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsVariantable { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
        public List<AttributeValueQueryResult> AttributeValues { get; set; }


    }


    public class AttributeValueQueryResult
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

    }
}
