using Framework.Core.Model.Enums;

using System;

namespace Catalog.ApiContract.Response.Command.BannerCommands
{
    public class CreateBannerLocationResult
    {
        public Guid Id { get; set; }
        public Domain.Enums.BannerType BannerType { get; set; }
        public int Order { get; set; }
        public Domain.Enums.BannerLocationType? Location { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ActionId { get; set; }
        public ChannelCode? ProductChannelCode { get; set; }
    }
}