﻿using CycleFinder.Models.Candles;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class EventMarker : ICandleStickMarker
    {
        public CandleStick Candle { get; protected set; }

        public Color Color { get; protected set; }

        public string Text { get; protected set; }

        public virtual MarkerPosition Position => MarkerPosition.BelowBar;

        public virtual MarkerShape Shape => MarkerShape.Circle;

        public EventMarker(CandleStick candle, Color color, string text = null)
        {
            Candle = candle;
            Color = color;
            Text = text;
        }
    }
}