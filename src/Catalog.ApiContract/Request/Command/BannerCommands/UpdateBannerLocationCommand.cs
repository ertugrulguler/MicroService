using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.BannerCommands
{
    public class UpdateBannerLocationCommand : IRequest<ResponseBase<object>>
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}