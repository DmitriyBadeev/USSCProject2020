using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using USSC.Infrastructure.Models;
using USSC.Services.OrganizationServices;
using USSC.Web.ViewModels;
using USSC.Web.ViewModels.Employee;

namespace USSC.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IOrganizationService _organizationService;

        public EmployeeController(IEmployeeService employeeService, IOrganizationService organizationService)
        {
            _employeeService = employeeService;
            _organizationService = organizationService;
        }

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
            var position = _employeeService.GetPositionById(model.SelectedPositionId);
            var organization = _organizationService.GetById(model.OrganizationId);

            var entity = new Employee()
            {
                Name = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Email = model.Email,
                BirthDay = model.BirthDay,
                MedicalPolicy = model.MedicalPolicy,
                PassportNumber = model.PassportNumber,
                PassportSeries = model.PassportSeries,
                Phone = model.Phone,
                Position = position,
                Organization = organization
            };

            _employeeService.Add(entity);

            return RedirectToAction("Details", "Organization", new { organizationId = model.OrganizationId });
        }
    }
}