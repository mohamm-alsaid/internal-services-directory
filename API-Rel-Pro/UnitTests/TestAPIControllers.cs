using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using API_Rel_Pro.Models;

namespace API_Rel_Pro.Controllers
{
    [TestClass]
    public class TestAPIControllerMethods
    {
        [TestMethod]
        public void TestInMemoryDBConnection()
        {
            // Build in-mem db
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                // In-memory db exists only while connection is open
                var options = new DbContextOptionsBuilder<ContosoContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create schema in db
                using (var context = new ContosoContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
