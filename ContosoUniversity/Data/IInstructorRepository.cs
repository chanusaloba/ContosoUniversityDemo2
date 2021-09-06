using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Data
{
    public interface IInstructorRepository : IDisposable
    {
        Task<InstructorIndexData> GetInstructors(int? id, int? courseId);
        DbSet<Instructor> GetInstructors();
        Task<Instructor> GetInstructorById(int? id);
        List<AssignedCourseData> PopulateCourseData(Instructor instructor);
        Task Save();
        void InsertInstructor(Instructor instructor);
        Task<Instructor> EditGetInstructorById(int? id);
        void UpdateInstructionCourses(string[] selectedCourses, Instructor instructorToUpdate);
        Task DeleteInstructor(int id);
        void UpdateInstructor(Instructor instructor);
    }
}
