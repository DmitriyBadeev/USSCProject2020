using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        ISubsystemRepository Subsystems { get; }

        void SaveChanges();

        Task SaveChangesAsync();
    }
}