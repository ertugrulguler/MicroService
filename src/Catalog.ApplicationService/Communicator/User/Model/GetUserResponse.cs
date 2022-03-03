using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.User.Model
{
    public class GetUserResponse
    {
        public string FullName { get; set; }
        public List<string> UserTags { get; set; }
        public Guid Id { get; set; }
    }
}