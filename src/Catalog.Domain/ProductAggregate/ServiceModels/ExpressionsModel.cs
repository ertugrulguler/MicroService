using Catalog.Domain.CategoryAggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class ExpressionsModel
    {
        public List<Category> categorySubList { get; set; }
        public Expression<Func<Product, bool>> expressionAllProduct { get; set; }
        public List<List<Guid>> attributeAllIdList { get; set; }
        public Expression<Func<ProductSeller, bool>> expressionAllProductSeller { get; set; }
    }
}
