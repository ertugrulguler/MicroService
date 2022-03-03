using Catalog.ApiContract.Response.Command.BrandCommands;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Command.BrandCommands
{
    public class CreateBrandCommand : IRequest<ResponseBase<CreateBrand>>
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }

    }
}
