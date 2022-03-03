using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Contract.Model
{
    public class TemplatePreviewRequest
    {
        public string TemplateCode { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
