using System;
using ContosoUniversity.Models;
using System.Threading.Tasks;

namespace ContosoUniversity.Data
{
    public class UnitOfWork : IDisposable
    {
        private SchoolContext _context;
        private DepartmentRepository _departmentRepository;
        private InstructorRepository _instructorRepository;
        private HomeRepository _homeRepository;
        private StudentRepository _studentRepository;
        private GenericRepository<Course> _courseRepository;

        public UnitOfWork(SchoolContext schoolContext)
        {
            _context = schoolContext;
        }

        public DepartmentRepository DepartmentRepository
        {
            get
            {

                if (_departmentRepository == null)
                {
                    _departmentRepository = new DepartmentRepository(_context);
                }
                return _departmentRepository;
            }
        }

        public InstructorRepository InstructorRepository
        {
            get
            {

                if (_instructorRepository == null)
                {
                    _instructorRepository = new InstructorRepository(_context);
                }
                return _instructorRepository;
            }
        }

        public HomeRepository HomeRepository
        {
            get
            {

                if (_homeRepository == null)
                {
                    _homeRepository = new HomeRepository(_context);
                }
                return _homeRepository;
            }
        }

        public StudentRepository StudentRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _studentRepository = new StudentRepository(_context);
                }
                return _studentRepository;
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
