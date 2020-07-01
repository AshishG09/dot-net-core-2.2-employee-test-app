using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [EmailAddress]
        [ValidEmailDomain(allowedDomain: "guhe.com", ErrorMessage = "Email domain must be guhe.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Confirmation password do not match the actual password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
