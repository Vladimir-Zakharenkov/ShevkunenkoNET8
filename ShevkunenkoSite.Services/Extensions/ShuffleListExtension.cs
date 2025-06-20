namespace ShevkunenkoSite.Services.Extensions
{
    public static class ShuffleListExtension
    {
        private static readonly Random rng = new();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            Random random = new();

            var originalList = list.ToList();

            List<T> shuffleList = [];

            List<T> shuffleListDistinct = [];

            while (true)
            {
                if (shuffleListDistinct.Count == 18 || shuffleListDistinct.Count == originalList.Count)
                {
                    break;
                }

                shuffleList.Add(originalList[random.Next(originalList.Count)]);

                shuffleListDistinct = [.. shuffleList.Distinct()];
            }

            return shuffleListDistinct;
        }
    }
}
