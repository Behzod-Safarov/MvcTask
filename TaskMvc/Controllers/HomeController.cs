using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskMvc.Models;
using TaskMvc.Services;

namespace TaskMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> ImportCSV(IFormFile file)
        {
            try
            {
                var (total, saved, skipped, skippedPayrolls) = await _employeeService.ImportEmployeesAsync(file);
                ViewBag.Message = $"📋 Total: {total} | ✅ Saved: {saved} | ⚠ Skipped: {skipped} (Existing: {string.Join(", ", skippedPayrolls)})";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Json(new { data = employees });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return PartialView("_EditEmployeeModal", employee);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Invalid data", errors });
            }

            var success = await _employeeService.UpdateEmployeeAsync(employee);
            return Json(new { success, message = success ? "Updated successfully" : "Employee not found" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
