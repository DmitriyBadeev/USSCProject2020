using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;

namespace USSC.Services.OrganizationServices
{
    public class PositionService : IPositionService
    {
        private readonly IApplicationDataService _applicationData;
        private readonly ILogger<OrganizationService> _logger;

        public PositionService(IApplicationDataService applicationData, ILogger<OrganizationService> logger)
        {
            _applicationData = applicationData;
            _logger = logger;
        }

        public IEnumerable<Position> GetAll()
        {
            return _applicationData.Data.Positions.GetAll();
        }

        public void Delete(int positionId)
        {
            var position = GetById(positionId);

            _logger.LogInformation($"Removing position {position.Name}");

            _applicationData.Data.Positions.Remove(position);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Position {position.Name} has deleted successfully");
        }

        public Position GetById(int positionId)
        {
            return _applicationData.Data.Organizations
                .GetPositions()
                .FirstOrDefault(p => p.Id == positionId);
        }

        public void Edit(int positionId, string name)
        {
            _logger.LogInformation($"Editing position {name}");

            var position = GetById(positionId);

            position.Name = name;

            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Position {name} has edited successfully");
        }

        public void Add(string name)
        {
            _logger.LogInformation($"Adding position {name}");

            var position = new Position()
            {
                Name = name
            };

            _applicationData.Data.Positions.Add(position);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Position {name} has added successfully");
        }
    }
}
