using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum DiskStatus
    {
        [Display(Name = "Chưa thuê")]
        WAITING = 0,
        [Display(Name = "Đang thuê")]
        RENTING = 1,
        [Display(Name = "Đang được giữ")]
        HOLDING = 2,
        [Display(Name = "Xoá")]
        DELETED = 3
    }
}