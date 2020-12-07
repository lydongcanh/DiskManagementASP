using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class DiskTitle
    {

        public int Id { get; set; }
        /// <summary>
        /// Tên title
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// giá
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public TitleStatus Status { get; set; }

        public virtual DiskType DiskType { get; set; }
    }
}