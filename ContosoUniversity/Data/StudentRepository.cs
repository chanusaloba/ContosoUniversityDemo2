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

        public IQueryable<Student> GetStudents()
        {
            return _context.Students;
        }

        public async Task<Student> GetStudentByID(int? id)
        {
            return await _context.Students.Include(s => s.Enrollments)
                            .ThenInclude(e => e.Course)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<Student> EditGetStudentByID(int? id)
        {
            return await _context.Students.FindAsync(id);
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
