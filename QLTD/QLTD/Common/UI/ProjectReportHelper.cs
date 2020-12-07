//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity.Core.Objects;
//using System.Linq;
//using System.Web;
//using Ehr.Bussiness;
//using Ehr.Common.Constraint;
//using Ehr.Common.Tools;
//using Ehr.Models;

//namespace Ehr.Common.UI
//{
//	public class ProjectReportHelper
//	{
//		public DataTable ReportProjectNew ( EProject project )
//		{
//			var newprojects = project.ProjectNewDetail.ToList ( );
//			DataTable table = new DataTable ( "Tuyen dung du an" );
//			table.Columns.Add ( new DataColumn ( "Region",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "RFM",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Target",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Completion",typeof ( System.Int32 ) ) );
//			table.AcceptChanges ( );

//			foreach(EProjectNew p in newprojects)
//			{
//				DataRow row = table.NewRow ( );
//				row["Region"] = p.Store.CityRegion.Region.RegionName;
//				row["RFM"] = p.RegionalManager;
//				row["Target"] = p.NumberOfRequested;
//				row["Completion"] = p.Candidates.Count;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}


//			var myquery = table.AsEnumerable ( )
//						 .GroupBy ( r1 => new { Region = r1.Field<string> ( "Region" ),RFM = r1.Field<string> ( "RFM" ) } )
//						 .Select ( g => new
//						 {
//							 Region = g.Key.Region,
//							 RFM = g.Key.RFM,
//							 Target = g.Sum ( r => r.Field<int> ( "Target" ) ),
//							 Completion = g.Sum ( r => r.Field<int> ( "Completion" ) )
//						 } ).ToList ( );

//			DataTable newTable = new DataTable ( );
//			List<DataRow> newRows = new List<DataRow> ( );
//			if(myquery != null)
//			{
//				foreach(var element in myquery)
//				{
//					var row = table.NewRow ( );
//					row["Region"] = element.Region;
//					row["RFM"] = element.RFM;
//					row["Target"] = element.Target;
//					row["Completion"] = element.Completion;
//					newRows.Add ( row );
//				}
//				newTable = newRows.CopyToDataTable ( );
//			}
//			return newTable;
//		}
//		public static List<ViewReportProjectNew> ReportProjectNew_List ( EProject project )
//		{
//			var newprojects = project.ProjectNewDetail.ToList ( );
//			DataTable table = new DataTable ( "Tuyen dung du an" );
//			table.Columns.Add ( new DataColumn ( "Region",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "RFM",typeof ( System.String ) ) );
//			table.Columns.Add ( new DataColumn ( "Target",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Completion",typeof ( System.Int32 ) ) );
//			table.AcceptChanges ( );

//			foreach(EProjectNew p in newprojects)
//			{
//				DataRow row = table.NewRow ( );
//				row["Region"] = p.Store.CityRegion.Region.RegionName;
//				row["RFM"] = p.RegionalManager;
//				row["Target"] = p.NumberOfRequested;
//				row["Completion"] = p.Candidates.Count;
//				table.Rows.Add ( row );
//				table.AcceptChanges ( );
//			}


//			var myquery = table.AsEnumerable ( )
//						 .GroupBy ( r1 => new { Region = r1.Field<string> ( "Region" ),RFM = r1.Field<string> ( "RFM" ) } )
//						 .Select ( g => new
//						 {
//							 Region = g.Key.Region,
//							 RFM = g.Key.RFM,
//							 Target = g.Sum ( r => r.Field<int> ( "Target" ) ),
//							 Completion = g.Sum ( r => r.Field<int> ( "Completion" ) )
//						 } ).ToList ( );

//			DataTable newTable = new DataTable ( );
//			List<ViewReportProjectNew> list = new List<ViewReportProjectNew> ( );
//			if(myquery != null)
//			{
//				foreach(var element in myquery)
//				{
//					ViewReportProjectNew view = new ViewReportProjectNew ( );
//					var row = table.NewRow ( );
//					view.Region = element.Region;
//					view.RFM = element.RFM;
//					view.Target = element.Target;
//					view.Completion = element.Completion;
//					list.Add ( view );
//				}
//			}
//			return list;
//		}

//		/// <summary>
//		/// Báo cáo theo khu vực - graph1
//		/// </summary>
//		/// <param name="unitwork"></param>
//		/// <param name="project"></param>
//		/// <returns></returns>
//		public static ProjectReplaceReport_Region ReportByRegion_1 ( UnitWork unitwork,EProject project,string[] months,string status,string[] _timeranges )
//		{
//			DataSet graph1 = new DataSet ( );
//			DataSet graph2 = new DataSet ( );
//			DataSet graph3 = new DataSet ( );
//			DataSet graph4 = new DataSet ( );
//			DataSet graph5 = new DataSet ( );
//			var now = DateTime.Now;
//			List<LeadTimeRange> timeranges = new List<LeadTimeRange> ( );
//			if(_timeranges.Length > 0)
//			{
//				foreach(string xyz in _timeranges)
//				{
//					if(xyz.Length > 0)
//					{
//						var l = (LeadTimeRange)Enum.Parse ( typeof ( LeadTimeRange ),xyz,true );
//						timeranges.Add ( l );
//					}
//				}
//			}
//			else
//			{
//				foreach(LeadTimeRange lt in (LeadTimeRange[])Enum.GetValues ( typeof ( LeadTimeRange ) ))
//				{
//					timeranges.Add ( lt );
//				}
//			}
//			/*
//			 * Tính leadtime bằng cách tính trung bình cộng các chỉ tiêu
//			 * /
//			/*Lấy các region theo các khoảng tháng (union lại với nhau)
//			Lấy Candidate trước
//			Sau đó distinct region*/
//			//lấy list region theo project			
//			var regions = project.ProjectReplaceDetail.Select ( c => c.Store.CityRegion.Region ).Distinct ( ).OrderBy ( c => c.Id ).ToList ( );
//			#region Khởi tạo bảng đầu tiên
//			DataTable table = new DataTable ( "REGION" );
//			//table.Columns.Add ( new DataColumn ( "ID",typeof ( System.Int32 ) ) );
//			table.Columns.Add ( new DataColumn ( "Regional",typeof ( System.String ) ) );
//			table.AcceptChanges ( );
//			DataRow _row = table.NewRow ( );
//			foreach(Region r in regions)
//			{
//				_row = table.NewRow ( );
//				//_row["ID"] = r.Id;
//				_row["Regional"] = r.RegionName;
//				table.Rows.Add ( _row );
//				table.AcceptChanges ( );
//			}
//			//thêm dòng Grand total
//			_row = table.NewRow ( );
//			//_row["ID"] = 0;
//			_row["Regional"] = "Grand total";
//			table.Rows.Add ( _row );
//			table.AcceptChanges ( );
//			graph1.Tables.Add ( table.Copy ( ) );
//			graph2.Tables.Add ( table.Copy ( ) );
//			graph3.Tables.Add ( table.Copy ( ) );
//			graph4.Tables.Add ( table.Copy ( ) );
//			graph5.Tables.Add ( table.Copy ( ) );
//			#endregion
//			if(months.Length > 0)
//			{
//				foreach(string month in months)
//				{
//					if(month.Length > 0)
//					{
//						#region tính theo month
//						string[] m = month.Split ( '/' );
//						table = new DataTable ( month );
//						table.Columns.Add ( new DataColumn ( "Target",typeof ( System.Int32 ) ) );
//						table.Columns.Add ( new DataColumn ( "Actual",typeof ( System.Int32 ) ) );
//						table.Columns.Add ( new DataColumn ( "Rate",typeof ( System.Double ) ) );
//						table.Columns.Add ( new DataColumn ( "Leadtime",typeof ( System.Double ) ) );
//						table.Columns.Add ( new DataColumn ( "Remain",typeof ( System.Double ) ) );
//						table.AcceptChanges ( );
//						int r_month = int.Parse ( m[0] );
//						int r_year = int.Parse ( m[1] );
//						//xác định khoảng thời gian để query
//						DateTime tempdate = new DateTime ( r_year,r_month,1 );
//						DateTime minDate = DataConverter.UI2_GetMinMax ( tempdate,true );
//						DateTime maxDate = DataConverter.UI2_GetMinMax ( tempdate,false );
//						int totalTarget = 0;
//						int totalActual = 0;
//						foreach(Region r in regions)
//						{
//							//lấy danh sách các dự án thay thế theo tháng và vùng
//							var _eproject_replace = project.ProjectReplaceDetail.Where ( c => c.Store.CityRegion.Region.Id == r.Id && c.StartingDate.Value.Month == r_month && c.StartingDate.Value.Year == r_year );
//							var ll = _eproject_replace.ToList ( );							
//							_row = table.NewRow ( );
//							int target = _eproject_replace.Sum ( c => c.NumberOfRequested );
//							int actual = _eproject_replace.Sum ( c => c.Candidates.Count ( ) );
//							_row["Target"] = target;
//							_row["Actual"] = actual;
//							_row["Remain"] = target-actual;
//							totalTarget += target;
//							totalActual += actual;
//							double finalRate = 0;
//							if(target > 0)
//							{
//								finalRate = Math.Round ( 100 * ((double)actual / target),1,MidpointRounding.AwayFromZero );
//							}
//							_row["Rate"] = finalRate;
//							/*
//							 * Tính trung bình lead time
//							 */
//							var notnull = _eproject_replace.Where ( c => c.CompletedDate != null ).Select ( c => new
//							{
//								index = c.Id,
//								leadtime = (c.CompletedDate - c.StartingDate).Value.TotalDays
//							} );
							
//							var null_slot = _eproject_replace.Where ( c => c.CompletedDate == null ).Select ( c => new
//							{
//								index = c.Id,
//								leadtime = (now - c.StartingDate).Value.TotalDays
//							} );
//							var listFinal = null_slot.Union ( notnull );
//							listFinal = listFinal.Where ( c => c.leadtime >= 0 );
//							double average = 0;
//							if(listFinal.ToList().Count > 0)
//								average = listFinal.Average ( c => c.leadtime );
//							if(average < 0)
//								average = 0;
//							average = Math.Round (average,0,MidpointRounding.AwayFromZero );
//							_row["Leadtime"] = average;
//							table.Rows.Add ( _row );
//							table.AcceptChanges ( );
//						}
//						//tính grand total
//						_row = table.NewRow ( );
//						_row["Target"] = totalTarget;
//						_row["Actual"] = totalActual;
//						_row["Remain"] = totalTarget-totalActual;
//						double totalRate = 0;
//						if(totalTarget > 0)
//						{
//							totalRate = Math.Round ( 100 * ((double)totalActual / totalTarget),1,MidpointRounding.AwayFromZero );
//						}
//						_row["Rate"] = totalRate;
//						var _total_eproject_replace = project.ProjectReplaceDetail.Where (c=> c.StartingDate.Value.Month == r_month && c.StartingDate.Value.Year == r_year );						
//						var _totalnotnull = _total_eproject_replace.Where ( c => c.CompletedDate != null ).Select ( c => new
//						{
//							index = c.Id,
//							leadtime = (c.CompletedDate - c.StartingDate).Value.TotalDays
//						} );						
//						var _totalnull_slot = _total_eproject_replace.Where ( c => c.CompletedDate == null ).Select ( c => new
//						{
//							index = c.Id,
//							leadtime = (now - c.StartingDate).Value.TotalDays
//						} );
//						var _totallistFinal = _totalnull_slot.Union ( _totalnotnull );
//						_totallistFinal = _totallistFinal.Where ( c => c.leadtime >= 0 );
//						double _totalaverage = 0;
//							if(_totallistFinal.ToList().Count > 0)
//								_totalaverage = _totallistFinal.Average ( c => c.leadtime );
//						_totalaverage = Math.Round (_totalaverage,0,MidpointRounding.AwayFromZero );
//						_row["Leadtime"] = _totalaverage;
//						table.Rows.Add ( _row );
//						table.AcceptChanges ( );
//						graph1.Tables.Add ( table.Copy ( ) );
//						#endregion

//						#region tính theo week
//						// báo cáo theo tuần
//						//tìm tất cả các tuần trong tháng báo cáo
//						var dates = Enumerable.Range ( 1,DateTime.DaysInMonth ( r_year,r_month ) ).Select ( n => new DateTime ( r_year,r_month,n ) );
//						var weekends = from d in dates
//									   where d.DayOfWeek == DayOfWeek.Monday
//									   select d;
//						foreach(WeekOfMonth _d in DataConverter.GetWeeksOfMonth ( r_month,r_year ))
//						{
//							//bảng này để lưu cho graph 2							
//							DataTable week_table = new DataTable ( "W" + _d.WeekName.ToString ( ) + " (" + month + ")" );
//							week_table.Columns.Add ( new DataColumn ( "Target",typeof ( System.Int32 ) ) );
//							week_table.Columns.Add ( new DataColumn ( "Actual",typeof ( System.Int32 ) ) );
//							week_table.Columns.Add ( new DataColumn ( "Rate",typeof ( System.Double ) ) );
//							week_table.Columns.Add ( new DataColumn ( "Leadtime",typeof ( System.Double ) ) );
//							week_table.AcceptChanges ( );


//							DateTime from = _d.FromDate;
//							DateTime to = _d.ToDate;
//							totalTarget = 0;
//							totalActual = 0;
//							int tweek = _d.WeekName;

//							//lấy tất cả range lead time trong tuần này
//							//var ranges = project.ProjectReplaceDetail.Where ( c => c.TargetOfWeek== tweek).Select(c=>c.LeadTimeRange).Distinct().ToList();
//							//bảng này để lưu cho graph 3
//							DataTable graph3_table = new DataTable ( "W" + _d.WeekName.ToString ( ) + " (" + month + ")" );
//							foreach(LeadTimeRange r in timeranges)
//							{
//								graph3_table.Columns.Add ( new DataColumn ( EZEnumHelper<LeadTimeRange>.GetDisplayValue ( r ),typeof ( System.Int32 ) ) );
//							}
//							//thêm cột Total
//							graph3_table.Columns.Add ( new DataColumn ( "W" + _d.WeekName.ToString ( ) + " Total",typeof ( System.Int32 ) ) );
//							graph3_table.AcceptChanges ( );

//							foreach(Region r in regions)
//							{
//								#region Phần này cho graph 2
//								//lấy danh sách các dự án thay thế theo tháng và vùng
//								var _eproject_replace = project.ProjectReplaceDetail.Where ( c => c.Store.CityRegion.Region.Id == r.Id && c.TargetOfWeek == tweek );


//								var ll = _eproject_replace.ToList ( );
//								_row = week_table.NewRow ( );
//								int target = _eproject_replace.Sum ( c => c.NumberOfRequested );
//								int actual = _eproject_replace.Sum ( c => c.Candidates.Count ( ) );
//								_row["Target"] = target;
//								_row["Actual"] = actual;
//								totalTarget += target;
//								totalActual += actual;
//								double finalRate = 0;
//								if(target > 0)
//								{
//									finalRate = Math.Round ( 100 * ((double)actual / target),1,MidpointRounding.AwayFromZero );
//								}
//								_row["Rate"] = finalRate;
//								//confirm lại chổ này
//								_row["Leadtime"] = 0;
//								week_table.Rows.Add ( _row );
//								week_table.AcceptChanges ( );
//								#endregion

//								#region Phần này dành cho graph 3
//								DataRow graph3_row = graph3_table.NewRow ( );
//								int total3 = 0;
//								foreach(LeadTimeRange _r in timeranges)
//								{
//									//đếm theo range
//									var _eproject_replace3 = project.ProjectReplaceDetail.Where ( c => c.Store.CityRegion.Region.Id == r.Id && c.TargetOfWeek == tweek && c.LeadTimeRange == _r );
//									int temptotal3 = _eproject_replace3.Sum ( c => c.Candidates.Count );
//									total3 += temptotal3;
//									graph3_row[EZEnumHelper<LeadTimeRange>.GetDisplayValue ( _r )] = temptotal3;
//								}
//								graph3_row["W" + _d.WeekName.ToString ( ) + " Total"] = total3;
//								graph3_table.Rows.Add ( graph3_row );
//								graph3_table.AcceptChanges ( );
//								#endregion
//							}

//							#region phần dành cho graph 3
//							graph3_table = DataConverter.SumDataTable ( graph3_table );
//							graph3.Tables.Add ( graph3_table.Copy ( ) );
//							#endregion

//							#region phần dành cho graph 2
//							//tính grand total
//							_row = week_table.NewRow ( );
//							_row["Target"] = totalTarget;
//							_row["Actual"] = totalActual;

//							totalRate = 0;
//							if(totalTarget > 0)
//							{
//								totalRate = Math.Round ( 100 * ((double)totalActual / totalTarget),1,MidpointRounding.AwayFromZero );
//							}
//							_row["Rate"] = totalRate;
//							//confirm lại chổ này
//							_row["Leadtime"] = 0;
//							week_table.Rows.Add ( _row );
//							week_table.AcceptChanges ( );

//							graph2.Tables.Add ( week_table.Copy ( ) );
//							#endregion
//						}
//						#endregion
//					}
//				}
//			}
//			return new ProjectReplaceReport_Region ( ) { Graph1 = graph1,Graph2 = graph2,Graph3 = graph3,Graph4 = graph4,Graph5 = graph5 };
//		}

//	}
//	public class ProjectReplaceReport_Region
//	{
//		public DataSet Graph1
//		{
//			get;set;
//		}
//		public DataSet Graph2
//		{
//			get;set;
//		}
//		public DataSet Graph3
//		{
//			get;set;
//		}
//		public DataSet Graph4
//		{
//			get;set;
//		}
//		public DataSet Graph5
//		{
//			get;set;
//		}
//	}
//	public class ViewReportProjectNew
//	{
//		public string Region
//		{
//			get; set;
//		}
//		public string RFM
//		{
//			get; set;
//		}
//		public int Target
//		{
//			get; set;
//		}
//		public int Completion
//		{
//			get; set;
//		}
//	}
//}