using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class CalculateLeafPathsCommandHandler : IRequestHandler<CalculateLeafPathsCommand, ResponseBase<object>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work

        public CalculateLeafPathsCommandHandler(ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
        }
        public async Task<ResponseBase<object>> Handle(CalculateLeafPathsCommand request, CancellationToken cancellationToken)
        {
            var categoryList = await _categoryRepository.FilterByAsync(c => c.Type == CategoryTypeEnum.MainCategory);
            foreach (var category in categoryList)
            {
                if (category.ParentId == null)
                {
                    category.LeafPath = null;
                    _categoryRepository.Update(category);
                    continue;
                }
                var subCategories = categoryList.Where(c => c.ParentId == category.Id);
                if (subCategories.Any())
                {
                    category.LeafPath = null;
                    _categoryRepository.Update(category);
                }
                else
                {
                    var parentId = category.ParentId;
                    StringBuilder path = new StringBuilder(category.ParentId.ToString());
                    while (true)
                    {
                        if (parentId == null)
                            break;
                        var parentCategory = categoryList.FirstOrDefault(c => c.Id == parentId);
                        if (parentCategory.ParentId != null)
                        {
                            path.Append(",");
                            path.Append(parentCategory.ParentId);
                        }
                        parentId = parentCategory.ParentId;
                    }
                    category.LeafPath = path.ToString();
                    _categoryRepository.Update(category);

                }
            }
            await _dbContextHandler.SaveChangesAsync();
            return new ResponseBase<object>() { Success = true };
        }
    }
}
