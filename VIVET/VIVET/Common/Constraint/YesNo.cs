using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum YesNo
    {
        [Display(Name = "Không")]
        NO = 0,
        [Display(Name = "Có")]
        YES = 1
    }
}