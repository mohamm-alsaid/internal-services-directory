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
        Task<List<Service>> GetAllServices();
        Task<Service> GetServiceByIdAsync(int id);
        Task<Community> GetCommunityByIdAsync(int id);
        Task<Language> GetLanguageByIdAsync(int id);
        Task<Location> GetLocationByIdAsync(int id);
    }

    public class ServiceContextManager : IServiceContextManager
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServiceContextManager(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllServices()
        {
            return await _context.Service
                .OrderBy(s => s.ServiceName)
                .Include(s => s.Contact)
                .Include(s => s.Department)
                .Include(s => s.Division)
                .Include(s => s.Program)
                .Include(s => s.ServiceCommunityAssociation)
                .Include(s => s.ServiceLanguageAssociation)
                .Include(s => s.ServiceLocationAssociation)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Service> GetServiceByIdAsync(int id)
        {
            var service = await _context.Service
                .Where(s => s.ServiceId == id)
                .Include(s => s.Contact)
                .Include(s => s.Department)
                .Include(s => s.Division)
                .Include(s => s.Program)
                .Include(s => s.ServiceCommunityAssociation)
                .Include(s => s.ServiceLanguageAssociation)
                .Include(s => s.ServiceLocationAssociation)
                .AsNoTracking()
                .SingleOrDefaultAsync();


            //Add logic here to populate relation lists
            //   NVM - can't be here, as we don't have access to DTOs
            //   so we have nowhere to put Communities. No access to either
            //   CommunityV1DTO or to ServiceV1DTO's List<CommunityV1DTO>

            return service;
        }

        public async Task<Community> GetCommunityByIdAsync(int id)
        {
            return await _context.Community
                .Where(c => c.CommunityId == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<Language> GetLanguageByIdAsync(int id)
        {
            return await _context.Language
                .Where(l => l.LanguageId == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        public async Task<Location> GetLocationByIdAsync(int id)
        {

            return await _context.Location
                .Where(l => l.LocationId == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
    }
}
