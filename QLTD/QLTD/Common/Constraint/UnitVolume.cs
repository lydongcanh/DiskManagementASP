using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum UnitVolume
    {
        [Display(Name = "lít")]
        LITRE = 0,
        [Display(Name = "kg")]
        KILO = 1,
        [Display(Name = "mg")]
        MG = 2,
        [Display(Name = "g")]
        GRAM = 3,
        [Display(Name = "IU")]
        IU = 4,
        [Display(Name = "ml")]
        ML = 5,
        [Display(Name = "-")]
        NULL = 6,
    }
}