using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class AuditChange
    {
        public string DateTimeStamp { get; set; }
        public AuditActionType AuditActionType { get; set; }
        public string DataModel { get; set; }
        public string Username { get; set; }
        public List<AuditDelta> Changes { get; set; }
        public AuditChange()
        {
            Changes = new List<AuditDelta>();
        }
    }
}