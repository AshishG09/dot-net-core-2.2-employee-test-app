using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public string Index()
        {
            return _employeeRepository.GetEmployee(1).Name;
        }

        public ViewResult Details()
        {
            Employee employee = _employeeRepository.GetEmployee(1);
            return View(employee);
        }

        public ViewResult DetailsAbsolutePath()
        {
            Employee employee = _employeeRepository.GetEmployee(1);
            return View("/MyViews/Test.cshtml");
        }

        public ViewResult DetailsRelativePath()
        {
            Employee employee = _employeeRepository.GetEmployee(1);
            return View("../../MyViews/Test");
        }
    }
}
