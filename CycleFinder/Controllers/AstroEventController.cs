﻿using CycleFinder.Calculations.Services.Astro;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class AstroEventController : ControllerBase
    {
        private readonly ILogger<AstroEventController> _logger;
        private readonly IQueryParameterProcessor _parameterProcessor;
        private readonly IAstroEventCalculator _astroEventCalculator;

        private readonly int[] _validyears = new int[] { 2021, 2022 };

        public AstroEventController(ILogger<AstroEventController> logger, IQueryParameterProcessor parameterProcessor, IAstroEventCalculator aspectCalculator)
        {
            _logger = logger;
            _parameterProcessor = parameterProcessor;
            _astroEventCalculator = aspectCalculator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstroEventDto>>> GetAstroEvents(
            [FromQuery] int year,
            string planets)
        {
            if (!IsValidYear(year))
            {
                return NotFound();
            }

            var planetEnums = _parameterProcessor.PlanetsFromString(planets);

            if (!planetEnums.Any())
            {
                return NotFound();
            }

            var (from, to) = CalculateDatesForYear(year);

            return Ok((await _astroEventCalculator.GetAstroEvents(from, to, planetEnums))
                .Select(_ => _.ToAstroEventDto()).OrderBy(_ => _.Time));
        }

        private bool IsValidYear(int year) => _validyears.Contains(year);
        
        //TODO for dev purposes only return 2 months
        //private static (DateTime from, DateTime to) CalculateDatesForYear(int year) => (new DateTime(year, 1, 1).ToUniversalTime(), new DateTime(year, 2, 28).ToUniversalTime());
        private static (DateTime from, DateTime to) CalculateDatesForYear(int year) => (new DateTime(year-1, 1, 1).ToUniversalTime(), new DateTime(year, 12, 31).ToUniversalTime());
    }
}
