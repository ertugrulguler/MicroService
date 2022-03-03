using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Enums
{
    public enum CardUserType
    {
        None = 0,
        [Display(Name = "Bireysel")]
        Individual = 1,
        [Display(Name = "Ticari")]
        Commercial = 2
    }

}
