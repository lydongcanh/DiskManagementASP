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
using Ehr.Data;
using Ehr.Models;

namespace Ehr.APIController
{
    public class QuestionnairesController : ApiController
    {
        private EhrDbContext db = new EhrDbContext();

        // GET: DBS/Questionnaires
        public IQueryable<Questionnaire> GetQuestionnaires()
        {
            return db.Questionnaires;
        }

        // GET: DBS/Questionnaires/5
        [ResponseType(typeof(Questionnaire))]
        public IHttpActionResult GetQuestionnaire(int id)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            return Ok(questionnaire);
        }

        // PUT: DBS/Questionnaires/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestionnaire(int id, Questionnaire questionnaire)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionnaire.Id)
            {
                return BadRequest();
            }

            db.Entry(questionnaire).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionnaireExists(id))
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

        // POST: DBS/Questionnaires
        [ResponseType(typeof(Questionnaire))]
        public IHttpActionResult PostQuestionnaire(Questionnaire questionnaire)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questionnaires.Add(questionnaire);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = questionnaire.Id }, questionnaire);
        }

        // DELETE: DBS/Questionnaires/5
        [ResponseType(typeof(Questionnaire))]
        public IHttpActionResult DeleteQuestionnaire(int id)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            db.Questionnaires.Remove(questionnaire);
            db.SaveChanges();

            return Ok(questionnaire);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionnaireExists(int id)
        {
            return db.Questionnaires.Count(e => e.Id == id) > 0;
        }
    }
}