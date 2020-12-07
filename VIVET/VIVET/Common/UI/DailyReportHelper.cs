//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;
//using Ehr.Bussiness;
//using Ehr.Common.Constraint;
//using Ehr.Common.Tools;
//using Ehr.Models;

//namespace Ehr.Common.UI
//{
//	public class DailyReportHelper
//	{
//		/// <summary>
//		/// Báo cáo chỉ tiêu theo khu vực của tháng chỉ định
//		/// </summary>
//		/// <param name="unitwork"></param>
//		/// <param name="project"></param>
//		/// <param name="dateTime"></param>
//		/// <returns></returns>
//		public static List<Daily_1_1_Model> Daily_1_1_List (UnitWork unitwork, EProject project,DateTime dateTime)
//		{
//			int year = dateTime.Year;
//			int month = dateTime.Month;
//			var newprojects = project.ProjectNewDetail.Where ( c =>(c.StartingDate.Value.Year==year && c.StartingDate.Value.Month==month) || (c.EndingDate.Value.Year==year && c.EndingDate.Value.Month==month)).ToList ( );
//			DataTable table = new DataTable ( "DailyReport" );
//			table.Columns.Add ( new DataColumn ( "RegionID",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Region",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Target",typeof ( System.Int32 ) ) );
//			table.AcceptChanges ( );

//			foreach(EProjectNew p in newprojects)
//			{
//				DataRow row = table.NewRow ( );
//				row["RegionID"] = p.Store.CityRegion.Region.Id;
//				row["Region"] = p.Store.CityRegion.Region.RegionName;
//				row["Target"] = p.NumberOfRequested;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}
//			var replaceprojects = project.ProjectReplaceDetail.Where ( c =>(c.StartingDate.Value.Year==year && c.StartingDate.Value.Month==month) || (c.EndingDate.Value.Year==year && c.EndingDate.Value.Month==month)).ToList ( );
//			foreach(EProjectReplace p in replaceprojects)
//			{
//				DataRow row = table.NewRow ( );
//				row["RegionID"] = p.Store.CityRegion.Region.Id;
//				row["Region"] = p.Store.CityRegion.Region.RegionName;
//				row["Target"] = p.NumberOfRequested;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}

//			var myquery = table.AsEnumerable ( )
//						 .GroupBy ( r1 => new { ID= r1.Field<int> ( "RegionID" ),Region = r1.Field<string> ( "Region" ) } )
//						 .Select ( g => new
//						 {
//							 RegionID = g.Key.ID,
//							 Region = g.Key.Region,
//							 Target = g.Sum ( r => r.Field<int> ( "Target" ) )
//						 } ).ToList ( );
			
//			List<Daily_1_1_Model> list = new List<Daily_1_1_Model> ( );
//			if(myquery != null)
//			{
//				foreach(var element in myquery)
//				{
//					Daily_1_1_Model view = new Daily_1_1_Model ( );
//					var row = table.NewRow ( );
//					view.ID = element.RegionID;
//					view.Region = element.Region;					
//					view.Target = element.Target;
//					view.CVRequested = element.Target*10;
//					//get CV received
//					var candidates = unitwork.Candidate.Get ( c => c.Form.Project.Id == project.Id && c.CityRegion.Region.Id==element.RegionID && c.Approval!= Constraint.ApprovalStatus.DELETED && (c.SubmissionDate.Value.Year == year && c.SubmissionDate.Value.Month == month) ).ToList ( ).Count;
//					//calculate completion rate
//					double rate = 0;
//					if(element.Target > 0)
//					{
//						rate = candidates / ((double)element.Target * 10.0d);
//					}
//					view.CVReceived = candidates;
//					view.CompletionRate = rate;
//					list.Add ( view );
//				}
//			}
//			return list.OrderBy(c=>c.ID).ToList();
//		}
//		/// <summary>
//		/// Graph thứ 2
//		/// </summary>
//		/// <param name="unitwork"></param>
//		/// <param name="project"></param>
//		/// <param name="dateTime"></param>
//		/// <returns></returns>
//		public static DataTable Daily_Graph2 ( UnitWork unitwork,EProject project,DateTime dateTime )
//		{
//			DataTable table = new DataTable ( "DailyReport" );			
//			table.Columns.Add ( new DataColumn ( "Nguồn ứng viên",typeof ( System.String ) ) );			
//			table.AcceptChanges ( );
//			//đưa nguồn ứng viên vào
//			foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//			{
//				DataRow row = table.NewRow ( );				
//				row["Nguồn ứng viên"] = EZEnumHelper<EmploymentSource>.GetDisplayValue(esource);				
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}
			
//			//số tháng cần báo cáo
//			int months = 5;
//			//calculate from index			
//			bool stopIndex = false;
//			int colZeroCount = -1;
//			//create all columns
//			for(int i = months;i >= 0;i--)
//			{
//				if(!stopIndex)
//				{
//					colZeroCount++;
//				}
//				DateTime rpDate = dateTime.AddMonths ( (-1) * i );
//				DateTime minDate =DataConverter.UI2_GetMinMax ( rpDate,true );
//				DateTime maxDate = DataConverter.UI2_GetMinMax ( rpDate,false );;
//				var columnName = DataConverter.ToMonthYear ( rpDate );
//				table.Columns.Add ( new DataColumn ( columnName,typeof ( System.String ) ) );
//				table.AcceptChanges ( );
//				//tính lượng ứng viên theo từng nguồn		
//				int count = -1;				
//				var _cand = unitwork.Candidate.Get ( c => c.Form.Project.Id == project.Id && (c.SubmissionDate < maxDate && c.SubmissionDate > minDate) );
//				foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//				{
//					count++;
//					//tìm ứng viên đúng nguồn
//					var candidates = _cand.Where( c=>c.CandidateSource == esource ).ToList().Count;
//					//lấy % trên tổng
//					double percent = 0;
//					if(_cand.Count ( ) > 0)
//					{
//						percent=Math.Round ( 100*candidates/(double)_cand.Count(),1,MidpointRounding.AwayFromZero );
//					}
//					table.Rows[count][columnName] = percent;
//					if(percent > 0)
//					{
//						stopIndex = true;
//					}
//				}				
//				table.AcceptChanges ( );
//			}
//			//tính cột trung bình
//			table.Columns.Add ( new DataColumn ( "Trung bình",typeof ( System.Double ) ) );			
//			table.AcceptChanges ( );
//			for(var i = 0;i < table.Rows.Count;i++)
//			{
//				double all = 0;
//				for(var j = 1;j < table.Columns.Count - 1;j++)//Bỏ cột đầu (Cột nguồn) và 1 cột cuối (cột trung bình đang xét)
//				{
//					all += double.Parse ( table.Rows[i][j].ToString() );
//				}
//				//cập nhật cột giá trị
//				var value = all / (months-colZeroCount+1);
//				//var value = DataConverter.AverageA(all);
//				table.Rows[i]["Trung bình"] = value;
//				table.AcceptChanges ( );
//			}
//			return table;
//		}

//		/// <summary>
//		/// Chi tiết số lượng theo từng nguồn
//		/// </summary>
//		/// <param name="_Daily_Graph2">Là DailyReportHelper.Daily_Graph2</param>
//		/// <param name="unitwork"></param>
//		/// <param name="project"></param>
//		/// <param name="dateTime"></param>
//		/// <param name="_Daily_1_1_Listr">Là DailyReportHelper._Daily_1_1_Listr</param>
//		/// <returns></returns>
//		public static DataTable Daily_Graph3 ( DataTable _Daily_Graph2,UnitWork unitwork,EProject project,DateTime dateTime,List<Daily_1_1_Model> _Daily_1_1_Listr )
//		{
//			var date = dateTime;
//			TimeSpan difference = project.EndingDate - project.StartingDate;
//			TimeSpan differenceFrom = date - project.StartingDate;
//			int noofday = difference.Days + 1;
//			double timegone = 100 * (float)(differenceFrom.Days + 1) / noofday;
//			timegone = Math.Round ( timegone,1,MidpointRounding.AwayFromZero );

//			//tính tổng hồ sơ cần có
//			int sum = 0;
//			foreach(Daily_1_1_Model m in _Daily_1_1_Listr)
//			{
//				sum += m.CVRequested;
//			}
//			DataTable table = new DataTable ( "DailyReport" );
//			table.Columns.Add ( new DataColumn ( "Nguồn ứng viên",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Tổng số hồ sơ cần có",typeof ( System.Double ) ) );
//			table.Columns.Add ( new DataColumn ( "Tổng số hồ sơ nhận được",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Số hồ sơ có thể sử dụng được",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Tỉ lệ hoàn thành vs time gone",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Status",typeof ( System.String ) ) );
//			table.AcceptChanges ( );
//			//đưa nguồn ứng viên vào
//			int index = -1;
//			int rate_index = _Daily_Graph2.Columns.Count - 1;
//			//lấy ra các ứng viên trước để lọc cho nhanh
//			DateTime minDate = DataConverter.UI2_GetMinMax ( dateTime,true );
//			DateTime maxDate = DataConverter.UI2_GetMinMax ( dateTime,false );
//			;
//			var _cand = unitwork.Candidate.Get ( c => c.Form.Project.Id == project.Id && (c.SubmissionDate < maxDate && c.SubmissionDate > minDate) );
//			//Tổng số hồ sơ cần có
//			double totalExpect = 0;
//			int totalReceived = 0;
//			foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//			{
//				index++;
//				DataRow row = table.NewRow ( );
//				row["Nguồn ứng viên"] = EZEnumHelper<EmploymentSource>.GetDisplayValue ( esource );
//				//tính số hồ sơ cần có
//				double _expect = 0;
//				_expect = double.Parse ( _Daily_Graph2.Rows[index][rate_index].ToString ( ) ) / 100;
//				_expect = _expect * sum;
//				_expect = Math.Round ( _expect,0,MidpointRounding.AwayFromZero );
//				row["Tổng số hồ sơ cần có"] = _expect;
//				totalExpect += _expect;
//				//tính số hồ sơ đã nhận
//				int received = _cand.Where ( c => c.CandidateSource == esource ).ToList ( ).Count;
//				totalReceived += received;
//				row["Tổng số hồ sơ nhận được"] = received;
//				//số hồ sơ có thể sử dụng được
//				int usable=_cand.Where ( c => c.CandidateSource == esource && c.Approval!= ApprovalStatus.DELETED ).ToList ( ).Count;
//				row["Số hồ sơ có thể sử dụng được"] = usable;
//				//Tính tỷ lệ
//				double rate = 0;
//				if(_expect > 0)
//					rate = Math.Round ( 100 * (received / _expect),0,MidpointRounding.AwayFromZero );
//				row["Tỉ lệ hoàn thành vs time gone"] = rate.ToString()+" %";
//				//tính status
//				var track = "<span class='badge bg-red'>Off track</span>";
//				if(rate >= timegone || rate >= 100)
//				{
//					track = "<span class='badge bg-success'>On track</span>";
//				}
//				row["Status"] = track;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}
//			//tính 2 dòng cuối
//			//thêm 2 dòng cuối là Tổng số hồ sơ nhận được và số hồ sơ có thể sử dụng
//			DataRow _row = table.NewRow ( );
//			_row["Nguồn ứng viên"] = "Tổng số hồ sơ nhận được";
//			table.Rows.Add ( _row );
//			table.AcceptChanges ( );
//			_row = table.NewRow ( );
//			_row["Nguồn ứng viên"] = "Số hồ sơ có thể sử dụng (đã lọc trùng)";
//			table.Rows.Add ( _row );
//			table.AcceptChanges ( );
//			index = table.Rows.Count - 2;
//			//số lượng cần
//			table.Rows[index]["Tổng số hồ sơ cần có"] = totalExpect;
//			//số lượng nhận
//			table.Rows[index]["Tổng số hồ sơ nhận được"] = totalReceived;
//			//tỷ lệ hoàn thành
//			double totalRate = 0;
//			if(totalExpect > 0)
//			{
//				totalRate = Math.Round ( 100 * (totalReceived / totalExpect),0,MidpointRounding.AwayFromZero );
//			}
//			table.Rows[index]["Tỉ lệ hoàn thành vs time gone"] = totalRate.ToString()+" %";
//			//status
//			var state = "<span class='badge bg-red'>Off track</span>";
//			if(totalRate >= timegone || totalRate >= 100)
//			{
//				state = "<span class='badge bg-success'>On track</span>";
//			}
//			table.Rows[index]["Status"] = state;
//			//số hồ sơ đã lọc
//			int filtered=_cand.Where ( c => c.Approval !=  ApprovalStatus.DELETED ).ToList ( ).Count;
//			table.Rows[index+1]["Số hồ sơ có thể sử dụng được"] = filtered;
//			//tỷ lệ hoàn thành
//			double finalCompleted = 0;
//			if(filtered > 0)
//			{
//				finalCompleted= Math.Round ( 100 * ((double)filtered / totalReceived),0,MidpointRounding.AwayFromZero );
//			}
//			table.Rows[index+1]["Tỉ lệ hoàn thành vs time gone"] = finalCompleted.ToString()+" %";
//			table.AcceptChanges ( );
//			return table;
//		}

//		public static DataSet Daily_Graph4 ( UnitWork unitwork,EProject project,DateTime dateTime,List<Daily_1_1_Model> _Daily_1_1_List,DataTable _Daily_Graph2 )
//		{
//			DataSet ds = new DataSet ( );
//			//first table is for labeling
//			DataTable _table = new DataTable ( "EZ" );
//			_table.Columns.Add ( new DataColumn ( "Nguồn ứng viên",typeof ( System.String ) ) );
//			_table.AcceptChanges ( );
//			foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//			{
//				DataRow row = _table.NewRow ( );
//				row["Nguồn ứng viên"] = EZEnumHelper<EmploymentSource>.GetDisplayValue ( esource );
//				_table.Rows.Add ( row );
//				_table.AcceptChanges ( );
//			}
//			DataRow _row = _table.NewRow ( );
//			_row["Nguồn ứng viên"] = "Tổng";
//			_table.Rows.Add ( _row );
//			_table.AcceptChanges ( );
//			_row = _table.NewRow ( );
//			_row["Nguồn ứng viên"] = "Số hồ sơ khả dụng";
//			_table.Rows.Add ( _row );
//			_table.AcceptChanges ( );
//			//thêm bảng đầu tiên để làm label
//			ds.Tables.Add ( _table.Copy ( ) );
//			//Lọc candidate trước
//			DateTime minDate = DataConverter.UI2_GetMinMax ( dateTime,true );
//			DateTime maxDate = DataConverter.UI2_GetMinMax ( dateTime,false );
//			var _cand = unitwork.Candidate.Get ( c => c.Form.Project.Id == project.Id && (c.SubmissionDate < maxDate && c.SubmissionDate > minDate) );
//			//lấy ra số cột mà Daily graph 2 có để lấy rate
//			int cols = _Daily_Graph2.Columns.Count - 1;
//			//Thêm từng bảng con bên trong
//			foreach(Daily_1_1_Model m in _Daily_1_1_List)
//			{
//				DataTable table = new DataTable ( m.Region );
//				table.Columns.Add ( new DataColumn ( "Số hồ sơ cần có",typeof ( System.Double ) ) );
//				table.Columns.Add ( new DataColumn ( "Tổng số hồ sơ nhận được",typeof ( System.Int32 ) ) );
//				table.Columns.Add ( new DataColumn ( "Tỉ lệ hoàn thành vs time gone",typeof ( System.Double ) ) );
//				table.AcceptChanges ( );
//				//tính tỷ lệ từng chỉ tiêu
//				int index = -1;
//				double totalTarget = 0;
//				double totalReceived = 0;
//				foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//				{
//					index++;
//					//lấy % phân bổ
//					double rate = (double)_Daily_Graph2.Rows[index][cols];
//					int numCV = m.CVRequested;
//					double target = Math.Round (numCV * (rate/100),0,MidpointRounding.AwayFromZero );
//					totalTarget += target;
//					DataRow row = table.NewRow ( );
//					row["Số hồ sơ cần có"] = target;
//					//tính số hồ sơ nhận được
//					int num = _cand.Where ( c => c.CityRegion.Region.Id == m.ID && c.CandidateSource == esource ).Count ( );
//					row["Tổng số hồ sơ nhận được"] = num;
//					totalReceived += num;
//					//tỷ lệ
//					double finalRate = 0;
//					if(target > 0)
//					{
//						finalRate = Math.Round ( 100 * (num / target),1,MidpointRounding.AwayFromZero );
//					}
//					row["Tỉ lệ hoàn thành vs time gone"] = finalRate;
//					table.Rows.Add ( row );
//					table.AcceptChanges ( );
//				}
//				//add 2 dòng cuối là tổng số hồ sơ cần có, nhận được và dòng hồ sơ khả dụng
//				_row = table.NewRow ( );
//				_row["Số hồ sơ cần có"] = totalTarget;
//				_row["Tổng số hồ sơ nhận được"] = totalReceived;
//				double _totalRate = 0;
//				if(totalTarget > 0)
//				{
//					_totalRate = Math.Round ( 100 * (totalReceived / totalTarget),1,MidpointRounding.AwayFromZero );
//				}
//				_row["Tỉ lệ hoàn thành vs time gone"] = _totalRate;
//				table.Rows.Add ( _row );
//				table.AcceptChanges ( );
//				//dòng hồ sơ khả dụng
//				_row = table.NewRow ( );
//				int filtered=_cand.Where ( c => c.CityRegion.Region.Id==m.ID && c.Approval !=  ApprovalStatus.DELETED ).Count();
//				_row["Tổng số hồ sơ nhận được"] = filtered;
//				table.Rows.Add ( _row );
//				table.AcceptChanges ( );

//				ds.Tables.Add ( table.Copy ( ) );
//			}
//			return ds;
//		}

//		/// <summary>
//		/// Báo cáo số ứng viên theo tỉnh thành
//		/// </summary>
//		/// <param name="unitwork"></param>
//		/// <param name="project"></param>
//		/// <param name="dateTime"></param>
//		/// <returns></returns>
//		public static List<Daily_1_2_Model> Daily_1_2_List (UnitWork unitwork, EProject project,DateTime dateTime)
//		{
//			int year = dateTime.Year;
//			int month = dateTime.Month;
//			var newprojects = project.ProjectNewDetail.Where ( c =>(c.StartingDate.Value.Year==year && c.StartingDate.Value.Month==month) || (c.EndingDate.Value.Year==year && c.EndingDate.Value.Month==month)).ToList ( );
//			DataTable table = new DataTable ( "DailyReport" );
//			table.Columns.Add ( new DataColumn ( "CityID",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Region",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "CityName",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Target",typeof ( System.Int32 ) ) );
//			table.AcceptChanges ( );

//			foreach(EProjectNew p in newprojects)
//			{
//				DataRow row = table.NewRow ( );
//				row["CityID"] = p.Store.CityRegion.City.Id;
//				row["Region"] = p.Store.CityRegion.Region.RegionName;
//				row["CityName"] = p.Store.CityRegion.City.CityName;
//				row["Target"] = p.NumberOfRequested;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}
//			var replaceprojects = project.ProjectReplaceDetail.Where ( c =>(c.StartingDate.Value.Year==year && c.StartingDate.Value.Month==month) || (c.EndingDate.Value.Year==year && c.EndingDate.Value.Month==month)).ToList ( );
//			foreach(EProjectReplace p in replaceprojects)
//			{
//				DataRow row = table.NewRow ( );
//				row["CityID"] = p.Store.CityRegion.City.Id;
//				row["Region"] = p.Store.CityRegion.Region.RegionName;
//				row["CityName"] = p.Store.CityRegion.City.CityName;
//				row["Target"] = p.NumberOfRequested;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}

//			var myquery = table.AsEnumerable ( )
//						 .GroupBy ( r1 => new { ID= r1.Field<int> ( "CityID" ),Region = r1.Field<string> ( "Region" ),CityName = r1.Field<string> ( "CityName" )  } )
//						 .Select ( g => new
//						 {
//							 CityID = g.Key.ID,
//							 Region = g.Key.Region,
//							 CityName = g.Key.CityName,
//							 Target = g.Sum ( r => r.Field<int> ( "Target" ) )
//						 } ).ToList ( );
			
//			List<Daily_1_2_Model> list = new List<Daily_1_2_Model> ( );
//			if(myquery != null)
//			{
//				foreach(var element in myquery)
//				{
//					Daily_1_2_Model view = new Daily_1_2_Model ( );
//					var row = table.NewRow ( );
//					view.ID = element.CityID;
//					view.Region = element.Region;					
//					view.City = element.CityName;
//					view.Target = element.Target;
//					view.CVRequested = element.Target*10;
//					//get CV received
//					var candidates = unitwork.Candidate.Get ( c => c.Form.Project.Id == project.Id && c.CityRegion.City.Id==element.CityID && c.Approval!= Constraint.ApprovalStatus.DELETED && (c.SubmissionDate.Value.Year == year && c.SubmissionDate.Value.Month == month) ).ToList ( ).Count;
//					//calculate completion rate
//					double rate = 0;
//					if(element.Target > 0)
//					{
//						rate = candidates / ((double)element.Target * 10.0d);
//					}
//					view.CVReceived = candidates;
//					view.CompletionRate = rate;
//					list.Add ( view );
//				}
//			}
//			return list;
//		}

//		/// <summary>
//		/// Báo cáo nguồn ứng viên theo những ngày gần nhất
//		/// </summary>
//		/// <param name="unitwork"></param>
//		/// <param name="project"></param>
//		/// <param name="dateTime"></param>
//		/// <returns></returns>
//		public static DataTable Daily_II ( UnitWork unitwork,EProject project,DateTime dateTime )
//		{
//			DataTable table = new DataTable ( "DailyReport" );			
//			table.Columns.Add ( new DataColumn ( "Nguồn ứng viên",typeof ( System.String ) ) );			
//			table.AcceptChanges ( );
//			//đưa nguồn ứng viên vào
//			foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//			{
//				DataRow row = table.NewRow ( );				
//				row["Nguồn ứng viên"] = EZEnumHelper<EmploymentSource>.GetDisplayValue(esource);				
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}
//			//thêm 2 dòng cuối là Tổng số hồ sơ nhận được và số hồ sơ có thể sử dụng
//			DataRow _row = table.NewRow ( );			
//			_row["Nguồn ứng viên"] = "Tổng số hồ sơ nhận được";				
//			table.Rows.Add ( _row );
//			table.AcceptChanges ( );
//			_row = table.NewRow ( );			
//			_row["Nguồn ứng viên"] = "Số hồ sơ có thể sử dụng (đã lọc trùng)";				
//			table.Rows.Add ( _row );
//			table.AcceptChanges ( );
//			//số ngày cần báo cáo
//			int days = 7;
//			//create all columns
//			for(int i = days;i > 0;i--)
//			{
//				DateTime rpDate = dateTime.AddDays ( (-1) * i );
//				DateTime minDate =DataConverter.UI2DateTimeMinMax ( rpDate,true );
//				DateTime maxDate = DataConverter.UI2DateTimeMinMax ( rpDate,false );;
//				var columnName = DataConverter.Date2DayMonth ( rpDate );
//				table.Columns.Add ( new DataColumn ( columnName,typeof ( System.String ) ) );
//				table.AcceptChanges ( );
//				//tính lượng ứng viên theo từng nguồn		
//				int count = -1;				
//				var _cand = unitwork.Candidate.Get ( c => c.Form.Project.Id == project.Id && (c.SubmissionDate < maxDate && c.SubmissionDate > minDate) );
//				foreach(EmploymentSource esource in (EmploymentSource[])Enum.GetValues ( typeof ( EmploymentSource ) ))
//				{
//					count++;
//					//tìm ứng viên đúng nguồn
//					var candidates = _cand.Where( c=>c.CandidateSource == esource ).ToList().Count;					
//					table.Rows[count][columnName] = candidates;					
//				}
//				//số hồ sơ có thể sử dụng
//				var usable = unitwork.Candidate.Get ( c => c.Form.Project.Id==project.Id &&(c.SubmissionDate<maxDate && c.SubmissionDate>minDate)&& c.Approval== ApprovalStatus.DELETED ).ToList().Count;
//				//nạp dữ liệu cho 2 dòng cuối
//				table.Rows[count+1][columnName] = _cand.Count();	
//				table.Rows[count+2][columnName] = usable;//hồ sơ khả dụng

//				table.AcceptChanges ( );
//			}
//			return table;
//		}
//	}
//	public class Daily_1_1_Model
//	{
//		/// <summary>
//		/// Region ID
//		/// </summary>
//		public int ID
//		{
//			get;set;
//		}
//		public string Region
//		{
//			get;set;
//		}
//		public int Target
//		{
//			get;set;
//		}
//		public int CVRequested
//		{
//			get;set;
//		}
//		public int CVReceived
//		{
//			get;set;
//		}
//		public double CompletionRate
//		{
//			get;set;
//		}
//	}

//	public class Daily_1_2_Model
//	{
//		public int ID
//		{
//			get;set;
//		}
//		public string Region
//		{
//			get;set;
//		}
//		public string City
//		{
//			get;set;
//		}
//		public int Target
//		{
//			get;set;
//		}
//		public int CVRequested
//		{
//			get;set;
//		}
//		public int CVReceived
//		{
//			get;set;
//		}
//		public double CompletionRate
//		{
//			get;set;
//		}
//	}
//}