

using Framework.Core.Model;

namespace Catalog.ApiContract.Response
{
    public class PageResponse
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public PageResponse(int totalCount, PagerInput pagerInput)
        {
            this.PageSize = pagerInput.PageSize;
            this.PageIndex = pagerInput.PageIndex;
            this.TotalCount = totalCount;
            this.TotalPages = this.TotalCount / this.PageSize;

            if (this.TotalCount % this.PageSize > 0)
                this.TotalPages++;
        }
    }
}
