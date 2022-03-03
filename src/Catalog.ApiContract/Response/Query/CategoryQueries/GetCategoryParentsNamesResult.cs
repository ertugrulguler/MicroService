using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class GetCategoryParentsNamesResult
    {
        Dictionary<Guid, string> CategoryDic { get; set; }

    }
}
