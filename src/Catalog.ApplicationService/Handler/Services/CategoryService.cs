using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.CategoryAggregate.ServiceModel;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly ICategoryAttributeValueMapRepository _categoryAttributeValueMapRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IGeneralAssembler _genaralAssembler;
        public CategoryService(ICategoryRepository categoryRepository, IGeneralAssembler genaralAssembler,
            ICategoryAttributeRepository categoryAttributeRepository, ICategoryAttributeValueMapRepository categoryAttributeValueMapRepository, IProductCategoryRepository productCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryAttributeValueMapRepository = categoryAttributeValueMapRepository;
            _productCategoryRepository = productCategoryRepository;
            _genaralAssembler = genaralAssembler;
        }
        public ICollection<Category> CategoryWithParents(Guid id, List<Category> categoryList)
        {
            var categoryWithParents = new List<Category>();

            var category = categoryList.FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                while (category != null && category.ParentId.HasValue)
                {
                    categoryWithParents.Add(category);
                    category = categoryList.FirstOrDefault(x => x.Id == category.ParentId.Value);
                }

            }
            if (category != null)
                categoryWithParents.Add(category); //Ana kategorisi null olanı ekler.
            return categoryWithParents;
        }
        public bool ReadFromExcelWithCategoryAllRelation(IFormFile fi)
        {
            var extension = Path.GetExtension(fi?.FileName);
            if (fi != null && extension.ToLower() == ".xlsx")
            {
                var stream = fi.OpenReadStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workbook = package.Workbook;

                    #region Category

                    var worksheetCategory = workbook.Worksheets[0];
                    int rowCountCategory = worksheetCategory.Dimension.End.Row;
                    var listCategory = new List<ExcelDataModelCategory>();

                    for (int row = 2; row <= rowCountCategory; row++)
                    {
                        var seoName = _genaralAssembler.GetSeoName(worksheetCategory.Cells[row, 2].Value?.ToString(), Domain.Enums.SeoNameType.Category);
                        var rowExcel = new ExcelDataModelCategory
                        {
                            CategoryCode = worksheetCategory.Cells[row, 1].Value?.ToString(),
                            CategoryName = worksheetCategory.Cells[row, 2].Value?.ToString(),
                            IsLeaf = CheckStringToBool(worksheetCategory.Cells[row, 3].Value?.ToString()),
                            CategoryId = new Guid(worksheetCategory.Cells[row, 4].Value?.ToString()),
                            CategoryParentId = new Guid(worksheetCategory.Cells[row, 5].Value?.ToString()),
                            IsActive = CheckStringToBool(worksheetCategory.Cells[row, 6].Value?.ToString()),
                            SeoName = seoName,
                            IsRequiredIdNumber = CheckStringToBool(worksheetCategory.Cells[row, 7].Value?.ToString()),
                            IsReturnable = CheckStringToBool(worksheetCategory.Cells[row, 8].Value?.ToString())
                        };
                        listCategory.Add(rowExcel);
                    }

                    var cat = InsertCategoryToDb(listCategory);
                    if (!cat)
                        throw new BusinessRuleException(ApplicationMessage.CategoryNotCorrectFormat,
                                                        ApplicationMessage.CategoryNotCorrectFormat.Message(),
                                                        ApplicationMessage.CategoryNotCorrectFormat.UserMessage());


                    #endregion

                    #region CategoryAttribute

                    var worksheetCategoryAttribute = workbook.Worksheets[1];
                    int rowCountCategoryAttribute = worksheetCategoryAttribute.Dimension.End.Row;
                    var listCategoryAttribute = new List<ExcelDataModelCategoryAttribute>();

                    for (int row = 2; row <= rowCountCategoryAttribute; row++)
                    {
                        var rowExcel = new ExcelDataModelCategoryAttribute
                        {
                            CategoryId = new Guid(worksheetCategoryAttribute.Cells[row, 1].Value?.ToString()),
                            AttributeId = new Guid(worksheetCategoryAttribute.Cells[row, 2].Value?.ToString()),
                            IsFilter = CheckStringToBool(worksheetCategoryAttribute.Cells[row, 3].Value?.ToString()),
                            FilterOrder = CheckStringToInt(worksheetCategoryAttribute.Cells[row, 4].Value?.ToString()),
                            IsVariantable = CheckStringToBool(worksheetCategoryAttribute.Cells[row, 5].Value?.ToString()),
                            IsRequired = CheckStringToBool(worksheetCategoryAttribute.Cells[row, 6].Value?.ToString()),
                            IsListed = CheckStringToBool(worksheetCategoryAttribute.Cells[row, 7].Value?.ToString()),
                            Id = new Guid(worksheetCategoryAttribute.Cells[row, 8].Value?.ToString()),
                            IsActive = CheckStringToBool(worksheetCategoryAttribute.Cells[row, 9].Value?.ToString())


                        };
                        listCategoryAttribute.Add(rowExcel);
                    }

                    var catAtt = InsertCategoryAttributeToDb(listCategoryAttribute);
                    if (!catAtt)
                        throw new BusinessRuleException(ApplicationMessage.CategoryAttributeNotCorrectFormat,
                                                        ApplicationMessage.CategoryAttributeNotCorrectFormat.Message(),
                                                        ApplicationMessage.CategoryAttributeNotCorrectFormat.UserMessage());

                    #endregion

                    #region CategoryAttributeMap

                    var worksheetCategoryAttributeMap = workbook.Worksheets[2];
                    int rowCountCategoryAttributeMap = worksheetCategoryAttributeMap.Dimension.End.Row;
                    var listCategoryAttributeMap = new List<ExcelDataModelCategoryAttributeMap>();

                    for (int row = 2; row <= rowCountCategoryAttributeMap; row++)
                    {
                        var rowExcel = new ExcelDataModelCategoryAttributeMap
                        {
                            Id = new Guid(worksheetCategoryAttributeMap.Cells[row, 1].Value?.ToString()),
                            AttributeValueId = new Guid(worksheetCategoryAttributeMap.Cells[row, 2].Value?.ToString()),
                            CategoryAttributeId = new Guid(worksheetCategoryAttributeMap.Cells[row, 3].Value?.ToString()),
                            IsActive = CheckStringToBool(worksheetCategoryAttributeMap.Cells[row, 4].Value?.ToString())

                        };
                        listCategoryAttributeMap.Add(rowExcel);
                    }

                    var catAttMap = InsertCategoryAttributeMapToDb(listCategoryAttributeMap);
                    if (!catAttMap)
                        throw new BusinessRuleException(ApplicationMessage.CategoryAttributeMapNotCorrectFormat,
                                                        ApplicationMessage.CategoryAttributeMapNotCorrectFormat.Message(),
                                                        ApplicationMessage.CategoryAttributeMapNotCorrectFormat.UserMessage());


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
        private static int CheckStringToInt(string str)
        {
            if (str == "0" || string.IsNullOrWhiteSpace(str))
                return 0;

            return int.Parse(str);
        }

        private bool InsertCategoryToDb(List<ExcelDataModelCategory> categoryList)
        {
            var dbCategoryList = categoryList.Select(x => new Category(x.CategoryId, x.CategoryCode, x.SeoName, x.CategoryName,
                x.CategoryName, x.CategoryParentId, x.IsActive, isRequiredIdNumber: x.IsRequiredIdNumber, isReturnable: x.IsReturnable)).ToList();
            return _categoryRepository.BulkMerge(dbCategoryList);
        }
        private bool InsertCategoryAttributeToDb(List<ExcelDataModelCategoryAttribute> categoryAttributeList)
        {
            var dbCategoryAttributeList = categoryAttributeList.Select(x => new CategoryAttribute(x.Id, x.AttributeId, x.CategoryId, x.IsVariantable, x.IsRequired, x.IsActive, x.IsFilter, x.FilterOrder, x.IsListed)).ToList();
            var result = _categoryAttributeRepository.BulkMerge(dbCategoryAttributeList);
            return result;

        }
        private bool InsertCategoryAttributeMapToDb(List<ExcelDataModelCategoryAttributeMap> categoryAttributeMapList)
        {
            var dbCategoryAttributeMapList = categoryAttributeMapList.Select(x => new CategoryAttributeValueMap(x.Id, x.CategoryAttributeId, x.AttributeValueId, x.IsActive)).ToList();
            var result = _categoryAttributeValueMapRepository.BulkMerge(dbCategoryAttributeMapList);
            return result;

        }
        public async Task<List<CategoryIdAndNameforProducts>> GetCagetoriesIdAndNameProducts(List<Guid> productList)
        {
            var list = new List<CategoryIdAndNameforProducts>();

            var pcList = await _productCategoryRepository.FilterByAsync(o => productList.Contains(o.ProductId));
            var categoryList = await _categoryRepository.FilterByAsync(p => pcList.Select(u => u.CategoryId).Contains(p.Id) && p.Type == CategoryTypeEnum.MainCategory);
            categoryList.Distinct().ToList().ForEach(p => list.Add(new CategoryIdAndNameforProducts { Id = p.Id, Name = p.Name }));

            return list;
        }

        public async Task<Dictionary<Guid, string>> CategoryWithNameParents(List<Category> categoryList)
        {
            // cat listesini bir anda cek bulup onun leafleri çek
            //leaf listesi geldi
            var list = _categoryRepository.FilterByAsync(h => h.IsActive).Result;
            var categoryNameDic = new Dictionary<Guid, string>();
            List<string> names = new List<string>();
            var categoryWithParents = new List<Dictionary<Guid, string>>();
            foreach (var item in categoryList)
            {
                var categoryName = "->" + item.Name;
                var categoryId = new Guid();
                var splitCategory = item.LeafPath.Split(',');
                foreach (var leafItem in splitCategory)
                {
                    categoryId = new Guid(leafItem);
                    var categoryItem = list.Where(g => g.Id == categoryId)?.FirstOrDefault();
                    categoryName = "->" + categoryItem?.Name + categoryName;
                }
                categoryNameDic.Add(item.Id, categoryName);
            }
            return categoryNameDic;
        }

        public async Task<Category> GetCategetoryByName(string name, string code)
        {
            var category = await _categoryRepository.FindByAsync(p => p.SeoName == name && p.IsActive && p.Code == code);
            return category;
        }
        public async Task<Guid?> GetCategetoryByCode(string code)
        {
            var category = await _categoryRepository.FindByAsync(p => p.Code == code && p.IsActive);
            return category?.Id;
        }
    }
}
