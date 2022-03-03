using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.BrandCommands;
using Catalog.Domain.BrandAggregate;
using Framework.Core.Model;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Assembler
{
    public class BrandAssembler : IBrandAssembler
    {

        public ResponseBase<CreateBrand> MapToCreateBrandCommandResult(Brand Brand)
        {
            var createBrand = new CreateBrand()
            {
                Name = Brand.Name,
                LogoUrl = Brand.LogoUrl,
                Website = Brand.WebSite,
            };
            return new ResponseBase<CreateBrand>()
            {
                Data = createBrand,
                Success = true
            };
        }

        public ResponseBase<BrandDto> MapToUpdateBrandCommandResult(Brand Brand)
        {
            return new()
            {
                Data = new BrandDto
                {
                    Name = Brand.Name,
                    LogoUrl = Brand.LogoUrl,
                    Website = Brand.WebSite,
                },
                Success = true
            };
        }

        public ResponseBase<List<BrandDto>> MapToGetBrandQueryResult(List<Brand> brandsList)
        {
            var brandDtoList = new List<BrandDto>();
            foreach (var brand in brandsList)
            {
                brandDtoList.Add(new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Website = brand.WebSite,
                    LogoUrl = brand.LogoUrl
                });
            }

            return new()
            {
                Data = brandDtoList,
                Success = true
            };
        }

        public ResponseBase<BrandDto> MapToDeleteBrandCommandResult(Brand Brand)

        {
            return new()
            {
                Data = new BrandDto
                {
                    Id = Brand.Id,
                },
                Success = true
            };
        }


        public ResponseBase<List<BrandDto>> MapToGetBrandsIdAndNameQueryResult(List<Brand> brandsList)
        {
            var brandDtoList = new List<BrandDto>();
            foreach (var brand in brandsList)
            {
                brandDtoList.Add(new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name,
                });
            }

            return new()
            {
                Data = brandDtoList,
                Success = true
            };
        }
        public ResponseBase<List<BrandDto>> MapToGetBrandQueryResultForBO(List<Catalog.Domain.ValueObject.BrandDto> brandsList)
        {
            var brandDtoList = new List<BrandDto>();
            foreach (var brand in brandsList)
            {
                brandDtoList.Add(new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Website = brand.Website,
                    LogoUrl = brand.LogoUrl,
                    Status = brand.Status
                });
            }

            return new()
            {
                Data = brandDtoList,
                Success = true
            };
        }


    }


}



