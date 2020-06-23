using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public IHostingEnvironment HostingEnvironment { get; }

        public HomeController(IEmployeeRepository employeeRepository,
                              IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            HostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetEmployees();
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            /*
             * Below exception was only for demonstration of Part 60 --handling exceptions globally
             */
            //throw new Exception();

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            homeDetailsViewModel.Employee = employee;
            homeDetailsViewModel.PageTitle = "Employee details";
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photo != null)
                {
                    uniqueFileName = ProcessUploadedFile(model);
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Department = model.Department,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);

                return RedirectToAction("details", new { id = newEmployee.Id });
            }

            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath,
                Id = employee.Id
                
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employeeEditViewModel)
        {
            if (ModelState.IsValid)
            {
                if (employeeEditViewModel != null)
                {
                    Employee employee = _employeeRepository.GetEmployee(employeeEditViewModel.Id);
                    employee.Department = employeeEditViewModel.Department;
                    employee.Email = employeeEditViewModel.Email;
                    employee.Name = employeeEditViewModel.Name;
                    if(employeeEditViewModel.Photo != null)
                    {
                        employee.PhotoPath = ProcessUploadedFile(employeeEditViewModel);
                        if(employeeEditViewModel.ExistingPhotoPath != null)
                        {
                            string filePath = Path.Combine(HostingEnvironment.WebRootPath, "images", employeeEditViewModel.ExistingPhotoPath);
                            System.IO.File.Delete(filePath);
                        }
                    }
                    else
                    {
                        employee.PhotoPath = employeeEditViewModel.ExistingPhotoPath;
                    }
                     
                    _employeeRepository.Update(employee);

                    return RedirectToAction("Details", employee);
                }

            }

            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null && model.Photo.FileName != null)
            {
                uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                var filePath = Path.Combine(HostingEnvironment.WebRootPath, "images");
                using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
