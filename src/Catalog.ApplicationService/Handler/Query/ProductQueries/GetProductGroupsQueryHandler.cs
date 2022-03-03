using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductGroupsQueryHandler : IRequestHandler<GetProductGroupsQuery, ResponseBase<List<string>>>
    {
        private readonly IProductGroupRepository _productGroupRepository;

        public GetProductGroupsQueryHandler(IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        public async Task<ResponseBase<List<string>>> Handle(GetProductGroupsQuery request, CancellationToken cancellationToken)
        {
            var groupList = await _productGroupRepository.AllAsync(); //TODO: Mecburum :)
            var returnList = groupList.GroupBy(x => x.GroupCode).Select(g => g.First()).Select(p => p.GroupCode).ToList();

            return new ResponseBase<List<string>> { Data = returnList, Success = true };
        }
    }
}
