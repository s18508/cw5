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


            /*
            if (stud.FirstName == null || stud.LastName == null || stud.IndexNumber == null)
            {
                return BadRequest("Brak danych");
            }
            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                //var ts = con.BeginTransaction();
                com.CommandText = "exec zad1 @IndexNumber,@FirstName,@LastName,@BirthDate,@StudyName,@Semester";
                com.Parameters.AddWithValue("IndexNumber", stud.IndexNumber);
                com.Parameters.AddWithValue("FirstName", stud.FirstName);
                com.Parameters.AddWithValue("LastName", stud.LastName);
                com.Parameters.AddWithValue("BirthDate", stud.BirthDate);
                com.Parameters.AddWithValue("StudyName", stud.Studies);
                com.Parameters.AddWithValue("Semester", stud.Semester);
                try
                {
                    com.ExecuteNonQuery();
                    //ts.Commit();
                }
                catch (Exception exc)
                {
                    //ts.Rollback();
                    return BadRequest(exc.Message);
                }            
            }
            return Ok("OK");
            */ // tu wszytko ladnie dziala nie liczac rollbakow 
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

            /*
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (var com = new SqlCommand()) 
            {
                com.Connection = client;
                client.Open();
                com.CommandText = "zad2 @StudyName, @Semester";
                com.Parameters.AddWithValue("StudyName", en.Study);
                com.Parameters.AddWithValue("Semester", en.Semester);
                try
                {
                    com.ExecuteNonQuery();
                    //ts.Commit();
                }
                catch (Exception exc)
                {
                    //ts.Rollback();
                    return BadRequest(exc.Message);
                }

            }
            return Ok("OK");
        }
        */
        }
    }
}