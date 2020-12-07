using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class RentDetail
    {
        public int Id { get; set; }
        /// <summary>
        /// hạn trả đĩa
        /// </summary>
        public int ReceiptNumber { get; set; }
        /// <summary>
        /// Đĩa thuê
        /// </summary>
        public virtual Disk Disk { get; set; }
        /// <summary>
        /// Giá thuê
        /// </summary>
        public double Prices { get; set; }
        /// <summary>
        /// Phí trễ hạn
        /// </summary>
        public double LateCharge { get; set; }
        public virtual Rent Rent { get; set; }
        public virtual RentReceipt RentReceipt { get; set; }
    }
}