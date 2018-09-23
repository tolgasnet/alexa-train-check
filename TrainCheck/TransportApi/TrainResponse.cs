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
            var departuresAll = Departures.All;
            var activeDepartures = departuresAll.Where(d => !d.IsCancelled());

            return activeDepartures.Take(count).ToList();
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

        public string Status { get; set; }

        public bool IsCancelled()
        {
            return Status == "CANCELLED";
        }
    }
}