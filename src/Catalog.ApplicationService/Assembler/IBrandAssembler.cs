using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.BrandCommands;
using Catalog.Domain.BrandAggregate;
using Framework.Core.Model;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Assembler
{
    public interface IBrandAssembler
    {
        ResponseBase<CreateBrand> MapToCreateBrandCommandResult(Brand Brand);
        ResponseBase<List<BrandDto>> MapToGetBrandQueryResultForBO(List<Catalog.Domain.ValueObject.BrandDto> brandsList);
        ResponseBase<BrandDto> MapToUpdateBrandCommandResult(Brand Brand);
        ResponseBase<List<BrandDto>> MapToGetBrandQueryResult(List<Brand> Brand);
        ResponseBase<BrandDto> MapToDeleteBrandCommandResult(Brand Brand);
        ResponseBase<List<BrandDto>> MapToGetBrandsIdAndNameQueryResult(List<Brand> Brand);

    }
}



