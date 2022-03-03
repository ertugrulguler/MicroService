using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.BannerCommands
{
    public class DeleteBannerCommand : IRequest<ResponseBase<object>>
    {
        public Guid Id { get; set; }
    }
}