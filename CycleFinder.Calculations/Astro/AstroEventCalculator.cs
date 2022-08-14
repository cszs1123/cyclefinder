﻿using CycleFinder.Calculations.Astro;
using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CycleFinder.Calculations.Math.Extremes;
using CycleFinder.Calculations.Astro.Aspects;
using CycleFinder.Calculations.Astro.Extremes;

namespace CycleFinder.Calculations.Services.Astro
{
    public class AstroEventCalculator : IAstroEventCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly IAspectCalculator _aspectCalculator;
        private readonly IAstroExtremeCalculator _extremeCalculator;

        public AstroEventCalculator(
            IEphemerisEntryRepository ephemerisEntryRepository, 
            IAspectCalculator aspectCalculator,
            IAstroExtremeCalculator extremeCalculator)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _aspectCalculator = aspectCalculator;
            _extremeCalculator = extremeCalculator;
        }

        public async Task<IEnumerable<AstroEvent>> GetAstroEvents(
            DateTime from, 
            DateTime to, 
            IEnumerable<Planet> planets, 
            IEnumerable<ExtremeType> extremes,
            IEnumerable<AspectType> aspects)
        {
            List<AstroEvent> ret = new();

            if (extremes.Any())
            {
                ret.AddRange(await _extremeCalculator.GetExtremes(from, to, planets, extremes));
            }

            if (aspects.Any())
            {
                ret.AddRange(await _aspectCalculator.GetAspects(from, to, planets, aspects));
            }

            return ret;
         }


        public async Task<IEnumerable<AstroEvent>> GetAspectsBetweenPlanets(DateTime from, DateTime to, Planet smallerPlanet, Planet largerPlanet, IEnumerable<AspectType> aspects)
        {
            List<AstroEvent> ret = new();

            if (aspects.Any())
            {
                ret.AddRange(await _aspectCalculator.GetAspectsForPlanetPairs(from, to, smallerPlanet, largerPlanet, aspects));
            }

            return ret;
        }
    }
}