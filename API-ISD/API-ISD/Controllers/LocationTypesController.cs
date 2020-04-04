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
    public class LocationTypesController : ApiController
    {
        private InternalServicesDirectoryV1Entities db = new InternalServicesDirectoryV1Entities();

        // GET: api/LocationTypes
        public IQueryable<LocationType> GetLocationTypes()
        {
            return db.LocationTypes;
        }

        // GET: api/LocationTypes/5
        [ResponseType(typeof(LocationType))]
        public IHttpActionResult GetLocationType(int id)
        {
            LocationType locationType = db.LocationTypes.Find(id);
            if (locationType == null)
            {
                return NotFound();
            }

            return Ok(locationType);
        }

        // PUT: api/LocationTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLocationType(int id, LocationType locationType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationType.locationTypeID)
            {
                return BadRequest();
            }

            db.Entry(locationType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationTypeExists(id))
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

        // POST: api/LocationTypes
        [ResponseType(typeof(LocationType))]
        public IHttpActionResult PostLocationType(LocationType locationType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LocationTypes.Add(locationType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = locationType.locationTypeID }, locationType);
        }

        // DELETE: api/LocationTypes/5
        [ResponseType(typeof(LocationType))]
        public IHttpActionResult DeleteLocationType(int id)
        {
            LocationType locationType = db.LocationTypes.Find(id);
            if (locationType == null)
            {
                return NotFound();
            }

            db.LocationTypes.Remove(locationType);
            db.SaveChanges();

            return Ok(locationType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationTypeExists(int id)
        {
            return db.LocationTypes.Count(e => e.locationTypeID == id) > 0;
        }
    }
}