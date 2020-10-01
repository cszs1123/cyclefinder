﻿using CycleFinder.Extensions;
using CycleFinder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CycleFinder.Services
{
    public class BinanceDataService : IDataService
    {
        private const string _rootUrl = "https://api.binance.com";
        private readonly int _limit = 1000;
        private readonly DateTime _firstTradeDate = new DateTime(2016, 01, 01);

        protected Dictionary<Endpoint, string> Endpoints = new Dictionary<Endpoint, string>()
        {
            { Endpoint.Connectivity, "/api/v3/ping" },
            { Endpoint.MarketData, $"/api/v3/klines" },
            { Endpoint.ExchangeInfo, "/api/v3/exchangeInfo"},
        };

        private static readonly HttpClient client = new HttpClient();

        public bool IsAlive { get { return CheckConnection().Result; } }

        public async Task<List<Symbol>> ListSymbols()
        {
            var result = JObject.Parse(await client.GetStringAsync(_rootUrl + Endpoints[Endpoint.ExchangeInfo]));
            return result["symbols"].Select(_ => new Symbol(_["symbol"].ToString(), _["quoteAsset"].ToString())).ToList();
        }


        public async Task<List<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null)
        {
            var str = await client.GetStringAsync(BuildMarketDataRequest(symbol, timeFrame, startTime, endTime));
            List<List<double>> candles = JsonConvert.DeserializeObject<List<List<double>>>(str);

            return candles.Select(_ => new CandleStick(_[0], _[1], _[2], _[3], _[4], _[5])).ToList();
        }

        public async Task<List<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrame.Daily:
                    return (await GetAllDailyData(symbol, timeFrame, _firstTradeDate)).OrderBy(_ => _.Time).ToList();
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<List<CandleStick>> GetAllDailyData(string symbol, TimeFrame timeFrame, DateTime startTime)
        {
            var candles = (await GetData(symbol, timeFrame, startTime));
            var diff = candles.First().Time - DateTime.Now;

            return Math.Abs(diff.TotalDays) > _limit ?  (await GetAllDailyData(symbol, timeFrame, candles.Last().Time.AddDays(1))).Concat(candles).ToList() : candles;
        }

        private async Task<bool> CheckConnection()
        {
            return await client.GetStringAsync(_rootUrl + Endpoints[Endpoint.Connectivity]) == "{}";
        }


        private string BuildMarketDataRequest(string symbol, TimeFrame timeframe, DateTime? startTime, DateTime? endTime)
        {
            var sb = new StringBuilder(_rootUrl + Endpoints[Endpoint.MarketData] + "?");
            if (String.IsNullOrEmpty(symbol)) throw new Exception("Symbol cannot be null");

            sb.Append($"symbol={symbol}");
            sb.Append($"&interval={timeframe.GetDescription()}");

            if (startTime != null) sb.Append($"&startTime={new DateTimeOffset(startTime.GetValueOrDefault().ToUniversalTime()).ToUnixTimeMilliseconds()}");
            if (endTime != null) sb.Append($"&endTime={new DateTimeOffset(endTime.GetValueOrDefault().ToUniversalTime()).ToUnixTimeMilliseconds()}");

            sb.Append($"&limit={_limit}");

            return sb.ToString();
        }


    }
}
