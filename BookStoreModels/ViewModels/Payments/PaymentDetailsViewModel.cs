using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Payments.Dictionaries;

namespace BookStoreViewModels.ViewModels.Payments
{
    public class PaymentDetailsViewModel : BaseViewModel
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethodViewModel? PaymentMethod { get; set; }
        public TransactionStatusViewModel? TransactionStatus { get; set; }
    }
}
