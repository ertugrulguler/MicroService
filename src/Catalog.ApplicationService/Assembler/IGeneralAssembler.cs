using Catalog.Domain.Enums;

namespace Catalog.ApplicationService.Assembler
{
    public interface IGeneralAssembler
    {
        OrderBy GetOrderBy(string orderBy, bool isSearch);
        string GetSeoName(string name, SeoNameType type);
    }
}
