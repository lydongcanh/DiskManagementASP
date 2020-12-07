using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class LateCharge
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã trễ hẹn
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// tiền nợ
        /// </summary>
        public double ChargeOwed { get; set; }
        /// <summary>
        /// Phiếu trả
        /// </summary>
        public virtual RentReceipt RentReceipt { get; set; }
    }
}