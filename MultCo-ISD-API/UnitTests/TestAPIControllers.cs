using System;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.Controllers;
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

        [TestMethod]
        public void TestGetPrograms()
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
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.SaveChanges();

                    ProgramsController controller = new ProgramsController(context);
                    var actionResult = controller.GetProgram().Result;

                    Assert.IsNotNull(actionResult);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /* 
         * Test Methods for Services Controller
         *  NOTE: Cannot test failure w/out refactor to view a return code from Controller return type, this might make these tests not very helpful if we can't look at return codes
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
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    ServicesController controller = new ServicesController(context);
                    var actionResult = controller.GetService().Result;

                    Assert.IsNotNull(actionResult);
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

                    ServicesController controller = new ServicesController(context);
                    var actionResult = controller.GetService(2).Result;

                    Assert.IsNotNull(actionResult);
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

                    var serv = new Service();
                    ServicesController controller = new ServicesController(context);
                    var actionResult = controller.PostService(serv).Result;

                    Assert.IsNotNull(actionResult);
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
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    var serv = new Service { ServiceName = "Panda Adoption Society" };
                    var controller = new ServicesController(context);
                    var actionResult = controller.PutService(1, serv).Result;

                    Assert.IsNotNull(actionResult);
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

                    var controller = new ServicesController(context);
                    var actionResult = controller.DeleteService(2).Result;

                    Assert.IsNotNull(actionResult);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
