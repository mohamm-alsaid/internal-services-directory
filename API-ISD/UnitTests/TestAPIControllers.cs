using System;
using System.Data;
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
                    // How to seed the data base & how to 
                }

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
