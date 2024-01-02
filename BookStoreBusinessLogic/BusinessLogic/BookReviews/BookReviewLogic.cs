namespace BookStoreBusinessLogic.BusinessLogic.BookReviews
{
    public interface IBookReviewLogic
    {
        Dictionary<int, int> CalculateAllScoreOccurrences(Dictionary<int, int> scoreOccurrences);
    }

    public class BookReviewLogic : IBookReviewLogic
    {
        public Dictionary<int, int> CalculateAllScoreOccurrences(Dictionary<int, int> scoreOccurrences)
        {
            var allScores = Enumerable.Range(1, 5).ToDictionary(score => score, score => 0);

            return allScores
                .Concat(scoreOccurrences)
                .GroupBy(x => x.Key)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(pair => pair.Value)
                );
        }
    }
}
