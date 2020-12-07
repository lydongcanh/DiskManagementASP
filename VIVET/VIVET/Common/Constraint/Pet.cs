using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum Pet
    {
        [Display(Name = "Trâu")]
        BUFFALO = 1,
        [Display(Name = "Gia súc")]
        CATTLE = 2,
        [Display(Name = "Gia cầm")]
        POULTRY = 3,
        [Display(Name = "Heo")]
        PIG = 4,
        [Display(Name = "Chó")]
        DOG = 5,
        [Display(Name = "Mèo")]
        CAT = 6,
        [Display(Name = "Dê")]
        GOAT = 7,
        [Display(Name = "Cút")]
        QUAIL = 8,
        [Display(Name = "Cừu")]
        SHEEP = 9,
        [Display(Name = "Vịt Xiêm")]
        MUSCOVY_DUCK = 10,
        [Display(Name = "Ngỗng")]
        GOOSE = 11,
        [Display(Name = "Ngựa")]
        HORSE = 12,
        [Display(Name = "Gà")]
        CHICKEN = 13,
        [Display(Name = "Bê")]
        CALF = 14,
        [Display(Name = "Gà con")]
        CHICK = 15,
        [Display(Name = "Vịt")]
        DUCK = 16,

    }
}