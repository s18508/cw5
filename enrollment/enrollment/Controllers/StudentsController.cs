using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using enrollment.Models;
using Microsoft.AspNetCore.Mvc;

namespace enrollment.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<Models.Student>();

            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "select * from Student";
                client.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    list.Add(st);
                }
            }


            return Ok(list);
        }
    }
}