using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TrainCheck.TransportApi
{
    public class TrainResponse
    {
        public Departures Departures { get; set; }

        public IList<Departure> GetLiveDepartures(int count)
        {
            return Departures.All.Take(count).ToList();
        }
    }

    public class Departures
    {
        public IEnumerable<Departure> All { get; set; }
    }

    public class Departure
    {
        [JsonProperty("expected_departure_time")]
        public string ExpectedDepartureTime { get; set; }

        [JsonProperty("aimed_departure_time")]
        public string AimedDepartureTime { get; set; }

        public string Status { get; set; }

        public Departure(
            string aimed = "12:00",
            string expected = "12:00",
            string status = "ON TIME")
        {
            AimedDepartureTime = aimed;
            ExpectedDepartureTime = expected;
            Status = status;
        }

        public string GetTime()
        {
            return ExpectedDepartureTime ?? AimedDepartureTime;
        }

        public string ImportantStatus()
        {
            var importantStatus = new[]
            {
                "LATE",
                "CANCELLED",
                "NO REPORT",
                "OFF ROUTE"
            };

            return importantStatus.Contains(Status) ? " " + Status : string.Empty;
        }
    }
}