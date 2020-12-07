using Ehr.Common.Constraint;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class ReportModel
	{
        public string MonthYear
		{
			get;set;
		}
		public double Quantity
		{
			get;set;
		}
    }
	public class ReportNew
	{
		public string X
		{
			get;set;
		}
		public double Y
		{
			get; set;
		}
	}
}