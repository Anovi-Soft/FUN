using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BTCE.Models
{
    public class Ticker
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore, Column(TypeName = "int")]
        public TradePair TradePair { get; set; }
        
        [JsonProperty("high")]
        public float Hight { get; set; }

        [JsonProperty("low")]
        public float Low { get; set; }

        [JsonProperty("avg")]
        public float Avg { get; set; }

        [JsonProperty("vol")]
        public float Volume { get; set; }

        [JsonProperty("vol_cur")]
        public float CurrentVolume { get; set; }

        [JsonProperty("last")]
        public float Last { get; set; }

        [JsonProperty("buy")]
        public float Buy { get; set; }

        [JsonProperty("sell")]
        public float Sell { get; set; }

        [JsonProperty("updated")]
        public uint Updated { get; set; }

        [JsonProperty("server_time")]
        public uint ServerTime { get; set; }
    }
}
