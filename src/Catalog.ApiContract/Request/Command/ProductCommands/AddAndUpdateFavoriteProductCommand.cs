using Catalog.ApiContract.Contract;
using Framework.Core.Model;

using MediatR;

using System;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class AddAndUpdateFavoriteProductCommand : IRequest<ResponseBase<FavoriteProductDto>>
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public bool IsActive { get; set; }

    }
}
