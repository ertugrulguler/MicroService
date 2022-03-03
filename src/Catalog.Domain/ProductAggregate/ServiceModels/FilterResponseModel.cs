using Catalog.Domain.CategoryAggregate;
using System;
using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class FilterResponseModel
    {
        public List<Category> categorySubList { get; set; }
        public List<Guid> sellerIdList { get; set; }
        public List<List<Guid>> attributeAllIdList { get; set; }
        public List<string> salePriceList { get; set; }
        public List<Guid> brandIdList { get; set; }
        public List<string> codeList { get; set; }
        public List<Guid> searchList { get; set; }
    }

}
