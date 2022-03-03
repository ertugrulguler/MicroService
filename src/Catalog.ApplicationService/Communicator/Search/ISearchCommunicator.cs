using Catalog.ApplicationService.Communicator.Search.Model;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Search
{
    public interface ISearchCommunicator
    {
        Task<DidYouMeanResponse> DidYouMean(DidYouMeanRequest request);
        Task<SearchResponse> Search(SearchRequest request);
    }
}
