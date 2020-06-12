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
    public class TestProgramControllers
    {

        [TestMethod]
        public void TestGetProgram()
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
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.SaveChanges();

                    ProgramController controller = new ProgramController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var program = result.Value as ProgramV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(program);
                    Assert.AreEqual(2, program.ProgramId);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetProgramNotFound()
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
                    context.Program.Add(new Program());
                    context.SaveChanges();

                    ProgramController controller = new ProgramController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No program from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [TestMethod]
        public void TestPutProgram()
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
                    var program = new Program();
                    context.Program.Add(program);
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.SaveChanges();

                    ProgramController controller = new ProgramController(context);
                    program.ProgramId = 1;
                    program.ProgramName = "program1";
                    program.SponsorName = "sponsor1";
                    program.ProgramOfferNumber = "offerNumber1";
                    program.OfferType = "offerType1";
                    var actionResult = controller.PutProgram(program.ProgramId, program.ToProgramV1DTO()).Result;
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
        public void TestPutProgramNotFound()
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
                    var program = new Program();
                    context.Program.Add(program);
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.SaveChanges();

                    ProgramController controller = new ProgramController(context);
                    program.ProgramId = 4;
                    program.ProgramName = "program1";
                    program.SponsorName = "sponsor1";
                    program.ProgramOfferNumber = "offerNumber1";
                    program.OfferType = "offerType1";
                    var actionResult = controller.PutProgram(program.ProgramId, program.ToProgramV1DTO()).Result;
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
