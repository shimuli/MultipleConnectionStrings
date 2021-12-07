using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultipleConnectionStrings.Helpers;
using MultipleConnectionStrings.Models.Property;
using MultipleConnectionStrings.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MultipleConnectionStrings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly MultipleConnectionStringContext _connection;
        private readonly PropertyDbContext _propertyContext;
        public readonly IConfiguration _configuration;
        public PropertyController(MultipleConnectionStringContext connection, PropertyDbContext propertyContext, IConfiguration configuration)
        {
            _connection = connection;
            _propertyContext = propertyContext;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("GetCars")]
        public async Task<IActionResult> GetCars()
        {
            ChangeConnectionString();           
            var cars = await _propertyContext.Vehicles.ToListAsync();
            return Ok(cars);
        }

        [Authorize]
        [HttpGet("GetHouses")]
        public async Task<IActionResult> GetHouses()
        {
            ChangeConnectionString();
            var houses = await _propertyContext.Houses.ToListAsync();
            return Ok(houses);
        }

        private void ChangeConnectionString()
        {
            string databaseId = Utilities.UserClaim(User.Identity as ClaimsIdentity, "clientDatabase");
            _propertyContext.Database.SetDbConnection(Utilities.GetStringConnection(databaseId, _propertyContext));
        }
    }
}
