using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class Rent
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã phiếu thuê
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ngày thuê
        /// </summary>
        public DateTime RentDate { get; set; }
        /// <summary>
        /// chi tiết phiếu thuê
        /// </summary>
        public virtual ICollection<RentDetail> RentDetails { get; set; }
    }
}