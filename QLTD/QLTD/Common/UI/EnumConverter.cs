using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ehr.Common.Constraint;

namespace Ehr.Common.UI
{
    public class EZEnumConverter
    {
        public static QuestionStatus QuestionStatusConvert(string status)
        {
            if (status == null)
                return QuestionStatus.HIDDEN;
            if (status.Equals("1"))
            {
                return QuestionStatus.REQUIRED;
            }
            else if (status.Equals("2"))
            {
                return QuestionStatus.OPTIONAL;
            }
            return QuestionStatus.HIDDEN;
        }
    }
}