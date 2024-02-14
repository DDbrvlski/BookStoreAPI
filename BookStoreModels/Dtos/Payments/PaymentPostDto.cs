using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Payments.Dictionaries;

namespace BookStoreDto.Dtos.Payments
{
    public class PaymentPostDto : BaseDto
    {
        public DateTime? PaymentDate { get; set; }
        public PaymentMethodDto? PaymentMethod { get; set; }
        public TransactionStatusDto? TransactionStatus { get; set; }
    }
}
