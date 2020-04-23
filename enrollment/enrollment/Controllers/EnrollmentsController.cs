using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using enrollment.Models;
using enrollment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace enrollment.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;
        public IConfiguration _configuration;
        public object Configuration { get; private set; }

        public EnrollmentsController(IStudentsDbService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        //
       
        [HttpPost]
        [Authorize(Roles ="employee")]
        public IActionResult EnrollStudent(Student stud)
        {
         
            
            string messege = _service.EnrollStudent(stud);
            if (messege.Equals("OK"))
            {
                return Ok("ok");
            }
            else
            {
                return BadRequest(messege);
            }

        }


        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(Enrollment en)
        {
          

            string messege = _service.PromoteStudent(en);
            if (messege.Equals("OK"))
            {
                return Ok("ok");
            }
            else
            {
                return BadRequest(messege);
            }

        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            if (!_service.CheckLoginData(request))
            {
                return BadRequest("brak dostepu");
            }
            var claimst = new[] { new Claim(ClaimTypes.Role, "student"), new Claim(ClaimTypes.Name, request.login) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer : "gakko",
                audience : "students",
                claims: claimst,
                signingCredentials: creds);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            }
            );

        }
    }
}