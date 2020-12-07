using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum PermisstionStatus
    {
		/// <summary>
		/// Cho phép sử dụng quyền
		/// </summary>
        ACTIVATED=1,
		/// <summary>
		/// Ngăn sử dụng quyền
		/// </summary>
        NOTACTIVATED=0
    }
}