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
    public class TestDepartmentControllers
    {

        [TestMethod]
        public void TestGetDepartment()
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
                    context.Department.Add(new Department { DepartmentCode = 1 });
                    context.Department.Add(new Department { DepartmentCode = 2 });
                    context.Department.Add(new Department { DepartmentCode = 3 });
                    context.SaveChanges();

                    DepartmentController controller = new DepartmentController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as OkObjectResult;
                    var department = result.Value as DepartmentV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(department);
                    Assert.AreEqual(2, department.DepartmentId);
                    Assert.AreEqual(2, department.DepartmentCode);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetDepartmentNotFound()
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
                    context.Department.Add(new Department { DepartmentCode = 1 });
                    context.SaveChanges();

                    DepartmentController controller = new DepartmentController(context);
                    var actionResult = controller.Get(2).Result;
                    var result = actionResult as NotFoundObjectResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(404, result.StatusCode);
                    Assert.AreEqual("No department from given id found.", result.Value);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [TestMethod]
        public void TestPutDepartment()
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
                    var department = new Department { DepartmentCode = 1 };
                    context.Department.Add(department);
                    context.Department.Add(new Department { DepartmentCode = 2 });
                    context.Department.Add(new Department { DepartmentCode = 3 });
                    context.SaveChanges();

                    DepartmentController controller = new DepartmentController(context);
                    department.DepartmentId = 1;
                    department.DepartmentName = "department1";
                    department.DepartmentCode = 4;

                    var actionResult = controller.PutDepartment(department.DepartmentId, department.ToDepartmentV1DTO()).Result;
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
        public void TestPutDepartmentNotFound()
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
                    var department = new Department { DepartmentCode = 1 };
                    context.Department.Add(new Department { DepartmentCode = 1 });
                    context.Department.Add(new Department { DepartmentCode = 2 });
                    context.Department.Add(new Department { DepartmentCode = 3 });
                    context.SaveChanges();

                    DepartmentController controller = new DepartmentController(context);
                    department.DepartmentId = 4;
                    department.DepartmentName = "department4";
                    department.DepartmentCode = 4;

                     var actionResult = controller.PutDepartment(department.DepartmentId, department.ToDepartmentV1DTO()).Result;
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
