using MultCo_ISD_API.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.V1.DTO;
using Microsoft.CodeAnalysis.Operations;


namespace MultCo_ISD_API.V1.ControllerContexts
{
	public interface IContextManager
	{
		Task<List<Service>> GetAllServices(int pageSize, int pageIndex);
		Task<Service> GetServiceByIdAsync(int id);
		//currently nullable because relational table ids that aren't the primary key are nullable
        Task<List<Service>> GetServicesFromIdList(List<int> ids);
		Task<List<Service>> GetServicesFromIdListPaginated(List<int> ids, int pageSize, int pageNum);
		Task<List<Service>> GetServicesFromProgramId(int ids);
		Task<List<Service>> GetServicesByName(string name, int pageSize, int pageNum);
		Task<List<Service>> GetServicesFromDepartmentId(int? id, int pageSize, int pageNum);
		Task<List<Service>> GetServicesFromDivisionId(int? id, int pageSize, int pageNum);
		Task<List<Service>> GetServicesFromDivisionAndDepartmentId(int? divId, int? deptId, int pageSize, int pageNum);
		Task<Community> GetCommunityByIdAsync(int id);
		Task<Division> GetDivisionByIdAsync(int id);
		Task<Community> GetCommunityByNameAsync(string name);
		Task<Language> GetLanguageByIdAsync(int id);
		Task<Department> GetDepartmentByIdAsync(int id);
		Task<Program> GetProgramByIdAsync(int id);
		Task<Location> GetLocationByIdAsync(int id);
		Task<LocationType> GetLocationTypeByIdAsync(int id);
		Task<Contact> GetContactByIdAsync(int id);
		Task<List<ServiceCommunityAssociation>> GetServiceCommunityAssociationsByCommunityIdAsync(int id);
		Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdAsync(int id);
		Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdListAsync(List<int> ids);
		Task<List<ServiceLocationAssociation>> GetServiceLocationAssociationsByLocationIdListAsync(List<int> ids);
		Task<List<Language>> GetLanguagesByNameListAsync(List<string> langs);
		
		Task PostAsync(ServiceV1DTO serviceDTO);
		Task PutAsync(ServiceV1DTO serviceDTO);
		Task<List<Location>> GetLocationsByBuildingId(string buildingid);
		Task PutLanguageAsync(LanguageV1DTO languageDTO);
		Task PutCommunityAsync(CommunityV1DTO communityDTO);
		Task PutDivisionAsync(DivisionV1DTO divisionDTO);
		Task PutDepartmentAsync(DepartmentV1DTO departmentDTO);
		Task PutProgramAsync(ProgramV1DTO programDTO);
		Task PutLocationAsync(LocationV1DTO locationDTO);
		Task PutLocationTypeAsync(LocationTypeV1DTO locationTypeDTO);
		Task PutContactAsync(ContactV1DTO contactDTO);
	}

	public class ContextManager : IContextManager
	{
		private readonly InternalServicesDirectoryV1Context _context;

		public ContextManager(InternalServicesDirectoryV1Context context)
		{
			_context = context;
		}
		public async Task PostAsync(ServiceV1DTO serviceDTO)
		{

			#region Handle one-to-one relations

			#region Check to see if attempting to add duplicate contact/department/division/program with different ID
			/* 
             * We are checking to see if the incoming Service is attempting to 
             * re-create an existing department/division using a different ID.
             * This should be corrected to reference the existing department/division.
             * We choose the passed DTO over the passed ID number, and change the ID
             * to match the deperment/division pulled from the database context by "...Code".
            */
			await EnsureNoContactDuplicate(serviceDTO);
			await EnsureNoDepartmentDuplicate(serviceDTO);
			await EnsureNoDivisionDuplicate(serviceDTO);
			await EnsureNoProgramDuplicate(serviceDTO);
			#endregion

			#region Check existence of one-to-one items
			await EnsureContactExistence(serviceDTO);
			await EnsureDepartmentExistence(serviceDTO);
			await EnsureDivisionExistence(serviceDTO);
			await EnsureProgramExistence(serviceDTO);
			#endregion

			#endregion
			var service = new Service();

			_context.Service.Add(service);
			_context.Entry(service).CurrentValues.SetValues(serviceDTO);

			await _context.SaveChangesAsync();

			#region Handle many-to-many relations
			foreach (var cDTO in serviceDTO.CommunityDTOs)
			{
				var community = await EnsureCommunityExistence(cDTO);
				var sca = new ServiceCommunityAssociation { ServiceId = service.ServiceId, CommunityId = community.CommunityId };
				_context.ServiceCommunityAssociation.Add(sca);
			}

			foreach (var lDTO in serviceDTO.LanguageDTOs)
			{
				var language = await EnsureLanguageExistence(lDTO);
				var sla = new ServiceLanguageAssociation { ServiceId = service.ServiceId, LanguageId = language.LanguageId };
				_context.ServiceLanguageAssociation.Add(sla);
			}

			foreach (var lDTO in serviceDTO.LocationDTOs)
			{
				var location = await EnsureLocationExistence(lDTO);
				var sla = new ServiceLocationAssociation { ServiceId = service.ServiceId, LocationId = location.LocationId };
				_context.ServiceLocationAssociation.Add(sla);
			}

			#endregion
			await _context.SaveChangesAsync();
		}

		public async Task PutAsync(ServiceV1DTO serviceDTO)
		{
			var service = await _context.Service
				.Where(s => s.ServiceId == serviceDTO.ServiceId)
				.SingleOrDefaultAsync();

			#region Handle one-to-one relations
			if (service.ContactId != serviceDTO.ContactId)
			{
				await EnsureNoContactDuplicate(serviceDTO);
				await EnsureContactExistence(serviceDTO);
			}
			if (service.DepartmentId != serviceDTO.DepartmentId)
			{
				await EnsureNoDepartmentDuplicate(serviceDTO);
				await EnsureDepartmentExistence(serviceDTO);
			}
			if (service.DivisionId != serviceDTO.DivisionId)
			{
				await EnsureNoDivisionDuplicate(serviceDTO);
				await EnsureDivisionExistence(serviceDTO);
			}
			if (service.ProgramId != serviceDTO.ProgramId)
			{
				await EnsureNoProgramDuplicate(serviceDTO);
				await EnsureProgramExistence(serviceDTO);
			}
			#endregion

			#region Handle many-to-many relations

			var communityIds = new HashSet<int>();
			var scas = await _context.ServiceCommunityAssociation
				.Where(sca => sca.ServiceId == serviceDTO.ServiceId)
				.ToListAsync();
			foreach (var cDTO in serviceDTO.CommunityDTOs)
			{
				//Build community if it needs to be built.
				var community = await EnsureCommunityExistence(cDTO);
				communityIds.Add(community.CommunityId);
			}
			foreach (var id in communityIds)
			{
				var sca = scas.FirstOrDefault(s => s.CommunityId == id);

				// If this is a new entry in SCA table, we need to build the entry and add to table.
				if (sca == null)
				{
					var scaEntry = new ServiceCommunityAssociation { ServiceId = serviceDTO.ServiceId, CommunityId = id };
					_context.ServiceCommunityAssociation.Add(scaEntry);
				}
				else
				{
					scas.Remove(sca);
				}
			}
			foreach (var sca in scas)
			{
				_context.ServiceCommunityAssociation.Remove(sca);
			}


			var languageIds = new HashSet<int>();
			var slas = await _context.ServiceLanguageAssociation
				.Where(sla => sla.ServiceId == serviceDTO.ServiceId)
				.ToListAsync();
			foreach (var lDTO in serviceDTO.LanguageDTOs)
			{
				var language = await EnsureLanguageExistence(lDTO);
				languageIds.Add(language.LanguageId);
			}
			foreach (var id in languageIds)
			{
				var sla = slas.FirstOrDefault(l => l.LanguageId == id);

				if (sla == null)
				{
					var slaEntry = new ServiceLanguageAssociation { ServiceId = serviceDTO.ServiceId, LanguageId = id };
					_context.ServiceLanguageAssociation.Add(slaEntry);
				}
				else
				{
					slas.Remove(sla);
				}
			}
			foreach (var sla in slas)
			{
				_context.ServiceLanguageAssociation.Remove(sla);
			}


			var locationIds = new HashSet<int>();
			var slocs = await _context.ServiceLocationAssociation
				.Where(sla => sla.ServiceId == serviceDTO.ServiceId)
				.ToListAsync();
			foreach (var lDTO in serviceDTO.LocationDTOs)
			{
				var location = await EnsureLocationExistence(lDTO);
				locationIds.Add(location.LocationId);
			}
			foreach (var id in locationIds)
			{
				var sla = slocs.FirstOrDefault(l => l.LocationId == id);

				if (sla == null)
				{
					var slaEntry = new ServiceLocationAssociation { ServiceId = service.ServiceId, LocationId = id };
					_context.ServiceLocationAssociation.Add(slaEntry);
				}
				else
				{
					slocs.Remove(sla);
				}
			}
			foreach (var sla in slocs)
			{
				_context.ServiceLocationAssociation.Remove(sla);
			}

			#endregion
			_context.Entry(service).CurrentValues.SetValues(serviceDTO);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Service>> GetAllServices(int pageSize, int pageIndex)
		{
			return await _context.Service
				.OrderBy(s => s.ServiceName)
				.Where(s => (s.Active == true || s.ExpirationDate > DateTime.Now))
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

		public async Task<List<Service>> GetServicesFromIdList(List<int> ids)
		{
			return await _context.Service
					.Where(s => ids.Contains(s.ServiceId) && (s.Active == true || s.ExpirationDate > DateTime.Now))
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

		public async Task<List<Service>> GetServicesFromIdListPaginated(List<int> ids, int pageSize, int pageNum)
		{
			var services = await _context.Service
					.Where(s => ids.Contains(s.ServiceId) && (s.Active == true || s.ExpirationDate > DateTime.Now))
					.Include(s => s.Contact)
					.Include(s => s.Department)
					.Include(s => s.Division)
					.Include(s => s.Program)
					.Include(s => s.ServiceCommunityAssociation)
					.Include(s => s.ServiceLanguageAssociation)
					.Include(s => s.ServiceLocationAssociation)
					.ToListAsync()
					.ConfigureAwait(false);


			return GetPageOfServicesFromServiceList(services, pageSize, pageNum);
		}

		public async Task<List<Service>> GetServicesByName(string name, int pageSize, int pageNum)
		{
			var services =  await _context.Service
					.Where(s => s.ServiceName.ToLower().Contains(name.ToLower()) && (s.Active == true || s.ExpirationDate > DateTime.Now))
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

			return GetPageOfServicesFromServiceList(services, pageSize, pageNum);

		}

		public async Task<Community> GetCommunityByIdAsync(int id)
		{
			return await _context.Community
				.Where(c => c.CommunityId == id)
				.AsNoTracking()
				.SingleOrDefaultAsync();
		}

		public async Task<List<Service>> GetServicesFromProgramId(int id)
		{
			return await _context.Service
				.Where(s => s.ProgramId == id)
				.ToListAsync()
				.ConfigureAwait(false);
		}

		public async Task<List<Service>> GetServicesFromDepartmentId(int? id, int pageSize, int pageNum)
		{
			var services = await _context.Service
				.Where(s => s.DepartmentId == id)
				.ToListAsync()
				.ConfigureAwait(false);

			return GetPageOfServicesFromServiceList(services, pageSize, pageNum);
		}

		public async Task<List<Service>> GetServicesFromDivisionId(int? id, int pageSize, int pageNum)
		{
			var services = await _context.Service
				.Where(s => s.DivisionId == id)
				.ToListAsync()
				.ConfigureAwait(false);

			return GetPageOfServicesFromServiceList(services, pageSize, pageNum);
		}

		public async Task<List<Service>> GetServicesFromDivisionAndDepartmentId(int? divId, int? deptId, int pageSize, int pageNum)
		{
			var services = await _context.Service
				.Where(s => s.DepartmentId == deptId && s.DivisionId == divId)
				.ToListAsync()
				.ConfigureAwait(false);

			return GetPageOfServicesFromServiceList(services, pageSize, pageNum);
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

		public async Task<List<ServiceLanguageAssociation>> GetServiceLanguageAssociationsByLanguageIdListAsync(List<int> ids)
		{
			return await _context.ServiceLanguageAssociation
				.Where(sla => ids.Contains(sla.LanguageId))
				.ToListAsync()
				.ConfigureAwait(false);
		}

		public async Task<List<ServiceLocationAssociation>> GetServiceLocationAssociationsByLocationIdListAsync(List<int> ids)
		{
			return await _context.ServiceLocationAssociation
				.Where(sla => ids.Contains(sla.LocationId))
				.ToListAsync()
				.ConfigureAwait(false);
		}

		public async Task<List<Language>> GetLanguagesByNameListAsync(List<string> langs)
		{
			langs.ForEach(l => l.ToLower());

			return await _context.Language
				.Where(l => langs.Contains(l.LanguageName.ToLower()))
				.ToListAsync()
				.ConfigureAwait(false);
		}

		#region Helpers
		private async Task EnsureNoContactDuplicate(ServiceV1DTO serviceDTO)
		{
			if (serviceDTO.ContactDTO == null)
			{
				return;
			}
			var contact = await _context.Contact
				.Where(c => c.EmailAddress == serviceDTO.ContactDTO.EmailAddress && c.ContactName == serviceDTO.ContactDTO.ContactName && serviceDTO.ContactDTO.PhoneNumber == c.PhoneNumber)
				.FirstOrDefaultAsync();
			if (contact != null)
			{
				serviceDTO.ContactId = contact.ContactId;
				serviceDTO.ContactDTO = null;
			}
		}

		private async Task EnsureContactExistence(ServiceV1DTO serviceDTO)
		{
			var contact = await _context.Contact
				.Where(c => c.ContactId == serviceDTO.ContactId)
				.SingleOrDefaultAsync();
			if (contact == null && serviceDTO.ContactDTO != null)
			{
				var c = new Contact();
				_context.Contact.Add(c);
				_context.Entry(c).CurrentValues.SetValues(serviceDTO.ContactDTO);
				await _context.SaveChangesAsync();
				serviceDTO.ContactId = c.ContactId;
			}
		}

		private async Task EnsureNoDepartmentDuplicate(ServiceV1DTO serviceDTO)
		{
			if (serviceDTO.DepartmentDTO == null)
			{
				return;
			}
			var department = await _context.Department
				.Where(d => d.DepartmentCode == serviceDTO.DepartmentDTO.DepartmentCode)
				.SingleOrDefaultAsync();
			if (department != null)
			{
				serviceDTO.DepartmentId = department.DepartmentId;
				serviceDTO.DepartmentDTO = null;
			}
		}

		private async Task EnsureDepartmentExistence(ServiceV1DTO serviceDTO)
		{
			var department = await _context.Department
				.Where(d => d.DepartmentId == serviceDTO.DepartmentId)
				.SingleOrDefaultAsync();
			if (department == null && serviceDTO.DepartmentDTO != null)
			{
				var d = new Department();
				_context.Department.Add(d);
				_context.Entry(d).CurrentValues.SetValues(serviceDTO.DepartmentDTO);
				await _context.SaveChangesAsync();
				serviceDTO.DepartmentId = d.DepartmentId;
			}
		}

		private async Task EnsureNoDivisionDuplicate(ServiceV1DTO serviceDTO)
		{
			if (serviceDTO.DivisionDTO == null)
			{
				return;
			}
			var division = await _context.Division
				.Where(d => d.DivisionCode == serviceDTO.DivisionDTO.DivisionCode)
				.SingleOrDefaultAsync();
			if (division != null)
			{
				serviceDTO.DivisionId = division.DivisionId;
				serviceDTO.DivisionDTO = null;
			}
		}

		private async Task EnsureDivisionExistence(ServiceV1DTO serviceDTO)
		{
			var division = await _context.Division
				.Where(d => d.DivisionId == serviceDTO.DivisionId)
				.SingleOrDefaultAsync();
			if (division == null && serviceDTO.DivisionDTO != null)
			{
				var d = new Division();
				_context.Division.Add(d);
				_context.Entry(d).CurrentValues.SetValues(serviceDTO.DivisionDTO);
				await _context.SaveChangesAsync();
				serviceDTO.DivisionId = d.DivisionId;
			}
		}

		private async Task EnsureNoProgramDuplicate(ServiceV1DTO serviceDTO)
		{
			if (serviceDTO.ProgramDTO == null)
			{
				return;
			}
			var program = await _context.Program
				.Where(p => p.ProgramOfferNumber == serviceDTO.ProgramDTO.ProgramOfferNumber)
				.FirstOrDefaultAsync();
			if (program != null)
			{
				serviceDTO.ProgramId = program.ProgramId;
				serviceDTO.ProgramDTO = null;
			}
		}

		private async Task EnsureProgramExistence(ServiceV1DTO serviceDTO)
		{
			var program = await _context.Program
				.Where(p => p.ProgramId == serviceDTO.ProgramId)
				.SingleOrDefaultAsync();
			if (program == null && serviceDTO.ProgramDTO != null)
			{
				var p = new Program();
				_context.Program.Add(p);
				_context.Entry(p).CurrentValues.SetValues(serviceDTO.ProgramDTO);
				await _context.SaveChangesAsync();
				serviceDTO.ProgramId = p.ProgramId;
			}
		}

		private async Task<Community> EnsureCommunityExistence(CommunityV1DTO cDTO)
		{
			var community = await _context.Community
				.Where(c => c.CommunityName == cDTO.CommunityName)
				.SingleOrDefaultAsync();
			if (community == null)
			{
				community = new Community();
				_context.Community.Add(community);
				_context.Entry(community).CurrentValues.SetValues(cDTO);
				await _context.SaveChangesAsync();
			}
			return community;
		}

		private async Task<Language> EnsureLanguageExistence(LanguageV1DTO lDTO)
		{
			var language = await _context.Language
				.Where(l => l.LanguageName == lDTO.LanguageName)
				.SingleOrDefaultAsync();
			if (language == null)
			{
				language = new Language();
				_context.Language.Add(language);
				_context.Entry(language).CurrentValues.SetValues(lDTO);
				await _context.SaveChangesAsync();
			}
			return language;
		}

		private async Task<Location> EnsureLocationExistence(LocationV1DTO lDTO)
		{
			var location = await _context.Location
					.Where(l => l.LocationName == lDTO.LocationName)
					.SingleOrDefaultAsync();
			if (location == null)
			{
				location = new Location();

				if (lDTO.LocationTypeDTO != null)
				{
					var ltype = await _context.LocationType
						.Where(l => l.LocationTypeId == location.LocationTypeId)
						.SingleOrDefaultAsync();
					if (ltype == null)
					{
						ltype = new LocationType();
						_context.LocationType.Add(ltype);
						_context.Entry(ltype).CurrentValues.SetValues(lDTO.LocationTypeDTO);
						await _context.SaveChangesAsync();
					}
					lDTO.LocationTypeId = ltype.LocationTypeId;
				}
				_context.Location.Add(location);
				_context.Entry(location).CurrentValues.SetValues(lDTO);
				await _context.SaveChangesAsync();
			}
			return location;
		}

		#endregion

		public async Task<List<Location>> GetLocationsByBuildingId(string buildingid)
		{
			return await _context.Location
				.Where(l => l.BuildingId == buildingid)
				.ToListAsync()
				.ConfigureAwait(false);
		}

		private List<Service> GetPageOfServicesFromServiceList(List<Service> services, int pageSize, int pageNum)
		{
			var size = services.Count();
			var lastPage = Math.Floor((double)(size / pageSize)); //want this to determine the index of the last page

			//if they request a page that's beyond the last page, return an empty list
			if (pageNum > lastPage)
			{
				return new List<Service>();
			}

			//if the whole list is less than a page size, return everything we have
			if (size < pageSize)
			{
				return services;
			}

			//if we're looking at the last page, but the last page isn't an entire page, return however many services there are left
			if ((services.Count() < pageSize * (pageNum + 1)) && pageNum == lastPage)
			{
				return services.GetRange((pageNum * pageSize), size - pageSize);
			}

			//otherwise, return the desired page
			return services.GetRange(pageSize * pageNum, pageSize);
		}
		public async Task PutLanguageAsync(LanguageV1DTO languageDTO)
		{
			var language = await _context.Language
				.Where(l => l.LanguageId == languageDTO.LanguageId)
				.SingleOrDefaultAsync();

			_context.Entry(language).CurrentValues.SetValues(languageDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutCommunityAsync(CommunityV1DTO communityDTO)
		{
			var community = await _context.Community
				.Where(c => c.CommunityId == communityDTO.CommunityId)
				.SingleOrDefaultAsync();

			_context.Entry(community).CurrentValues.SetValues(communityDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutDivisionAsync(DivisionV1DTO divisionDTO)
		{
			var division = await _context.Division
				.Where(d => d.DivisionId == divisionDTO.DivisionId)
				.SingleOrDefaultAsync();

			_context.Entry(division).CurrentValues.SetValues(divisionDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutDepartmentAsync(DepartmentV1DTO departmentDTO)
		{
			var department = await _context.Department
				.Where(d => d.DepartmentId == departmentDTO.DepartmentId)
				.SingleOrDefaultAsync();

			_context.Entry(department).CurrentValues.SetValues(departmentDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutProgramAsync(ProgramV1DTO programDTO)
		{
			var program = await _context.Program
				.Where(p => p.ProgramId == programDTO.ProgramId)
				.SingleOrDefaultAsync();

			_context.Entry(program).CurrentValues.SetValues(programDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutLocationAsync(LocationV1DTO locationDTO)
		{
			var location = await _context.Location
				.Where(l => l.LocationId == locationDTO.LocationId)
				.SingleOrDefaultAsync();

			_context.Entry(location).CurrentValues.SetValues(locationDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutLocationTypeAsync(LocationTypeV1DTO locationTypeDTO)
		{
			var locationType = await _context.LocationType
				.Where(l => l.LocationTypeId == locationTypeDTO.LocationTypeId)
				.SingleOrDefaultAsync();

			_context.Entry(locationType).CurrentValues.SetValues(locationTypeDTO);

			await _context.SaveChangesAsync();
		}
		public async Task PutContactAsync(ContactV1DTO contactDTO)
		{
			var contact = await _context.Contact
				.Where(c => c.ContactId == contactDTO.ContactId)
				.SingleOrDefaultAsync();

			_context.Entry(contact).CurrentValues.SetValues(contactDTO);

			await _context.SaveChangesAsync();
		}
		public async Task<Language> GetLanguageByIdAsync(int id)
		{
			return await _context.Language
				.Where(l => l.LanguageId == id)
				.AsNoTracking()
				.SingleOrDefaultAsync();
		}

		public async Task<Division> GetDivisionByIdAsync(int id)
		{
			return await _context.Division
				.Where(d => d.DivisionId == id)
				.AsNoTracking()
				.SingleOrDefaultAsync();
		}
		public async Task<Department> GetDepartmentByIdAsync(int id)
		{
			return await _context.Department
				.Where(d => d.DepartmentId == id)
				.AsNoTracking()
				.SingleOrDefaultAsync();
		}
		public async Task<Program> GetProgramByIdAsync(int id)
		{
			return await _context.Program
				.Where(p => p.ProgramId == id)
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
		public async Task<LocationType> GetLocationTypeByIdAsync(int id)
		{

			return await _context.LocationType
				.Where(l => l.LocationTypeId == id)
				.AsNoTracking()
				.SingleOrDefaultAsync();
		}
		public async Task<Contact> GetContactByIdAsync(int id)
		{

			return await _context.Contact
				.Where(c => c.ContactId == id)
				.AsNoTracking()
				.SingleOrDefaultAsync();
		}
	}
}
