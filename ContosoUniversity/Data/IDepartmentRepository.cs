using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public interface IDepartmentRepository : IDisposable
    {
        Task<List<Department>> GetDepartments();
        Task<Department> GetDepartmentById(int? id);
        Task<Department> EditGetDepartmentById(int? id);
        void AssignRowVersion(byte[] rowVersion, Department departmentToUpdate);
        Task Save();
        void InsertDepartment(Department department);
        Task DeleteDepartment(Department department);
    }
}
