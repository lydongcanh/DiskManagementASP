using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum RentDetailState
    {
        [Display(Name = "Đang thuê")]
        WAITING = 0,
        [Display(Name = "Đã trả đĩa")]
        DONE = 1,
    }
}