using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum Origin
    {
        [Display(Name = "Sản phẩm nhập khẩu")]
        IMPORTED = 0,
        [Display(Name = "Sản phẩm sản xuất trong nước")]
        DOMESTIC = 1,
    }
}