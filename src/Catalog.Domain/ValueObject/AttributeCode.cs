using Framework.Core.Model;

namespace Catalog.Domain.ValueObject
{
    public class AttributeCode
    {
        //max 6 mn 4 karakter olur, 0 ile başlayamaz, son rakamı karakter olur
        public string Code { get; protected set; }
        public AttributeCode(string code)
        {
            if (code.Length < 4 || code.Length > 6)
            {
                throw new BusinessRuleException(ApplicationMessage.AttributeCodeInvalid,
                    ApplicationMessage.AttributeCodeInvalid.Message(),
                    ApplicationMessage.AttributeCodeInvalid.UserMessage());
            }

            Code = code;
        }
    }
}