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
    public class TestLocationControllers
    {

        [TestMethod]
        public void TestGetLocation()
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
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.SaveChanges();

                    LocationController controller = new LocationController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var location = result.Value as LocationV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(location);
                    Assert.AreEqual(2, location.LocationId);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetLocationNotFound()
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
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.SaveChanges();

                    LocationController controller = new LocationController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No location from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutLocation()
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
                    context.LocationType.Add(new LocationType { LocationTypeName = "locationType1" });
                    context.SaveChanges();
                    var location = new Location { LocationTypeId = 1 };
                    context.Location.Add(location);
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.SaveChanges();

                    LocationController controller = new LocationController(context);
                    location.LocationId = 1;
                    location.LocationName = "location1";
                    var actionResult = controller.PutLocation(location.LocationId, location.ToLocationV1DTO()).Result;
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
        public void TestPutLocationNotFound()
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
                    context.LocationType.Add(new LocationType { LocationTypeName = "locationType1" });
                    context.SaveChanges();
                    var location = new Location { LocationTypeId = 1 };
                    context.Location.Add(location);
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.Location.Add(new Location { LocationTypeId = 1 });
                    context.SaveChanges();

                    LocationController controller = new LocationController(context);
                    location.LocationId = 4;
                    location.LocationName = "location4";
                    var actionResult = controller.PutLocation(location.LocationId, location.ToLocationV1DTO()).Result;
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
