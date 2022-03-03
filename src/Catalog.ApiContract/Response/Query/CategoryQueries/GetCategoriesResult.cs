using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class GetCategoriesResult
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool Leaf { get; set; }
        public List<GetCategoriesResult> SubCategories { get; set; }
        public List<GetCategoriesResult> SuggestedCategories { get; set; }

        public GetCategoriesResult()
        {
            SubCategories = new List<GetCategoriesResult>();
            SuggestedCategories = new List<GetCategoriesResult>();
        }
    }
}