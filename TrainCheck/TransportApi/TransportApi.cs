using System;
using TrainCheck.Config;
using TrainCheck.Infrastructure;

namespace TrainCheck.TransportApi
{
    public interface ITransportApi
    {
        TrainResponse Get(string from, string destination);
    }

    public class TransportApi : ITransportApi
    {
        private readonly IStandardHttpClient _standardHttpClient;
        private readonly TransportApiSettings _transportApiSettings;
        private readonly TrainStationSettings _stationSettings;

        public TransportApi(
            IStandardHttpClient standardHttpClient,
            TransportApiSettings transportApiSettings,
            TrainStationSettings stationSettings)
        {
            _standardHttpClient = standardHttpClient;
            _transportApiSettings = transportApiSettings;
            _stationSettings = stationSettings;
        }

        public TrainResponse Get(string from, string destination)
        {
            var homeCode = _stationSettings.GetCode(from);
            var destinationCode = _stationSettings.GetCode(destination);

            Logger.Log($"Fetching services from {from}({homeCode}) to {destination}({destinationCode})");

            var uri = GetUrl(homeCode, destinationCode);

            return _standardHttpClient.GetAsync<TrainResponse>(uri);
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
    }
}