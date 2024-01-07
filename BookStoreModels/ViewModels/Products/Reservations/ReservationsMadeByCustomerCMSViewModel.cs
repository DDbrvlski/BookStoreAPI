namespace BookStoreViewModels.ViewModels.Products.Reservations
{
    public class ReservationsMadeByCustomerCMSViewModel
    {
        public int CustomerId { get; set; }
        public List<ReservationViewModel> ReservationList { get; set; }
    }
}
