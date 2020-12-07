using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum State
    {
        [Display(Name = "Đã duyệt")]
        APPROVE = 1,
        [Display(Name = "Chờ duyệt")]
        WAIT = 2,
        [Display(Name = "Từ chối")]
        REJECT = 3,
    }
}