using System;
using System.Data;
using System.Web.Http;
using DataModelAccess;
using API_ISD.Models;
using API_ISD.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
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
                var options = new DbContextOptionsBuilder<ISDContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create schema in db
                using (var context = new ISDContext(options))
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
                var options = new DbContextOptionsBuilder<ISDContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new ISDContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Programs.Add(new Program { offerType = "offer", sponsorName = "sponsor" });
                    context.SaveChanges();

                    ProgramsController controller = new ProgramsController();
                    var programs = controller.GetPrograms();
                    foreach(var item in programs)
                    {
                        Assert.AreEqual(item.programID, 1);
                        Assert.AreEqual(item.offerType, "offer1"); //don't really know why the key is being appended
                        Assert.AreEqual(item.sponsorName, "sponsor1");
                    }
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
