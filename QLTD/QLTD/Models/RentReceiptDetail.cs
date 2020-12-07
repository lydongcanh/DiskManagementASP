using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class RentReceiptDetail
    {
        public int Id { get; set; }
        /// <summary>
        /// ngày trả
        /// </summary>
        public DateTime ReceiptDate { get; set; }

        public virtual Disk Disk { get; set; }
        public virtual RentReceipt RentReceipt { get; set; }
    }
}