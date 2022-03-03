using Catalog.Domain.AttributeAggregate.ServiceModels;

using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.AttributeQueries
{
    public class GetAttributeAndValueQueryResult
    {
        public Dictionary<string, AttributeIdAndRequiredList> AttributeNameDic { get; set; }
        public Dictionary<string, Guid> AttributeValueNamesDic { get; set; }
    }
}
