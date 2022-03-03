using System.Threading.Tasks;

namespace Catalog.Domain
{
    public interface IDbContextHandler
    {
        Task SaveChangesAsync();
    }
}