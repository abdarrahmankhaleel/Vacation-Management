using Microsoft.AspNetCore.Mvc;
using VacationManagement.Data;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class VacationTypesController : Controller
    {
        private readonly VacationDbContext _context;

        public VacationTypesController(VacationDbContext context)
        {
            _context = context;
        }
        public IActionResult VacationTypes()
        {
            return View(_context.VacationTypes.OrderBy(x=>x.Id).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VacationType model)
        {
            if (ModelState.IsValid)
            {
                var Result= _context.VacationTypes.FirstOrDefault(x => x.VacationName == model.VacationName.Trim());

                if (Result == null)
                {
                _context.VacationTypes.Add(model);
                _context.SaveChanges();
                return RedirectToAction("VacationTypes");
                }
                ViewBag.ErrorMsg = false;
            }
            return View(model);
        }
        public IActionResult Edit(int? id)
        {
            return View(_context.VacationTypes.FirstOrDefault(x=>x.Id==id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VacationType model)
        {
            if (ModelState.IsValid)
            {
                _context.VacationTypes.Update(model);
                _context.SaveChanges();
                return RedirectToAction("VacationTypes");
            }
            return View(model);
        }
        public IActionResult Delete(int? id)
        {
            return View(_context.VacationTypes.FirstOrDefault(x=>x.Id==id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(VacationType model)
        {
            if (model!=null)
            {
                _context.VacationTypes.Remove(model);
                _context.SaveChanges();
                return RedirectToAction("VacationTypes");
            }
            return View(model);
        }
    }
}
