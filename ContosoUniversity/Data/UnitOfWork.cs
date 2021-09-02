using System;
using ContosoUniversity.Models;
using System.Threading.Tasks;

namespace ContosoUniversity.Data
{
    public class UnitOfWork : IDisposable
    {
        private SchoolContext _context;
        private GenericRepository<Department> _departmentRepository;
        private GenericRepository<Course> _courseRepository;

        public UnitOfWork(SchoolContext schoolContext)
        {
            _context = schoolContext;
        }

        public GenericRepository<Department> DepartmentRepository
        {
            get
            {

                if (_departmentRepository == null)
                {
                    _departmentRepository = new GenericRepository<Department>(_context);
                }
                return _departmentRepository;
            }
        }

        public GenericRepository<Course> CourseRepository
        {
            get
            {

                if (_courseRepository == null)
                {
                    _courseRepository = new GenericRepository<Course>(_context);
                }
                return _courseRepository;
            }
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
