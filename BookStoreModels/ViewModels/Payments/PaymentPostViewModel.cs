using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Payments.Dictionaries;

namespace BookStoreViewModels.ViewModels.Payments
{
    public class PaymentPostViewModel : BaseViewModel
    {
        public DateTime? PaymentDate { get; set; }
        public PaymentMethodViewModel? PaymentMethod { get; set; }
        public TransactionStatusViewModel? TransactionStatus { get; set; }
    }
}
