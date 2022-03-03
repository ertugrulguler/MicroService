using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Enums
{
    public enum PaymentType
    {
        None = 0,
        [Display(Name = "Banka/Kredi Kartı")]
        DebitCreditCard = 1,
        [Display(Name = "İstanbul Kart")]
        IstanbulCard = 2,
        [Display(Name = "Taksitli Ek Hesap")]
        OverdraftByInstallment = 3
    }

}
