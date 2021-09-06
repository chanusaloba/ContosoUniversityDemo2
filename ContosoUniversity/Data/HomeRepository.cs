using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Data
{
    public class HomeRepository : IHomeRepository, IDisposable
    {
        private SchoolContext _context;

        public HomeRepository(SchoolContext context)
        {
            _context = context;
        }

        public IQueryable<EnrollmentDateGroup> About()
        {
            var data =
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };

            return data;
        }

        public void Dispose()
        {
            
        }

    }
}
