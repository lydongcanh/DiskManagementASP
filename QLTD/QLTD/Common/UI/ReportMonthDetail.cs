//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using System.Linq;
//using System.Web;
//using Ehr.Bussiness;
//using Ehr.Common.Constraint;
//using Ehr.Common.Tools;
//using Ehr.Models;

//namespace Ehr.Common.UI
//{
//	public class ReportMonthDetail
//	{
//		public static DataTable Init_Table1 ( string tablename )
//		{
//			DataTable table = new DataTable ( tablename );
//			table.Columns.Add ( new DataColumn ( "Region",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Số nhân viên cần tuyển",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Số hồ sơ cần có",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Actual CV",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Unused Lead of Month",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Khu vực chưa tuyển dụng",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Scan_Pass",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Scan_Failed",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Scan_Pending",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Scan_Total",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R1_Pass",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R1_Failed",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R1_Pending",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R1_Total",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R2_Pass",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R2_Failed",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R2_Pending",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "R2_Total",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Convertion (HR/RFM)",typeof ( System.Double ) ) );
//			table.Columns.Add ( new DataColumn ( "Convertion (Sourcing/RFM)",typeof ( System.Double ) ) );
//			table.AcceptChanges ( );
//			return table;
//		}
//		public static DataSet Graph1 ( UnitWork unitWork,string idproject,string valueoption,string option,string year )
//		{
//			DataSet ds = new DataSet ( );
//			var idprj = int.Parse ( idproject );
//			var opt = int.Parse ( option );
//			var value = int.Parse ( valueoption );
//			var yr = int.Parse ( year );
//			//var project = unitWork.EProject.GetById ( idprj );
//			if(project == null)
//				return ds;
//			//xác định các tháng cần báo cáo
//			List<MonthYearReporter> list = new List<MonthYearReporter> ( );//dòng dữ liệu
//			if(opt == 1)
//			{
//				//add 1 tháng
//				list.Add ( new MonthYearReporter ( ) { Month = value,Year = yr } );
//			}
//			else if(opt == 2)
//			{
//				//add 3 tháng theo quý
//				if(value == 1)
//				{
//					for(int i = 1;i <= 3;i++)
//						list.Add ( new MonthYearReporter ( ) { Month = i,Year = yr } );
//				}
//				else if(value == 2)
//				{
//					for(int i = 4;i <= 6;i++)
//						list.Add ( new MonthYearReporter ( ) { Month = i,Year = yr } );
//				}
//				else if(value == 3)
//				{
//					for(int i = 7;i <= 9;i++)
//						list.Add ( new MonthYearReporter ( ) { Month = i,Year = yr } );
//				}
//				else if(value == 4)
//				{
//					for(int i = 10;i <= 12;i++)
//						list.Add ( new MonthYearReporter ( ) { Month = i,Year = yr } );
//				}
//			}
//			else
//			{
//				//theo năm
//				for(int i = 1;i <= 12;i++)
//					list.Add ( new MonthYearReporter ( ) { Month = i,Year = yr } );
//			}
//			//lấy các region theo project
//			var regions = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( );//.OrderBy ( c => c.Id ).ToList ( );
//			var regions2 = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( );
//			regions = regions.Union ( regions2 ).Distinct ( ).OrderBy ( c => c.Id ).ToList ( );
//			//lấy danh sách các candidate theo project và năm
//			var candidates = unitWork.Candidate.Get ( c => c.Form.Project.Id == project.Id && c.SubmissionDate.Value.Year == yr );
//			#region khởi tạo các biến tổng
//			var total_actualcv = 0;//actual CV
//			var total_requestednum = 0;//số nhân viên cần tuyển
//			var total_requestedCV = 0;//số CV cần có
//			var total_waitcount = 0;//Chưa tuyển dụng
//			var total_unused_count = 0;
//			//scan
//			var total_cvScan_Fail = 0;
//			var total_cvScan_Pass = 0;
//			var total_cvScan_Pending = 0;
//			var total_cvScan_Total = 0;
//			//1st round
//			var total_fRoundQ_Fail = 0;
//			var total_fRoundQ_Pass = 0;
//			var total_fRoundQ_Pending = 0;
//			var total_fRoundQ_Total = 0;
//			//2nd round
//			var total_sRoundQ_Fail = 0;
//			var total_sRoundQ_Pass = 0;
//			var total_sRoundQ_Pending = 0;
//			var total_sRoundQ_Total = 0;

//			double total_conversion_HR = 0;
//			double total_conversion_SOURCING = 0;
//			#endregion
//			//mỗi region là  một datatable
//			foreach(Region region in regions)
//			{
//				var r_actualcv = 0;//actual CV
//				var r_requestednum = 0;//số nhân viên cần tuyển
//				var r_requestedCV = 0;//số CV cần có
//				var r_waitcount = 0;//Chưa tuyển dụng
//				var r_unused_count = 0;
//				//scan
//				var r_cvScan_Fail = 0;
//				var r_cvScan_Pass = 0;
//				var r_cvScan_Pending = 0;
//				var r_cvScan_Total = 0;
//				//1st round
//				var r_fRoundQ_Fail = 0;
//				var r_fRoundQ_Pass = 0;
//				var r_fRoundQ_Pending = 0;
//				var r_fRoundQ_Total = 0;
//				//2nd round
//				var r_sRoundQ_Fail = 0;
//				var r_sRoundQ_Pass = 0;
//				var r_sRoundQ_Pending = 0;
//				var r_sRoundQ_Total = 0;

//				double r_conversion_HR = 0;
//				double r_conversion_SOURCING = 0;

//				DataTable table = Init_Table1 ( region.RegionName ).Clone ( );
//				foreach(MonthYearReporter m in list)
//				{
//					var f_cand = candidates.Where ( s => s.CityRegion.Region.Id == region.Id && s.SubmissionDate?.Month == m.Month );// Actual CV					
//					var unused = f_cand.Where ( c => c.Approval == ApprovalStatus.PENDING );//Unused Lead of Month
//					var requestednum1 = project.ProjectNewDetail.Where ( f => f.Store.CityRegion.Region.Id == region.Id && (f.EndingDate>=m.Min && f.EndingDate<=m.Max) ).Sum ( c => c.NumberOfRequested );
//					var requestednum2 = project.ProjectReplaceDetail.Where ( f => f.Store.CityRegion.Region.Id == region.Id && (f.EndingDate>=m.Min && f.EndingDate<=m.Max) ).Sum ( c => c.NumberOfRequested );

//					var wait = f_cand.Where ( c => c.Approval == ApprovalStatus.WAIT );//Khu vực chưa tuyển dụng

//					var candidateCVScan = f_cand.Where ( s => s.Round1_Date != null );
//					var candidateFirstRound = f_cand.Where ( s => s.Round2_Date != null );
//					var candidateSecondRound = f_cand.Where ( s => s.Round3_Date != null );

//					var actualcv = f_cand.Count ( );//actual CV
//					var requestednum = requestednum1 + requestednum2;//số nhân viên cần tuyển
//					var requestedCV = requestednum * 10;//số CV cần có
//					var waitcount = wait.Count ( );//Chưa tuyển dụng
//					var unused_count = unused.Count ( );
//					//scan
//					var cvScan_Fail = candidateCVScan.Where ( s => s.Result_R1 == Result.FAILED ).Count ( );
//					var cvScan_Pass = candidateCVScan.Where ( s => s.Result_R1 == Result.PASSED ).Count ( );
//					var cvScan_Pending = candidateCVScan.Where ( s => s.Result_R1 == Result.WAIT ).Count ( );
//					var cvScan_Total = cvScan_Fail + cvScan_Pass + cvScan_Pending;
//					//1st round
//					var fRoundQ_Fail = candidateFirstRound.Where ( s => s.Result_R2 == Result.FAILED ).Count ( );
//					var fRoundQ_Pass = candidateFirstRound.Where ( s => s.Result_R2 == Result.PASSED ).Count ( );
//					var fRoundQ_Pending = candidateFirstRound.Where ( s => s.Result_R2 == Result.WAIT ).Count ( );
//					var fRoundQ_Total = fRoundQ_Fail + fRoundQ_Pass + fRoundQ_Pending;
//					//2nd round
//					var sRoundQ_Fail = candidateSecondRound.Where ( s => s.Result_R3 == Result.FAILED ).Count ( );
//					var sRoundQ_Pass = candidateSecondRound.Where ( s => s.Result_R3 == Result.PASSED ).Count ( );
//					var sRoundQ_Pending = candidateSecondRound.Where ( s => s.Result_R3 == Result.WAIT ).Count ( );
//					var sRoundQ_Total = sRoundQ_Fail + sRoundQ_Pass + sRoundQ_Pending;

//					double conversion_HR = 0;
//					double conversion_SOURCING = 0;
//					if(sRoundQ_Pass > 0)
//					{
//						conversion_HR = Math.Round ( ((double)cvScan_Pass / sRoundQ_Pass),1 );
//						conversion_SOURCING = Math.Round ( ((double)actualcv / sRoundQ_Pass),1 );
//					}
//					#region  thêm 1 row bình thường cho datatable
//					DataRow row = table.NewRow ( );
//					row["Region"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName ( m.Month );
//					row["Số nhân viên cần tuyển"] = requestednum;
//					row["Số hồ sơ cần có"] = requestedCV;
//					row["Actual CV"] = actualcv;
//					row["Unused Lead of Month"] = unused_count;
//					row["Khu vực chưa tuyển dụng"] = waitcount;
//					row["Scan_Pass"] = cvScan_Pass;
//					row["Scan_Failed"] = cvScan_Fail;
//					row["Scan_Pending"] = cvScan_Pending;
//					row["Scan_Total"] = cvScan_Total;
//					row["R1_Pass"] = fRoundQ_Pass;
//					row["R1_Failed"] = fRoundQ_Fail;
//					row["R1_Pending"] = fRoundQ_Pending;
//					row["R1_Total"] = fRoundQ_Total;
//					row["R2_Pass"] = sRoundQ_Pass;
//					row["R2_Failed"] = sRoundQ_Fail;
//					row["R2_Pending"] = sRoundQ_Pending;
//					row["R2_Total"] = sRoundQ_Total;
//					row["Convertion (HR/RFM)"] = conversion_HR;
//					row["Convertion (Sourcing/RFM)"] = conversion_SOURCING;
//					table.Rows.Add ( row );
//					#endregion

//					#region Thêm 1 datarow upto
//					r_actualcv += actualcv;//actual CV
//					r_requestednum += requestednum;//số nhân viên cần tuyển
//					r_requestedCV += requestedCV;//số CV cần có
//					r_waitcount += waitcount;//Chưa tuyển dụng
//					r_unused_count += unused_count;
//					//scan
//					r_cvScan_Fail += cvScan_Fail;
//					r_cvScan_Pass += cvScan_Pass;
//					r_cvScan_Pending += cvScan_Pending;
//					r_cvScan_Total += cvScan_Total;
//					//1st round
//					r_fRoundQ_Fail += fRoundQ_Fail;
//					r_fRoundQ_Pass += fRoundQ_Pass;
//					r_fRoundQ_Pending += fRoundQ_Pending;
//					r_fRoundQ_Total += fRoundQ_Total;
//					//2nd round
//					r_sRoundQ_Fail += sRoundQ_Fail;
//					r_sRoundQ_Pass += sRoundQ_Pass;
//					r_sRoundQ_Pending += sRoundQ_Pending;
//					r_sRoundQ_Total += sRoundQ_Total;

//					if(r_sRoundQ_Pass > 0)
//					{
//						r_conversion_HR = Math.Round ( ((double)r_cvScan_Pass / r_sRoundQ_Pass),1 );
//						r_conversion_SOURCING = Math.Round ( ((double)r_actualcv / r_sRoundQ_Pass),1 );
//					}

//					row = table.NewRow ( );
//					row["Region"] = "Up to " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName ( m.Month );
//					row["Số nhân viên cần tuyển"] = r_requestednum;
//					row["Số hồ sơ cần có"] = r_requestedCV;
//					row["Actual CV"] = r_actualcv;
//					row["Unused Lead of Month"] = r_unused_count;
//					row["Khu vực chưa tuyển dụng"] = r_waitcount;
//					row["Scan_Pass"] = r_cvScan_Pass;
//					row["Scan_Failed"] = r_cvScan_Fail;
//					row["Scan_Pending"] = r_cvScan_Pending;
//					row["Scan_Total"] = r_cvScan_Total;
//					row["R1_Pass"] = r_fRoundQ_Pass;
//					row["R1_Failed"] = r_fRoundQ_Fail;
//					row["R1_Pending"] = r_fRoundQ_Pending;
//					row["R1_Total"] = r_fRoundQ_Total;
//					row["R2_Pass"] = r_sRoundQ_Pass;
//					row["R2_Failed"] = r_sRoundQ_Fail;
//					row["R2_Pending"] = r_sRoundQ_Pending;
//					row["R2_Total"] = r_sRoundQ_Total;
//					row["Convertion (HR/RFM)"] = r_conversion_HR;
//					row["Convertion (Sourcing/RFM)"] = r_conversion_SOURCING;
//					table.Rows.Add ( row );
//					#endregion
//				}
//				#region thêm row đầu tiên cho region
//				DataRow s_row = table.NewRow ( );
//				s_row["Region"] = region.RegionName;
//				s_row["Số nhân viên cần tuyển"] = r_requestednum;
//				s_row["Số hồ sơ cần có"] = r_requestedCV;
//				s_row["Actual CV"] = r_actualcv;
//				s_row["Unused Lead of Month"] = r_unused_count;
//				s_row["Khu vực chưa tuyển dụng"] = r_waitcount;
//				s_row["Scan_Pass"] = r_cvScan_Pass;
//				s_row["Scan_Failed"] = r_cvScan_Fail;
//				s_row["Scan_Pending"] = r_cvScan_Pending;
//				s_row["Scan_Total"] = r_cvScan_Total;
//				s_row["R1_Pass"] = r_fRoundQ_Pass;
//				s_row["R1_Failed"] = r_fRoundQ_Fail;
//				s_row["R1_Pending"] = r_fRoundQ_Pending;
//				s_row["R1_Total"] = r_fRoundQ_Total;
//				s_row["R2_Pass"] = r_sRoundQ_Pass;
//				s_row["R2_Failed"] = r_sRoundQ_Fail;
//				s_row["R2_Pending"] = r_sRoundQ_Pending;
//				s_row["R2_Total"] = r_sRoundQ_Total;
//				s_row["Convertion (HR/RFM)"] = r_conversion_HR;
//				s_row["Convertion (Sourcing/RFM)"] = r_conversion_SOURCING;
//				table.Rows.InsertAt ( s_row,0 );
//				#endregion

//				#region Tính các chỉ số tổng
//				total_actualcv += r_actualcv;//actual CV
//				total_requestednum += r_requestednum;//số nhân viên cần tuyển
//				total_requestedCV += r_requestedCV;//số CV cần có
//				total_waitcount += r_waitcount;//Chưa tuyển dụng
//				total_unused_count += r_unused_count;
//				//scan
//				total_cvScan_Fail += r_cvScan_Fail;
//				total_cvScan_Pass += r_cvScan_Pass;
//				total_cvScan_Pending += r_cvScan_Pending;
//				total_cvScan_Total += r_cvScan_Total;
//				//1st round
//				total_fRoundQ_Fail += r_fRoundQ_Fail;
//				total_fRoundQ_Pass += r_fRoundQ_Pass;
//				total_fRoundQ_Pending += r_fRoundQ_Pending;
//				total_fRoundQ_Total += r_fRoundQ_Total;
//				//2nd round
//				total_sRoundQ_Fail += r_sRoundQ_Fail;
//				total_sRoundQ_Pass += r_sRoundQ_Pass;
//				total_sRoundQ_Pending += r_sRoundQ_Pending;
//				total_sRoundQ_Total += r_sRoundQ_Total;
//				#endregion
//				ds.Tables.Add ( table.Copy ( ) );
//			}
//			//khởi tạo datatable cuối cùng
//			if(total_sRoundQ_Pass > 0)
//			{
//				total_conversion_HR = Math.Round ( ((double)total_cvScan_Pass / total_sRoundQ_Pass),1 );
//				total_conversion_SOURCING = Math.Round ( ((double)total_actualcv / total_sRoundQ_Pass),1 );
//			}
//			DataTable _table = Init_Table1 ( "Total" ).Clone ( );
//			DataRow _row = _table.NewRow ( );
//			_row["Region"] = "Total";
//			_row["Số nhân viên cần tuyển"] = total_requestednum;
//			_row["Số hồ sơ cần có"] = total_requestedCV;
//			_row["Actual CV"] = total_actualcv;
//			_row["Unused Lead of Month"] = total_unused_count;
//			_row["Khu vực chưa tuyển dụng"] = total_waitcount;
//			_row["Scan_Pass"] = total_cvScan_Pass;
//			_row["Scan_Failed"] = total_cvScan_Fail;
//			_row["Scan_Pending"] = total_cvScan_Pending;
//			_row["Scan_Total"] = total_cvScan_Total;
//			_row["R1_Pass"] = total_fRoundQ_Pass;
//			_row["R1_Failed"] = total_fRoundQ_Fail;
//			_row["R1_Pending"] = total_fRoundQ_Pending;
//			_row["R1_Total"] = total_fRoundQ_Total;
//			_row["R2_Pass"] = total_sRoundQ_Pass;
//			_row["R2_Failed"] = total_sRoundQ_Fail;
//			_row["R2_Pending"] = total_sRoundQ_Pending;
//			_row["R2_Total"] = total_sRoundQ_Total;
//			_row["Convertion (HR/RFM)"] = total_conversion_HR;
//			_row["Convertion (Sourcing/RFM)"] = total_conversion_SOURCING;
//			_table.Rows.InsertAt ( _row,0 );
//			ds.Tables.Add ( _table.Copy ( ) );
//			return ds;
//		}


//		public static DataSet Graph2 ( UnitWork unitWork,string idproject,string valueoption,string option,string year )
//		{
//			DataSet ds = new DataSet ( );
//			var idprj = int.Parse ( idproject );
//			var opt = int.Parse ( option );
//			var value = int.Parse ( valueoption );
//			var yr = int.Parse ( year );
//			var frommonth = 1;
//			var tomonth = 12;
//			if(opt == 1)
//			{
//				frommonth = value;
//				tomonth = value;
//			}
//			else if(opt == 2)
//			{
//				//add 3 tháng theo quý
//				if(value == 1)
//				{
//					frommonth = 1;
//					tomonth = 3;
//				}
//				else if(value == 2)
//				{
//					frommonth = 4;
//					tomonth = 6;
//				}
//				else if(value == 3)
//				{
//					frommonth = 7;
//					tomonth = 9;
//				}
//				else if(value == 4)
//				{
//					frommonth = 10;
//					tomonth = 12;
//				}
//			}
//			MonthYearReporter m1 = new MonthYearReporter ( ) { Month = frommonth,Year = yr };
//			MonthYearReporter m2 = new MonthYearReporter ( ) { Month = tomonth,Year = yr };

//			var project = unitWork.EProject.GetById ( idprj );
//			if(project == null)
//				return ds;
			
//			//lấy các region theo project
//			var regions = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( );//.OrderBy ( c => c.Id ).ToList ( );
//			var regions2 = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( );
//			regions = regions.Union ( regions2 ).Distinct ( ).OrderBy ( c => c.Id ).ToList ( );
//			//lấy danh sách các candidate theo project và năm
//			var candidates = unitWork.Candidate.Get ( c => c.Form.Project.Id == project.Id && c.SubmissionDate.Value.Year == yr );
//			#region khởi tạo các biến tổng
//			var total_actualcv = 0;//actual CV
//			var total_requestednum = 0;//số nhân viên cần tuyển
//			var total_requestedCV = 0;//số CV cần có
//			var total_waitcount = 0;//Chưa tuyển dụng
//			var total_unused_count = 0;
//			//scan
//			var total_cvScan_Fail = 0;
//			var total_cvScan_Pass = 0;
//			var total_cvScan_Pending = 0;
//			var total_cvScan_Total = 0;
//			//1st round
//			var total_fRoundQ_Fail = 0;
//			var total_fRoundQ_Pass = 0;
//			var total_fRoundQ_Pending = 0;
//			var total_fRoundQ_Total = 0;
//			//2nd round
//			var total_sRoundQ_Fail = 0;
//			var total_sRoundQ_Pass = 0;
//			var total_sRoundQ_Pending = 0;
//			var total_sRoundQ_Total = 0;

//			double total_conversion_HR = 0;
//			double total_conversion_SOURCING = 0;
//			#endregion
//			//mỗi region là  một datatable
//			foreach(Region region in regions)
//			{
//				var r_actualcv = 0;//actual CV
//				var r_requestednum = 0;//số nhân viên cần tuyển
//				var r_requestedCV = 0;//số CV cần có
//				var r_waitcount = 0;//Chưa tuyển dụng
//				var r_unused_count = 0;
//				//scan
//				var r_cvScan_Fail = 0;
//				var r_cvScan_Pass = 0;
//				var r_cvScan_Pending = 0;
//				var r_cvScan_Total = 0;
//				//1st round
//				var r_fRoundQ_Fail = 0;
//				var r_fRoundQ_Pass = 0;
//				var r_fRoundQ_Pending = 0;
//				var r_fRoundQ_Total = 0;
//				//2nd round
//				var r_sRoundQ_Fail = 0;
//				var r_sRoundQ_Pass = 0;
//				var r_sRoundQ_Pending = 0;
//				var r_sRoundQ_Total = 0;

//				double r_conversion_HR = 0;
//				double r_conversion_SOURCING = 0;

//				DataTable table = Init_Table1 ( region.RegionName ).Clone ( );
//				//lấy danh sách các tỉnh thành của region
//				var cities = unitWork.CityRegion.Get ( c => c.Region.Id ==region.Id && c.Customer.Id == project.Customer.Id ).OrderBy(c=>c.City.CityName).ToList();
//				foreach(CityRegion city in cities)
//				{
//					var f_cand = candidates.Where ( s => s.CityRegion.Id == city.Id && (s.SubmissionDate?.Month >= frommonth &&  s.SubmissionDate?.Month <= tomonth));// Actual CV					
//					var unused = f_cand.Where ( c => c.Approval == ApprovalStatus.PENDING );//Unused Lead of Month
//					var requestednum1 = project.ProjectNewDetail.Where ( f => f.Store.CityRegion.Id == city.Id && (f.EndingDate>=m1.Min && f.EndingDate<=m2.Max) ).Sum ( c => c.NumberOfRequested );

//					var requestednum2 = project.ProjectReplaceDetail.Where ( f => f.Store.CityRegion.Id == city.Id && (f.EndingDate>=m1.Min && f.EndingDate<=m2.Max) ).Sum ( c => c.NumberOfRequested );

//					var wait = f_cand.Where ( c => c.Approval == ApprovalStatus.WAIT );//Khu vực chưa tuyển dụng

//					var candidateCVScan = f_cand.Where ( s => s.Round1_Date != null );
//					var candidateFirstRound = f_cand.Where ( s => s.Round2_Date != null );
//					var candidateSecondRound = f_cand.Where ( s => s.Round3_Date != null );

//					var actualcv = f_cand.Count ( );//actual CV
//					var requestednum = requestednum1 + requestednum2;//số nhân viên cần tuyển
//					var requestedCV = requestednum * 10;//số CV cần có
//					var waitcount = wait.Count ( );//Chưa tuyển dụng
//					var unused_count = unused.Count ( );
//					//scan
//					var cvScan_Fail = candidateCVScan.Where ( s => s.Result_R1 == Result.FAILED ).Count ( );
//					var cvScan_Pass = candidateCVScan.Where ( s => s.Result_R1 == Result.PASSED ).Count ( );
//					var cvScan_Pending = candidateCVScan.Where ( s => s.Result_R1 == Result.WAIT ).Count ( );
//					var cvScan_Total = cvScan_Fail + cvScan_Pass + cvScan_Pending;
//					//1st round
//					var fRoundQ_Fail = candidateFirstRound.Where ( s => s.Result_R2 == Result.FAILED ).Count ( );
//					var fRoundQ_Pass = candidateFirstRound.Where ( s => s.Result_R2 == Result.PASSED ).Count ( );
//					var fRoundQ_Pending = candidateFirstRound.Where ( s => s.Result_R2 == Result.WAIT ).Count ( );
//					var fRoundQ_Total = fRoundQ_Fail + fRoundQ_Pass + fRoundQ_Pending;
//					//2nd round
//					var sRoundQ_Fail = candidateSecondRound.Where ( s => s.Result_R3 == Result.FAILED ).Count ( );
//					var sRoundQ_Pass = candidateSecondRound.Where ( s => s.Result_R3 == Result.PASSED ).Count ( );
//					var sRoundQ_Pending = candidateSecondRound.Where ( s => s.Result_R3 == Result.WAIT ).Count ( );
//					var sRoundQ_Total = sRoundQ_Fail + sRoundQ_Pass + sRoundQ_Pending;

//					double conversion_HR = 0;
//					double conversion_SOURCING = 0;
//					if(sRoundQ_Pass > 0)
//					{
//						conversion_HR = Math.Round ( ((double)cvScan_Pass / sRoundQ_Pass),1 );
//						conversion_SOURCING = Math.Round ( ((double)actualcv / sRoundQ_Pass),1 );
//					}
//					#region  thêm 1 row bình thường cho datatable
//					DataRow row = table.NewRow ( );
//					row["Region"] = city.City.CityName;
//					row["Số nhân viên cần tuyển"] = requestednum;
//					row["Số hồ sơ cần có"] = requestedCV;
//					row["Actual CV"] = actualcv;
//					row["Unused Lead of Month"] = unused_count;
//					row["Khu vực chưa tuyển dụng"] = waitcount;
//					row["Scan_Pass"] = cvScan_Pass;
//					row["Scan_Failed"] = cvScan_Fail;
//					row["Scan_Pending"] = cvScan_Pending;
//					row["Scan_Total"] = cvScan_Total;
//					row["R1_Pass"] = fRoundQ_Pass;
//					row["R1_Failed"] = fRoundQ_Fail;
//					row["R1_Pending"] = fRoundQ_Pending;
//					row["R1_Total"] = fRoundQ_Total;
//					row["R2_Pass"] = sRoundQ_Pass;
//					row["R2_Failed"] = sRoundQ_Fail;
//					row["R2_Pending"] = sRoundQ_Pending;
//					row["R2_Total"] = sRoundQ_Total;
//					row["Convertion (HR/RFM)"] = conversion_HR;
//					row["Convertion (Sourcing/RFM)"] = conversion_SOURCING;
//					table.Rows.Add ( row );
//					#endregion

//					#region Thêm 1 datarow upto
//					r_actualcv += actualcv;//actual CV
//					r_requestednum += requestednum;//số nhân viên cần tuyển
//					r_requestedCV += requestedCV;//số CV cần có
//					r_waitcount += waitcount;//Chưa tuyển dụng
//					r_unused_count += unused_count;
//					//scan
//					r_cvScan_Fail += cvScan_Fail;
//					r_cvScan_Pass += cvScan_Pass;
//					r_cvScan_Pending += cvScan_Pending;
//					r_cvScan_Total += cvScan_Total;
//					//1st round
//					r_fRoundQ_Fail += fRoundQ_Fail;
//					r_fRoundQ_Pass += fRoundQ_Pass;
//					r_fRoundQ_Pending += fRoundQ_Pending;
//					r_fRoundQ_Total += fRoundQ_Total;
//					//2nd round
//					r_sRoundQ_Fail += sRoundQ_Fail;
//					r_sRoundQ_Pass += sRoundQ_Pass;
//					r_sRoundQ_Pending += sRoundQ_Pending;
//					r_sRoundQ_Total += sRoundQ_Total;

//					if(r_sRoundQ_Pass > 0)
//					{
//						r_conversion_HR = Math.Round ( ((double)r_cvScan_Pass / r_sRoundQ_Pass),1 );
//						r_conversion_SOURCING = Math.Round ( ((double)r_actualcv / r_sRoundQ_Pass),1 );
//					}
//					#endregion
//				}
//				#region thêm row đầu tiên cho region
//				DataRow s_row = table.NewRow ( );
//				s_row["Region"] = region.RegionName;
//				s_row["Số nhân viên cần tuyển"] = r_requestednum;
//				s_row["Số hồ sơ cần có"] = r_requestedCV;
//				s_row["Actual CV"] = r_actualcv;
//				s_row["Unused Lead of Month"] = r_unused_count;
//				s_row["Khu vực chưa tuyển dụng"] = r_waitcount;
//				s_row["Scan_Pass"] = r_cvScan_Pass;
//				s_row["Scan_Failed"] = r_cvScan_Fail;
//				s_row["Scan_Pending"] = r_cvScan_Pending;
//				s_row["Scan_Total"] = r_cvScan_Total;
//				s_row["R1_Pass"] = r_fRoundQ_Pass;
//				s_row["R1_Failed"] = r_fRoundQ_Fail;
//				s_row["R1_Pending"] = r_fRoundQ_Pending;
//				s_row["R1_Total"] = r_fRoundQ_Total;
//				s_row["R2_Pass"] = r_sRoundQ_Pass;
//				s_row["R2_Failed"] = r_sRoundQ_Fail;
//				s_row["R2_Pending"] = r_sRoundQ_Pending;
//				s_row["R2_Total"] = r_sRoundQ_Total;
//				s_row["Convertion (HR/RFM)"] = r_conversion_HR;
//				s_row["Convertion (Sourcing/RFM)"] = r_conversion_SOURCING;
//				table.Rows.InsertAt ( s_row,0 );
//				#endregion

//				#region Tính các chỉ số tổng
//				total_actualcv += r_actualcv;//actual CV
//				total_requestednum += r_requestednum;//số nhân viên cần tuyển
//				total_requestedCV += r_requestedCV;//số CV cần có
//				total_waitcount += r_waitcount;//Chưa tuyển dụng
//				total_unused_count += r_unused_count;
//				//scan
//				total_cvScan_Fail += r_cvScan_Fail;
//				total_cvScan_Pass += r_cvScan_Pass;
//				total_cvScan_Pending += r_cvScan_Pending;
//				total_cvScan_Total += r_cvScan_Total;
//				//1st round
//				total_fRoundQ_Fail += r_fRoundQ_Fail;
//				total_fRoundQ_Pass += r_fRoundQ_Pass;
//				total_fRoundQ_Pending += r_fRoundQ_Pending;
//				total_fRoundQ_Total += r_fRoundQ_Total;
//				//2nd round
//				total_sRoundQ_Fail += r_sRoundQ_Fail;
//				total_sRoundQ_Pass += r_sRoundQ_Pass;
//				total_sRoundQ_Pending += r_sRoundQ_Pending;
//				total_sRoundQ_Total += r_sRoundQ_Total;
//				#endregion
//				ds.Tables.Add ( table.Copy ( ) );
//			}
//			//khởi tạo datatable cuối cùng
//			if(total_sRoundQ_Pass > 0)
//			{
//				total_conversion_HR = Math.Round ( ((double)total_cvScan_Pass / total_sRoundQ_Pass),1 );
//				total_conversion_SOURCING = Math.Round ( ((double)total_actualcv / total_sRoundQ_Pass),1 );
//			}
//			DataTable _table = Init_Table1 ( "Total" ).Clone ( );
//			DataRow _row = _table.NewRow ( );
//			_row["Region"] = "Total";
//			_row["Số nhân viên cần tuyển"] = total_requestednum;
//			_row["Số hồ sơ cần có"] = total_requestedCV;
//			_row["Actual CV"] = total_actualcv;
//			_row["Unused Lead of Month"] = total_unused_count;
//			_row["Khu vực chưa tuyển dụng"] = total_waitcount;
//			_row["Scan_Pass"] = total_cvScan_Pass;
//			_row["Scan_Failed"] = total_cvScan_Fail;
//			_row["Scan_Pending"] = total_cvScan_Pending;
//			_row["Scan_Total"] = total_cvScan_Total;
//			_row["R1_Pass"] = total_fRoundQ_Pass;
//			_row["R1_Failed"] = total_fRoundQ_Fail;
//			_row["R1_Pending"] = total_fRoundQ_Pending;
//			_row["R1_Total"] = total_fRoundQ_Total;
//			_row["R2_Pass"] = total_sRoundQ_Pass;
//			_row["R2_Failed"] = total_sRoundQ_Fail;
//			_row["R2_Pending"] = total_sRoundQ_Pending;
//			_row["R2_Total"] = total_sRoundQ_Total;
//			_row["Convertion (HR/RFM)"] = total_conversion_HR;
//			_row["Convertion (Sourcing/RFM)"] = total_conversion_SOURCING;
//			_table.Rows.InsertAt ( _row,0 );
//			ds.Tables.Add ( _table.Copy ( ) );
//			return ds;
//		}


//		public static DataSet Graph3 ( UnitWork unitWork,string idproject,string valueoption,string option,string year )
//		{
//			DataSet ds = new DataSet ( );
//			var idprj = int.Parse ( idproject );
//			var opt = int.Parse ( option );
//			var value = int.Parse ( valueoption );
//			var yr = int.Parse ( year );
//			var frommonth = 1;
//			var tomonth = 12;
//			if(opt == 1)
//			{
//				frommonth = value;
//				tomonth = value;
//			}
//			else if(opt == 2)
//			{
//				//add 3 tháng theo quý
//				if(value == 1)
//				{
//					frommonth = 1;
//					tomonth = 3;
//				}
//				else if(value == 2)
//				{
//					frommonth = 4;
//					tomonth = 6;
//				}
//				else if(value == 3)
//				{
//					frommonth = 7;
//					tomonth = 9;
//				}
//				else if(value == 4)
//				{
//					frommonth = 10;
//					tomonth = 12;
//				}
//			}
//			MonthYearReporter m1 = new MonthYearReporter ( ) { Month = frommonth,Year = yr };
//			MonthYearReporter m2 = new MonthYearReporter ( ) { Month = tomonth,Year = yr };

//			var project = unitWork.EProject.GetById ( idprj );
//			if(project == null)
//				return ds;
			
//			//lấy các region theo project
//			var regions = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( );//.OrderBy ( c => c.Id ).ToList ( );
//			var regions2 = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( );
//			regions = regions.Union ( regions2 ).Distinct ( ).OrderBy ( c => c.Id ).ToList ( );
//			//lấy danh sách các candidate theo project và năm
//			var candidates = unitWork.Candidate.Get ( c => c.Form.Project.Id == project.Id && c.SubmissionDate.Value.Year == yr );
//			#region khởi tạo các biến tổng
//			var total_actualcv = 0;//actual CV
//			var total_requestednum = 0;//số nhân viên cần tuyển
//			var total_requestedCV = 0;//số CV cần có
//			var total_waitcount = 0;//Chưa tuyển dụng
//			var total_unused_count = 0;
//			//scan
//			var total_cvScan_Fail = 0;
//			var total_cvScan_Pass = 0;
//			var total_cvScan_Pending = 0;
//			var total_cvScan_Total = 0;
//			//1st round
//			var total_fRoundQ_Fail = 0;
//			var total_fRoundQ_Pass = 0;
//			var total_fRoundQ_Pending = 0;
//			var total_fRoundQ_Total = 0;
//			//2nd round
//			var total_sRoundQ_Fail = 0;
//			var total_sRoundQ_Pass = 0;
//			var total_sRoundQ_Pending = 0;
//			var total_sRoundQ_Total = 0;

//			double total_conversion_HR = 0;
//			double total_conversion_SOURCING = 0;
//			#endregion
//			//mỗi region là  một datatable
//			foreach(Region region in regions)
//			{
//				var r_actualcv = 0;//actual CV
//				var r_requestednum = 0;//số nhân viên cần tuyển
//				var r_requestedCV = 0;//số CV cần có
//				var r_waitcount = 0;//Chưa tuyển dụng
//				var r_unused_count = 0;
//				//scan
//				var r_cvScan_Fail = 0;
//				var r_cvScan_Pass = 0;
//				var r_cvScan_Pending = 0;
//				var r_cvScan_Total = 0;
//				//1st round
//				var r_fRoundQ_Fail = 0;
//				var r_fRoundQ_Pass = 0;
//				var r_fRoundQ_Pending = 0;
//				var r_fRoundQ_Total = 0;
//				//2nd round
//				var r_sRoundQ_Fail = 0;
//				var r_sRoundQ_Pass = 0;
//				var r_sRoundQ_Pending = 0;
//				var r_sRoundQ_Total = 0;

//				double r_conversion_HR = 0;
//				double r_conversion_SOURCING = 0;

//				DataTable table = Init_Table1 ( region.RegionName ).Clone ( );
//				//lấy danh sách các rfm
//				var frm1 = project.ProjectNewDetail.Where ( p => p.Store.CityRegion.Region.Id == region.Id ).Select ( c => new PairRFM_Cityregion ( ) { RFM=c.RegionalManager, Region=c.Store.CityRegion.Region} );
//				var frm2 = project.ProjectReplaceDetail.Where ( p => p.Store.CityRegion.Region.Id == region.Id ).Select ( c => new PairRFM_Cityregion ( ) { RFM=c.RegionalManager, Region=c.Store.CityRegion.Region} );
//				var frm = frm1.Union ( frm2,new PairRFM_CityregionComparer() );
//				//frm = frm.Distinct ( ).ToList ( );
//				foreach(PairRFM_Cityregion _frm in frm)
//				{
//					var f_cand = candidates.Where ( s => s.CityRegion.Region.Id == _frm.Region.Id && (s.SubmissionDate?.Month >= frommonth &&  s.SubmissionDate?.Month <= tomonth));// Actual CV					
//					var unused = f_cand.Where ( c => c.Approval == ApprovalStatus.PENDING );//Unused Lead of Month
//					var requestednum1 = project.ProjectNewDetail.Where ( f => f.Store.CityRegion.Region.Id == _frm.Region.Id && (f.EndingDate>=m1.Min && f.EndingDate<=m2.Max) ).Sum ( c => c.NumberOfRequested );

//					var requestednum2 = project.ProjectReplaceDetail.Where ( f => f.Store.CityRegion.Region.Id == _frm.Region.Id && (f.EndingDate>=m1.Min && f.EndingDate<=m2.Max) ).Sum ( c => c.NumberOfRequested );

//					var wait = f_cand.Where ( c => c.Approval == ApprovalStatus.WAIT );//Khu vực chưa tuyển dụng

//					var candidateCVScan = f_cand.Where ( s => s.Round1_Date != null );
//					var candidateFirstRound = f_cand.Where ( s => s.Round2_Date != null );
//					var candidateSecondRound = f_cand.Where ( s => s.Round3_Date != null );

//					var actualcv = f_cand.Count ( );//actual CV
//					var requestednum = requestednum1 + requestednum2;//số nhân viên cần tuyển
//					var requestedCV = requestednum * 10;//số CV cần có
//					var waitcount = wait.Count ( );//Chưa tuyển dụng
//					var unused_count = unused.Count ( );
//					//scan
//					var cvScan_Fail = candidateCVScan.Where ( s => s.Result_R1 == Result.FAILED ).Count ( );
//					var cvScan_Pass = candidateCVScan.Where ( s => s.Result_R1 == Result.PASSED ).Count ( );
//					var cvScan_Pending = candidateCVScan.Where ( s => s.Result_R1 == Result.WAIT ).Count ( );
//					var cvScan_Total = cvScan_Fail + cvScan_Pass + cvScan_Pending;
//					//1st round
//					var fRoundQ_Fail = candidateFirstRound.Where ( s => s.Result_R2 == Result.FAILED ).Count ( );
//					var fRoundQ_Pass = candidateFirstRound.Where ( s => s.Result_R2 == Result.PASSED ).Count ( );
//					var fRoundQ_Pending = candidateFirstRound.Where ( s => s.Result_R2 == Result.WAIT ).Count ( );
//					var fRoundQ_Total = fRoundQ_Fail + fRoundQ_Pass + fRoundQ_Pending;
//					//2nd round
//					var sRoundQ_Fail = candidateSecondRound.Where ( s => s.Result_R3 == Result.FAILED ).Count ( );
//					var sRoundQ_Pass = candidateSecondRound.Where ( s => s.Result_R3 == Result.PASSED ).Count ( );
//					var sRoundQ_Pending = candidateSecondRound.Where ( s => s.Result_R3 == Result.WAIT ).Count ( );
//					var sRoundQ_Total = sRoundQ_Fail + sRoundQ_Pass + sRoundQ_Pending;

//					double conversion_HR = 0;
//					double conversion_SOURCING = 0;
//					if(sRoundQ_Pass > 0)
//					{
//						conversion_HR = Math.Round ( ((double)cvScan_Pass / sRoundQ_Pass),1 );
//						conversion_SOURCING = Math.Round ( ((double)actualcv / sRoundQ_Pass),1 );
//					}
//					#region  thêm 1 row bình thường cho datatable
//					DataRow row = table.NewRow ( );
//					row["Region"] = _frm.RFM;
//					row["Số nhân viên cần tuyển"] = requestednum;
//					row["Số hồ sơ cần có"] = requestedCV;
//					row["Actual CV"] = actualcv;
//					row["Unused Lead of Month"] = unused_count;
//					row["Khu vực chưa tuyển dụng"] = waitcount;
//					row["Scan_Pass"] = cvScan_Pass;
//					row["Scan_Failed"] = cvScan_Fail;
//					row["Scan_Pending"] = cvScan_Pending;
//					row["Scan_Total"] = cvScan_Total;
//					row["R1_Pass"] = fRoundQ_Pass;
//					row["R1_Failed"] = fRoundQ_Fail;
//					row["R1_Pending"] = fRoundQ_Pending;
//					row["R1_Total"] = fRoundQ_Total;
//					row["R2_Pass"] = sRoundQ_Pass;
//					row["R2_Failed"] = sRoundQ_Fail;
//					row["R2_Pending"] = sRoundQ_Pending;
//					row["R2_Total"] = sRoundQ_Total;
//					row["Convertion (HR/RFM)"] = conversion_HR;
//					row["Convertion (Sourcing/RFM)"] = conversion_SOURCING;
//					table.Rows.Add ( row );
//					#endregion

//					#region Thêm 1 datarow upto
//					r_actualcv += actualcv;//actual CV
//					r_requestednum += requestednum;//số nhân viên cần tuyển
//					r_requestedCV += requestedCV;//số CV cần có
//					r_waitcount += waitcount;//Chưa tuyển dụng
//					r_unused_count += unused_count;
//					//scan
//					r_cvScan_Fail += cvScan_Fail;
//					r_cvScan_Pass += cvScan_Pass;
//					r_cvScan_Pending += cvScan_Pending;
//					r_cvScan_Total += cvScan_Total;
//					//1st round
//					r_fRoundQ_Fail += fRoundQ_Fail;
//					r_fRoundQ_Pass += fRoundQ_Pass;
//					r_fRoundQ_Pending += fRoundQ_Pending;
//					r_fRoundQ_Total += fRoundQ_Total;
//					//2nd round
//					r_sRoundQ_Fail += sRoundQ_Fail;
//					r_sRoundQ_Pass += sRoundQ_Pass;
//					r_sRoundQ_Pending += sRoundQ_Pending;
//					r_sRoundQ_Total += sRoundQ_Total;

//					if(r_sRoundQ_Pass > 0)
//					{
//						r_conversion_HR = Math.Round ( ((double)r_cvScan_Pass / r_sRoundQ_Pass),1 );
//						r_conversion_SOURCING = Math.Round ( ((double)r_actualcv / r_sRoundQ_Pass),1 );
//					}
//					#endregion
//				}
//				#region thêm row đầu tiên cho region
//				DataRow s_row = table.NewRow ( );
//				s_row["Region"] = region.RegionName;
//				s_row["Số nhân viên cần tuyển"] = r_requestednum;
//				s_row["Số hồ sơ cần có"] = r_requestedCV;
//				s_row["Actual CV"] = r_actualcv;
//				s_row["Unused Lead of Month"] = r_unused_count;
//				s_row["Khu vực chưa tuyển dụng"] = r_waitcount;
//				s_row["Scan_Pass"] = r_cvScan_Pass;
//				s_row["Scan_Failed"] = r_cvScan_Fail;
//				s_row["Scan_Pending"] = r_cvScan_Pending;
//				s_row["Scan_Total"] = r_cvScan_Total;
//				s_row["R1_Pass"] = r_fRoundQ_Pass;
//				s_row["R1_Failed"] = r_fRoundQ_Fail;
//				s_row["R1_Pending"] = r_fRoundQ_Pending;
//				s_row["R1_Total"] = r_fRoundQ_Total;
//				s_row["R2_Pass"] = r_sRoundQ_Pass;
//				s_row["R2_Failed"] = r_sRoundQ_Fail;
//				s_row["R2_Pending"] = r_sRoundQ_Pending;
//				s_row["R2_Total"] = r_sRoundQ_Total;
//				s_row["Convertion (HR/RFM)"] = r_conversion_HR;
//				s_row["Convertion (Sourcing/RFM)"] = r_conversion_SOURCING;
//				table.Rows.InsertAt ( s_row,0 );
//				#endregion

//				#region Tính các chỉ số tổng
//				total_actualcv += r_actualcv;//actual CV
//				total_requestednum += r_requestednum;//số nhân viên cần tuyển
//				total_requestedCV += r_requestedCV;//số CV cần có
//				total_waitcount += r_waitcount;//Chưa tuyển dụng
//				total_unused_count += r_unused_count;
//				//scan
//				total_cvScan_Fail += r_cvScan_Fail;
//				total_cvScan_Pass += r_cvScan_Pass;
//				total_cvScan_Pending += r_cvScan_Pending;
//				total_cvScan_Total += r_cvScan_Total;
//				//1st round
//				total_fRoundQ_Fail += r_fRoundQ_Fail;
//				total_fRoundQ_Pass += r_fRoundQ_Pass;
//				total_fRoundQ_Pending += r_fRoundQ_Pending;
//				total_fRoundQ_Total += r_fRoundQ_Total;
//				//2nd round
//				total_sRoundQ_Fail += r_sRoundQ_Fail;
//				total_sRoundQ_Pass += r_sRoundQ_Pass;
//				total_sRoundQ_Pending += r_sRoundQ_Pending;
//				total_sRoundQ_Total += r_sRoundQ_Total;
//				#endregion
//				ds.Tables.Add ( table.Copy ( ) );
//			}
//			//khởi tạo datatable cuối cùng
//			if(total_sRoundQ_Pass > 0)
//			{
//				total_conversion_HR = Math.Round ( ((double)total_cvScan_Pass / total_sRoundQ_Pass),1 );
//				total_conversion_SOURCING = Math.Round ( ((double)total_actualcv / total_sRoundQ_Pass),1 );
//			}
//			DataTable _table = Init_Table1 ( "Total" ).Clone ( );
//			DataRow _row = _table.NewRow ( );
//			_row["Region"] = "Total";
//			_row["Số nhân viên cần tuyển"] = total_requestednum;
//			_row["Số hồ sơ cần có"] = total_requestedCV;
//			_row["Actual CV"] = total_actualcv;
//			_row["Unused Lead of Month"] = total_unused_count;
//			_row["Khu vực chưa tuyển dụng"] = total_waitcount;
//			_row["Scan_Pass"] = total_cvScan_Pass;
//			_row["Scan_Failed"] = total_cvScan_Fail;
//			_row["Scan_Pending"] = total_cvScan_Pending;
//			_row["Scan_Total"] = total_cvScan_Total;
//			_row["R1_Pass"] = total_fRoundQ_Pass;
//			_row["R1_Failed"] = total_fRoundQ_Fail;
//			_row["R1_Pending"] = total_fRoundQ_Pending;
//			_row["R1_Total"] = total_fRoundQ_Total;
//			_row["R2_Pass"] = total_sRoundQ_Pass;
//			_row["R2_Failed"] = total_sRoundQ_Fail;
//			_row["R2_Pending"] = total_sRoundQ_Pending;
//			_row["R2_Total"] = total_sRoundQ_Total;
//			_row["Convertion (HR/RFM)"] = total_conversion_HR;
//			_row["Convertion (Sourcing/RFM)"] = total_conversion_SOURCING;
//			_table.Rows.InsertAt ( _row,0 );
//			ds.Tables.Add ( _table.Copy ( ) );
//			return ds;
//		}
//	}
//	public class PairRFM_CityregionComparer:IEqualityComparer<PairRFM_Cityregion>
//	{
//		public bool Equals ( PairRFM_Cityregion p1,PairRFM_Cityregion p2 )
//		{
//			if(p1.Region.Id == p2.Region.Id && p1.RFM == p2.RFM)
//			{
//				return true;
//			}
//			return false;
//		}

//		public int GetHashCode ( PairRFM_Cityregion obj )
//		{
//			return obj.Region.GetHashCode ( );
//		}
//	}
//	public class PairRFM_Cityregion
//	{
//		public Region Region
//		{
//			get;set;
//		}
//		public string RFM
//		{
//			get;set;
//		}
//	}
//	public class MonthYearReporter
//	{
//		public int Month
//		{
//			get;set;
//		}
//		public int Year
//		{
//			get;set;
//		}

//		public DateTime Max
//		{
//			get
//			{
//				DateTime dt = new DateTime ( Year,Month,1 );
//				return DataConverter.UI2_GetMinMax ( dt,false );
//			}
//		}
//		public DateTime Min
//		{
//			get
//			{
//				DateTime dt = new DateTime ( Year,Month,1 );
//				return DataConverter.UI2_GetMinMax ( dt,true );
//			}
//		}
//	}
//}