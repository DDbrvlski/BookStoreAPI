namespace BookStoreBusinessLogic.BusinessLogic.Discounts
{
    public interface IDiscountLogic
    {
        decimal CalculateItemPriceWithDiscountCode(decimal itemPrice, decimal percentOfDiscount);
    }

    public class DiscountLogic : IDiscountLogic
    {
        public decimal CalculateItemPriceWithDiscountCode(decimal itemPrice, decimal percentOfDiscount)
        {
            return itemPrice * (1 - (percentOfDiscount / 100));
        }
    }
}
