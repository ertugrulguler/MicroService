using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.BrandCommands
{
    public class DeleteBrandCommand : IRequest<ResponseBase<BrandDto>>
    {
        public Guid Id { get; set; }

    }
}
