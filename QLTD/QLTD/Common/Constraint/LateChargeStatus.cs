using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum LateChargeStatus
    {
        [Display(Name = "Chưa thanh toán")]
        NONE = 0,
        [Display(Name = "Đã thanh toán")]
        DONE = 1,
    }
}