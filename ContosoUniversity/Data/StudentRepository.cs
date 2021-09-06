using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ContosoUniversity.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public IQueryable<Student> GetStudents(string searchString, string sortOrder)
        {
            var students = _context.Students.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                students = _context.Students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            return students;
        }

        public async Task<Student> GetStudentByID(int? id)
        {
            var students = _context.Students.AsQueryable();

            return await students.Include(s => s.Enrollments)
                            .ThenInclude(e => e.Course)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<Student> DeleteGetStudentByID(int? id)
        {
            return await _context.Students.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
        }

        public void InsertStudent(Student student)
        {
            _context.Students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            _context.Students.Remove(student);
        }

        public void UpdateStudent(Student student)
        {
            _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
