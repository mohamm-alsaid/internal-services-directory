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
    public class TestDivisionControllers
    {


        [TestMethod]
        public void TestGetDivision()
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
                    context.Division.Add(new Division { DivisionCode = 1 });
                    context.Division.Add(new Division { DivisionCode = 2 });
                    context.Division.Add(new Division { DivisionCode = 3 });
                    context.SaveChanges();

                    DivisionController controller = new DivisionController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var division = result.Value as DivisionV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(division);
                    Assert.AreEqual(2, division.DivisionId);
                    Assert.AreEqual(2, division.DivisionCode);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetDivisionNotFound()
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
                    context.Division.Add(new Division { DivisionCode = 1 });
                    context.SaveChanges();

                    DivisionController controller = new DivisionController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;


                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No division from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [TestMethod]
        public void TestPutDivision()
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
                    var division = new Division { DivisionCode = 1 };
                    context.Division.Add(division);
                    context.Division.Add(new Division { DivisionCode = 2 });
                    context.Division.Add(new Division { DivisionCode = 3 });
                    context.SaveChanges();

                    DivisionController controller = new DivisionController(context);
                    division.DivisionId = 1;
                    division.DivisionName = "division1";
                    division.DivisionCode = 4;

                    var actionResult = controller.PutDivision(division.DivisionId, division.ToDivisionV1DTO()).Result;
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
        public void TestPutDivisionNotFound()
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
                    var division = new Division { DivisionCode = 1 };
                    context.Division.Add(division);
                    context.Division.Add(new Division { DivisionCode = 2 });
                    context.Division.Add(new Division { DivisionCode = 3 });
                    context.SaveChanges();

                    DivisionController controller = new DivisionController(context);
                    division.DivisionId = 4;
                    division.DivisionName = "division4";
                    division.DivisionCode = 4;

                    var actionResult = controller.PutDivision(division.DivisionId, division.ToDivisionV1DTO()).Result;
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
