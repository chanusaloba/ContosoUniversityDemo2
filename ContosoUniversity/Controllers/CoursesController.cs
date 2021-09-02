using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;
        private UnitOfWork _unitOfWork;

        public CoursesController(SchoolContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = _unitOfWork.CourseRepository.Get(includeProperties: "Department");
            return View(await courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _unitOfWork.CourseRepository.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Credits,DepartmentID,Title")] Course course)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CourseRepository.Insert(course);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _unitOfWork.CourseRepository.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("CourseID,Title,Credits,DepartmentID")]
         Course course)
        {
            try
            {
                _unitOfWork.CourseRepository.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Course course = await _unitOfWork.CourseRepository.GetByID(id);
            await _unitOfWork.CourseRepository.Delete(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = _unitOfWork.DepartmentRepository.Get(
             orderBy: q => q.OrderBy(d => d.Name));

            ViewBag.DepartmentID = new SelectList(departmentsQuery.Result, "DepartmentID", "Name", selectedDepartment);
        }
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
