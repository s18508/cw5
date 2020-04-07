using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using enrollment.Models;
using enrollment.Services;
using Microsoft.AspNetCore.Mvc;

namespace enrollment.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;

        public EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        //

        [HttpPost]
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
    }
}