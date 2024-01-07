using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.Reservations
{
    public class ReservationViewModel : BaseViewModel
    {
        public int PositionInQueue { get; set; }
        public ReservationBookItemViewModel Item { get; set; }
    }
}
