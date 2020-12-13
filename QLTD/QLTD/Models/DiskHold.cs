using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class DiskHold
    {
        public int Id { get; set; }
        /// <summary>
        /// Ngày đặt
        /// </summary>
        public DateTime HoldDate { get; set; }
        /// <summary>
        /// Khách đặt
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// Đĩa
        /// </summary>
        public virtual Disk Disk { get; set; }
    }
}