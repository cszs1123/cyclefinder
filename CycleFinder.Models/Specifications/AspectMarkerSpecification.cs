﻿using System;

namespace CycleFinder.Models.Specifications
{
    public class AspectMarkerSpecification : MarkerSpecification
    {
        public override bool IsValid => Planet1 != Planet2 && From != null; 

        public DateTime From { get; }
        public Planet Planet1 { get; }
        public Planet Planet2 { get; }

        public AspectMarkerSpecification(DateTime from, Planet planet1, Planet planet2)
        {
            From = from;
            Planet1 = planet1;
            Planet2 = planet2;
        }
    }
}
