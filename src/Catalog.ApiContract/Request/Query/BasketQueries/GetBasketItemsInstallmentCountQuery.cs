using Catalog.ApiContract.Response.Query.BasketQueries;
using Catalog.Domain.Enums;
using Catalog.Domain.ValueObject;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BasketQueries
{
    public class GetBasketItemsInstallmentCountQuery : IRequest<ResponseBase<List<BasketDetailInstallment>>>
    {
        public List<RequestBasketInfo> BasketProducts { get; set; }
        public PaymentType PaymentType { get; set; }
        public CardUserType CardUserType { get; set; }

    }
}
