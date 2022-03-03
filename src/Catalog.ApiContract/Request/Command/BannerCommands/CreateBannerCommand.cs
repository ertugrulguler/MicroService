using Catalog.ApiContract.Contract;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.BannerCommands
{
    public class CreateBannerCommand : IRequest<ResponseBase<List<string>>>
    {
        public BannerActionType ActionType { get; set; }
        public BannerType BannerType { get; set; }
        public BannerLocationType Location { get; set; }
        public Guid BannerLocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ActionId { get; set; } //catId,ProdId,pSellerId
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BannerFiltersDto> BannerFilters { get; set; }
        public int? MMActionId { get; set; }
        public string MinAndroidVersion { get; set; }
        public string MinIosVersion { get; set; }

    }

}


