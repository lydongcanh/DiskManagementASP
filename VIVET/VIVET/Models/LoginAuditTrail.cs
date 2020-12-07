using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class LoginAuditTrail
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
        /// Ip máy login
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Trạng thái login 
        /// </summary>
        public YesNo Status { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
    }
}