using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class Order
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã hoá đơn
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public DateTime PayDate { get; set; }
        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public virtual LateCharge Detail { get; set; }
    }
}