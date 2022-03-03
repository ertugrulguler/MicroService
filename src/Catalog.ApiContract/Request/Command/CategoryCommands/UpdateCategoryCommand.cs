using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class UpdateCategoryCommand : IRequest<ResponseBase<CategoryDto>>
    {
        public Guid Id { get; set; }
        //public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool HasProduct { get; set; }
        public List<Guid> CategoryAttributeIds { get; set; }
    }
}
