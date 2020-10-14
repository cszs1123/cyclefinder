﻿using CycleFinder.Models.Candles;
using System;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class HighCandleMarker : CandleMarkerBase
    {
        public override MarkerPositions Position => MarkerPositions.AboveBar;

        public override MarkerShapes Shape => MarkerShapes.ArrowDown;

        public HighCandleMarker(CandleStick candle, Color color, int? id = null, int? turnId = null)
        {
            Candle = candle;
            Color = color;
            Text = turnId.HasValue ? $"TURN #{id}/{turnId}" : $"HIGH {(id == null ? "" : "#")}{id}";
        }
    }
}
