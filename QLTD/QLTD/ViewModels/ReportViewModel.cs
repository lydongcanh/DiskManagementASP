using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class ReportViewModel
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Tổng đĩa đang thuê
        /// </summary>
        public int DiskRentTotal { get; set; }
        /// <summary>
        /// Tổng đĩa trễ hạn
        /// </summary>
        public int DiskLate { get; set; }
        /// <summary>
        /// Tên đĩa
        /// </summary>
        public string Disk { get; set; }
    }
}