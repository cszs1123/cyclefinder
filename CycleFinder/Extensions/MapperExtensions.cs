﻿using CycleFinder.Dtos;
using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Markers;

namespace CycleFinder.Extensions
{
    public static class MapperExtensions
    {
        public static CandleStickDto ToCandleStickDto(this CandleStick candleStick)
        {
            return new CandleStickDto(candleStick.TimeInSeconds, candleStick.Open, candleStick.High, candleStick.Low, candleStick.Close, candleStick.Volume);
        }

        public static CandleStickMarkerDto ToCandleStickMarkerDto(
            this ICandleStickMarker candleMarker)
        {
            return new CandleStickMarkerDto(candleMarker);
        }
    }
}
