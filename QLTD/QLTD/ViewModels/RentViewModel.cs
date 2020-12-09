using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class RentViewModel
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
        public string RentDate { get; set; }
        /// <summary>
        /// khách hàng
        /// </summary>
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
        /// <summary>
        /// khách hàng
        /// </summary>
        public RentStatus Status { get; set; }
    }
}