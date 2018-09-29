using System;
using System.Net.Http;
using Newtonsoft.Json;
using TrainCheck.Config;

namespace TrainCheck.TransportApi
{
    public interface ITransportApi
    {
        TrainResponse Get(string from, string destination);
    }

    public class TransportApi : ITransportApi
    {
        private readonly TransportApiSettings _transportApiSettings;
        private readonly TrainStationSettings _stationSettings;

        public TransportApi(
            TransportApiSettings transportApiSettings, 
            TrainStationSettings stationSettings)
        {
            _transportApiSettings = transportApiSettings;
            _stationSettings = stationSettings;
        }

        public TrainResponse Get(string from, string destination)
        {
            var homeCode = _stationSettings.GetCode(from);
            var destinationCode = _stationSettings.GetCode(destination);

            Logger.Log($"Fetching services from {from}({homeCode}) to {destination}({destinationCode})");

            var uri = GetUrl(homeCode, destinationCode);

            return GetResponse(uri);
        }

        private Uri GetUrl(string homeStation, string destination)
        {
            var baseUrl = _transportApiSettings.BaseUrl;
            var appId = _transportApiSettings.AppId;
            var appKey = _transportApiSettings.AppKey;
            var walkingTime = _stationSettings.WalkingTimeInMinutes.ToString().PadLeft(2, '0');

            var endpoint = $"{baseUrl}/train/station/{homeStation}/live.json?" +
                           $"app_id={appId}&app_key={appKey}&darwin=false" +
                           $"&calling_at={destination}&from_offset=PT00:{walkingTime}:00&train_status=passenger";

            return new Uri(endpoint);
        }

        private static TrainResponse GetResponse(Uri uri)
        {
            Xray.Begin("TransportApi");

            Logger.Log($"Sending transportApi request: {uri}");

            var response = new HttpClient().GetAsync(uri).Result;

            Xray.End();

            Logger.Log($"Received http response. Response status code: {response.StatusCode}");

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TrainResponse>(responseContent);
        }
    }
}