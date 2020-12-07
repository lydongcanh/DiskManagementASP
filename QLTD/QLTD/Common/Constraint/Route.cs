using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum Route
    {
        [Display(Name = "Miệng")]
        ORAL = 1,
        [Display(Name = "Tiêm Chích")]
        INJECTABLE = 2,
        [Display(Name = "Pha nước")]
        WATER = 3,
        [Display(Name = "Trộn thức ăn")]
        FEED = 4,
    }
}