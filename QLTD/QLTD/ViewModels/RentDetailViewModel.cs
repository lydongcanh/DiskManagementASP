using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class RentDetailViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// hạn trả đĩa
        /// </summary>
        public int ReceiptNumber { get; set; }
        /// <summary>
        /// Đĩa thuê
        /// </summary>
        public int DiskId { get; set; }
        public string DiskCode { get; set; }
        public string DiskName { get; set; }
        public string TypeName { get; set; }
        /// <summary>
        /// Giá thuê
        /// </summary>
        public double Prices { get; set; }
        /// <summary>
        /// Phí trễ hạn
        /// </summary>
        public double LateCharge { get; set; }
        public int RentId { get; set; }
        public int RentReceiptId { get; set; }
        public string Status { get; set; }
    }
}