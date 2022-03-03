using Catalog.Domain.Entities;
using Framework.Core.Model;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductGroup : Entity
    {
        public Guid ProductId { get; protected set; }

        public string GroupCode { get; protected set; }

        protected ProductGroup()
        {

        }

        public ProductGroup(Guid productId, string groupCode) : this()
        {
            if (string.IsNullOrWhiteSpace(groupCode))
                throw new BusinessRuleException(ApplicationMessage.InvalidParameter,
                ApplicationMessage.InvalidParameter.Message(),
                ApplicationMessage.InvalidParameter.UserMessage());

            ProductId = productId;
            GroupCode = groupCode;
        }

        public void SetProductGroup(Guid productId, string groupCode)
        {
            if (string.IsNullOrWhiteSpace(groupCode))
                throw new BusinessRuleException(ApplicationMessage.InvalidParameter,
                ApplicationMessage.InvalidParameter.Message(),
                ApplicationMessage.InvalidParameter.UserMessage());

            ProductId = productId;
            GroupCode = groupCode;
        }

        public void SetGroupCode(string groupCode)
        {
            if (string.IsNullOrWhiteSpace(groupCode))
                throw new BusinessRuleException(ApplicationMessage.InvalidParameter,
                ApplicationMessage.InvalidParameter.Message(),
                ApplicationMessage.InvalidParameter.UserMessage());

            GroupCode = groupCode + "-";
        }
    }
}
