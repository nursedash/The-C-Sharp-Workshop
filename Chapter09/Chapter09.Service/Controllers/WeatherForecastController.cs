﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chapter09.Service.Models;
using Chapter09.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Chapter09.Service.Controllers
{
    [Authorize]
    [ApiController]
    [RequiredScope("access_as_user")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService1;
        private readonly IWeatherForecastService _weatherForecastService2;
        private readonly ILogger _logger;

        public WeatherForecastController(ILoggerFactory logger, IWeatherForecastService weatherForecastService1, IWeatherForecastService weatherForecastService2)
        {
            _weatherForecastService1 = weatherForecastService1;
            _weatherForecastService2 = weatherForecastService2;
            _logger = logger.CreateLogger(typeof(WeatherForecastController).FullName);
        }

        [HttpGet("error")]
        public IEnumerable<WeatherForecast> GetError()
        {
            _logger.LogError("Whoops");
            throw new Exception("Something went wrong");
        }

        [HttpGet("weekday/{day}")]
        public IActionResult GetWeekday(int day)
        {
            var result = _weatherForecastService1.GetWeekday(day);
            result = _weatherForecastService2.GetWeekday(day);
            return Ok(result);
        }

        /// <summary>
        /// Gets weather forecast for now.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWeatherForecast()
        {
            var weatherForecast = await _weatherForecastService1.GetWeatherForecast(DateTime.UtcNow);
            if (weatherForecast == null) return NotFound();
            return Ok(weatherForecast);
        }

        /// <summary>
        /// Saves a forecast at forecast date.
        /// </summary>
        /// <param name="weatherForecast">Date which identifies a forecast. Using short date time string for identity.</param>
        /// <returns>201 with a link to an action to fetch a created forecast.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult SaveWeatherForecast(WeatherForecast weatherForecast)
        {
            _weatherForecastService1.SaveWeatherForecast(weatherForecast);
            return CreatedAtAction("GetWeatherForecast", new { date = weatherForecast.Date.ToShortDateString() }, weatherForecast);
        }
    }
}