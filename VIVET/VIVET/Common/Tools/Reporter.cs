using Ehr.Bussiness;
using Ehr.Common.UI;
using Ehr.Controllers;
using Ehr.Models;
using Ehr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Common.Tools
{
	public class Reporter
	{
		private readonly UnitWork unitWork;
		public Reporter(UnitWork unitWork)
		{
			this.unitWork = unitWork;
		}
		public static string GetMyCustomTheme()
		{
			return @"
            <Chart>
<ChartAreas>
    <ChartArea
        Name=""Default""
        _Template_=""All""
        BackColor=""Transparent""
        BackSecondaryColor=""White""
        BorderColor=""64, 64, 64, 64""
        BorderDashStyle=""Solid""
        ShadowColor=""Transparent"">
        <AxisY>
            <LabelStyle Font=""Trebuchet MS, 8.25pt, style=Bold"" />
        </AxisY>
        <AxisX LineColor=""64, 64, 64, 64"">
            <LabelStyle Interval=""1"" Font=""Trebuchet MS, 8.25pt, style=Bold"" />
        </AxisX>
    </ChartArea>
</ChartAreas>
<Legends>
    <Legend _Template_=""All"" BackColor=""Transparent"" Docking=""Bottom"" Font=""Trebuchet MS, 8.25pt, style=Bold"" LegendStyle=""Row"">
    </Legend>
</Legends>
<BorderSkin SkinStyle=""Emboss"" />
</Chart>";
		}
		public List<ReportModel> ReportByMonth()
		{
			List<ReportModel> list = new List<ReportModel>();
			DateTime now = DateTime.Now;
			for (int i = 11; i >= 0; i--)
			{
				DateTime date = now.AddMonths(-1 * i);
				string label = DataConverter.GetMonthYear(date);
				DateTime mindate = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
				DateTime maxdate = DataConverter.UI2DateTimeMinMax(DataConverter.LastDateOfMonth(date.Month, date.Year), false);
				double items = 0;
				try
				{
					items = unitWork.ProductInfor.Get(c => c.CollectedDate <= maxdate && c.CollectedDate >= mindate).Sum(c => c.AmountProduct);
				}
				catch { }
				list.Add(new ReportModel() { MonthYear = label, Quantity = items });
			}
			return list;
		}
		public List<ReportNew> ReportByProduct()
		{
			List<ReportNew> list = new List<ReportNew>();
			//get all products
			Ehr.Data.EhrDbContext context = new Data.EhrDbContext();
			var cosolidate =
								from c in context.ProductInfors
								group c by new
								{
									c.Product,
									c.AmountProduct
								} into gcs
								select new ReportNew()
								{
									X = gcs.Key.Product,
									Y = gcs.Key.AmountProduct
								};
			return cosolidate.ToList();
		}
		public List<ReportNew> ReportByAntibiotics(int userid)
		{
			var user = unitWork.User.GetById(userid);		

			var products = unitWork.ProductInfor.Get();// (from a in context.ProductInfors where a.UserId == userid select a);
			if (!user.IsCentral)
			{
				if (user.UserType == Constraint.UserType.LEAD)
				{
					Ehr.Data.EhrDbContext context = new Data.EhrDbContext();
					//get all user in a province
					var province = user.Province;
					var users = unitWork.User.Get(c => c.Province == province);
					List<int> ids = new List<int>();
					foreach (User u in users)
					{
						ids.Add(u.Id);
					}
					products = products.Where(d=>ids.Any(s => s == d.UserId));
				}
				else
					products = products.Where(c => c.UserId == userid);
			}
			
			
			List<ReportNew> result = products
			.GroupBy(x => x.Antimicrobial)
			.Select(g => new ReportNew()
			{
				X = g.Key,
				Y = g.Sum(x => x.AntiAmount > 0 ? x.AntiAmount * x.AmountProduct : 0)
			}).OrderBy(n => n.Y).ToList();		
			
			return result;
		}
	}
}