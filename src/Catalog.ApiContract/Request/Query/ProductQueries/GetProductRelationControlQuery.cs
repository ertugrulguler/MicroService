using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;


namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductRelationControlQuery : IRequest<ResponseBase<List<string>>>
    {
        public List<AttributeWithValueRequiredforCategory> CategoryAttributeValue { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public string BarcodeNo { get; set; }
    }
    public class AttributeWithValueRequiredforCategory
    {
        public Guid AttributeId { get; set; }
        public Guid AttributeValueId { get; set; }
    }
}

