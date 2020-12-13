using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class OrderRent
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã phiếu thuê
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// ngày trả
        /// </summary>
        public int RentLenght { get; set; }
        /// <summary>
        /// Ngày thuê
        /// </summary>
        public DateTime RentDate { get; set; }
        /// <summary>
        /// Ngày trả
        /// </summary>
        public DateTime ReceiptDate { get; set; }
        /// <summary>
        /// khách hàng
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// khách hàng
        /// </summary>
        public RentStatus Status { get; set; }
        /// <summary>
        /// chi tiết phiếu thuê
        /// </summary>
        public virtual ICollection<OrderDetail> RentDetails { get; set; }
    }
}