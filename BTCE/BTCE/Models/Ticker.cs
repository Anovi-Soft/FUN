using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

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

        public override bool Equals(object obj) =>
            obj is Ticker && Equals((Ticker) obj);

        protected bool Equals(Ticker other) => 
            Equals(TradePair, other.TradePair) &&
            Hight.Equals(other.Hight) &&
            Low.Equals(other.Low) &&
            Avg.Equals(other.Avg) &&
            Volume.Equals(other.Volume) &&
            CurrentVolume.Equals(other.CurrentVolume) &&
            Last.Equals(other.Last) &&
            Buy.Equals(other.Buy) &&
            Sell.Equals(other.Sell);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (TradePair?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Hight.GetHashCode();
                hashCode = (hashCode * 397) ^ Low.GetHashCode();
                hashCode = (hashCode * 397) ^ Avg.GetHashCode();
                hashCode = (hashCode * 397) ^ Volume.GetHashCode();
                hashCode = (hashCode * 397) ^ CurrentVolume.GetHashCode();
                hashCode = (hashCode * 397) ^ Last.GetHashCode();
                hashCode = (hashCode * 397) ^ Buy.GetHashCode();
                hashCode = (hashCode * 397) ^ Sell.GetHashCode();
                return hashCode;
            }
        }

        public bool UpdateMe(Ticker potencialGreather) =>
             Updated < potencialGreather.Updated && !Equals(potencialGreather);
    }
}
