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
    public class ProgramCommunityAssociationsController : ApiController
    {
        private InternalServicesDirectoryV1Entities db = new InternalServicesDirectoryV1Entities();

        // GET: api/ProgramCommunityAssociations
        public IQueryable<ProgramCommunityAssociation> GetProgramCommunityAssociations()
        {
            return db.ProgramCommunityAssociations;
        }

        // GET: api/ProgramCommunityAssociations/5
        [ResponseType(typeof(ProgramCommunityAssociation))]
        public IHttpActionResult GetProgramCommunityAssociation(int id)
        {
            ProgramCommunityAssociation programCommunityAssociation = db.ProgramCommunityAssociations.Find(id);
            if (programCommunityAssociation == null)
            {
                return NotFound();
            }

            return Ok(programCommunityAssociation);
        }

        // PUT: api/ProgramCommunityAssociations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProgramCommunityAssociation(int id, ProgramCommunityAssociation programCommunityAssociation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != programCommunityAssociation.programCommunityAssociationID)
            {
                return BadRequest();
            }

            db.Entry(programCommunityAssociation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramCommunityAssociationExists(id))
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

        // POST: api/ProgramCommunityAssociations
        [ResponseType(typeof(ProgramCommunityAssociation))]
        public IHttpActionResult PostProgramCommunityAssociation(ProgramCommunityAssociation programCommunityAssociation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProgramCommunityAssociations.Add(programCommunityAssociation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = programCommunityAssociation.programCommunityAssociationID }, programCommunityAssociation);
        }

        // DELETE: api/ProgramCommunityAssociations/5
        [ResponseType(typeof(ProgramCommunityAssociation))]
        public IHttpActionResult DeleteProgramCommunityAssociation(int id)
        {
            ProgramCommunityAssociation programCommunityAssociation = db.ProgramCommunityAssociations.Find(id);
            if (programCommunityAssociation == null)
            {
                return NotFound();
            }

            db.ProgramCommunityAssociations.Remove(programCommunityAssociation);
            db.SaveChanges();

            return Ok(programCommunityAssociation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProgramCommunityAssociationExists(int id)
        {
            return db.ProgramCommunityAssociations.Count(e => e.programCommunityAssociationID == id) > 0;
        }
    }
}