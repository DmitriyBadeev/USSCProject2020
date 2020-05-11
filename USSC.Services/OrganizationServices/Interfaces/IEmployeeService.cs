using USSC.Infrastructure.Models;

namespace USSC.Services.OrganizationServices
{
    public interface IEmployeeService
    {
        Position GetPositionById(int id);
        Employee GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Remove(int id);
    }
}