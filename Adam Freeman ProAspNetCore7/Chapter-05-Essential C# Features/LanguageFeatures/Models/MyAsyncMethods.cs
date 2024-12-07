#region Listing 5.53 The contents of the MyAsyncMethods.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class MyAsyncMethods
//    {
//        public static Task<long?> GetPageLength()
//        {
//            HttpClient client = new HttpClient();

//            var httpTask = client.GetAsync("http://manning.com");

//            return httpTask.ContinueWith((Task<HttpResponseMessage> antecedent) =>
//            {
//                return antecedent.Result.Content.Headers.ContentLength;
//            });
//        }
//    }
//}

#endregion

#region Listing 5.54 Using the async and await keywords in the MyAsyncMethods.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class MyAsyncMethods
//    {
//        public async static Task<long?> GetPageLength()
//        {
//            HttpClient client = new HttpClient();

//            var httpMessage = await client.GetAsync("https://shevkunenko.site");

//            return httpMessage.Content.Headers.ContentLength;
//        }
//    }
//}

#endregion

#region Listing 5.56 Adding a method in the MyAsyncMethods.cs file in the Models folder

//namespace LanguageFeatures.Models
//{
//    public class MyAsyncMethods
//    {
//        public async static Task<long?> GetPageLength()
//        {
//            HttpClient client = new HttpClient();

//            var httpMessage = await client.GetAsync("http://manning.com");

//            return httpMessage.Content.Headers.ContentLength;
//        }

//        public static async Task<IEnumerable<long?>> GetPageLengths(List<string> output, params string[] urls)
//        {
//            List<long?> results = new List<long?>();

//            HttpClient client = new HttpClient();

//            foreach (string url in urls)
//            {
//                output.Add($"Started request for {url}");

//                var httpMessage = await client.GetAsync($"https://{url}");

//                results.Add(httpMessage.Content.Headers.ContentLength);

//                output.Add($"Completed request for {url}");
//            }

//            return results;
//        }
//    }
//}

#endregion

#region Listing 5.58 Using an asynchronous enumerable in the MyAsyncMethods.cs file in the Models folder

namespace LanguageFeatures.Models
{
    public class MyAsyncMethods
    {
        public async static Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();

            var httpMessage = await client.GetAsync("http://manning.com");

            return httpMessage.Content.Headers.ContentLength;
        }

        public static async IAsyncEnumerable<long?> GetPageLengths(List<string> output, params string[] urls)
        {
            HttpClient client = new HttpClient();

            foreach (string url in urls)
            {
                output.Add($"Started request for {url}");

                var httpMessage = await client.GetAsync($"https://{url}");

                output.Add($"Completed request for {url}");

                yield return httpMessage.Content.Headers.ContentLength;
            }
        }
    }
}

#endregion