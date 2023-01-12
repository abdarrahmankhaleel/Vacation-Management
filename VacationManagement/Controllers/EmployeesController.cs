using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationManagement.Data;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly VacationDbContext _context;

        public EmployeesController(VacationDbContext context)
        {
            _context = context;
        }
        public IActionResult Employees()
        {
            return View(_context.Employees.Include(x=>x.Department).OrderBy(x=>x.Id).ToList());
        }
        public IActionResult Create()
        {
            ViewBag.Departmens=_context.Departments.OrderBy(x=>x.Name).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Employees");
            }
            ViewBag.Departmens=_context.Departments.OrderBy(x=>x.Name).ToList();
            return View(model);
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.Departmens=_context.Departments.OrderBy(x=>x.Name).ToList();
            return View(_context.Employees.Include(x => x.Department).FirstOrDefault(x=>x.Id==id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Employees");
            }
            ViewBag.Departmens=_context.Departments.OrderBy(x=>x.Name).ToList();
            return View(model);
        }
        public IActionResult Delete(int? id)
        {
            ViewBag.Departmens=_context.Departments.OrderBy(x=>x.Name).ToList();
            return View(_context.Employees.Include(x=>x.Department).FirstOrDefault(x=>x.Id==id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Employee model)
        {
            if (model!=null)
            {
                _context.Employees.Remove(model);
                _context.SaveChanges();
                return RedirectToAction("Employees");
            }
            ViewBag.Departmens=_context.Departments.OrderBy(x=>x.Name).ToList();
            return View(model);
        }
    }
}
