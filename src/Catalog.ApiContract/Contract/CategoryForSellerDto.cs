using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Contract
{

    public class CategoryForSellerDto
    {
        public string Title { get; set; }
        public List<CategoryListForSellerDto> Categories { get; set; }

        public CategoryForSellerDto(string title)
        {
            Title = title;
            Categories = new List<CategoryListForSellerDto>();

        }
    }

    public class CategoryListForSellerDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public bool Leaf { get; set; }
        public List<CategoryListForSellerDto> SubCategories { get; set; }

        public CategoryListForSellerDto()
        {
            SubCategories = new List<CategoryListForSellerDto>();
        }
    }

}
