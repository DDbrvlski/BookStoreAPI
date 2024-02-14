using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Payments.Dictionaries;

namespace BookStoreDto.Dtos.Payments
{
    public class PaymentDetailsDto : BaseDto
    {
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentMethodDto? PaymentMethod { get; set; }
        public TransactionStatusDto? TransactionStatus { get; set; }
    }
}
