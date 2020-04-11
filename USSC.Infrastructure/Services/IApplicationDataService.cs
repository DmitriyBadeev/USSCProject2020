using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Services
{
    public interface IApplicationDataService
    {
        public IUnitOfWork Data { get; }
    }
}