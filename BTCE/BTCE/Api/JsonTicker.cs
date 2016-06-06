using System;
using BTCE.Models;
using Newtonsoft.Json;

namespace BTCE.Api
{
    public class JsonTicker
    {
        [JsonProperty("ticker")]
        public Ticker Ticker { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public static Ticker Parse(string json)
        {
            var result = JsonConvert.DeserializeObject<JsonTicker>(json);
            if (result.Error != null)
                throw new ArgumentException(result.Error);
            return result.Ticker;
        }

        public static bool TryParse(string json, ref Ticker ticker)
        {
            var result = JsonConvert.DeserializeObject<JsonTicker>(json);
            if (result.Error != null) return false;
            ticker = result.Ticker;
            return true;
        }
    }
}
