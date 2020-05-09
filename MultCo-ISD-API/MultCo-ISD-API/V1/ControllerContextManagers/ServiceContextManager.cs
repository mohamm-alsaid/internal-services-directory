using MultCo_ISD_API.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.ControllerContexts
{
    public interface IServiceContextManager
    {
        Task<List<Service>> GetAllServices();
        Task<Service> GetServiceByIdAsync(int id);
        //currently nullable because relational table ids that aren't the primary key are nullable
        Task<List<Service>> GetServicesFromIdList(List<int?> ids);
        Task<Community> GetCommunityByIdAsync(int id);
        Task<Community> GetCommunityByNameAsync(string name);
        Task<List<ServiceCommunityAssociation>> GetServiceCommunityAssociationsByCommunityIdAsync(int id);
        Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdAsync(int id);
        Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdListAsync(List<int?> ids);
        Task<Language> GetLanguageByIdAsync(int id);
        Task<List<Language>> GetLanguagesByNameListAsync(List<string> langs);
        Task<Location> GetLocationByIdAsync(int id);

        Task PostAsync(ServiceV1DTO serviceDTO);
    }

    public class ServiceContextManager : IServiceContextManager
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServiceContextManager(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        public async Task PostAsync(ServiceV1DTO serviceDTO)
        {


            //check if contact exists in database
            var contact = await _context.Contact
                .Where(c => c.ContactId == serviceDTO.ContactId)
                .SingleOrDefaultAsync() ?? null;
            if (contact == null && serviceDTO.ContactId != null)
            {
                var c = new Contact();
                _context.Contact.Add(c);
                _context.Entry(c).CurrentValues.SetValues(serviceDTO.ContactDTO);
                await _context.SaveChangesAsync();
                serviceDTO.ContactId = c.ContactId;
            }

            var department = await _context.Department
                .Where(d => d.DepartmentId == serviceDTO.DepartmentId)
                .SingleOrDefaultAsync() ?? null;
            if (department == null && serviceDTO.DepartmentId != null)
            {
                var d = new Department();
                _context.Department.Add(d);
                _context.Entry(d).CurrentValues.SetValues(serviceDTO.DepartmentDTO);
                await _context.SaveChangesAsync();
                serviceDTO.DepartmentId = d.DepartmentId;
            }

            var division = await _context.Division
                .Where(d => d.DivisionId == serviceDTO.DivisionId)
                .SingleOrDefaultAsync() ?? null;
            if (division == null && serviceDTO.DivisionId != null)
            {
                var d = new Division();
                _context.Division.Add(d);
                _context.Entry(d).CurrentValues.SetValues(serviceDTO.DivisionDTO);
                await _context.SaveChangesAsync();
                serviceDTO.DivisionId = d.DivisionId;
            }

            var program = await _context.Program
                .Where(p => p.ProgramId == serviceDTO.ProgramId)
                .SingleOrDefaultAsync() ?? null;
            if (program == null && serviceDTO.ProgramId != null)
            {
                var p = new Program();
                _context.Program.Add(p);
                _context.Entry(p).CurrentValues.SetValues(serviceDTO.ProgramDTO);
                await _context.SaveChangesAsync();
                serviceDTO.ProgramId = p.ProgramId;
            }

            var service = new Service();

            //add service to context
            _context.Service.Add(service);
            _context.Entry(service).CurrentValues.SetValues(serviceDTO);

            await _context.SaveChangesAsync();
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
    }
}
