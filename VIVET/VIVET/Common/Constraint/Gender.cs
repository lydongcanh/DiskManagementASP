using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ehr.Common.Constraint
{
    public enum Gender
    {
		[Display(Name = "Nam")]
        MALE,
		[Display(Name = "Nữ")]
        FEMALE,
		[Display(Name = "Khác")]
        OTHER
    }
}