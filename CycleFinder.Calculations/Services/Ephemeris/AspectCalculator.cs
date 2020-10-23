﻿using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public class AspectCalculator : IAspectCalculator
    {
        private static readonly double _orb = 1.00;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        private static readonly Func<Planet, EphemerisEntry, Coordinates> _planetSelector =
            (planet, entry) => planet switch
                {
                    Planet.Moon => entry.Moon,
                    Planet.Sun => entry.Sun,
                    Planet.Mercury => entry.Mercury,
                    Planet.Venus => entry.Venus,
                    Planet.Mars => entry.Mars,
                    Planet.Jupiter => entry.Jupiter,
                    Planet.Saturn => entry.Saturn,
                    Planet.Uranus => entry.Uranus,
                    Planet.Neptune => entry.Neptune,
                    Planet.Pluto => entry.Pluto,
                    _ => null,
                };

        public AspectCalculator(IEphemerisEntryRepository ephemerisEntryRepository)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        public async Task<IEnumerable<Aspect>> GetAspects(DateTime startTime, Planet planet1, Planet planet2)
        {
            var ephem = await _ephemerisEntryRepository.GetEntries(startTime);
            var ret = new List<Aspect>();

            foreach (var entry in ephem)
            {
                var aspectType = GetAspectType(GetCircularDifference(_planetSelector(planet1, entry).Longitude, _planetSelector(planet2, entry).Longitude), _orb);
                if (aspectType != null)
                {
                    ret.Add(new Aspect(entry.Time, planet1, planet2, aspectType.Value));
                }
            }

            return ret;
        }

        private static double GetCircularDifference(double l1, double l2) => System.Math.Abs(360 - l1 - (360 - l2));

        private static AspectType? GetAspectType(double diff, double orb)
        {
            return diff switch
            {
                double d when 0 - orb > d && 0 + orb < d => AspectType.Conjunction,
                double d when 180 - orb > d && 180 + orb < d => AspectType.Opposition,
                double d when 120 - orb > d && 120 + orb < d => AspectType.Trine,
                double d when 90 - orb > d && 90 + orb < d => AspectType.Square,
                double d when 60 - orb > d && 60 + orb < d => AspectType.Sextile,
                _ => null,
            };
        }
    }
}