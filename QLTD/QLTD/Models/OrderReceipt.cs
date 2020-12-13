using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class OrderReceipt
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã phiếu trả
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ngày trả đĩa
        /// </summary>
        public DateTime ReceiptDate { get; set; }
        /// <summary>
        /// khách hàng
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// trạng thái
        /// </summary>
        public ReceiptStatus Status { get; set; }

        /// <summary>
        /// chi tiết phiếu thuê
        /// </summary>
        public virtual ICollection<OrderDetail> RentDetails { get; set; }
    }
}