using MultCo_ISD_API.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.ControllerContexts
{
    public interface IServiceContextManager
    {
        Task<Service> GetByIdAsync(int id);
    }

    public class ServiceContextManager : IServiceContextManager
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServiceContextManager(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            var service = await _context.Service
                .Where(s => s.ServiceId == id)
                .Include(s => s.Contact)
                .Include(s => s.Department)
                .Include(s => s.Division)
                .Include(s => s.Program)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            //Add logic here to populate relation lists
            //   NVM - can't be here, as we don't have access to DTOs
            //   so we have nowhere to put Communities. No access to either
            //   CommunityV1DTO or to ServiceV1DTO's List<CommunityV1DTO>

            return service;
        }
    }
}
