using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum TitleStatus
    {
        [Display(Name = "Còn bán")]
        PENDING = 0,
        [Display(Name = "Ngưng bán")]
        CLOSED = 1,
        [Display(Name = "Xoá")]
        DELETED = 2
    }
}