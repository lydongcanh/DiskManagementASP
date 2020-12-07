using Ehr.Common.Constraint;
using Ehr.Models;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Ehr.Bussiness
{
    public class AuditTrailBussiness
    {
        private readonly UnitWork unitWork;

        public AuditTrailBussiness(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        public AuditChange GetAudit(int ID)
        {
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            var AuditTrail = db.AuditTrails.Where(s => s.Id == ID).FirstOrDefault();
            var serializer = new XmlSerializer(typeof(AuditDelta));

            AuditChange auditChange = new AuditChange();
            auditChange.DateTimeStamp = AuditTrail.DateTimeStamp.ToString();
            auditChange.Username = AuditTrail.Username;
            auditChange.AuditActionType = AuditTrail.AuditActionType;
            auditChange.DataModel = AuditTrail.DataModel;
            List<AuditDelta> delta = new List<AuditDelta>();
            delta = JsonConvert.DeserializeObject<List<AuditDelta>>(AuditTrail.Changes);

            auditChange.Changes.AddRange(delta);
            return auditChange;
        }

        public void CreateAuditTrail(AuditActionType Action, long KeyFieldID,string Model ,object OldObject, object NewObject,string Username)
        {
            // Láy các field đã thay đổi
            CompareLogic compObjects = new CompareLogic();
            compObjects.Config.MaxDifferences = 99;
            ComparisonResult compResult = compObjects.Compare(OldObject, NewObject);
            List<AuditDelta> DeltaList = new List<AuditDelta>();
            foreach (var change in compResult.Differences)
            {
                AuditDelta delta = new AuditDelta();
                delta.FieldName = change.PropertyName.Substring(0, change.PropertyName.Length);
                delta.ValueBefore = change.Object1Value;
                delta.ValueAfter = change.Object2Value;
                DeltaList.Add(delta);
            }
            
            AuditTrail audit = new AuditTrail();
            audit.AuditActionType = Action;
            audit.DataModel = Model;
            audit.DateTimeStamp = DateTime.Now;
            audit.KeyFieldID = KeyFieldID;
            audit.ValueBefore = JsonConvert.SerializeObject(OldObject);
            audit.ValueAfter = JsonConvert.SerializeObject(NewObject);
            audit.Changes = JsonConvert.SerializeObject(DeltaList);
            audit.Username = Username;
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            db.Entry<AuditTrail>(audit).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
        }

        public void CreateLoginAuditTrail(string Username,string IpAddress,YesNo Status, string Note)
        {
            LoginAuditTrail audit = new LoginAuditTrail();
            audit.Username = Username;
            audit.DateTimeStamp = DateTime.Now;
            audit.IpAddress = IpAddress;
            audit.Status = Status;
            audit.Note = Note;
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            db.Entry<LoginAuditTrail>(audit).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
        }
        
    }
}