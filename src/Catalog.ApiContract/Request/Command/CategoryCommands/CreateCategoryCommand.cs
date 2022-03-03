using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class CreateCategoryCommand : IRequest<ResponseBase<CreateCategory>>
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public CategoryTypeEnum Type { get; set; }
        public bool HasAll { get; set; }
        public bool Suggested { get; set; }
        public CategoryImageDto CategoryImage { get; set; }
    }
}
