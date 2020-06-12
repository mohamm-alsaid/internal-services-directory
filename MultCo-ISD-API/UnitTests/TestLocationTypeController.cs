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
    public class TestLocationTypeControllers
    {


        [TestMethod]
        public void TestGetLocationType()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // test a successful case
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.LocationType.Add(new LocationType { LocationTypeName = "location1" });
                    context.LocationType.Add(new LocationType { LocationTypeName = "location2" });
                    context.LocationType.Add(new LocationType { LocationTypeName = "location3" });
                    context.SaveChanges();

                    LocationTypeController controller = new LocationTypeController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var locationType = result.Value as LocationTypeV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(locationType);
                    Assert.AreEqual(2, locationType.LocationTypeId);
                    Assert.AreEqual("location2", locationType.LocationTypeName);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetLocationTypeNotFound()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // test a successful case
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.LocationType.Add(new LocationType { LocationTypeName = "location1" });
                    context.SaveChanges();

                    LocationTypeController controller = new LocationTypeController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No locationType from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutLocationType()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // test a successful case
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    var locationType = new LocationType { LocationTypeName = "locationType1" };
                    context.LocationType.Add(locationType);
                    context.LocationType.Add(new LocationType { LocationTypeName = "locationType2" });
                    context.LocationType.Add(new LocationType { LocationTypeName = "locationType3" });
                    context.SaveChanges();

                    LocationTypeController controller = new LocationTypeController(context);
                    locationType.LocationTypeId = 1;
                    locationType.LocationTypeName = "locationType4";
                    var actionResult = controller.PutLocationType(locationType.LocationTypeId, locationType.ToLocationTypeV1DTO()).Result;
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
        public void TestPutLocationTypeNotFound()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // test a successful case
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    var locationType = new LocationType { LocationTypeName = "locationType1" };
                    context.LocationType.Add(locationType);
                    context.LocationType.Add(new LocationType { LocationTypeName = "locationType2" });
                    context.LocationType.Add(new LocationType { LocationTypeName = "locationType3" });
                    context.SaveChanges();

                    LocationTypeController controller = new LocationTypeController(context);
                    locationType.LocationTypeId = 4;
                    locationType.LocationTypeName = "locationType4";
                    var actionResult = controller.PutLocationType(locationType.LocationTypeId, locationType.ToLocationTypeV1DTO()).Result;
                    var result = actionResult as NotFoundResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
