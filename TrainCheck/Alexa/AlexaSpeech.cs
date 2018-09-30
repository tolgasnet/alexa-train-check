using System.Collections.Generic;
using System.Linq;
using Alexa.NET;
using Alexa.NET.Response;
using TrainCheck.Config;
using TrainCheck.Infrastructure;
using TrainCheck.TransportApi;

namespace TrainCheck.Alexa
{
    public interface IAlexaSpeech
    {
        SkillResponse TellTimeTable(string destinationName = null);
    }

    public class AlexaSpeech : IAlexaSpeech
    {
        private readonly ITransportApi _transportApi;
        private readonly TrainStationSettings _stationSettings;

        public AlexaSpeech(ITransportApi transportApi, TrainStationSettings stationSettings)
        {
            _transportApi = transportApi;
            _stationSettings = stationSettings;
        }

        public SkillResponse TellTimeTable(string destinationName = null)
        {
            if (string.IsNullOrEmpty(destinationName))
            {
                destinationName = _stationSettings.DefaultDestination;
            }

            var liveTrainResponse = _transportApi.Get(_stationSettings.DefaultHome, destinationName);

            var liveDepartures = liveTrainResponse.GetLiveDepartures(_stationSettings.NumberOfServices);

            return GetDepartures(destinationName, liveDepartures);
        }

        private static SkillResponse GetDepartures(string destinationName, IList<Departure> liveDepartures)
        {
            var updates = liveDepartures.Select(d => $"{d.GetTime()}{d.ImportantStatus()}").ToList();

            var trains = string.Join(", ", updates);

            var pluralNoun = updates.Count > 1 ? "s" : string.Empty;
            var verb = updates.Count > 1 ? "are" : "is";

            var speech = new SsmlOutputSpeech
            {
                Ssml = liveDepartures.Any() ?
                    $"<speak>Your next train{pluralNoun} to {destinationName} {verb} at: {trains}</speak>" :
                    $"<speak>There are no trains to {destinationName} at the moment</speak>"
            };

            Logger.Log($"Returning speech: `{speech.Ssml}`");

            return ResponseBuilder.Tell(speech);
        }
    }
}