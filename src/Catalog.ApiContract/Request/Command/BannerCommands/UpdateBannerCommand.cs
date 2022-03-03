using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.BannerCommands
{
    public class UpdateBannerCommand : IRequest<ResponseBase<object>>
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string ImageUrl { get; set; }
        public int? MMActionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}