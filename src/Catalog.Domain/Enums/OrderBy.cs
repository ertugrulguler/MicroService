using System.ComponentModel;

namespace Catalog.Domain.Enums
{
    public enum OrderBy
    {
        [Description("Varsayılan")]
        Suggession = 0,

        [Description("Fiyat Artan")]
        AscPrice = 1,

        [Description("Fiyat Azalan")]
        DescPrice = 2,

        [Description("En Yeniler")]
        Newest = 3,
        [Description("None")]
        None = 4,
    }

    public enum OrderByDate
    {
        CreateDate = 0,
        UpdateDate = 1
    }
}
