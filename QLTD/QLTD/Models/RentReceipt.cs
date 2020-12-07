using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class RentReceipt
    {
        public int Id { get; set; }
        /// <summary>
        /// khách hàng
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// chi tiết hoá đơn
        /// </summary>
        public virtual ICollection<RentReceiptDetail> RentReceiptDetail { get; set; }
    }
}