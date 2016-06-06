using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BTCE.Models;

namespace BTCE.Api
{
    public class BtceApi
    {
        private const string BtceUrl = "https://btc-e.nz/api/2/";

        private static string TickerUrl(string tradeType) => 
            Path.Combine(BtceUrl, tradeType, "ticker");
        private static string FeeUrl(string tradeType) =>
            Path.Combine(BtceUrl, tradeType, "fee");
        private static string Get(string url)
        {
            //TODO Something wrong with get responce on ssl/ tls connection
            var stream = WebRequest.Create(url)
                .GetResponse()
                .GetResponseStream();
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static Ticker GetTicker(TradePair pair)
        {
            var url = TickerUrl(pair.ToString());
            var json = Get(url);
            return JsonTicker.Parse(json);
        }
    }
}
