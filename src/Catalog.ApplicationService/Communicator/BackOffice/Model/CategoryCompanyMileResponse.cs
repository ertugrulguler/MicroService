using System;

namespace Catalog.ApplicationService.Communicator.BackOffice.Model
{
    public class CategoryCompanyMileResponse
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal MinMileAmount { get; set; }
    }
}
