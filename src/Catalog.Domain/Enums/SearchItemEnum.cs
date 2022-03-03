using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Enums
{
    public enum SearchItemEnum
    {
        [Display(Name = "product")]
        Product = 0,
        [Display(Name = "category")]
        Category = 1,
        [Display(Name = "brand")]
        Brand = 2,
        [Display(Name = "seller")]
        Seller = 3,
        [Display(Name = "word")]
        Word = 4,
        [Display(Name = "search")]
        Search = 5
    }
}
