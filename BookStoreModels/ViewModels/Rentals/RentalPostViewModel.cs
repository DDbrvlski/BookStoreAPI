using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Rentals
{
    public class RentalPostViewModel : BaseViewModel
    {
        public DateTime StartDate { get; set; }
        public int? BookItemID { get; set; }
        public int? PaymentMethodID { get; set; }
        public int? RentalTypeID { get; set; }
    }
}
