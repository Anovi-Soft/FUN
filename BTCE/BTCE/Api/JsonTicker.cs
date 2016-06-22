using System;
using BTCE.Models;
using Newtonsoft.Json;

namespace BTCE.Api
{
    public class JsonTicker : BtceJson
    {
        [JsonProperty("ticker")]
        public Ticker Ticker { get; set; }

        public static Ticker Parse(string json)
        {
            var result = JsonConvert.DeserializeObject<JsonTicker>(json);
            if (result.IsIncorrect)
                throw new ArgumentException(result.Error);
            return result.Ticker;
        }

        public static bool TryParse(string json, out Ticker ticker)
        {
            var result = JsonConvert.DeserializeObject<JsonTicker>(json);
            if (result.IsIncorrect)
            {
                ticker = null;
                return false;
            }
            ticker = result.Ticker;
            return true;
        }
    }
}
