using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class DepartmentRepository : IDepartmentRepository, IDisposable
    {
        private SchoolContext _context;

        public DepartmentRepository(SchoolContext context)
        {
            _context = context;
        }

        public Task<List<Department>> GetDepartments()
        {
            var Result = _context.Departments.Include(d => d.Administrator).ToListAsync();
            return Result;
        }

        public List<Department> GetDepartmentsOrderByName()
        {
            var Result = _context.Departments.OrderBy(x => x.Name).ToList();
            return Result;
        }

        public async Task<Department> GetDepartmentById(int? id)
        {
            var department = await _context.Departments
                    .Include(i => i.Administrator)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.DepartmentID == id);

            return department;
        }

        public void InsertDepartment(Department department)
        {
            _context.Add(department);
        }

        public void UpdateDepartment(Department departmentToUpdate)
        {
            _context.Entry(departmentToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task<Department> EditGetDepartmentById(int? id)
        {
            var departmentToUpdate = await _context.Departments.Include(i => i.Administrator).FirstOrDefaultAsync(m => m.DepartmentID == id);
            return departmentToUpdate;
        }

        public async Task DeleteDepartment(Department department)
        {
            if (await _context.Departments.AnyAsync(m => m.DepartmentID == department.DepartmentID))
            {
                _context.Departments.Remove(department);
                await Save();
            }
        }

        public void AssignRowVersion(byte[] rowVersion, Department departmentToUpdate)
        {
            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;
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
