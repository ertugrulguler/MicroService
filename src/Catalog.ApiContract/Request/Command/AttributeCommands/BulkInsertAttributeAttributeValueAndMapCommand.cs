﻿using Framework.Core.Model;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Catalog.ApiContract.Request.Command.AttributeCommands
{
    public class BulkInsertAttributeAttributeValueAndMapCommand : IRequest<ResponseBase<object>>
    {
        public IFormFile File { get; set; }
    }
}
