using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Common.UI
{
	public enum PostedStatus
	{
		OK=1,
		FAILED=0
	}
    public class AjaxStatus
    {
        public PostedStatus Status
		{
			get;set;
		}
		public string Message
		{
			get; set;
		}
    }
}