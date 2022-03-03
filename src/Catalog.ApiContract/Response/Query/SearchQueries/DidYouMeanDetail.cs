
using Catalog.Domain.ProductAggregate.ServiceModels;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.SearchQueries
{
    public class DidYouMeanDetail
    {
        public DidYouMeanDetail()
        {
            Search = new List<SearchData>();
        }
        public List<SearchData> Search { get; set; }

    }

    public class SearchData
    {
        public string SearchType { get; set; }
        public string SearchTypeText { get; set; }
        public string CategoryName { get; set; }
        public DeeplinkData DeeplinkData { get; set; }
        public FilterData Filter { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public string SeoUrl { get; set; }

    }

    public class FilterData
    {
        public string Query { get; set; }
        public List<FilterModel> FilterModel { get; set; }
    }

    public class DeeplinkData
    {
        public int Id { get; set; }
        public string DeepLink { get; set; }
    }

}
