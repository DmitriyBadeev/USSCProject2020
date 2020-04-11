using System;
using System.Collections.Generic;
using System.Text;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Services
{
    public class ApplicationDataService : IApplicationDataService
    {
        public ApplicationDataService(AppDbContext context)
        {
            Data = new UnitOfWork(context);
        }

        public IUnitOfWork Data { get; }
    }
}
