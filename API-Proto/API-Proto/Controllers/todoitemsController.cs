using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TodoitemDataAccess;

/* To generate this, connection string from App.config in TodoitemDataAccess has to be in Web.config in API-Proto */ 
namespace API_Proto.Controllers
{
    public class todoitemsController : ApiController
    {
        private TodoitemEntities db = new TodoitemEntities();

        // GET: api/todoitems
        public IQueryable<todoitem> Gettodoitems()
        {
            return db.todoitems;
        }

        // GET: api/todoitems/5
        [ResponseType(typeof(todoitem))]
        public IHttpActionResult Gettodoitem(int id)
        {
            todoitem todoitem = db.todoitems.Find(id);
            if (todoitem == null)
            {
                return NotFound();
            }

            return Ok(todoitem);
        }

        // PUT: api/todoitems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttodoitem(int id, todoitem todoitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todoitem.id)
            {
                return BadRequest();
            }

            db.Entry(todoitem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!todoitemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/todoitems
        [ResponseType(typeof(todoitem))]
        public IHttpActionResult Posttodoitem(todoitem todoitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.todoitems.Add(todoitem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = todoitem.id }, todoitem);
        }

        // DELETE: api/todoitems/5
        [ResponseType(typeof(todoitem))]
        public IHttpActionResult Deletetodoitem(int id)
        {
            todoitem todoitem = db.todoitems.Find(id);
            if (todoitem == null)
            {
                return NotFound();
            }

            db.todoitems.Remove(todoitem);
            db.SaveChanges();

            return Ok(todoitem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool todoitemExists(int id)
        {
            return db.todoitems.Count(e => e.id == id) > 0;
        }
    }
}