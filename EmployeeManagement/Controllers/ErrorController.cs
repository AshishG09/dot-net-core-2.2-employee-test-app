using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult ErrorPageHandler(int statusCode)
        {
            var originalURLRequest = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404: ViewBag.ErrorMsg = "Sorry, the requested resource was not found!";
                    ViewBag.Path = originalURLRequest.OriginalPath;
                    ViewBag.QS = originalURLRequest.OriginalQueryString;
                    break;
            }
            return View("NotFound");
        }
    }
}
