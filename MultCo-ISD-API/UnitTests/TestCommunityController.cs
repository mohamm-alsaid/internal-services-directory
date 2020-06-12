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
    public class TestCommunityControllers
    {

        [TestMethod]
        public void TestGetCommunity()
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
                    context.Community.Add(new Community());
                    context.Community.Add(new Community());
                    context.Community.Add(new Community());
                    context.SaveChanges();

                    CommunityController controller = new CommunityController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var community = result.Value as CommunityV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(community);
                    Assert.AreEqual(2, community.CommunityId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [TestMethod]
        public void TestGetCommunityNotFound()
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
                    context.Community.Add(new Community());
                    context.SaveChanges();

                    CommunityController controller = new CommunityController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(actionResult);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No community from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [TestMethod]
        public void TestPutCommunity()
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
                    var community = new Community();
                    context.Community.Add(community);
                    context.Community.Add(new Community());
                    context.Community.Add(new Community());
                    context.SaveChanges();

                    CommunityController controller = new CommunityController(context);
                    community.CommunityId = 1;
                    community.CommunityName = "community1";
                    var actionResult = controller.PutCommunity(community.CommunityId, community.ToCommunityV1DTO()).Result;
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
        public void TestPutCommunityNotFound()
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
                    var community = new Community();
                    context.Community.Add(community);
                    context.Community.Add(new Community());
                    context.Community.Add(new Community());
                    context.SaveChanges();

                    CommunityController controller = new CommunityController(context);
                    community.CommunityId = 4;
                    community.CommunityName = "community4";
                    var actionResult = controller.PutCommunity(community.CommunityId, community.ToCommunityV1DTO()).Result;
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
