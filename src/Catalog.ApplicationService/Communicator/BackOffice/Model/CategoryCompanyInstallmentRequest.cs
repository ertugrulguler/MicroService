using System;

namespace Catalog.ApplicationService.Communicator.BackOffice.Model
{
    public class CategoryCompanyInstallmentRequest
    {
        public Guid SellerId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
