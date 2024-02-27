using BookStoreAPI.Enums;
using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Payments
{
    public interface IPaymentService
    {
        Task<Payment> CreateNewPayment(int paymentMethodId, decimal amount);
        Task MakePaymentAsync(int paymentId);
    }

    public class PaymentService(BookStoreContext context) : IPaymentService
    {
        public async Task<Payment> CreateNewPayment(int paymentMethodId, decimal amount)
        {
            //Ustawienie statusu transakcji ze względu na płatność
            //Zakończone dla płatności online/kartą
            //Niezakończone dla płatności gotówką
            int transactionStatusId;
            DateTime? paymentDate = null;

            if (paymentMethodId == (int)PaymentMethodEnum.PlatnoscPrzyOdbiorze)
            {
                transactionStatusId = (int)TransactionStatusEnum.WTrakcie;
            }
            else
            {
                transactionStatusId = (int)TransactionStatusEnum.Zakonczona;
                paymentDate = DateTime.UtcNow;
            }
            //Do zmiany po dodaniu prawdziwej płatności
            //-----------------------------------------------

            Payment payment = new Payment()
            {
                Amount = amount,
                PaymentDate = paymentDate,
                PaymentMethodID = paymentMethodId,
                TransactionStatusID = transactionStatusId,
            };

            await context.Payment.AddAsync(payment);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return payment;
        }

        //Symulacja płatności
        public async Task MakePaymentAsync(int paymentId)
        {
            var payment = await context.Payment.Where(x => x.IsActive && x.Id == paymentId).FirstOrDefaultAsync();

            if (payment == null)
            {
                throw new PaymentException("Wystąpił błąd z przetwarzaniem płatności.");
            }

            payment.TransactionStatusID = (int)TransactionStatusEnum.Zakonczona;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
