using Ehr.Bussiness;
using Ehr.Common.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace Ehr.Controllers
{
    public class ReportController : BaseController
    {
		private readonly UnitWork unitWork;
		public ReportController(UnitWork unitWork)
		{
			this.unitWork = unitWork;
		}
		// GET: Report
		public ActionResult Index()
        {
			//unitWork.ProductInfor.Get(c=>c.)
            return View();
        }
		public ActionResult Dashboard()
		{
			//lấy dữ liệu theo tháng hiện tại quay trở về 12 thàng
			Reporter report = new Reporter(unitWork);
			return View(report.ReportByMonth());
		}
		public ActionResult DashboardProduct()
		{
			//lấy dữ liệu theo tháng hiện tại quay trở về 12 thàng
			Reporter report = new Reporter(unitWork);
			return View(report.ReportByProduct());
		}

		public ActionResult DashboardAntibiotics()
		{
			//lấy dữ liệu theo tháng hiện tại quay trở về 12 thàng
			Reporter report = new Reporter(unitWork);
			return View(report.ReportByAntibiotics(User.UserId));
		}

		[HttpGet]
		public ActionResult CreateChartByMonth()
		{
			Reporter report = new Reporter(unitWork);
			var list = report.ReportByMonth();
			Bitmap image = new Bitmap(960, 400);
			Graphics g = Graphics.FromImage(image);
			var chart1 = new System.Web.UI.DataVisualization.Charting.Chart();
			chart1.Width = 960;
			chart1.Height = 400;
			chart1.EnableTheming = true;
			
			chart1.ChartAreas.Add("xAxis").BackColor = System.Drawing.Color.FromArgb(64, System.Drawing.Color.White);			
			chart1.Series.Add("xAxis");		
			chart1.DataSource = list;
			chart1.Series["xAxis"].XValueMember = "MonthYear";
			chart1.Series["xAxis"].YValueMembers = "Quantity";
			chart1.ChartAreas["xAxis"].AxisX.Title = "Tháng/ năm";
			chart1.ChartAreas["xAxis"].AxisY.Title = "Số lượng sản phẩm";
			chart1.Series["xAxis"].IsValueShownAsLabel = true;
			chart1.BackColor = Color.Transparent;
			
			MemoryStream imageStream = new MemoryStream();
			chart1.SaveImage(imageStream, ChartImageFormat.Png);
			chart1.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;			
			Response.ContentType = "image/png";
			imageStream.WriteTo(Response.OutputStream);
			g.Dispose();
			image.Dispose();
			return null;
		}
	}
}