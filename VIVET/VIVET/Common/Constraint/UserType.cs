using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum UserType
	{
        [Display(Name = "Cửa hàng")]
        STORE = 0,
        [Display(Name = "Quản lý")]
        LEAD = 1
	}
}