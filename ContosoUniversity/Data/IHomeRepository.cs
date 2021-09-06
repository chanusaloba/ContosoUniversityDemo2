using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Data
{
    public interface IHomeRepository : IDisposable
    {
        IQueryable<EnrollmentDateGroup> About();
    }
}
