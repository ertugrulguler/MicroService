using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Query.AttributeQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;

using Framework.Core.Model;

using MediatR;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.AttributeQueries
{
    public class GetAttributeValueQueryHandler : IRequestHandler<GetAttributeValueQuery, ResponseBase<List<AttributeValueDto>>>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAttributeAssembler _attributeAssembler;
        public GetAttributeValueQueryHandler(IAttributeValueRepository attributeValueRepository,
                                                  IDbContextHandler dbContextHandler,
                                                  IAttributeAssembler attributeAssembler)
        {
            _attributeValueRepository = attributeValueRepository;
            _dbContextHandler = dbContextHandler;
            _attributeAssembler = attributeAssembler;
        }

        public async Task<ResponseBase<List<AttributeValueDto>>> Handle(GetAttributeValueQuery request, CancellationToken cancellationToken)
        {
            var list = new List<AttributeValue>();
            if (request.AttributeId == null) list = await _attributeValueRepository.AllAsync();
            else list = await _attributeValueRepository.FilterByAsync(y => y.AttributeId == request.AttributeId);

            if (list.Any())
                return _attributeAssembler.MapToGetAttributeValueQueryResult(list);

            throw new BusinessRuleException(ApplicationMessage.EmptyList,
            ApplicationMessage.EmptyList.Message(),
            ApplicationMessage.EmptyList.UserMessage());
        }
    }

}
