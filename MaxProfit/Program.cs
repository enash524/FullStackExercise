using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MaxProfit.Models;
using Newtonsoft.Json;
using Nito.AsyncEx;

namespace MaxProfit
{
    internal class Program
    {
        #region Private Fields

        private static readonly string baseAccessUrl = "https://id.livevol.com/";
        private static readonly string baseApiUrl = "https://api.livevol.com/";
        private static readonly string clientId = "web_api_demo";
        private static readonly string clientSecret = "KM83bv!kqa";
        private static Token accessToken = null;

        #endregion Private Fields

        #region Private Methods

        private static string BuildQueryString(List<KeyValuePair<string, string>> data)
        {
            List<string> items = new List<string>();

            foreach (KeyValuePair<string, string> kvp in data)
                items.Add($"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}");

            return string.Join("&", items);
        }

        private static decimal GenerateMaxProfit(List<SymbolHistoryItem> symbolHistoryItems)
        {
            decimal profit = 0M;

            foreach (SymbolHistoryItem item in symbolHistoryItems)
            {
                List<SymbolHistoryItem> remaining = symbolHistoryItems.FindAll(i => i.Date > item.Date);

                if (remaining.Count > 0)
                {
                    decimal tempProfit = GenerateMaxProfit(item, remaining);

                    if (tempProfit > profit)
                        profit = tempProfit;
                }
            }

            return profit;
        }

        private static decimal GenerateMaxProfit(SymbolHistoryItem startItem, List<SymbolHistoryItem> symbolHistoryItems)
        {
            decimal profit = 0M;

            foreach (SymbolHistoryItem item in symbolHistoryItems)
            {
                decimal tempProfit = item.Price.Open - startItem.Price.Open;

                if (tempProfit > profit)
                    profit = tempProfit;
            }

            return profit;
        }

        private static async Task<Token> GetAccessToken()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAccessUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);

                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope", "api.market")
                };

                using (FormUrlEncodedContent content = new FormUrlEncodedContent(postData))
                {
                    using (HttpResponseMessage response = await client.PostAsync("connect/token", content))
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        Token responseToken = JsonConvert.DeserializeObject<Token>(jsonString);

                        return responseToken;
                    }
                }
            }
        }

        private static async Task<List<SymbolHistoryItem>> GetSymbolHistoryItems(string symbol, DateTime startDate, DateTime endDate)
        {
            using (HttpClient client = new HttpClient())
            {
                List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("start_date", startDate.ToString("yyyy-MM-dd")),
                    new KeyValuePair<string, string>("end_date", endDate.ToString("yyyy-MM-dd"))
                };

                string qs = BuildQueryString(data);

                client.BaseAddress = new Uri(baseApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                using (HttpResponseMessage response = await client.GetAsync($"v1/delayed/market/symbols/{symbol}/history?{qs}"))
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<SymbolHistoryItem> symbolHistory = JsonConvert.DeserializeObject<List<SymbolHistoryItem>>(jsonString);

                    return symbolHistory;
                }
            }
        }

        private static void Main(string[] args)
        {
            try
            {
                AsyncContext.Run(() => MainAsync(args));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("--- Press Any Key To Continue ---");
                Console.ReadKey(true);
            }
        }

        private static async Task MainAsync(string[] args)
        {
            string symbol = "CBOE";
            DateTime startDate = new DateTime(2018, 5, 5);
            DateTime endDate = new DateTime(2018, 5, 11);

            accessToken = await GetAccessToken();
            List<SymbolHistoryItem> symbolHistory = await GetSymbolHistoryItems(symbol, startDate, endDate);
            decimal profit = GenerateMaxProfit(symbolHistory);

            Console.WriteLine("Symbol: {0}", symbol);
            Console.WriteLine("Start Date: {0}", startDate.ToString("yyyy-MM-dd"));
            Console.WriteLine("End Date: {0}", endDate.ToString("yyyy-MM-dd"));
            Console.WriteLine("Maximum Profit: {0}", profit.ToString("c2"));
        }

        #endregion Private Methods
    }
}