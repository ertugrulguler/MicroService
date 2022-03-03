using Catalog.Domain.Enums;

namespace Catalog.ApplicationService.Assembler
{
    public class GeneralAssembler : IGeneralAssembler
    {
        public OrderBy GetOrderBy(string orderBy, bool isSearch)
        {
            OrderBy sort = OrderBy.Suggession;
            if (orderBy == "en-yeniler")
                sort = OrderBy.Newest;
            else if (orderBy == "fiyat-artan")
                sort = OrderBy.AscPrice;
            else if (orderBy == "fiyat-azalan")
                sort = OrderBy.DescPrice;
            else if (isSearch && string.IsNullOrEmpty(orderBy))
                sort = OrderBy.None;
            return sort;
        }
        public string GetSeoName(string name, SeoNameType type)
        {
            var seoName = ConvertSeoName(name.ToLower());
            seoName = seoName.ToLower();
            if (!string.IsNullOrEmpty(seoName) && (type == SeoNameType.Brand || type == SeoNameType.Category || type == SeoNameType.OrderBy || type == SeoNameType.Seller || type == SeoNameType.AttributeValue))
            {
                if (seoName.Contains(" ")) seoName = seoName.Replace(" ", "-");
            }
            else if (!string.IsNullOrEmpty(seoName) && (type == SeoNameType.Attribute))
            {
                if (seoName.Contains(" ")) seoName = seoName.Replace(" ", "");
            }
            return seoName;
        }
        public string ConvertSeoName(string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            text = text.Replace("'", "");
            text = text.Replace("/", "");
            text = text.Replace("+", "");
            text = text.Replace(".", "-");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("}", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace("&", "");
            text = text.Replace(",", "");
            text = text.Replace("*", "");
            text = text.Replace(":", "");
            text = text.Replace(";", "");
            text = text.Replace("#", "");
            text = text.Replace("%", "");
            text = text.Replace("!", "");
            text = text.Replace("=", "");
            text = text.Replace("?", "");
            return text;
        }
    }
}
