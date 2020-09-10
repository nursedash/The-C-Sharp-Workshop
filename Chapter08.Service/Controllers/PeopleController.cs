﻿using Chapter08.Models;
using Chapter08.Service.Static;
using Microsoft.AspNetCore.Mvc;

namespace Chapter08.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {

        }

        [HttpPost]
        public IActionResult Post(Person person)
        {
            JsonFiles.Save(person, person.Name);
            return Ok();
        }
    }
}
