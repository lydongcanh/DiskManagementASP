using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class AuditTrail
    {
        public long Id { get; set; }

        /// <summary>
        /// Id liên kết 
        /// </summary>
        public long KeyFieldID { get; set; }

        /// <summary>
        /// Loại Action : Creat - Update - Delete
        /// </summary>
        public AuditActionType AuditActionType { get; set; }

        /// <summary>
        /// Thời gian thực hiện
        /// </summary>
        public DateTime DateTimeStamp { get; set; }

        /// <summary>
        /// Thực hiện trên Model :
        /// </summary>
        public string DataModel { get; set; }

        /// <summary>
        /// dữ liệu thay đổi
        /// </summary>
        public string Changes { get; set; }

        /// <summary>
        /// Giá trị trước thay đổi
        /// </summary>
        public string ValueBefore { get; set; }

        /// <summary>
        /// Giá trị sao thi thay đổi
        /// </summary>
        public string ValueAfter { get; set; }

        /// <summary>
        /// User thực hiện
        /// </summary>
        public string Username { get; set; }
    }
}