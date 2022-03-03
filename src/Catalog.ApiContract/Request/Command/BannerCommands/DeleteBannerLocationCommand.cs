using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Command.BannerCommands
{
    public class DeleteBannerLocationCommand : IRequest<ResponseBase<object>>
    {
        public BannerLocationType BannerLocationType { get; set; }
        public BannerType BannerType { get; set; }
        public int Order { get; set; }
        public ProductChannelCode ProductChannelCode { get; set; }
    }
}