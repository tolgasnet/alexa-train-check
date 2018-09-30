using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace TrainCheck.Infrastructure
{
    public interface IStandardHttpClient
    {
        T GetAsync<T>(Uri uri) where T: class;
    }

    public class StandardHttpClient : IStandardHttpClient
    {
        private readonly HttpClient _httpClient;

        public StandardHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public T GetAsync<T>(Uri uri) where T : class
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