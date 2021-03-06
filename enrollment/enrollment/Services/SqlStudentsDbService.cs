﻿using enrollment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace enrollment.Services
{
    public class SqlStudentsDbService : IStudentsDbService
    {
        public string EnrollStudent(Student stud)
        {
            if (stud.FirstName == null || stud.LastName == null || stud.IndexNumber == null)
            {
                return "Brak danych";
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
                    return exc.Message;
                }
            }
            return "OK";
        }

        public string CheckStudentIndex(string index)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                client.Open();
                com.CommandText = "select * from student where IndexNumber=@index";
                com.Parameters.AddWithValue("index", index);
                var dr = com.ExecuteReader();
                if (!dr.Read()) //jesli brak rekrdow zwroci info o braku studiow
                {
                    return null;
                }
                else return index;
            }
        }

        public string PromoteStudent(Enrollment en)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                client.Open();
                //var ts = con.BeginTransaction();
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
                    return exc.Message;
                }

            }
            return "OK";
        }

        public bool CheckLoginData(LoginRequest log)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                client.Open();
                com.CommandText = "select IndexNumber , haslo from Student where IndexNumber=@index AND haslo=@haslo";
                com.Parameters.AddWithValue("index", log.login);
                com.Parameters.AddWithValue("haslo", log.haslo);
                var dr = com.ExecuteReader();
                if (!dr.Read()) // jesli brak rekordow to zla autoryzacja
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
    }
    
}
