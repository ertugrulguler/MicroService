using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.AttributeCommands;
using Catalog.ApiContract.Response.Query.AttributeQueries;
using Catalog.Domain.AttributeAggregate;
using Framework.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.ApplicationService.Assembler
{
    public class AttributeAssembler : IAttributeAssembler
    {
        public ResponseBase<AttributeDto> MapToCreateAttributeCommandResult(Attribute attribute)
        {
            return new()
            {
                Data = new AttributeDto()
                {
                    Name = attribute.Name,
                    Description = attribute.Description,
                    DisplayName = attribute.DisplayName,
                },
                Success = true
            };
        }

        public ResponseBase<CreateAttributeValue> MapToCreateAttributeValueCommandResult(AttributeValue attributeValue)
        {
            var createAttributeValue = new CreateAttributeValue()
            {
                AttributeId = attributeValue.AttributeId.Value,
                Unit = attributeValue.Unit,
                Value = attributeValue.Value
            };
            return new ResponseBase<CreateAttributeValue>()
            {
                Data = createAttributeValue
            };
        }
        public ResponseBase<AttributeValueDto> MapToDeleteAttributeCommandResult(AttributeValue attributeValue)
        {
            return new()
            {
                Data = new AttributeValueDto
                {
                    Id = attributeValue.Id
                }
            };
        }

        public ResponseBase<GetAllAttributeNameWithValues> MapToGetAllAttributeNameWithValuesQueryResult(List<Attribute> attributes, List<AttributeValue> attributeValues)
        {
            var getAllAttributeNameWithValues = new GetAllAttributeNameWithValues()
            {
                AttributeNames = attributes.Select(x => new AttributeNames { Id = x.Id, Name = x.Name }).ToList(),
                AttributeValueNames = attributeValues.Select(x => new AttributeValueNames { Id = x.Id, Value = x.Value }).ToList()
            };
            return new ResponseBase<GetAllAttributeNameWithValues>()
            {
                Data = getAllAttributeNameWithValues,
                Success = true
            };
        }

        public ResponseBase<AttributeValueDto> MapToUpdateAttributeValueCommandResult(AttributeValue attributeValue)
        {
            return new()
            {
                Data = new AttributeValueDto
                {
                    AttributeId = attributeValue.AttributeId,
                    Unit = attributeValue.Unit,
                    Value = attributeValue.Value
                }
            };
        }
        public ResponseBase<List<AttributeValueDto>> MapToGetAttributeValueQueryResult(List<AttributeValue> list)
        {
            var attributeDtoList = new List<AttributeValueDto>();
            foreach (var value in list)
            {
                attributeDtoList.Add(new AttributeValueDto
                {
                    AttributeId = value.AttributeId,
                    Unit = value.Unit,
                    Value = value.Value
                });
            }

            return new()
            {
                Data = attributeDtoList,
                Success = true
            };
        }
    }
}
