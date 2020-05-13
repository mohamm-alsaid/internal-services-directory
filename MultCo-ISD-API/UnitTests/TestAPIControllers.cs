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
                        context.Service.Add(new Service());
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
                        context.Service.Add(new Service());
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
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
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
        public void TestDeleteService()
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
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    var controller = new ServiceController(context);
                    var actionResult = controller.DeleteService(2).Result;

                    Assert.IsNotNull(actionResult);
                    
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
                    Service serv = new Service();
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
                    Service serv1 = new Service();
                    context.Service.Add(serv1);
                    context.SaveChanges();

                    Service serv2 = new Service();
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
                    Service serv1 = new Service();
                    context.Service.Add(serv1);
                    context.SaveChanges();

                    Service serv2 = new Service();
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
                    Service serv2 = new Service();
                    serv2.ServiceId = 2;
                    Service serv3 = new Service();
                    serv3.ServiceId = 3;
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
                    Service serv2 = new Service();
                    serv2.ServiceId = 2;
                    Service serv3 = new Service();
                    serv3.ServiceId = 3;
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
    }
}
