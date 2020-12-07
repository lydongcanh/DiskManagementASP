using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum ProductType
    {
        [Display(Name = "Dạng bột")]
        POWDER = 0,
        [Display(Name = "Dạng dung dịch")]
        LIQUID = 1,
    }
}