using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoryInstallmentQuery : IRequest<ResponseBase<List<CategoryInstallmentDto>>>
    {
    }
}
