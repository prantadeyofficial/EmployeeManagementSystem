using System.Diagnostics;
using System.Linq;
using BeforeJOBexam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeforeJOBexam.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeDBContext employeeDB;

        public HomeController(EmployeeDBContext employeeDB)
        {
            this.employeeDB = employeeDB;
        }

        // ==========================================
        // 1. READ (List all employees)
        // ==========================================
        public IActionResult Index()
        {
            var employees = employeeDB.Employees.ToList();
            return View(employees);
        }

        // ==========================================
        // 2. CREATE (Get & Post)
        // ==========================================
        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeDB.Employees.Add(employee);
                employeeDB.SaveChanges();

                // ADD THIS LINE:
                TempData["SuccessMessage"] = "Employee created successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // ==========================================
        // 3. UPDATE / EDIT (Get & Post)
        // ==========================================
        // GET: Home/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = employeeDB.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employeeDB.Employees.Update(employee);
                    employeeDB.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // ==========================================
        // 4. DELETE (Get Confirmation & Post)
        // ==========================================
        // GET: Home/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = employeeDB.Employees.FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // ==========================================
        // 5. DETAILS (Read a single record)
        // ==========================================
        // GET: Home/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the employee matching the requested ID
            var employee = employeeDB.Employees.FirstOrDefault(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = employeeDB.Employees.Find(id);
            if (employee != null)
            {
                employeeDB.Employees.Remove(employee);
                employeeDB.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // HELPER methods & Default Actions
        // ==========================================
        private bool EmployeeExists(int id)
        {
            return employeeDB.Employees.Any(e => e.Id == id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}