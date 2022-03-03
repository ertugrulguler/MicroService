using System;

namespace Catalog.ApplicationService.Communicator.Parameter.Model
{
    public class IconResponse
    {
        public Guid Id { get; set; }
        public int BatchCode { get; set; }
        public string ImageCdnUrl { get; set; }
    }
}
