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
using DataModelAccess;

namespace API_ISD.Controllers
{
    public class ServiceLocationAssociationsController : ApiController
    {
        private InternalServicesDirectoryV1Entities db = new InternalServicesDirectoryV1Entities();

        // GET: api/ServiceLocationAssociations
        public IQueryable<ServiceLocationAssociation> GetServiceLocationAssociations()
        {
            return db.ServiceLocationAssociations;
        }

        // GET: api/ServiceLocationAssociations/5
        [ResponseType(typeof(ServiceLocationAssociation))]
        public IHttpActionResult GetServiceLocationAssociation(int id)
        {
            ServiceLocationAssociation serviceLocationAssociation = db.ServiceLocationAssociations.Find(id);
            if (serviceLocationAssociation == null)
            {
                return NotFound();
            }

            return Ok(serviceLocationAssociation);
        }

        // PUT: api/ServiceLocationAssociations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServiceLocationAssociation(int id, ServiceLocationAssociation serviceLocationAssociation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != serviceLocationAssociation.serviceLocationAssociation1)
            {
                return BadRequest();
            }

            db.Entry(serviceLocationAssociation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLocationAssociationExists(id))
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

        // POST: api/ServiceLocationAssociations
        [ResponseType(typeof(ServiceLocationAssociation))]
        public IHttpActionResult PostServiceLocationAssociation(ServiceLocationAssociation serviceLocationAssociation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServiceLocationAssociations.Add(serviceLocationAssociation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = serviceLocationAssociation.serviceLocationAssociation1 }, serviceLocationAssociation);
        }

        // DELETE: api/ServiceLocationAssociations/5
        [ResponseType(typeof(ServiceLocationAssociation))]
        public IHttpActionResult DeleteServiceLocationAssociation(int id)
        {
            ServiceLocationAssociation serviceLocationAssociation = db.ServiceLocationAssociations.Find(id);
            if (serviceLocationAssociation == null)
            {
                return NotFound();
            }

            db.ServiceLocationAssociations.Remove(serviceLocationAssociation);
            db.SaveChanges();

            return Ok(serviceLocationAssociation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceLocationAssociationExists(int id)
        {
            return db.ServiceLocationAssociations.Count(e => e.serviceLocationAssociation1 == id) > 0;
        }
    }
}