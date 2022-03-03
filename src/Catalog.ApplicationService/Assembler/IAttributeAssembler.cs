using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.AttributeCommands;
using Catalog.ApiContract.Response.Query.AttributeQueries;
using Catalog.Domain.AttributeAggregate;
using Framework.Core.Model;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Assembler
{
    public interface IAttributeAssembler
    {
        ResponseBase<AttributeDto> MapToCreateAttributeCommandResult(Catalog.Domain.AttributeAggregate.Attribute attribute);
        ResponseBase<CreateAttributeValue> MapToCreateAttributeValueCommandResult(Catalog.Domain.AttributeAggregate.AttributeValue attributeValue);
        ResponseBase<AttributeValueDto> MapToDeleteAttributeCommandResult(Catalog.Domain.AttributeAggregate.AttributeValue attributeValue);
        ResponseBase<AttributeValueDto> MapToUpdateAttributeValueCommandResult(Catalog.Domain.AttributeAggregate.AttributeValue attributeValue);
        ResponseBase<List<AttributeValueDto>> MapToGetAttributeValueQueryResult(List<Catalog.Domain.AttributeAggregate.AttributeValue> list);
        ResponseBase<GetAllAttributeNameWithValues> MapToGetAllAttributeNameWithValuesQueryResult(List<Attribute> attributes, List<AttributeValue> attributeValues);
    }
}