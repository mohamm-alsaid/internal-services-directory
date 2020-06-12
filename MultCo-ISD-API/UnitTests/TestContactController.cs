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
    public class TestContactControllers
    {
        [TestMethod]
        public void TestGetContact()
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
                    context.Contact.Add(new Contact());
                    context.Contact.Add(new Contact());
                    context.Contact.Add(new Contact());
                    context.SaveChanges();

                    ContactController controller = new ContactController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var contact = result.Value as ContactV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(contact);
                    Assert.AreEqual(2, contact.ContactId);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetContactNotFound()
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
                    context.Contact.Add(new Contact());
                    context.SaveChanges();

                    ContactController controller = new ContactController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No contact from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutContact()
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
                    var contact = new Contact();
                    context.Contact.Add(contact);
                    context.Contact.Add(new Contact());
                    context.Contact.Add(new Contact());
                    context.SaveChanges();

                    ContactController controller = new ContactController(context);
                    contact.ContactId = 1;
                    contact.ContactName = "contact1";
                    var actionResult = controller.PutContact(contact.ContactId, contact.ToContactV1DTO()).Result;
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
        public void TestPutContactNotFound()
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
                    var contact = new Contact();
                    context.Contact.Add(contact);
                    context.Contact.Add(new Contact());
                    context.Contact.Add(new Contact());
                    context.SaveChanges();

                    ContactController controller = new ContactController(context);
                    contact.ContactId = 4;
                    contact.ContactName = "contact1";
                    var actionResult = controller.PutContact(contact.ContactId, contact.ToContactV1DTO()).Result;
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
