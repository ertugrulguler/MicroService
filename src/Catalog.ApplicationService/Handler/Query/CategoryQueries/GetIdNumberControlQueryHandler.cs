using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetIdNumberControlQueryHandler : IRequestHandler<GetIdNumberControlQuery, ResponseBase<GetIdNumberControlQueryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetIdNumberControlQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }
        public async Task<ResponseBase<GetIdNumberControlQueryResult>> Handle(GetIdNumberControlQuery request, CancellationToken cancellationToken)
        {
            var IsRequiredIdNumber = false;

            var list = request.GetIdNumberControlInfos.GroupBy(x => x.SellerId, (k, g) => new
            {
                Key = k,
                Value = g.Sum(y => y.TotalPrice.Value)
            }).ToList();

            foreach (var keyValue in list)
            {
                if (keyValue.Value > 5000)
                {
                    IsRequiredIdNumber = true;
                    return new ResponseBase<GetIdNumberControlQueryResult> { Data = new GetIdNumberControlQueryResult { IsRequiredIdNumber = IsRequiredIdNumber }, Success = true };
                }
            }



            foreach (var item in request.GetIdNumberControlInfos)
            {

                var category = await _categoryRepository.FindByAsync(x => x.Id == item.CategoryId);

                if (category.IsRequiredIdNumber)
                {
                    IsRequiredIdNumber = true;
                    continue;
                }
            }

            return new ResponseBase<GetIdNumberControlQueryResult> { Data = new GetIdNumberControlQueryResult { IsRequiredIdNumber = IsRequiredIdNumber }, Success = true };
        }
    }
}
