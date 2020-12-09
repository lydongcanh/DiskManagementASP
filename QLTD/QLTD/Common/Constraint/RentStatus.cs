using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum RentStatus
    {
        [Display(Name = "Đang thuê")]
        RENTING = 0,
        [Display(Name = "Chờ thanh toán")]
        WAITING = 1,
        [Display(Name = "Thanh toán thiếu")]
        PENDING = 1,
        [Display(Name = "Đã thanh toán")]
        DONE = 1,
    }
}