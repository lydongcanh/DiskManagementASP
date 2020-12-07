using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Common.Constraint
{
    public enum RoleStatus
    {
		/// <summary>
		/// Cho phép sử dụng role
		/// </summary>
        ACTIVATED=1,
		/// <summary>
		/// Ngăn sử dụng role
		/// </summary>
        NOTACTIVATED=0
    }
}