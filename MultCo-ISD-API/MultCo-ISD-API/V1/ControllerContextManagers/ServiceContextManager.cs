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
        Task<List<Service>> GetAllServices(int pageSize, int pageIndex);
        Task<Service> GetServiceByIdAsync(int id);
        //currently nullable because relational table ids that aren't the primary key are nullable
        Task<List<Service>> GetServicesFromIdList(List<int?> ids);
        Task<List<Service>> GetServicesFromProgramId(int? ids);
        Task<List<Service>> GetServicesFromDepartmentId(int? id);
        Task<List<Service>> GetServicesFromDivisionId(int? id);
        Task<List<Service>> GetServicesFromDivisionAndDepartmentId(int? divId, int? deptId);
        Task<Community> GetCommunityByIdAsync(int id);
        Task<Community> GetCommunityByNameAsync(string name);
        Task<List<ServiceCommunityAssociation>> GetServiceCommunityAssociationsByCommunityIdAsync(int id);
        Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdAsync(int id);
        Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdListAsync(List<int?> ids);
        Task<List<ServiceLocationAssociation>> GetServiceLocationAssociationsByLocationIdListAsync(List<int?> ids);
        Task<Language> GetLanguageByIdAsync(int id);
        Task<List<Language>> GetLanguagesByNameListAsync(List<string> langs);
        Task<Location> GetLocationByIdAsync(int id);
        Task<List<Location>> GetLocationsByBuildingId(string buildingid);

    }

    public class ServiceContextManager : IServiceContextManager
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServiceContextManager(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllServices(int pageSize, int pageIndex)
        {
            return await _context.Service
                .OrderBy(s => s.ServiceName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
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

            return service;
        }

        public async Task<List<Service>> GetServicesFromIdList(List<int?> ids)
        {
            return await _context.Service
                    .Where(s => ids.Contains(s.ServiceId))
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

        public async Task<Community> GetCommunityByIdAsync(int id)
        {
            return await _context.Community
                .Where(c => c.CommunityId == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<List<Service>> GetServicesFromProgramId(int? id)
        {
            return await _context.Service
                .Where(s => s.ProgramId == id)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Service>> GetServicesFromDepartmentId(int? id)
        {
            return await _context.Service
                .Where(s => s.DepartmentId == id)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Service>> GetServicesFromDivisionId(int? id)
        {
            return await _context.Service
                .Where(s => s.DivisionId == id)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Service>> GetServicesFromDivisionAndDepartmentId(int? divId, int? deptId)
        {
            return await _context.Service
                .Where(s => s.DepartmentId == deptId && s.DivisionId == divId)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Community> GetCommunityByNameAsync(string name)
        {
            return await _context.Community
                .Where(c => c.CommunityName.ToLower() == name.ToLower())
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<List<ServiceCommunityAssociation>> GetServiceCommunityAssociationsByCommunityIdAsync(int id)
        {
            return await _context.ServiceCommunityAssociation
                .Where(sca => sca.CommunityId == id)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdAsync(int id)
        {
            return await _context.ServiceLanguageAssociation
                .Where(sla => sla.LanguageId == id)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdListAsync(List<int?> ids)
        {
            return await _context.ServiceLanguageAssociation
                .Where(sla => ids.Contains(sla.LanguageId))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<ServiceLocationAssociation>> GetServiceLocationAssociationsByLocationIdListAsync(List<int?> ids)
        {
            return await _context.ServiceLocationAssociation
                .Where(sla => ids.Contains(sla.LocationId))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Language> GetLanguageByIdAsync(int id)
        {
            return await _context.Language
                .Where(l => l.LanguageId == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        
        public async Task<List<Language>> GetLanguagesByNameListAsync(List<string> langs)
        {
            langs.ForEach(l => l.ToLower());

            return await _context.Language
                .Where(l => langs.Contains(l.LanguageName.ToLower()))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {

            return await _context.Location
                .Where(l => l.LocationId == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        
        public async Task<List<Location>> GetLocationsByBuildingId(string buildingid)
        {
            return await _context.Location
                .Where(l => l.BuildingId == buildingid)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
