using System;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class CategoryIdAndName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LeafPath { get; set; }
        public string LeafPathName { get; set; }
    }
}
