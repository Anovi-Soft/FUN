using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTCE.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BTCE.Api.Tests
{
    [TestFixture]
    class JsonTickerShould
    {
        [Test]
        public void UnParseError()
        {
            var json = "{\"error\":\"invalid pair\"}";
            Ticker ticker;
            JsonTicker.TryParse(json, out ticker).Should().Be(false);
        }

        [Test]
        public void ParseValidTicker()
        {
            var json =
                "{\"ticker\":{\"high\":745,\"low\":719,\"avg\":732,\"vol\":4514869.71172,\"vol_cur\":6193.53755,\"last\":724.999,\"buy\":724.999,\"sell\":724.421,\"updated\":1466323709,\"server_time\":1466323710}}";
            Ticker ticker;
            JsonTicker.TryParse(json, out ticker).Should().Be(true);
        }
    }
}
