using System.Collections.Generic;

namespace TrainCheck.Config
{
    public class TrainStationSettings : AppSetting
    {
        public string DefaultHome { get; set; }
        public string DefaultDestination { get; set; }
        public int NumberOfServices { get; set; }
        public int WalkingTimeInMinutes { get; set; }
        public IReadOnlyDictionary<string, string> Stations { get; set; }

        public string GetCode(string name)
        {
            return Stations[name.ToLower()];
        }
    }
}