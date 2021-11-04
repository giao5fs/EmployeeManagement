using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            logger.LogInformation("Hello, This is the index from home");
            var model = employeeRepository.GetAllEmployees();
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            Employee employee = employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                return View("EmployeeNotFound",id);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            ViewBag.PageTitle = "Employee Details";

            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel emp)
        {
            if (ModelState.IsValid)
            {
                string uniqueFile = ProcessUploadImages(emp);

                Employee newEmployee = new Employee
                {
                    Name = emp.Name,
                    Email = emp.Email,
                    Department = emp.Department,
                    PhotoPath = uniqueFile
                };
                employeeRepository.Add(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }
            return View(emp);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee emp = employeeRepository.GetEmployee(id);
            EmployeeEditViewModel newEmp = new EmployeeEditViewModel
            {
                Id = emp.Id,
                Name = emp.Name,
                Department = emp.Department,
                Email = emp.Email,
                ExistPhoto = emp.PhotoPath
            };
            return View(newEmp);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel emp)
        {
            if (ModelState.IsValid)
            {
                Employee employee = employeeRepository.GetEmployee(emp.Id);
                employee.Name = emp.Name;
                employee.Email = emp.Email;
                employee.Department = emp.Department;
                if (emp.Photos != null)
                {
                    if (emp.ExistPhoto != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", emp.ExistPhoto);
                        System.IO.File.Delete(filePath);
                    }
                }
                employee.PhotoPath = ProcessUploadImages(emp);
                employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        private string ProcessUploadImages(EmployeeCreateViewModel emp)
        {
            string uniqueFile = null;
            if (emp.Photos != null)
            {
                foreach (var Photo in emp.Photos)
                {
                    string path = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFile = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                    string filePath = Path.Combine(path, uniqueFile);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Photo.CopyTo(fileStream);
                    }

                }
            }

            return uniqueFile;
        }

        
    }
}
