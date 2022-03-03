using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.BrandCommands
{
    public class UpdateBrandCommand : IRequest<ResponseBase<BrandDto>>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }
        public bool Status { get; set; }

    }
}