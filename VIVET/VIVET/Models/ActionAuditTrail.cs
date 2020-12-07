using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class ActionAuditTrail
    {
        public long Id { get; set; }

        /// <summary>
        /// User thực hiện
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Thời gian thực hiện
        /// </summary>
        public DateTime DateTimeStamp { get; set; }

        /// <summary>
        /// hành động
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Trạng thái thực hiện 
        /// </summary>
        public YesNo Status { get; set; }

        /// <summary>
        /// Ứng viên
        /// </summary>
        public AuditActionType Actiontype { get; set; }
    }
}