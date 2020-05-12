using System;
using System.Collections.Generic;
using System.Text;
using USSC.Infrastructure.Models;

namespace USSC.Services.OrganizationServices
{
    public interface IPositionService
    {
        IEnumerable<Position> GetAll();
        void Delete(int positionId);
        Position GetById(int positionId);
        void Edit(int positionId, string name);
        void Add(string name);
    }
}
