using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationManagement.Data;
using VacationManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VacationManagement.Controllers
{
    public class VacationPlansController : Controller
    {
        private readonly VacationDbContext _context;

        public VacationPlansController(VacationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.RequestVacations
                .Include(x=>x.Employee)
                .Include(x=>x.VacationType)
                .OrderByDescending(x=>x.RequestDate)
                .ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VacationPlan model, int[] DayOfWeekCheckbox)
        {
            if (ModelState.IsValid)
            {
                var result = _context.VacationPlans
                    .Where(x => x.RequestVacation.EmployeeId == model.RequestVacation.EmployeeId
                    && x.VacationDate >= model.RequestVacation.StartDate
                    && x.VacationDate <= model.RequestVacation.EndDate);
                if (result != null)
                {
                    ViewBag.ErrorVacation = false;
                    return View(model);
                }

                for (DateTime date = model.RequestVacation.StartDate;
                    date <= model.RequestVacation.EndDate; date = date.AddDays(1))
                {
                    if(Array.IndexOf(DayOfWeekCheckbox,(int)date.DayOfWeek) != -1)
                    {
                        model.Id = 0;
                        model.VacationDate = date;
                        model.RequestVacation.RequestDate = DateTime.Now;
                        _context.VacationPlans.Add(model);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("Index");

            }
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.Employees = _context.Employees.OrderBy(x => x.Name).ToList();
            ViewBag.VactionTypes = _context.VacationTypes.OrderBy(x => x.VacationName).ToList();
            var Result = _context.RequestVacations
                .Include(x => x.Employee)
                .Include(x => x.VacationType)
                .Include(x => x.VacationPlanList)
                .FirstOrDefault(x => x.Id == id);
            return View(Result);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(RequestVacation model)
        {
            if (!ModelState.IsValid)
            {
            ViewBag.Employees = _context.Employees.OrderBy(x => x.Name).ToList();
            ViewBag.VactionTypes = _context.VacationTypes.OrderBy(x => x.VacationName).ToList();
            return View(model);
            }
            if(model.Approved==true)
                model.DateApproved=DateTime.Now;
            _context.RequestVacations.Update(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult GetVacationTypes()
        {
            return Json(_context.VacationTypes.OrderBy(x=>x.Id).ToList());
        }
        public IActionResult GetVacationsCount(int Id)
        {
            return Json(_context.VacationPlans.Where(x=>x.RequestVacationId==Id).Count());
        }

        public IActionResult Delete(int? Id)
        {
            return View(_context.RequestVacations
                .Include(x=>x.Employee)
                .Include(x=>x.VacationType)
                .Include(x=>x.VacationPlanList)
                .FirstOrDefault(x=>x.Id==Id));
        }
        public IActionResult Delete(RequestVacation model)
        {
            if(model != null)
            {
                _context.RequestVacations.Remove(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model); 
        }

        public IActionResult ViewReportVationPlan()
        {
            ViewBag.Employee = _context.Employees.OrderBy(x => x.Name).ToList();
            return View();
        }
        public IActionResult ViewReportVationPlan2()
        {
            ViewBag.Employee = _context.Employees.OrderBy(x => x.Name).ToList();
            return View();
        }
        public IActionResult GetReportVationPlan
            (int EmployeeId, DateTime FromDate,DateTime ToDate)
        {
            string Id = "";
            if (EmployeeId != 0 && EmployeeId.ToString() != "")
                Id = $"and Employees.Id={EmployeeId}";

            //        var sqlQuery = _context.SqlDataTable($@"SELECT distinct dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance,
            //SUM (dbo.VacationTypes.NumberDays) as ToltalVactions,
            //(dbo.Employees.VacationBalance - SUM (dbo.VacationTypes.NumberDays)) as Toltal
            //            FROM     dbo.Employees INNER JOIN
            //              dbo.RequestVacations ON dbo.Employees.Id = dbo.RequestVacations.EmployeeId INNER JOIN
            //              dbo.VacationPlans ON dbo.RequestVacations.Id = dbo.VacationPlans.RequestVacationId INNER JOIN
            //              dbo.VacationTypes ON dbo.RequestVacations.VacationTypeId = dbo.VacationTypes.Id
            //  where dbo.VacationPlans.VacationDate between 
            //                '" + FromDate.ToString("yyyy/MM/dd") + "' and '" + ToDate.ToString("yyyy/MM/dd") + "'"+
            //                " and dbo.RequestVacations.Approved='True'" +
            //                $"{Id} GROUP BY dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance");


            string sqlQuery =$@"SELECT distinct dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance,
				SUM (dbo.VacationTypes.NumberDays) as ToltalVactions,
				(dbo.Employees.VacationBalance - SUM (dbo.VacationTypes.NumberDays)) as Toltal
                FROM     dbo.Employees INNER JOIN
                  dbo.RequestVacations ON dbo.Employees.Id = dbo.RequestVacations.EmployeeId INNER JOIN
                  dbo.VacationPlans ON dbo.RequestVacations.Id = dbo.VacationPlans.RequestVacationId INNER JOIN
                  dbo.VacationTypes ON dbo.RequestVacations.VacationTypeId = dbo.VacationTypes.Id
				  where dbo.VacationPlans.VacationDate between 
                    '" + FromDate.ToString("yyyy/MM/dd") + "' and '" + ToDate.ToString("yyyy/MM/dd") + "'" +
                   " and dbo.RequestVacations.Approved='True'" +
                   $"{Id} GROUP BY dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance";

            var SpGetData = _context.SpGetReportsVactionPlans.FromSqlRaw(sqlQuery).ToList();

            ViewBag.Employee = _context.Employees.OrderBy(x => x.Name).ToList();

                        return View("ViewReportVationPlan2", SpGetData);

        }
    }
}
