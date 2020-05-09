using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using USSC.Web.ViewModels;
using USSC.Web.ViewModels.Employee;

namespace USSC.Web.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpGet]
        public IActionResult Index(int employeeId)
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddEmployee(int id)
        {
            var model = new PostEmployeeViewModel()
            {
                OrganizationId = id,
                Positions = new List<Select>(),
                BirthDay = new DateTime(2000, 1, 1)
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult AddEmployee(PostEmployeeViewModel model)
        {


            return RedirectToAction("Details", "Organization", new { organizationId = model.OrganizationId });
        }
    }
}