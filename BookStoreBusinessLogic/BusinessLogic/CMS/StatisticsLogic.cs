namespace BookStoreBusinessLogic.BusinessLogic.CMS
{
    public interface IStatisticsLogic
    {
        float PercentNumberOfAppearances(int appearances, int totalAppearances);
        float PercentNumberOfPrices(decimal soldPrice, decimal totalSoldPrice);
    }

    public class StatisticsLogic : IStatisticsLogic
    {
        public float PercentNumberOfAppearances(int appearances, int totalAppearances)
        {
            if (appearances > totalAppearances)
            {
                return 0;
            }
            float tempAppearances = (float)appearances / (float)totalAppearances;
            return tempAppearances * 100;
        }
        public float PercentNumberOfPrices(decimal soldPrice, decimal totalSoldPrice)
        {
            if (soldPrice > totalSoldPrice)
            {
                return 0;
            }
            return (float)((soldPrice / totalSoldPrice) * 100);
        }
    }
}
