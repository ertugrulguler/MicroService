using Catalog.ApiContract.Response.Command.BannerCommands;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.BannerCommands
{
    public class CreateBannerLocationCommand : IRequest<ResponseBase<CreateBannerLocationResult>>
    {
        public BannerLocationType BannerLocationType { get; set; }
        public Guid? ActionId { get; set; }
        public BannerType BannerType { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Framework.Core.Model.Enums.ChannelCode ProductChannelCode { get; set; }
    }
}


