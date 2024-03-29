﻿using CycleFinder.Models.Candles;
using System;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class EventMarker : ICandleStickMarker
    {
        public DateTime Time { get; }

        public Color Color { get; protected set; }

        public string Text { get; protected set; }

        public virtual MarkerPosition Position => MarkerPosition.BelowBar;

        public virtual MarkerShape Shape => MarkerShape.Circle;


        public EventMarker(DateTime time)
        {
            Time = time;
        }

        public EventMarker(DateTime time, Color color)
        {
            Time = time;
            Color = color;
        }
    }
}
