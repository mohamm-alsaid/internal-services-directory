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
    public class ServiceLanguageAssociationsController : ApiController
    {
        private InternalServicesDirectoryV1Entities db = new InternalServicesDirectoryV1Entities();

        // GET: api/ServiceLanguageAssociations
        public IQueryable<ServiceLanguageAssociation> GetServiceLanguageAssociations()
        {
            return db.ServiceLanguageAssociations;
        }

        // GET: api/ServiceLanguageAssociations/5
        [ResponseType(typeof(ServiceLanguageAssociation))]
        public IHttpActionResult GetServiceLanguageAssociation(int id)
        {
            ServiceLanguageAssociation serviceLanguageAssociation = db.ServiceLanguageAssociations.Find(id);
            if (serviceLanguageAssociation == null)
            {
                return NotFound();
            }

            return Ok(serviceLanguageAssociation);
        }

        // PUT: api/ServiceLanguageAssociations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServiceLanguageAssociation(int id, ServiceLanguageAssociation serviceLanguageAssociation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != serviceLanguageAssociation.serviceLanguageAssociation1)
            {
                return BadRequest();
            }

            db.Entry(serviceLanguageAssociation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLanguageAssociationExists(id))
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

        // POST: api/ServiceLanguageAssociations
        [ResponseType(typeof(ServiceLanguageAssociation))]
        public IHttpActionResult PostServiceLanguageAssociation(ServiceLanguageAssociation serviceLanguageAssociation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServiceLanguageAssociations.Add(serviceLanguageAssociation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = serviceLanguageAssociation.serviceLanguageAssociation1 }, serviceLanguageAssociation);
        }

        // DELETE: api/ServiceLanguageAssociations/5
        [ResponseType(typeof(ServiceLanguageAssociation))]
        public IHttpActionResult DeleteServiceLanguageAssociation(int id)
        {
            ServiceLanguageAssociation serviceLanguageAssociation = db.ServiceLanguageAssociations.Find(id);
            if (serviceLanguageAssociation == null)
            {
                return NotFound();
            }

            db.ServiceLanguageAssociations.Remove(serviceLanguageAssociation);
            db.SaveChanges();

            return Ok(serviceLanguageAssociation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceLanguageAssociationExists(int id)
        {
            return db.ServiceLanguageAssociations.Count(e => e.serviceLanguageAssociation1 == id) > 0;
        }
    }
}