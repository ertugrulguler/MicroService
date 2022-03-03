using Framework.Core.Model;

namespace Catalog.Domain.ValueObject
{
    public class ProductGroupCode
    {
        public string Code { get; protected set; }
        public ProductGroupCode(string code)
        {
            if (code.Length < 4 || code.Length > 8)
            {
                throw new BusinessRuleException(ApplicationMessage.AttributeCodeInvalid,
                    ApplicationMessage.AttributeCodeInvalid.Message(),
                    ApplicationMessage.AttributeCodeInvalid.UserMessage());
            }

            Code = code;
        }
    }
}
