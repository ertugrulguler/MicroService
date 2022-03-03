using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Contract
{

    public class CategoryModelForMobileDto
    {
        public string Title { get; set; }
        public List<CategoryListForMobileDto> Categories { get; set; }

        public CategoryModelForMobileDto(string title)
        {
            Title = title;
            Categories = new List<CategoryListForMobileDto>();

        }
    }

    public class CategoryListForMobileDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public bool Leaf { get; set; }
        public string Url { get; set; }

        public List<CategoryListForMobileDto> SubCategories { get; set; }
        public List<CategoryListForMobileDto> SuggestedCategories { get; set; }

        public CategoryListForMobileDto()
        {
            SubCategories = new List<CategoryListForMobileDto>();
            SuggestedCategories = new List<CategoryListForMobileDto>();
        }
    }

}
