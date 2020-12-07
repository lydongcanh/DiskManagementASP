using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class Disk
    {
        public int Id { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public DiskStatus Status { get; set; }

        public virtual DiskTitle DiskTitle { get; set; }
    }
}