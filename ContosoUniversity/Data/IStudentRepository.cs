using System;
using System.Collections.Generic;
using ContosoUniversity.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Data
{
    public interface IStudentRepository : IDisposable
    {
        IQueryable<Student> GetStudents(string searchString, string sortOrder);
        Task<Student> GetStudentByID(int? studentId);
        Task<Student> DeleteGetStudentByID(int? studentId);
        void InsertStudent(Student student);
        void DeleteStudent(Student student);
        void UpdateStudent(Student student);
        Task Save();
    }
}
