using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MultipleConnectionStrings.Dto;
using MultipleConnectionStrings.Helpers;
using MultipleConnectionStrings.Models.Property;
using MultipleConnectionStrings.Models.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultipleConnectionStrings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MultipleConnectionStringContext _connection;
        private readonly PropertyDbContext _propertyContext;
        public readonly IConfiguration _configuration;
        public AuthController(MultipleConnectionStringContext connection, PropertyDbContext propertyContext, IConfiguration configuration)
        {
            _connection = connection;
            _propertyContext = propertyContext;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userObj = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
                ConnectionId = registerDto.ConnectionId,
                DatePosted = DateTime.Now
            };

            await _connection.Users.AddAsync(userObj);
            await _connection.SaveChangesAsync();

            return Ok(registerDto);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var errors = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                errors = new { message = "Invalid credentials" }
            };
            User user = await _connection.Users.AsNoTracking()
                .Include(c => c.Connection).FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || user.Password != loginDto.Password) return BadRequest(errors);
            //create claims details based on the user information
            Claim[] claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Name),
                     new Claim("email", user.Email),
                    new Claim("clientDatabase", user.Connection.ConnectionName)
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            UserResponseDto responseUser = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ConnectionId = user.Connection.Id,
                DatePosted = user.DatePosted,
                DateUpdated = user.DateUpdated,

            };
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), user = responseUser });
        }




        private void SetContext(User user)
        {
            string DatabaseId = user.Connection.ConnectionName;
            _propertyContext.Database.SetDbConnection(Utilities.GetStringConnection(DatabaseId, _propertyContext));
        }

        
    }
}
