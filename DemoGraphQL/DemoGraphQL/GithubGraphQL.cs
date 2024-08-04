using Newtonsoft.Json;
using NLog;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DemoGraphQL
{
    internal static class GithubGraphQL
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        internal static void GithubGraphQlDemo()
        {
            logger.Info("GithubGraphQlDemo");
            var GithubAccount = ConfigurationManager.AppSettings["GitHub_Account"]; ;
            dynamic responseObj = null;
            try
            {
                var PersonalToken = ConfigurationManager.AppSettings["GitHub_Personal_Token"];

                var _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://api.github.com/graphql")
                };

                string basicValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{GithubAccount}:{PersonalToken}"));


                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicValue);
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyConsoleApp");

                var queryObject = new
                {
                    query = @"query { 
                     viewer { 
                     login
                     name

                     }
                 }",
                    variables = new { }
                };

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonConvert.SerializeObject(queryObject), Encoding.UTF8, "application/json")
                };


                using (var response = _httpClient.SendAsync(request))
                {
                    response?.Result.EnsureSuccessStatusCode();

                    var responseString = response?.Result.Content?.ReadAsStringAsync().Result;
                    responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);
                }

                logger.Info($"Response for github graphQL query: {responseObj.data.viewer}");
                Console.WriteLine(responseObj.data.viewer.login);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                msg = $"{msg}, Error from GraphQLAPI:{responseObj?.error}";
                logger.Error(msg);
            }

        }
    }
}
