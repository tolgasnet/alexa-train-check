using System;
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
            var distinctDepartures = departuresAll.Distinct();
            return distinctDepartures.Take(count).ToList();
        }
    }

    public class Departures
    {
        public IEnumerable<Departure> All { get; set; }
    }

    public class Departure : IEquatable<Departure>
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

        public string GetStatus()
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

        public bool Equals(Departure other)
        {
            if (other == null) return false;

            return GetTime() == other.GetTime() && GetStatus() == other.GetStatus();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (GetTime() != null ? GetTime().GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GetStatus() != null ? GetStatus().GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}