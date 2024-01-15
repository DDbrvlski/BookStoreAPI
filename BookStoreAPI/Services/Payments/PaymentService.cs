using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.Transactions;

namespace BookStoreAPI.Services.Payments
{
    public interface IPaymentService
    {
        Task<Payment> CreateNewPayment(int paymentMethodId, decimal amount, DateTime paymentDate);
    }

    public class PaymentService(BookStoreContext context) : IPaymentService
    {
        public async Task<Payment> CreateNewPayment(int paymentMethodId, decimal amount, DateTime paymentDate)
        {
            //Ustawienie statusu transakcji na sztywno ze względu na płatność
            //Zakończone dla płatności online/kartą
            //Niezakończone dla płatności gotówką
            int transactionStatusId;

            if (paymentMethodId == 4)
            {
                transactionStatusId = 1;
            }
            else
            {
                transactionStatusId = 2;
            }
            //Do zmiany po dodaniu prawdziwej płatności
            //-----------------------------------------------

            Payment payment = new Payment()
            {
                Amount = amount,
                Date = paymentDate,
                PaymentMethodID = paymentMethodId,
                TransactionStatusID = transactionStatusId,
            };

            await context.Payment.AddAsync(payment);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return payment;
        }
    }
}
