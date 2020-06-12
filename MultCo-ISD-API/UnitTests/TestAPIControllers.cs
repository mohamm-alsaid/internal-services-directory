using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.Controllers;
using MultCo_ISD_API.V1.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Sqlite;

namespace UnitTests
{
    [TestClass]
    public class TestAPIControllers
    {
        [TestMethod]
        public void TestInMemoryDBConnection()
        {
            // Build in-mem db
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            //Assert.AreEqual(connection.State, ConnectionState.Open);

            try
            {
                // In-mem db exists only while connection is open
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // Create schema in db
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.SaveChanges();
                }
            }
            finally
            {
                connection.Close();
            }
        }
        /* 
         * Test Methods for Services Controller
         */
        [TestMethod]
        public void TestGetServices()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    for(int i = 0; i < 100; i++)
                    {
                        context.Service.Add(new Service { Active = true });
                    }
                    context.SaveChanges();

                    // pageSize and pageIndex working correctly
                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.GetServices(20, 0).Result;
                    var result = actionResult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(20, services.Count());

                    // pageSize is too big for all services
                    actionResult = controller.GetServices(110, 0).Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(100, services.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestNotFoundServices()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    for (int i = 0; i < 100; i++)
                    {
                        context.Service.Add(new Service { Active = true });
                    }
                    context.SaveChanges();

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.GetServices(1, -1).Result;
                    var result = actionResult as NotFoundObjectResult;

                    // pageIndex doesn't exist
                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("Invalid page index or page size.", result.Value);

                    actionResult = controller.GetServices(100, 1).Result;
                    result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No services were found with the given page information.", result.Value);

                    actionResult = controller.GetServices(-1, 1).Result;
                    result = actionResult as NotFoundObjectResult;

                    // pageSize is negative
                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("Invalid page index or page size.", result.Value);

                    actionResult = controller.GetServices(-1, -1).Result;
                    result = actionResult as NotFoundObjectResult;

                    // pageSize and pageIndex are negative
                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("Invalid page index or page size.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Service.Add(new Service { Active = true });
                    context.Service.Add(new Service { Active = true });
                    context.Service.Add(new Service { Active = true });
                    context.SaveChanges();

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.GetService(2).Result;
                    var result = actionResult as OkObjectResult;
                    var service = result.Value as ServiceV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(service);
                    Assert.AreEqual(2, service.ServiceId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        
        [TestMethod]
        public void TestPostService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    var serv = new Service().ToServiceV1DTO();
                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(serv).Result;
                    var result = actionResult as NoContentResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(204, result.StatusCode);

                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPostServiceOneToOneWithExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    // Create a contact and insert into DB
                    Contact c = new Contact { ContactName = "contact1", EmailAddress = "contact1@email.com" };
                    context.Contact.Add(c);
                    context.SaveChanges();

                    // Create a department and insert into DB
                    Department dep = new Department { DepartmentCode = 1117, DepartmentName = "department1" };
                    context.Department.Add(dep);
                    context.SaveChanges();

                    // Create a division and insert into DB
                    Division div = new Division { DivisionCode = 1117, DivisionName = "division1" };
                    context.Division.Add(div);
                    context.SaveChanges();

                    // Create a program and insert into DB
                    Program p = new Program { SponsorName = "sponsor1", ProgramOfferNumber = "1234" };
                    context.Program.Add(p);
                    context.SaveChanges();

                    // Create a service and give it one-to-one relations to existing items
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.ProgramDTO = p.ToProgramV1DTO();
                    sDTO.DepartmentDTO = dep.ToDepartmentV1DTO();
                    sDTO.DivisionDTO = div.ToDivisionV1DTO();
                    sDTO.ContactDTO = c.ToContactV1DTO();

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Program.ToList().Count(), 1);
                    Assert.AreEqual(context.Department.ToList().Count(), 1);
                    Assert.AreEqual(context.Division.ToList().Count(), 1);
                    Assert.AreEqual(context.Contact.ToList().Count(), 1);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPostServiceOneToOneNonExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    Assert.AreEqual(context.Program.ToList().Count(), 0);
                    Assert.AreEqual(context.Department.ToList().Count(), 0);
                    Assert.AreEqual(context.Division.ToList().Count(), 0);
                    Assert.AreEqual(context.Contact.ToList().Count(), 0);

                    Contact c = new Contact { ContactName = "contact1", EmailAddress = "contact1@email.com", PhoneNumber = "555-555-5555" };
                    Department dep = new Department { DepartmentCode = 1117, DepartmentName = "department1" };
                    Division div = new Division { DivisionCode = 1117, DivisionName = "division1" };
                    Program p = new Program { SponsorName = "sponsor1", ProgramOfferNumber = "1234", OfferType = "offertype", ProgramName = "program1"};

                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.ProgramDTO = p.ToProgramV1DTO();
                    sDTO.DepartmentDTO = dep.ToDepartmentV1DTO();
                    sDTO.DivisionDTO = div.ToDivisionV1DTO();
                    sDTO.ContactDTO = c.ToContactV1DTO();

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Program.ToList().Count(), 1);
                    Assert.AreEqual(context.Department.ToList().Count(), 1);
                    Assert.AreEqual(context.Division.ToList().Count(), 1);
                    Assert.AreEqual(context.Contact.ToList().Count(), 1);

                    var contact = context.Contact.First();
                    Assert.AreEqual(contact.ContactName, c.ContactName);
                    Assert.AreEqual(contact.EmailAddress, c.EmailAddress);
                    Assert.AreEqual(contact.PhoneNumber, c.PhoneNumber);

                    var department = context.Department.First();
                    Assert.AreEqual(department.DepartmentCode, dep.DepartmentCode);
                    Assert.AreEqual(department.DepartmentName, dep.DepartmentName);

                    var division = context.Division.First();
                    Assert.AreEqual(division.DivisionCode, div.DivisionCode);
                    Assert.AreEqual(division.DivisionName, div.DivisionName);

                    var program = context.Program.First();
                    Assert.AreEqual(program.SponsorName, p.SponsorName);
                    Assert.AreEqual(program.ProgramOfferNumber, p.ProgramOfferNumber);
                    Assert.AreEqual(program.OfferType, p.OfferType);
                    Assert.AreEqual(program.ProgramName, p.ProgramName);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPostServiceManyToManyWithExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {                   
                    context.Database.EnsureCreated();

                    //Create a community and insert
                    Community c = new Community { CommunityName = "community_indb", CommunityDescription = "a community" };
                    context.Community.Add(c);
                    context.SaveChanges();

                    //Create a language and insert
                    Language lang = new Language { LanguageName = "esperanto" };
                    context.Language.Add(lang);
                    context.SaveChanges();

                    //Create a location and insert
                    LocationType loctype = new LocationType { LocationTypeName = "location_type" };
                    Location loc = new Location { LocationName = "location_one", LocationType = loctype};
                    context.LocationType.Add(loctype);
                    context.SaveChanges();
                    loc.LocationTypeId = loctype.LocationTypeId;
                    context.Location.Add(loc);
                    context.SaveChanges();
                    
                    //Create a service and give it the many-to-many relationships
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.CommunityDTOs.Add(c.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc.ToLocationV1DTO());


                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Community.ToList().Count(), 1);
                    Assert.AreEqual(context.ServiceCommunityAssociation.ToList().Count(), 1);

                    Assert.AreEqual(context.Language.ToList().Count(), 1);
                    Assert.AreEqual(context.ServiceLanguageAssociation.ToList().Count(), 1);

                    Assert.AreEqual(context.Location.ToList().Count(), 1);
                    Assert.AreEqual(context.ServiceLocationAssociation.ToList().Count(), 1);
                    Assert.AreEqual(context.LocationType.ToList().Count(), 1);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TesetPostServiceManyToManyNonExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;
                

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    
                    Community c = new Community { CommunityName = "community_indb", CommunityDescription = "a community" };
                    Language lang = new Language { LanguageName = "esperanto" };
                    LocationType loctype = new LocationType { LocationTypeName = "location_type" };
                    Location loc = new Location { LocationName = "location_one", LocationType = loctype };

                    //Create a service and give it the many-to-many relationships
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.CommunityDTOs.Add(c.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc.ToLocationV1DTO());

                    c = new Community { CommunityName = "community_indb_2", CommunityDescription = "another community" };
                    lang = new Language { LanguageName = "cobol" };
                    loctype = new LocationType { LocationTypeName = "different_location_type" };
                    loc = new Location { LocationName = "location_two", LocationType = loctype };

                    sDTO.CommunityDTOs.Add(c.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc.ToLocationV1DTO());

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Community.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceCommunityAssociation.ToList().Count(), 2);

                    Assert.AreEqual(context.Language.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLanguageAssociation.ToList().Count(), 2);

                    Assert.AreEqual(context.Location.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLocationAssociation.ToList().Count(), 2);
                    Assert.AreEqual(context.LocationType.ToList().Count(), 2);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service serv = new Service();
                    context.Service.Add(serv);
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    serv.ServiceName = "Panda Adoption Society";
                    var controller = new ServiceController(context);
                    var actionResult = controller.PutService(1, serv.ToServiceV1DTO()).Result;
                    var result = actionResult as NoContentResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(204, result.StatusCode);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutServiceOneToOneWithExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    //BUILDING THE SERVICE

                    // Create a contact and insert into DB
                    Contact c = new Contact { ContactName = "contact1", EmailAddress = "contact1@email.com" };
                    context.Contact.Add(c);
                    context.SaveChanges();

                    // Create a department and insert into DB
                    Department dep = new Department { DepartmentCode = 1117, DepartmentName = "department1" };
                    context.Department.Add(dep);
                    context.SaveChanges();

                    // Create a division and insert into DB
                    Division div = new Division { DivisionCode = 1117, DivisionName = "division1" };
                    context.Division.Add(div);
                    context.SaveChanges();

                    // Create a program and insert into DB
                    Program p = new Program { SponsorName = "sponsor1", ProgramOfferNumber = "1234" };
                    context.Program.Add(p);
                    context.SaveChanges();

                    // Create a service and give it one-to-one relations to existing items
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.ProgramDTO = p.ToProgramV1DTO();
                    sDTO.DepartmentDTO = dep.ToDepartmentV1DTO();
                    sDTO.DivisionDTO = div.ToDivisionV1DTO();
                    sDTO.ContactDTO = c.ToContactV1DTO();

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    //Testing PUT

                    //Test whether we can link service to existing items in DB
                    Contact c2 = new Contact { ContactName = "contact_two", EmailAddress = "contact_two@gmail.com" };
                    context.Contact.Add(c2);
                    context.SaveChanges();

                    // Create a department and insert into DB
                    Department dep2 = new Department { DepartmentCode = 9999, DepartmentName = "departmenttwo" };
                    context.Department.Add(dep2);
                    context.SaveChanges();

                    // Create a division and insert into DB
                    Division div2 = new Division { DivisionCode = 5555, DivisionName = "divisiontwo" };
                    context.Division.Add(div2);
                    context.SaveChanges();

                    // Create a program and insert into DB
                    Program p2 = new Program { SponsorName = "second sponsor", ProgramOfferNumber = "8765" };
                    context.Program.Add(p2);
                    context.SaveChanges();

                    sDTO = context.Service.First().ToServiceV1DTO();
                    sDTO.ContactDTO = c2.ToContactV1DTO();
                    sDTO.ContactId = null;
                    sDTO.DepartmentDTO = dep2.ToDepartmentV1DTO();
                    sDTO.DepartmentId = null;
                    sDTO.DivisionDTO = div2.ToDivisionV1DTO();
                    sDTO.DivisionId = null;
                    sDTO.ProgramDTO = p2.ToProgramV1DTO();
                    sDTO.ProgramId = null;

                    actionResult = controller.PutService(sDTO.ServiceId, sDTO).Result;
                    context.SaveChanges();
                    sDTO = context.Service.First().ToServiceV1DTO();
                    Assert.AreEqual(sDTO.ContactDTO.ContactId, 2);
                    Assert.AreEqual(sDTO.DepartmentDTO.DepartmentId, 2);
                    Assert.AreEqual(sDTO.DivisionDTO.DivisionId, 2);
                    Assert.AreEqual(sDTO.ProgramDTO.ProgramId, 2);

                    //Test whether we can create new items in DB through PUT

                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutServiceOneToOneNonExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    //BUILD THE SERVICE

                    // Create a contact and insert into DB
                    Contact c = new Contact { ContactName = "contact1", EmailAddress = "contact1@email.com" };
                    context.Contact.Add(c);
                    context.SaveChanges();

                    // Create a department and insert into DB
                    Department dep = new Department { DepartmentCode = 1117, DepartmentName = "department1" };
                    context.Department.Add(dep);
                    context.SaveChanges();

                    // Create a division and insert into DB
                    Division div = new Division { DivisionCode = 1117, DivisionName = "division1" };
                    context.Division.Add(div);
                    context.SaveChanges();

                    // Create a program and insert into DB
                    Program p = new Program { SponsorName = "sponsor1", ProgramOfferNumber = "1234" };
                    context.Program.Add(p);
                    context.SaveChanges();

                    // Create a service and give it one-to-one relations to existing items
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.ProgramDTO = p.ToProgramV1DTO();
                    sDTO.DepartmentDTO = dep.ToDepartmentV1DTO();
                    sDTO.DivisionDTO = div.ToDivisionV1DTO();
                    sDTO.ContactDTO = c.ToContactV1DTO();

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    //Testing PUT

                    Contact c3 = new Contact { ContactName = "contactthree", EmailAddress = "contact_three@gmail.com" };
                    Department dep3 = new Department { DepartmentCode = 11, DepartmentName = "d3" };
                    Division div3 = new Division { DivisionCode = 66, DivisionName = "div3" };
                    Program p3 = new Program { SponsorName = "third sponsor", ProgramOfferNumber = "88552" };

                    sDTO = context.Service.First().ToServiceV1DTO();
                    sDTO.ContactDTO = c3.ToContactV1DTO();
                    sDTO.ContactId = null;
                    sDTO.DepartmentDTO = dep3.ToDepartmentV1DTO();
                    sDTO.DepartmentId = null;
                    sDTO.DivisionDTO = div3.ToDivisionV1DTO();
                    sDTO.DivisionId = null;
                    sDTO.ProgramDTO = p3.ToProgramV1DTO();
                    sDTO.ProgramId = null;

                    actionResult = controller.PutService(sDTO.ServiceId, sDTO).Result;
                    context.SaveChanges();
                    sDTO = context.Service.First().ToServiceV1DTO();
                    Assert.AreEqual(sDTO.ContactDTO.ContactId, 2);
                    Assert.AreEqual(sDTO.DepartmentDTO.DepartmentId, 2);
                    Assert.AreEqual(sDTO.DivisionDTO.DivisionId, 2);
                    Assert.AreEqual(sDTO.ProgramDTO.ProgramId, 2);

                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutServiceManyToManyWithExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    //BUILDING THE SERVICE

                    //Create a community and insert
                    Community c = new Community { CommunityName = "community_indb", CommunityDescription = "a community" };
                    context.Community.Add(c);
                    context.SaveChanges();

                    //Create a language and insert
                    Language lang = new Language { LanguageName = "esperanto" };
                    context.Language.Add(lang);
                    context.SaveChanges();

                    //Create a location and insert
                    LocationType loctype = new LocationType { LocationTypeName = "location_type" };
                    Location loc = new Location { LocationName = "location_one", LocationType = loctype };
                    context.LocationType.Add(loctype);
                    context.SaveChanges();
                    loc.LocationTypeId = loctype.LocationTypeId;
                    context.Location.Add(loc);
                    context.SaveChanges();

                    //Create a service and give it the many-to-many relationships
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.CommunityDTOs.Add(c.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc.ToLocationV1DTO());

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    //DONE BUILDING SERVICE






                    //Testing PUT

                    //Create a community and insert
                    Community c2 = new Community { CommunityName = "c2", CommunityDescription = "another community" };
                    context.Community.Add(c2);
                    context.SaveChanges();

                    //Create a language and insert
                    Language lang2 = new Language { LanguageName = "haskell" };
                    context.Language.Add(lang2);
                    context.SaveChanges();

                    //Create a location and insert
                    LocationType loctype2 = new LocationType { LocationTypeName = "another location type" };
                    Location loc2 = new Location { LocationName = "LOCATION2 ", LocationType = loctype2 };
                    context.LocationType.Add(loctype2);
                    context.SaveChanges();
                    loc2.LocationTypeId = loctype2.LocationTypeId;
                    context.Location.Add(loc2);
                    context.SaveChanges();


                    //Adding entries to each relation
                    actionResult = controller.GetService(1).Result;
                    var result = actionResult as OkObjectResult;
                    sDTO = result.Value as ServiceV1DTO;
                    sDTO.CommunityDTOs.Add(c2.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang2.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc2.ToLocationV1DTO());

                    actionResult = controller.PutService(sDTO.ServiceId, sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Community.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceCommunityAssociation.ToList().Count(), 2);

                    Assert.AreEqual(context.Language.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLanguageAssociation.ToList().Count(), 2);

                    Assert.AreEqual(context.Location.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLocationAssociation.ToList().Count(), 2);
                    Assert.AreEqual(context.LocationType.ToList().Count(), 2);

                    //Removing entries from each relation
                    sDTO = context.Service.First().ToServiceV1DTO();
                    sDTO.CommunityDTOs = new List<CommunityV1DTO>();
                    sDTO.LanguageDTOs = new List<LanguageV1DTO>();
                    sDTO.LocationDTOs = new List<LocationV1DTO>();

                    actionResult = controller.PutService(sDTO.ServiceId, sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Community.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceCommunityAssociation.ToList().Count(), 0);

                    Assert.AreEqual(context.Language.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLanguageAssociation.ToList().Count(), 0);

                    Assert.AreEqual(context.Location.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLocationAssociation.ToList().Count(), 0);
                    Assert.AreEqual(context.LocationType.ToList().Count(), 2);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutServiceManyToManyNonExistingItems()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    //BUILDING THE SERVICE

                    //Create a community and insert
                    Community c = new Community { CommunityName = "community_indb", CommunityDescription = "a community" };
                    context.Community.Add(c);
                    context.SaveChanges();

                    //Create a language and insert
                    Language lang = new Language { LanguageName = "esperanto" };
                    context.Language.Add(lang);
                    context.SaveChanges();

                    //Create a location and insert
                    LocationType loctype = new LocationType { LocationTypeName = "location_type" };
                    Location loc = new Location { LocationName = "location_one", LocationType = loctype };
                    context.LocationType.Add(loctype);
                    context.SaveChanges();
                    loc.LocationTypeId = loctype.LocationTypeId;
                    context.Location.Add(loc);
                    context.SaveChanges();

                    //Create a service and give it the many-to-many relationships
                    ServiceV1DTO sDTO = new Service().ToServiceV1DTO();
                    sDTO.Active = true;
                    sDTO.CommunityDTOs.Add(c.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc.ToLocationV1DTO());

                    ServiceController controller = new ServiceController(context);
                    var actionResult = controller.PostService(sDTO).Result;
                    context.SaveChanges();

                    //DONE BUILDING SERVICE






                    //Testing PUT

                    Community c2 = new Community { CommunityName = "c2", CommunityDescription = "another community" };
                    Language lang2 = new Language { LanguageName = "haskell" };
                    LocationType loctype2 = new LocationType { LocationTypeName = "another location type" };
                    Location loc2 = new Location { LocationName = "LOCATION2 ", LocationType = loctype2 };



                    //Adding entries to each relation
                    actionResult = controller.GetService(1).Result;
                    var result = actionResult as OkObjectResult;
                    sDTO = result.Value as ServiceV1DTO;
                    sDTO.CommunityDTOs.Add(c2.ToCommunityV1DTO());
                    sDTO.LanguageDTOs.Add(lang2.ToLanguageV1DTO());
                    sDTO.LocationDTOs.Add(loc2.ToLocationV1DTO());

                    actionResult = controller.PutService(sDTO.ServiceId, sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Community.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceCommunityAssociation.ToList().Count(), 2);

                    Assert.AreEqual(context.Language.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLanguageAssociation.ToList().Count(), 2);

                    Assert.AreEqual(context.Location.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLocationAssociation.ToList().Count(), 2);
                    Assert.AreEqual(context.LocationType.ToList().Count(), 2);

                    //Removing entries from each relation
                    sDTO = context.Service.First().ToServiceV1DTO();
                    sDTO.CommunityDTOs = new List<CommunityV1DTO>();
                    sDTO.LanguageDTOs = new List<LanguageV1DTO>();
                    sDTO.LocationDTOs = new List<LocationV1DTO>();

                    actionResult = controller.PutService(sDTO.ServiceId, sDTO).Result;
                    context.SaveChanges();

                    Assert.AreEqual(context.Community.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceCommunityAssociation.ToList().Count(), 0);

                    Assert.AreEqual(context.Language.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLanguageAssociation.ToList().Count(), 0);

                    Assert.AreEqual(context.Location.ToList().Count(), 2);
                    Assert.AreEqual(context.ServiceLocationAssociation.ToList().Count(), 0);
                    Assert.AreEqual(context.LocationType.ToList().Count(), 2);
                }
            }
            finally
            {
                connection.Close();
            }
        }



        [TestMethod]
        public void TestRelationalTables()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service serv = new Service { Active = true };
                    serv.ServiceName = "service number one";
                    context.Service.Add(serv);
                    context.SaveChanges();
                    Community comm1 = new Community();
                    Community comm2 = new Community();
                    Community comm3 = new Community();
                    context.Community.Add(comm1);
                    context.Community.Add(comm2);
                    context.Community.Add(comm3);
                    context.SaveChanges();
                    ServiceCommunityAssociation sca = new ServiceCommunityAssociation();
                    sca.ServiceId = 1;
                    sca.CommunityId = 1;
                    ServiceCommunityAssociation sca1 = new ServiceCommunityAssociation();
                    sca1.ServiceId = 1;
                    sca1.CommunityId = 2;
                    context.ServiceCommunityAssociation.Add(sca);
                    context.ServiceCommunityAssociation.Add(sca1);
                    context.SaveChanges();
                    var controller = new ServiceController(context);
                    var output = controller.GetService(1);
                    var actionResult = output.Result;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByCommunity()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    //create a service
                    Service serv1 = new Service { Active = true };
                    context.Service.Add(serv1);
                    context.SaveChanges();

                    Service serv2 = new Service { Active = true };
                    context.Service.Add(serv2);

                    //create some communities
                    Community comm1 = new Community();
                    Community comm2 = new Community();
                    Community comm3 = new Community();
                    comm1.CommunityName = "comm1";
                    comm2.CommunityName = "comm2";
                    comm3.CommunityName = "comm3";
                    context.Community.Add(comm1);
                    context.Community.Add(comm2);
                    context.Community.Add(comm3);
                    context.SaveChanges();

                    //create some associations
                    ServiceCommunityAssociation sca = new ServiceCommunityAssociation();
                    sca.ServiceId = 1;
                    sca.CommunityId = 1;
                    ServiceCommunityAssociation sca1 = new ServiceCommunityAssociation();
                    sca1.ServiceId = 1;
                    sca1.CommunityId = 2;
                    ServiceCommunityAssociation sca2 = new ServiceCommunityAssociation();
                    sca2.ServiceId = 2;
                    sca2.CommunityId = 1;
                    context.ServiceCommunityAssociation.Add(sca);
                    context.ServiceCommunityAssociation.Add(sca1);
                    context.ServiceCommunityAssociation.Add(sca2);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Community("comm1");
                    var actionresult = output.Result;
                    var result = actionresult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(2, services.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByCommunityNotFound()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    //create a service
                    Service serv1 = new Service { Active = true };
                    context.Service.Add(serv1);
                    context.SaveChanges();
                    Service serv2 = new Service { Active = true };
                    context.Service.Add(serv2);

                    //create some communities
                    Community comm1 = new Community();
                    Community comm2 = new Community();
                    Community comm3 = new Community();
                    comm1.CommunityName = "comm1";
                    comm2.CommunityName = "comm2";
                    comm3.CommunityName = "comm3";
                    context.Community.Add(comm1);
                    context.Community.Add(comm2);
                    context.Community.Add(comm3);
                    context.SaveChanges();

                    //create some associations
                    ServiceCommunityAssociation sca = new ServiceCommunityAssociation();
                    sca.ServiceId = 1;
                    sca.CommunityId = 1;
                    ServiceCommunityAssociation sca1 = new ServiceCommunityAssociation();
                    sca1.ServiceId = 1;
                    sca1.CommunityId = 2;
                    ServiceCommunityAssociation sca2 = new ServiceCommunityAssociation();
                    sca2.ServiceId = 2;
                    sca2.CommunityId = 1;
                    context.ServiceCommunityAssociation.Add(sca);
                    context.ServiceCommunityAssociation.Add(sca1);
                    context.ServiceCommunityAssociation.Add(sca2);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Community("comm3");
                    var actionresult = output.Result;
                    var result = actionresult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);

                    output = controller.Community("comm4");
                    actionresult = output.Result;
                    result = actionresult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByLanguage()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service serv1 = new Service();
                    serv1.ServiceId = 1;
                    serv1.Active = true;
                    Service serv2 = new Service();
                    serv2.ServiceId = 2;
                    serv2.Active = true;
                    Service serv3 = new Service();
                    serv3.ServiceId = 3;
                    serv3.Active = true;
                    context.Service.Add(serv1);
                    context.Service.Add(serv2);
                    context.Service.Add(serv3);
                    context.SaveChanges();

                    Language lang1 = new Language();
                    lang1.LanguageId = 1;
                    lang1.LanguageName = "english";
                    Language lang2 = new Language();
                    lang2.LanguageId = 2;
                    lang2.LanguageName = "spanish";
                    Language lang3 = new Language();
                    lang3.LanguageId = 3;
                    lang3.LanguageName = "german";
                    context.Language.Add(lang1);
                    context.Language.Add(lang2);
                    context.Language.Add(lang3);
                    context.SaveChanges();

                    ServiceLanguageAssociation sla1 = new ServiceLanguageAssociation();
                    sla1.ServiceId = 1;
                    sla1.LanguageId = 1;
                    ServiceLanguageAssociation sla2 = new ServiceLanguageAssociation();
                    sla2.ServiceId = 2;
                    sla2.LanguageId = 1;
                    ServiceLanguageAssociation sla3 = new ServiceLanguageAssociation();
                    sla3.ServiceId = 3;
                    sla3.LanguageId = 1;
                    ServiceLanguageAssociation sla4 = new ServiceLanguageAssociation();
                    sla4.ServiceId = 2;
                    sla4.LanguageId = 2;
                    ServiceLanguageAssociation sla5 = new ServiceLanguageAssociation();
                    sla5.ServiceId = 3;
                    sla5.LanguageId = 2;
                    ServiceLanguageAssociation sla6 = new ServiceLanguageAssociation();
                    sla6.ServiceId = 3;
                    sla6.LanguageId = 3;
                    context.ServiceLanguageAssociation.Add(sla1);
                    context.ServiceLanguageAssociation.Add(sla2);
                    context.ServiceLanguageAssociation.Add(sla3);
                    context.ServiceLanguageAssociation.Add(sla4);
                    context.ServiceLanguageAssociation.Add(sla5);
                    context.ServiceLanguageAssociation.Add(sla6);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Language("spanish,german");
                    var actionResult = output.Result;
                    var result = actionResult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(2, services.Count());

                    output = controller.Language("english");
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(3, services.Count());

                    output = controller.Language("german");
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(1, services.Count());

                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByLanguageNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service serv1 = new Service();
                    serv1.ServiceId = 1;
                    serv1.Active = true;
                    Service serv2 = new Service();
                    serv2.ServiceId = 2;
                    serv2.Active = true;
                    Service serv3 = new Service();
                    serv3.ServiceId = 3;
                    serv3.Active = true;
                    context.Service.Add(serv1);
                    context.Service.Add(serv2);
                    context.Service.Add(serv3);
                    context.SaveChanges();

                    Language lang1 = new Language();
                    lang1.LanguageId = 1;
                    lang1.LanguageName = "english";
                    Language lang2 = new Language();
                    lang2.LanguageId = 2;
                    lang2.LanguageName = "spanish";
                    Language lang3 = new Language();
                    lang3.LanguageId = 3;
                    lang3.LanguageName = "german";
                    context.Language.Add(lang1);
                    context.Language.Add(lang2);
                    context.Language.Add(lang3);
                    context.SaveChanges();

                    ServiceLanguageAssociation sla1 = new ServiceLanguageAssociation();
                    sla1.ServiceId = 1;
                    sla1.LanguageId = 1;
                    ServiceLanguageAssociation sla2 = new ServiceLanguageAssociation();
                    sla2.ServiceId = 2;
                    sla2.LanguageId = 1;
                    ServiceLanguageAssociation sla3 = new ServiceLanguageAssociation();
                    sla3.ServiceId = 3;
                    sla3.LanguageId = 1;
                    ServiceLanguageAssociation sla4 = new ServiceLanguageAssociation();
                    sla4.ServiceId = 2;
                    sla4.LanguageId = 2;
                    ServiceLanguageAssociation sla5 = new ServiceLanguageAssociation();
                    sla5.ServiceId = 3;
                    sla5.LanguageId = 2;
                    ServiceLanguageAssociation sla6 = new ServiceLanguageAssociation();
                    sla6.ServiceId = 3;
                    sla6.LanguageId = 3;
                    context.ServiceLanguageAssociation.Add(sla1);
                    context.ServiceLanguageAssociation.Add(sla2);
                    context.ServiceLanguageAssociation.Add(sla3);
                    context.ServiceLanguageAssociation.Add(sla4);
                    context.ServiceLanguageAssociation.Add(sla5);
                    context.ServiceLanguageAssociation.Add(sla6);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Language("russian");
                    var actionResult = output.Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No languages from given names found.", result.Value);

                    Language lang4 = new Language();
                    lang4.LanguageName = "russian";
                    lang4.LanguageId = 4;
                    context.Language.Add(lang4);
                    context.SaveChanges();

                    output = controller.Language("russian");
                    actionResult = output.Result;
                    result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No relationships found for given language(s).", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByBuildingId()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                // In-mem db exists only while connection is open
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // Create schema in db
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    LocationType lt1 = new LocationType { LocationTypeName = "lt1" };
                    context.LocationType.Add(lt1);
                    context.SaveChanges();

                    Location l1 = new Location();
                    l1.BuildingId = "b1";
                    l1.LocationId = 1;
                    l1.LocationType = lt1;
                    Location l2 = new Location();
                    l2.BuildingId = "b2";
                    l2.LocationId = 2;
                    l2.LocationType = lt1;
                    Location l3 = new Location();
                    l3.BuildingId = "b2";
                    l3.LocationId = 3;
                    l3.LocationType = lt1;
                    context.Location.Add(l1);
                    context.Location.Add(l2);
                    context.Location.Add(l3);
                    context.SaveChanges();

                    Service s1 = new Service { ServiceId = 1, Active = true };
                    Service s2 = new Service { ServiceId = 2, Active = true };
                    Service s3 = new Service { ServiceId = 3, Active = true };
                    Service s4 = new Service { ServiceId = 4, Active = true };
                    Service s5 = new Service { ServiceId = 5, Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.Service.Add(s4);
                    context.Service.Add(s5);
                    context.SaveChanges();

                    ServiceLocationAssociation sla1 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 1, LocationId = 1, ServiceId = 1 };
                    ServiceLocationAssociation sla2 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 2, LocationId = 2, ServiceId = 2 };
                    ServiceLocationAssociation sla3 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 3, LocationId = 3, ServiceId = 3 };
                    ServiceLocationAssociation sla4 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 4, LocationId = 2, ServiceId = 4 };
                    ServiceLocationAssociation sla5 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 5, LocationId = 2, ServiceId = 5 };
                    context.ServiceLocationAssociation.Add(sla1);
                    context.ServiceLocationAssociation.Add(sla2);
                    context.ServiceLocationAssociation.Add(sla3);
                    context.ServiceLocationAssociation.Add(sla4);
                    context.ServiceLocationAssociation.Add(sla5);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.BuildingId("b2");
                    var actionResult = output.Result;
                    var result = actionResult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(4, services.Count());

                    output = controller.BuildingId("b1");
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(1, services.Count());

                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByBuildingIdNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                // In-mem db exists only while connection is open
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // Create schema in db
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();

                    LocationType lt1 = new LocationType { LocationTypeName = "lt1" };
                    context.LocationType.Add(lt1);
                    context.SaveChanges();

                    Location l1 = new Location();
                    l1.BuildingId = "b1";
                    l1.LocationId = 1;
                    l1.LocationType = lt1;
                    Location l2 = new Location();
                    l2.BuildingId = "b2";
                    l2.LocationId = 2;
                    l2.LocationType = lt1;
                    Location l3 = new Location();
                    l3.BuildingId = "b2";
                    l3.LocationId = 3;
                    l3.LocationType = lt1;
                    context.Location.Add(l1);
                    context.Location.Add(l2);
                    context.Location.Add(l3);
                    context.SaveChanges();

                    Service s1 = new Service { ServiceId = 1, Active = true };
                    Service s2 = new Service { ServiceId = 2, Active = true };
                    Service s3 = new Service { ServiceId = 3, Active = true };
                    Service s4 = new Service { ServiceId = 4, Active = true };
                    Service s5 = new Service { ServiceId = 5, Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.Service.Add(s4);
                    context.Service.Add(s5);
                    context.SaveChanges();

                    ServiceLocationAssociation sla1 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 1, LocationId = 1, ServiceId = 1 };
                    ServiceLocationAssociation sla2 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 2, LocationId = 2, ServiceId = 2 };
                    ServiceLocationAssociation sla3 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 3, LocationId = 3, ServiceId = 3 };
                    ServiceLocationAssociation sla4 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 4, LocationId = 2, ServiceId = 4 };
                    ServiceLocationAssociation sla5 = new ServiceLocationAssociation { ServiceLocationAssociation1 = 5, LocationId = 2, ServiceId = 5 };
                    context.ServiceLocationAssociation.Add(sla1);
                    context.ServiceLocationAssociation.Add(sla2);
                    context.ServiceLocationAssociation.Add(sla3);
                    context.ServiceLocationAssociation.Add(sla4);
                    context.ServiceLocationAssociation.Add(sla5);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.BuildingId("b3");
                    var actionResult = output.Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No locations found from given building id.", result.Value);

                    Location l4 = new Location();
                    l4.BuildingId = "b3";
                    l4.LocationId = 4;
                    l4.LocationType = lt1;
                    context.Location.Add(l4);
                    context.SaveChanges();

                    output = controller.BuildingId("b3");
                    actionResult = output.Result;
                    result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("Location(s) found have no relationships to any services.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByProgramId()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Program p1 = new Program { ProgramId = 1 };
                    Program p2 = new Program { ProgramId = 2 };
                    Program p3 = new Program { ProgramId = 3 };
                    context.Program.Add(p1);
                    context.Program.Add(p2);
                    context.Program.Add(p3);
                    context.SaveChanges();

                    Service s1 = new Service { ServiceId = 1, ProgramId = 1, Active = true };
                    Service s2 = new Service { ServiceId = 2, ProgramId = 2, Active = true };
                    Service s3 = new Service { ServiceId = 3, ProgramId = 3, Active = true };
                    Service s4 = new Service { ServiceId = 4, ProgramId = 2, Active = true };
                    Service s5 = new Service { ServiceId = 5, ProgramId = 3, Active = true };
                    Service s6 = new Service { ServiceId = 6, ProgramId = 3, Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.Service.Add(s4);
                    context.Service.Add(s5);
                    context.Service.Add(s6);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Program(3);
                    var actionResult = output.Result;
                    var result = actionResult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(3, services.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByProgramIdNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Program p1 = new Program { ProgramId = 1 };
                    Program p2 = new Program { ProgramId = 2 };
                    Program p3 = new Program { ProgramId = 3 };
                    context.Program.Add(p1);
                    context.Program.Add(p2);
                    context.Program.Add(p3);
                    context.SaveChanges();

                    Service s1 = new Service { ServiceId = 1, ProgramId = 1, Active = true };
                    Service s2 = new Service { ServiceId = 2, ProgramId = 2, Active = true };
                    Service s3 = new Service { ServiceId = 3, ProgramId = 3, Active = true };
                    Service s4 = new Service { ServiceId = 4, ProgramId = 2, Active = true };
                    Service s5 = new Service { ServiceId = 5, ProgramId = 3, Active = true };
                    Service s6 = new Service { ServiceId = 6, ProgramId = 3, Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.Service.Add(s4);
                    context.Service.Add(s5);
                    context.Service.Add(s6);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Program(4);
                    var actionResult = output.Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No services found with given program id.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByDivisionAndOrDepartmentId()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Division div1 = new Division { DivisionId = 1, DivisionCode = 1 };
                    Division div2 = new Division { DivisionId = 2, DivisionCode = 2 };
                    Division div3 = new Division { DivisionId = 3, DivisionCode = 3 };
                    context.Division.Add(div1);
                    context.Division.Add(div2);
                    context.Division.Add(div3);

                    Department dep1 = new Department { DepartmentId = 1, DepartmentCode = 1 };
                    Department dep2 = new Department { DepartmentId = 2, DepartmentCode = 2 };
                    Department dep3 = new Department { DepartmentId = 3, DepartmentCode = 3 };
                    context.Department.Add(dep1);
                    context.Department.Add(dep2);
                    context.Department.Add(dep3);

                    Service s1 = new Service { ServiceId = 1, DepartmentId = 1, DivisionId = 1, Active = true };
                    Service s2 = new Service { ServiceId = 2, DepartmentId = 2, DivisionId = 2, Active = true };
                    Service s3 = new Service { ServiceId = 3, DepartmentId = 3, DivisionId = 3, Active = true };
                    Service s4 = new Service { ServiceId = 4, DepartmentId = 3, DivisionId = 3, Active = true };
                    Service s5 = new Service { ServiceId = 5, DepartmentId = 2, DivisionId = 1, Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.Service.Add(s4);
                    context.Service.Add(s5);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.DepartmentAndOrDivisionId(3);
                    var actionResult = output.Result;
                    var result = actionResult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(2, services.Count());

                    output = controller.DepartmentAndOrDivisionId(null,2);
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(1, services.Count());

                    output = controller.DepartmentAndOrDivisionId(3, 3);
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(2, services.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByDivisionAndOrDepartmentIdNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Division div1 = new Division { DivisionId = 1, DivisionCode = 1 };
                    Division div2 = new Division { DivisionId = 2, DivisionCode = 2 };
                    Division div3 = new Division { DivisionId = 3, DivisionCode = 3 };
                    context.Division.Add(div1);
                    context.Division.Add(div2);
                    context.Division.Add(div3);

                    Department dep1 = new Department { DepartmentId = 1, DepartmentCode = 1 };
                    Department dep2 = new Department { DepartmentId = 2, DepartmentCode = 2 };
                    Department dep3 = new Department { DepartmentId = 3, DepartmentCode = 3 };
                    context.Department.Add(dep1);
                    context.Department.Add(dep2);
                    context.Department.Add(dep3);

                    Service s1 = new Service { ServiceId = 1, DepartmentId = 1, DivisionId = 1, Active = true };
                    Service s2 = new Service { ServiceId = 2, DepartmentId = 2, DivisionId = 2, Active = true };
                    Service s3 = new Service { ServiceId = 3, DepartmentId = 3, DivisionId = 3, Active = true };
                    Service s4 = new Service { ServiceId = 4, DepartmentId = 3, DivisionId = 3, Active = true };
                    Service s5 = new Service { ServiceId = 5, DepartmentId = 2, DivisionId = 1, Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.Service.Add(s4);
                    context.Service.Add(s5);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.DepartmentAndOrDivisionId(4);
                    var actionResult = output.Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No services found with valid arguments given.", result.Value);

                    output = controller.DepartmentAndOrDivisionId(null,4);
                    actionResult = output.Result;
                    result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No services found with valid arguments given.", result.Value);

                    output = controller.DepartmentAndOrDivisionId(null, null);
                    actionResult = output.Result;
                    var badRequestResult = actionResult as BadRequestObjectResult;

                    Assert.IsNotNull(badRequestResult);
                    Assert.AreEqual(400, badRequestResult.StatusCode);
                    Assert.AreEqual("No input given.", badRequestResult.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByName()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service s1 = new Service { ServiceName = "panda adoption service", Active = true };
                    Service s2 = new Service { ServiceName = "pangalactic gargle blaster service", Active = true };
                    Service s3 = new Service { ServiceName = "public health service", Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    for (int i = 0; i < 27; i++)
                    {
                        Service s = new Service { ServiceName = String.Format("loop iteration {0}", i), Active = true };
                        context.Service.Add(s);
                    }
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Name("pan");
                    var actionResult = output.Result;
                    var result = actionResult as OkObjectResult;
                    var services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(2, services.Count());

                    output = controller.Name("a", 20, 1);
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(10, services.Count());

                    //add some services that are expired
                    for (int i = 0; i < 10; i++)
                    {
                        Service s = new Service { ServiceName = String.Format("loop iteration {0}", i + 30), Active = false, ExpirationDate = DateTime.Now + TimeSpan.FromDays(1) };
                    }

                    //verify that they DON'T show up in the search
                    output = controller.Name("a", 20, 1);
                    actionResult = output.Result;
                    result = actionResult as OkObjectResult;
                    services = result.Value as IEnumerable<ServiceV1DTO>;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(services);
                    Assert.AreEqual(10, services.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetByNameNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service s1 = new Service { ServiceName = "panda adoption service", Active = true };
                    Service s2 = new Service { ServiceName = "pangalactic gargle blaster service", Active = true };
                    Service s3 = new Service { ServiceName = "public health service", Active = true };
                    context.Service.Add(s1);
                    context.Service.Add(s2);
                    context.Service.Add(s3);
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var output = controller.Name("glsadfngksdfjngksjd");
                    var actionResult = output.Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No services found with search query.", result.Value);

                    output = controller.Name("a", 20, 4);
                    actionResult = output.Result;
                    result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No services found with search query.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
