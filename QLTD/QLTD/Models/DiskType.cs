using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class DiskType
    {
        public int Id { get; set; }
        /// <summary>
        /// mã loại
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên loại
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
    }
}