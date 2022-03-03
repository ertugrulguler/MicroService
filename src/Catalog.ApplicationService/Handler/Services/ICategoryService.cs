using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.CategoryAggregate.ServiceModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategetoryByName(string name, string code);
        ICollection<Category> CategoryWithParents(Guid id, List<Category> categoryList);
        bool ReadFromExcelWithCategoryAllRelation(IFormFile fi);
        Task<Guid?> GetCategetoryByCode(string code);
        Task<List<CategoryIdAndNameforProducts>> GetCagetoriesIdAndNameProducts(List<Guid> productList);
        Task<Dictionary<Guid, string>> CategoryWithNameParents(List<Category> categoryList);
    }
}
