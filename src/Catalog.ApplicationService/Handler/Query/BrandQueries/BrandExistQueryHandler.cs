using Catalog.ApiContract.Request.Query.BrandQueries;
using Catalog.ApiContract.Response.Query.BrandQueries;
using Catalog.Domain.BrandAggregate;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BrandQueries
{
    public class BrandExistQueryHandler : IRequestHandler<BrandExistQuery, ResponseBase<BrandExist>>
    {
        private readonly IBrandRepository _brandRepository;

        public BrandExistQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<ResponseBase<BrandExist>> Handle(BrandExistQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.Exist(b => b.Id == request.Id);
            var brandExist = new BrandExist()
            {
                IsExist = brand,
                Id = request.Id
            };
            return new ResponseBase<BrandExist>() { Data = brandExist };
        }
    }
}