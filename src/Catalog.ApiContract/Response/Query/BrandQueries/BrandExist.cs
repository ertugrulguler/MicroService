using System;

namespace Catalog.ApiContract.Response.Query.BrandQueries
{
    public class BrandExist
    {
        public Guid Id { get; set; }
        public bool IsExist { get; set; }
    }

}
