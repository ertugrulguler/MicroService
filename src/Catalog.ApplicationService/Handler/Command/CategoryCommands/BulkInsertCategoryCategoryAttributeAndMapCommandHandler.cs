using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApplicationService.Handler.Services;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class BulkInsertCategoryCategoryAttributeAndMapCommandHandler : IRequestHandler<BulkInsertCategoryCategoryAttributeAndMapCommand, ResponseBase<object>>
    {
        private readonly ICategoryService _categoryService;

        public BulkInsertCategoryCategoryAttributeAndMapCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<ResponseBase<object>> Handle(BulkInsertCategoryCategoryAttributeAndMapCommand request, CancellationToken cancellationToken)
        {
            var result = _categoryService.ReadFromExcelWithCategoryAllRelation(request.File);
            return new ResponseBase<object> { Success = result };
        }
    }
}
