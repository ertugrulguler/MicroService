using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.AttributeAggregate.ServiceModels;
using Framework.Core.Model;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Attribute = Catalog.Domain.AttributeAggregate.Attribute;

namespace Catalog.ApplicationService.Handler.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;
        private readonly IGeneralAssembler _generalAssembler;


        public AttributeService(IAttributeRepository attributeRepository, IGeneralAssembler generalAssembler, IAttributeValueRepository attributeValueRepository, IAttributeMapRepository attributeMapRepository)
        {
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _attributeMapRepository = attributeMapRepository;
            _generalAssembler = generalAssembler;
        }

        public bool ReadFromExcelWithAttributeAllRelation(IFormFile fi)
        {
            var extension = Path.GetExtension(fi?.FileName);
            if (fi != null && extension.ToLower() == ".xlsx")
            {
                var stream = fi.OpenReadStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workbook = package.Workbook;


                    #region Attribute

                    var worksheetAttribute = workbook.Worksheets[0];
                    int rowCountAttribute = worksheetAttribute.Dimension.End.Row;
                    var listAttribute = new List<ExcelDataModelAttribute>();

                    for (int row = 2; row <= rowCountAttribute; row++)
                    {
                        var seoName = _generalAssembler.GetSeoName(worksheetAttribute.Cells[row, 2].Value?.ToString(), Domain.Enums.SeoNameType.Attribute);
                        var rowExcel = new ExcelDataModelAttribute
                        {

                            SeoName = seoName,
                            Code = worksheetAttribute.Cells[row, 1].Value?.ToString(),
                            Name = worksheetAttribute.Cells[row, 2].Value?.ToString(),
                            Id = new Guid(worksheetAttribute.Cells[row, 3].Value?.ToString()),
                            IsActive = CheckStringToBool(worksheetAttribute.Cells[row, 4].Value?.ToString())

                        };
                        listAttribute.Add(rowExcel);
                    }

                    var att = InsertAttributeToDb(listAttribute);
                    if (!att)
                        throw new BusinessRuleException(ApplicationMessage.AttributeNotCorrectFormat,
                                                        ApplicationMessage.AttributeNotCorrectFormat.Message(),
                                                        ApplicationMessage.AttributeNotCorrectFormat.UserMessage());


                    #endregion

                    #region AttributeValue

                    var worksheetAttributeValue = workbook.Worksheets[1];
                    int rowCountAttributeValue = worksheetAttributeValue.Dimension.End.Row;
                    var listAttributeValue = new List<ExcelDataModelAttributeValue>();

                    for (int row = 2; row <= rowCountAttributeValue; row++)
                    {

                        var seoName = _generalAssembler.GetSeoName(worksheetAttributeValue.Cells[row, 2].Value?.ToString(), Domain.Enums.SeoNameType.AttributeValue);
                        var rowExcel = new ExcelDataModelAttributeValue
                        {

                            Id = new Guid(worksheetAttributeValue.Cells[row, 1].Value?.ToString()),
                            Value = worksheetAttributeValue.Cells[row, 2].Value?.ToString(),
                            Code = worksheetAttributeValue.Cells[row, 3].Value?.ToString(),
                            IsActive = CheckStringToBool(worksheetAttributeValue.Cells[row, 4].Value?.ToString()),
                            SeoName = seoName
                        };
                        listAttributeValue.Add(rowExcel);
                    }

                    var attValue = InsertAttributeValueToDb(listAttributeValue);
                    if (!attValue)
                        throw new BusinessRuleException(ApplicationMessage.AttributeValueNotCorrectFormat,
                               ApplicationMessage.AttributeValueNotCorrectFormat.Message(),
                               ApplicationMessage.AttributeValueNotCorrectFormat.UserMessage());


                    #endregion

                    #region AttributeMap

                    var worksheetAttributeMap = workbook.Worksheets[2];
                    int rowCountAttributeMap = worksheetAttributeMap.Dimension.End.Row;
                    var listAttributeMap = new List<ExcelDataModelAttributeMap>();

                    for (int row = 2; row <= rowCountAttributeMap; row++)
                    {
                        var rowExcel = new ExcelDataModelAttributeMap
                        {
                            AttributeId = new Guid(worksheetAttributeMap.Cells[row, 1].Value?.ToString()),
                            AttributeValueId = new Guid(worksheetAttributeMap.Cells[row, 2].Value?.ToString()),
                            Id = new Guid(worksheetAttributeMap.Cells[row, 3].Value?.ToString()),
                            IsActive = CheckStringToBool(worksheetAttributeMap.Cells[row, 4].Value?.ToString())

                        };
                        listAttributeMap.Add(rowExcel);
                    }

                    var attMap = InsertAttributeMapToDb(listAttributeMap);
                    if (!attMap)
                        throw new BusinessRuleException(ApplicationMessage.AttributeMapNotCorrectFormat,
                               ApplicationMessage.AttributeMapNotCorrectFormat.Message(),
                               ApplicationMessage.AttributeMapNotCorrectFormat.UserMessage());

                    #endregion

                }
                return true;

            }
            return false;
        }
        private static bool CheckStringToBool(string str)
        {
            if (str == "0" || string.IsNullOrWhiteSpace(str) || str == "Hayır")
                return false;
            return true;
        }

        private bool InsertAttributeToDb(List<ExcelDataModelAttribute> attributeList)
        {
            var dbAttributeList = attributeList.Select(x => new Attribute(x.Id, x.Name, x.Code, x.IsActive, x.SeoName)).ToList();
            var result = _attributeRepository.BulkMerge(dbAttributeList);
            return result;
        }

        private bool InsertAttributeValueToDb(List<ExcelDataModelAttributeValue> attributeValueList)
        {
            var dbAttributeValueList = attributeValueList.Select(x => new AttributeValue(x.Id, x.Value, x.Code, x.IsActive, x.SeoName)).ToList();
            var result = _attributeValueRepository.BulkMerge(dbAttributeValueList);
            return result;
        }

        private bool InsertAttributeMapToDb(List<ExcelDataModelAttributeMap> attributeMapList)
        {
            var dbAttributeMapList = attributeMapList.Select(x => new AttributeMap(x.Id, x.AttributeId, x.AttributeValueId, x.IsActive)).ToList();
            var result = _attributeMapRepository.BulkMerge(dbAttributeMapList);
            return result;

        }

        public async Task<bool> UpdateAttributeValueOrder()
        {
            var attributeValueList = await _attributeValueRepository.FilterByAsync(u => u.Code != null);
            var list = _attributeValueRepository.GetAttributeValueOrder(attributeValueList);
            var result = _attributeValueRepository.BulkMerge(list);
            return result;
        }
    }
}
