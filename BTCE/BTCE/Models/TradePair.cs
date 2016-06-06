using System;
using System.Linq;

namespace BTCE.Models
{
    public class TradePair
    {
        public TradePair()
        {
        }
        public TradePair(CurrencyType @from, CurrencyType to)
        {
            From = @from;
            To = to;
        }
        public int Id { get; set; }
        public CurrencyType From { get; set; }
        public CurrencyType To { get; set; }

        public static TradePair[] AllPairs =>
            "BtcUsd,BtcRur,BtcEur,LtcBtc,LtcUsd,LtcRur,LtcEur,NmcBtc,NmcUsd,NvcBtc,NvcUsd,UsdRur,EurUsd,EurRur,PpcBtc,PpcUsd"
                .Split(',')
                .Select(Parse)
                .ToArray();

        public static implicit operator int(TradePair pair) =>
            ((byte)pair.From << 8) | ((byte)pair.To);
        public static implicit operator TradePair(int num) =>
            new TradePair
            {
                From = (CurrencyType)(num >> 8),
                To = (CurrencyType)(num & (int.MaxValue >> 8))
            };
        public static TradePair Parse(string line)=>
            new TradePair
            {
                From = (CurrencyType)Enum.Parse(typeof(CurrencyType), line.Substring(0, 3)),
                To = (CurrencyType)Enum.Parse(typeof(CurrencyType), line.Substring(3))
            };
        public override string ToString() =>
            From.ToString() + To;

    }
}
