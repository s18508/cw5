using enrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace enrollment.Services
{
    public interface IStudentsDbService
    {
        public string PromoteStudent(Enrollment en);
        public string EnrollStudent(Student stud);
        public string CheckStudentIndex(string index);
    }
}
