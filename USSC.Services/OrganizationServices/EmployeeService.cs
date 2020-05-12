using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;

namespace USSC.Services.OrganizationServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IApplicationDataService _applicationData;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IApplicationDataService applicationData, ILogger<EmployeeService> logger)
        {
            _applicationData = applicationData;
            _logger = logger;
        }

        public Position GetPositionById(int id)
        {
            return _applicationData.Data.Positions.Get(id);
        }

        public Employee GetById(int id)
        {
            var employee = _applicationData.Data.Employees.Get(id);

            var organization = _applicationData.Data.Organizations.Get(employee.OrganizationId);
            var position = _applicationData.Data.Positions.GetSingleOrDefault(p => p.EmployeeId == id);

            employee.Position = position;
            employee.Organization = organization;

            return employee;
        }

        public void Add(Employee employee)
        {
            _logger.LogInformation($"Adding employee with Id {employee.Id}");

            _applicationData.Data.Employees.Add(employee);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Employee with Id {employee.Id} added successfully");
        }

        public void Update(Employee employee)
        {
            _logger.LogInformation($"Updating employee with Id {employee.Id}");

            _applicationData.Data.Employees.Update(employee);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Employee with Id {employee.Id} updated successfully");
        }

        public void Remove(int id)
        {
            _logger.LogInformation($"Removing employee with Id {id}");

            var entity = _applicationData.Data.Employees.Get(id);
            _applicationData.Data.Employees.Remove(entity);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Employee with Id {id} removed successfully");
        }

        public void Edit(int id, string lastName, string name, string patronymic,
            string phone, DateTime birthDay, string medical, 
            string passNum, string passSer, int orgId, int posId)
        {
            var employee = GetById(id);

            _logger.LogInformation($"Editing employee with Id {employee.Id}");

            employee.LastName = lastName;
            employee.Name = name;
            employee.Patronymic = patronymic;
            employee.Phone = phone;
            employee.BirthDay = birthDay;
            employee.MedicalPolicy = medical;
            employee.PassportNumber = passNum;
            employee.PassportSeries = passSer;
            employee.Organization = _applicationData.Data.Organizations.Get(orgId);
            employee.Position = _applicationData.Data.Positions.Get(posId);

            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Employee with Id {employee.Id} edited successfully");
        }
    }
}
