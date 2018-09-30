using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace TrainCheck.Infrastructure
{
    public class StandardHttpClient
    {
        private readonly HttpClient _httpClient;

        public StandardHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public T GetAsync<T>(Uri uri)
        {
            Xray.Begin(typeof(T).Name);

            Logger.Log($"Sending http request: {uri}");

            var response = _httpClient.GetAsync(uri).Result;

            Xray.End();

            Logger.Log($"Received http response. Response status code: {response.StatusCode}");

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}