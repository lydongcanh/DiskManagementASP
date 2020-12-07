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
        /// Mã tiêu đề
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên tiêu đề
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// giá
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Phí trễ hạn
        /// </summary>
        public double LateCharge { get; set; }
        /// <summary>
        /// mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public TitleStatus Status { get; set; }

        /// <summary>
        /// Đĩa
        /// </summary>
        public virtual ICollection<Disk> Disks { get; set; }
        public virtual DiskType DiskType { get; set; }
    }
}