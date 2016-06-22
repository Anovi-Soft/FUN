using Newtonsoft.Json;

namespace BTCE.Api
{
    public class BtceJson
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        public bool IsIncorrect => !string.IsNullOrWhiteSpace(Error);
    }
}
