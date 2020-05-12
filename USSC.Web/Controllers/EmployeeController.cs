using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using USSC.Infrastructure.Models;
using USSC.Services.OrganizationServices;
using USSC.Services.UserServices.Interfaces;
using USSC.Web.ViewModels;
using USSC.Web.ViewModels.Employee;
using USSC.Web.ViewModels.Organization;

namespace USSC.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IOrganizationService _organizationService;
        private readonly IPositionService _positionService;
        private readonly IUserDataService _userData;

        public EmployeeController(IEmployeeService employeeService, IOrganizationService organizationService,
            IPositionService positionService, IUserDataService userData)
        {
            _employeeService = employeeService;
            _organizationService = organizationService;
            _positionService = positionService;
            _userData = userData;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            var organization = employee.Organization;

            if (organization.UserId == null)
            {
                throw new NullReferenceException("User hasn't in database");
            }

            var user = await _userData.GetUserData((int)organization.UserId);

            var organizationModel = new OrganizationViewModel()
            {
                Id = organization.Id,
                Name = organization.Name,
                INN = organization.INN,
                OGRN = organization.OGRN,
                Email = user.Email,
                Phone = user.Phone,
                UserName = $"{user.LastName} {user.Name} {user.Patronymic}"
            };

            var employeeModel = new EmployeeViewModel()
            {
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Email = employee.Email,
                Phone = employee.Phone,
                BirthDay = employee.BirthDay,
                MedicalPolicy = employee.MedicalPolicy,
                PassportNumber = employee.PassportNumber,
                PassportSeries = employee.PassportSeries,
                Position = employee.Position?.Name,
                Organization = organizationModel
            };

            return View(employeeModel);
        }

        [HttpGet]
        public IActionResult AddEmployee(int id)
        {
            var positions = _positionService.GetAll()
                .Select(p => new Select()
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            var model = new PostEmployeeViewModel()
            {
                OrganizationId = id,
                Positions = positions,
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

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var employee = _employeeService.GetById(id);

            var positions = _positionService.GetAll()
                .Select(p => new Select()
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            var employeeModel = new PostEmployeeViewModel()
            {
                OrganizationId = employee.Id,
                LastName = employee.LastName,
                Name = employee.Name,
                Email = employee.Email,
                Patronymic = employee.Patronymic,
                MedicalPolicy = employee.MedicalPolicy,
                Phone = employee.Phone,
                PassportNumber = employee.PassportNumber,
                PassportSeries = employee.PassportSeries,
                SelectedPositionId = employee.Position.Id,
                Positions = positions,
                BirthDay = employee.BirthDay
            };

            return View(employeeModel);
        }

        [HttpPost]
        public IActionResult EditEmployee(int id, PostEmployeeViewModel employeeModel)
        {
            _employeeService.Edit
                (
                    id, 
                    employeeModel.LastName,
                    employeeModel.Name,
                    employeeModel.Patronymic,
                    employeeModel.Phone,
                    employeeModel.BirthDay,
                    employeeModel.MedicalPolicy,
                    employeeModel.PassportNumber,
                    employeeModel.PassportSeries,
                    employeeModel.OrganizationId,
                    employeeModel.SelectedPositionId
                );

            return RedirectToAction("Details", "Organization", new { organizationId = employeeModel.OrganizationId });
        }

        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _employeeService.GetById(id);

            _employeeService.Remove(id);

            return RedirectToAction("Details", "Organization", new { organizationId = employee.OrganizationId });
        }
    }
}