using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employee;

        public HomeController(IEmployeeRepository employee)
        {
            this.employee = employee;
        }

        public ViewResult Index()
        {
            var model = employee.GetAllEmployees();
            return View(model);
        }
        public ViewResult Details()
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee.GetEmployee(1),
                PageTitle = "Employee Details"
            };
            ViewBag.PageTitle = "Employee Details";

            return View(homeDetailsViewModel);
        }
    }
}
