using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum AuditActionType
    {
        [Display(Name ="Tạo mới")]
        Create = 1,
        [Display(Name = "Cập nhật")]
        Update,
        [Display(Name = "Đã xóa")]
        Delete,
        [Display(Name = "Gửi mail")]
        SendMail,
        [Display(Name = "Chấp nhận hồ sơ")]
        Approval,
        [Display(Name = "Trạng thái")]
        StatusHistory
    }
}