using System;

namespace Catalog.ApplicationService.Communicator.BackOffice.Model
{
    public class CategoryCompanyInstallmentResponse
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Guid? CategoryId { get; set; }
        public int MaxInstallmentCount { get; set; }
        public bool UseOverdraftInstallment { get; set; }
        public int? OverdraftInstallmentCount { get; set; }
        public bool UseCommercialCardInstallment { get; set; }
        public int? CommercialCardInstallmentCount { get; set; }
    }
}
