//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Data;
//using System.Linq;
//using System.Web;
//using Ehr.Auth;
//using Ehr.Common.Constraint;
//using Ehr.Common.Tools;
//using Ehr.Models;

//namespace Ehr.Common.UI
//{
//    public class ReportMonthHelper
//    {
//        //public static List<SourcingViewReport> ReportMonthSourcingReport ( EProject project ,DateTime date)
//        //{
//        //    int year = date.Year;
//        //    int month = date.Month;
//        //    Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
//        //    var candidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id).ToList();
//        //    candidates = candidates.Where(c => (c.SubmissionDate.Value.Year == year && c.SubmissionDate.Value.Month <= month)).ToList();
//        //    DataTable table = new DataTable("Sourcing Report");
//        //    table.Columns.Add(new DataColumn("SourcesOfRecruiment", typeof(System.String)));
//        //    table.Columns.Add(new DataColumn("Explanation", typeof(System.String)));
//        //    table.Columns.Add(new DataColumn("Jan_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Fed_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Mar_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Apr_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("May_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Jun_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Jul_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Aug_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Sep_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Oct_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Nov_Cand", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Dec_Cand", typeof(System.Int32)));

//        //    table.Columns.Add(new DataColumn("Jan_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Fed_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Mar_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Apr_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("May_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Jun_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Jul_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Aug_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Sep_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Oct_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Nov_Emp", typeof(System.Int32)));
//        //    table.Columns.Add(new DataColumn("Dec_Emp", typeof(System.Int32)));
//        //    foreach (Candidate p in candidates)
//        //    {
//        //        DataRow row = table.NewRow();
//        //        row["Jan_Cand"] = 0; row["Fed_Cand"] = 0; row["Mar_Cand"] = 0; row["Apr_Cand"] = 0; row["May_Cand"] = 0; row["Jun_Cand"] = 0; row["Jul_Cand"] = 0; row["Aug_Cand"] = 0; row["Sep_Cand"] = 0; row["Oct_Cand"] = 0; row["Nov_Cand"] = 0; row["Dec_Cand"] = 0;
//        //        row["Jan_Emp"] = 0; row["Fed_Emp"] = 0; row["Mar_Emp"] = 0; row["Apr_Emp"] = 0; row["May_Emp"] = 0; row["Jun_Emp"] = 0; row["Jul_Emp"] = 0; row["Aug_Emp"] = 0; row["Sep_Emp"] = 0; row["Oct_Emp"] = 0; row["Nov_Emp"] = 0; row["Dec_Emp"] = 0;
//        //        row["SourcesOfRecruiment"] = p.CandidateSource.GetAttribute<DisplayAttribute>().Name;
//        //        var a = DateTime.Parse(p.SubmissionDate.ToString());
//        //        #region month
//        //        if (a.Month == 1)
//        //        {
//        //            if(p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Jan_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Jan_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if(a.Month == 2)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Fed_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Fed_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 3)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Mar_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Mar_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 4)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Apr_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Apr_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 5)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["May_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["May_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 6)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Jun_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Jun_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 7)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Jul_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Jul_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 8)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Aug_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Aug_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 9)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Sep_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Sep_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 10)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Oct_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Oct_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 11)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Nov_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Nov_Cand"] = 1;
//        //            }
//        //        }
//        //        else
//        //        if (a.Month == 12)
//        //        {
//        //            if (p.State == Constraint.InterviewStatus.DONE)
//        //            {
//        //                row["Dec_Emp"] = 1;
//        //            }
//        //            else
//        //            {
//        //                row["Dec_Cand"] = 1;
//        //            }
//        //        }
//        //        #endregion
//        //        table.Rows.Add(row);
//        //        table.AcceptChanges();
//        //    }
//        //    var myquery = table.AsEnumerable()
//        //                 .GroupBy(r1 => new { CandidateSource = r1.Field<string>("SourcesOfRecruiment")})
//        //                 .Select(g => new
//        //                 {
//        //                     CandidateSource = g.Key.CandidateSource,
//        //                     Jan_Cand = g.Sum ( r => r.Field<int>("Jan_Cand")),
//        //                     Fed_Cand = g.Sum(r => r.Field<int>("Fed_Cand")),
//        //                     Mar_Cand = g.Sum(r => r.Field<int>("Mar_Cand")),
//        //                     Apr_Cand = g.Sum(r => r.Field<int>("Apr_Cand")),
//        //                     May_Cand = g.Sum(r => r.Field<int>("May_Cand")),
//        //                     Jun_Cand = g.Sum(r => r.Field<int>("Jun_Cand")),
//        //                     Jul_Cand = g.Sum(r => r.Field<int>("Jul_Cand")),
//        //                     Aug_Cand = g.Sum(r => r.Field<int>("Aug_Cand")),
//        //                     Sep_Cand = g.Sum(r => r.Field<int>("Sep_Cand")),
//        //                     Oct_Cand = g.Sum(r => r.Field<int>("Oct_Cand")),
//        //                     Nov_Cand = g.Sum(r => r.Field<int>("Nov_Cand")),
//        //                     Dec_Cand = g.Sum(r => r.Field<int>("Dec_Cand")),

//        //                     Jan_Emp = g.Sum(r => r.Field<int>("Jan_Cand")),
//        //                     Fed_Emp = g.Sum(r => r.Field<int>("Fed_Emp")),
//        //                     Mar_Emp = g.Sum(r => r.Field<int>("Mar_Emp")),
//        //                     Apr_Emp = g.Sum(r => r.Field<int>("Apr_Emp")),
//        //                     May_Emp = g.Sum(r => r.Field<int>("May_Emp")),
//        //                     Jun_Emp = g.Sum(r => r.Field<int>("Jun_Emp")),
//        //                     Jul_Emp = g.Sum(r => r.Field<int>("Jul_Emp")),
//        //                     Aug_Emp = g.Sum(r => r.Field<int>("Aug_Emp")),
//        //                     Sep_Emp = g.Sum(r => r.Field<int>("Sep_Emp")),
//        //                     Oct_Emp = g.Sum(r => r.Field<int>("Oct_Emp")),
//        //                     Nov_Emp = g.Sum(r => r.Field<int>("Nov_Emp")),
//        //                     Dec_Emp = g.Sum(r => r.Field<int>("Dec_Emp")),

//        //                 }).ToList();
//        //    DataTable newTable = new DataTable();
//        //    List <SourcingViewReport> list = new List<SourcingViewReport>();
//        //    if (myquery != null)
//        //    {
//        //        foreach (var element in myquery)
//        //        {
//        //            SourcingViewReport view = new SourcingViewReport();
//        //            var row = table.NewRow();
//        //            view.SourcesOfRecruiment = element.CandidateSource;
//        //            view.Jan_Cand = element.Jan_Cand;
//        //            view.Fed_Cand = element.Fed_Cand;
//        //            view.Mar_Cand = element.Mar_Cand;
//        //            view.Apr_Cand = element.Apr_Cand;
//        //            view.May_Cand = element.May_Cand;
//        //            view.Jun_Cand = element.Jun_Cand;
//        //            view.Jul_Cand = element.Jul_Cand;
//        //            view.Aug_Cand = element.Aug_Cand;
//        //            view.Sep_Cand = element.Sep_Cand;
//        //            view.Oct_Cand = element.Oct_Cand;
//        //            view.Nov_Cand = element.Nov_Cand;
//        //            view.Dec_Cand = element.Dec_Cand;

//        //            view.Jan_Emp = element.Jan_Emp;
//        //            view.Fed_Emp = element.Fed_Emp;
//        //            view.Mar_Emp = element.Mar_Emp;
//        //            view.Apr_Emp = element.Apr_Emp;
//        //            view.May_Emp = element.May_Emp;
//        //            view.Jun_Emp = element.Jun_Emp;
//        //            view.Jul_Emp = element.Jul_Emp;
//        //            view.Aug_Emp = element.Aug_Emp;
//        //            view.Sep_Emp = element.Sep_Emp;
//        //            view.Oct_Emp = element.Oct_Emp;
//        //            view.Nov_Emp = element.Nov_Emp;
//        //            view.Dec_Emp = element.Dec_Emp;

//        //            list.Add(view);
//        //        }
//        //    }
//        //    return list;
//        //}
//        public static DataTable ReportMonthSourcingReport(EProject project, string[] months)
//        {
//            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
//            var candidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id).ToList();
//            var candidates_total = 0;
//            if (months.Length > 0)
//            {
//                foreach (var mon in months)
//                {
//                    if (mon.Length > 0)
//                    {
//                        string[] m = mon.Split('/');
//                        int r_month = int.Parse(m[0]);
//                        int r_year = int.Parse(m[1]);
//                        var candidates_ = candidates.Where(c => c.SubmissionDate.Value.Month == r_month && c.SubmissionDate.Value.Year == r_year).ToList().Count();
//                        candidates_total += candidates_;
//                    }
//                }
//            }            
            
//            //Sắp xếp lại theo tên
//            #region month of year
//            List<CustomMonth> MonthOfYear = new List<CustomMonth>();
//            if (months.Length > 0)
//            {
//                foreach (var mon in months)
//                {
//                    if (mon.Length > 0)
//                    {
//                        string[] m = mon.Split('/');
//                        int r_month = int.Parse(m[0]);
//                        int r_year = int.Parse(m[1]);
//                        CustomMonth customMonth = new CustomMonth();
//                        customMonth.Month = r_month;
//                        customMonth.Year = r_year;
//                        customMonth.Name = "Tháng " + r_month.ToString() + " " + r_year.ToString();
//                        MonthOfYear.Add(customMonth);
//                    }
//                }
//            }
//            #endregion

//            DataTable table = new DataTable("Sourcing Report");
//            table.Columns.Add(new DataColumn("Sources Of Recruiment", typeof(System.String)));
//            table.AcceptChanges();
//            #region Candidates
//            foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//            {
//                DataRow row = table.NewRow();
//                row["Sources Of Recruiment"] = EZEnumHelper<EmploymentSource>.GetDisplayValue(esource);
//                table.Rows.Add(row);
//                table.AcceptChanges();
//            }
//            DataRow _row = table.NewRow();
//            _row["Sources Of Recruiment"] = "Total";
//            table.Rows.Add(_row);
//            table.AcceptChanges();

//            foreach (var m in MonthOfYear)
//            {
//                var columnName = m.Name;
//                table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
//                table.AcceptChanges();
//                int count = -1;
//                int sum = 0;
//                foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//                {
//                    count++;
//                    //tìm ứng viên đúng nguồn
//                    var totalcandidates = candidates.Where(c => (c.SubmissionDate.Value.Year == m.Year && c.SubmissionDate.Value.Month == m.Month) && c.CandidateSource == esource).ToList().Count;
//                    sum += totalcandidates;
//                    table.Rows[count][columnName] = totalcandidates;
//                    table.AcceptChanges();
//                }
//                table.Rows[count + 1][columnName] = sum;
//                table.AcceptChanges();
//            }
//            table.Columns.Add(new DataColumn("YTD", typeof(System.String)));
//            table.Columns.Add(new DataColumn("%", typeof(System.String)));
//            table.AcceptChanges();
//            int count_ = -1;
//            double percent_total = 0;
//            foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//            {
//                count_++;
//                int sum = 0;
//                foreach (var m in MonthOfYear)
//                {
//                    //tìm ứng viên đúng nguồn
//                    var totalcandidates = candidates.Where(c => (c.SubmissionDate.Value.Year == m.Year && c.SubmissionDate.Value.Month == m.Month) && c.CandidateSource == esource).ToList().Count;
//                    sum += totalcandidates;
//                }
//                double percent = 0;
//                if (candidates_total != 0)
//                {
//                    percent = Math.Round(100 * ((double)sum / (double)candidates_total), 1, MidpointRounding.AwayFromZero);
//                }
//                percent_total += percent;
//                table.Rows[count_]["YTD"] = sum;
//                table.Rows[count_]["%"] = percent +"%";
//                table.AcceptChanges();
//            }
//            table.Rows[count_ + 1]["YTD"] = candidates_total;
//            table.Rows[count_ + 1]["%"] = "0%%";
//            if(percent_total > 0)
//            {
//                table.Rows[count_ + 1]["%"] = "100%";
//            }
//            table.AcceptChanges();
//            #endregion

//            #region Employee
//            foreach (var m in MonthOfYear)
//            {
//                var columnName = m.Name +"_";
//                table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
//                table.AcceptChanges();
//                int count = -1;
//                int sum = 0;
//                foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//                {
//                    count++;
//                    //tìm ứng viên đúng nguồn
//                    var totalcandidates = candidates.Where(c => c.ContractDate != null && (c.ContractDate.Value.Year == m.Year && c.ContractDate.Value.Month == m.Month) && c.CandidateSource == esource).ToList().Count;
//                    sum += totalcandidates;
//                    table.Rows[count][columnName] = totalcandidates;
//                    table.AcceptChanges();
//                }
//                table.Rows[count + 1][columnName] = sum;
//                table.AcceptChanges();
//            }
//            table.Columns.Add(new DataColumn("YTD_", typeof(System.String)));
//            table.Columns.Add(new DataColumn("%_", typeof(System.String)));
//            table.AcceptChanges();
//            int _count = -1;
//            var total_sum = candidates.Where(c => c.ContractDate != null).Count();
//            double _percent_total = 0;
//            foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//            {
//                _count++;
//                int sum = 0;
//                foreach (var m in MonthOfYear)
//                {
//                    //tìm ứng viên đúng nguồn
//                    var totalcandidates = candidates.Where(c => c.ContractDate != null && (c.ContractDate.Value.Year == m.Year && c.ContractDate.Value.Month == m.Month) && c.CandidateSource == esource).ToList().Count;
//                    sum += totalcandidates;
//                }
//                double percent = 0;
//                if (total_sum != 0)
//                {
//                    percent = Math.Round(100 * ((double)sum / (double)total_sum), 1, MidpointRounding.AwayFromZero);
//                }
//                _percent_total += percent;
//                table.Rows[_count]["YTD_"] = sum;
//                table.Rows[_count]["%_"] = percent + "%";
//                table.AcceptChanges();
//            }
//            table.Rows[count_ + 1]["YTD_"] = total_sum;
//            table.Rows[count_ + 1]["%_"] = "0%%";
//            if (_percent_total > 0)
//            {
//                table.Rows[count_ + 1]["%_"] = "100%";
//            }
//            table.AcceptChanges();
//            #endregion

//            return table;
//        }
//        public static List<DataTable> ReportMonthConvertChannel(EProject project, string[] months)
//        {
//            List<DataTable> lstable = new List<DataTable>();
//            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
//            var candidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id).ToList();
//            var candidates_total = 0;
//            if (months.Length > 0)
//            {
//                foreach (var mon in months)
//                {
//                    if (mon.Length > 0)
//                    {
//                        string[] m = mon.Split('/');
//                        int r_month = int.Parse(m[0]);
//                        int r_year = int.Parse(m[1]);
//                        var candidates_ = candidates.Where(c => c.SubmissionDate.Value.Month == r_month && c.SubmissionDate.Value.Year == r_year).ToList().Count();
//                        candidates_total += candidates_;
//                    }
//                }
//            }
//            //Lấy các khu vực thuộc store của dự án
//            var regions = new List<Region>();
//            foreach (var m in project.ProjectNewDetail)
//            {
//                if (!regions.Contains(m.Store.CityRegion.Region))
//                {
//                    regions.Add(m.Store.CityRegion.Region);
//                }
//            }
//            foreach (var m in project.ProjectReplaceDetail)
//            {
//                if (!regions.Contains(m.Store.CityRegion.Region))
//                {
//                    regions.Add(m.Store.CityRegion.Region);
//                }
//            }
//            //Sắp xếp lại theo tên
//            regions = regions.OrderBy(c => c.RegionName).ToList();
//            #region month of year
//            List<CustomMonth> MonthOfYear = new List<CustomMonth>();
//            if (months.Length > 0)
//            {
//                foreach (var mon in months)
//                {
//                    if (mon.Length > 0)
//                    {
//                        string[] m = mon.Split('/');
//                        int r_month = int.Parse(m[0]);
//                        int r_year = int.Parse(m[1]);
//                        CustomMonth customMonth = new CustomMonth();
//                        customMonth.Month = r_month;
//                        customMonth.Year = r_year;
//                        customMonth.Name = "Tháng " + r_month.ToString() + " " + r_year.ToString();
//                        MonthOfYear.Add(customMonth);
//                    }
//                }
//                #endregion
//            #region Interview Status 
//                List<CustomItem> interviewStatusList = new List<CustomItem>();
//                interviewStatusList.Add(new CustomItem { Id = 5, Name = "Pass" });
//                interviewStatusList.Add(new CustomItem { Id = 4, Name = "Fail" });
//                interviewStatusList.Add(new CustomItem { Id = 3, Name = "Pending" });
//                interviewStatusList.Add(new CustomItem { Id = 10, Name = "Total" });
//                #endregion
//                // một ensouce là 1 datatable
//                foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//                {
//                    DataTable table = new DataTable("ConvertChannel");
//                    table.Columns.Add(new DataColumn("Sources Of Recruiment", typeof(System.String)));
//                    table.AcceptChanges();
//                    DataRow row = table.NewRow();
//                    row["Sources Of Recruiment"] = EZEnumHelper<EmploymentSource>.GetDisplayValue(esource);
//                    table.Rows.Add(row);
//                    table.AcceptChanges();
//                    foreach (var newm in MonthOfYear)
//                    {
//                        DataRow row_m = table.NewRow();
//                        row_m["Sources Of Recruiment"] = newm.Name;
//                        table.Rows.Add(row_m);
//                        table.AcceptChanges();
//                    }
//                    table.AcceptChanges();

//                    var sumtotalregion = 0;
//                    double sumtpercentregion = 0;
//                    // Thêm cột theo khu vực
//                    foreach (var item in regions)
//                    {
//                        var regioni = item.Id;
//                        var columnName = item.RegionName;
//                        table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
//                        table.AcceptChanges();
//                        int sumregion = 0;
//                        int count = 0;
//                        foreach (var m in MonthOfYear)
//                        {
//                            count++;
//                            var totalcandidates = candidates.Where(c => (c.SubmissionDate.Value.Month == m.Month) &&(c.SubmissionDate.Value.Year == m.Year) && (c.CityRegion.Region.Id == regioni) && c.CandidateSource == esource).ToList().Count();
//                            sumregion += totalcandidates;
//                            table.Rows[count][columnName] = totalcandidates;
//                            table.AcceptChanges();
//                        }
//                        //var totalcandidateregion = sumregion;
//                        // côt tổng đâu tiên
//                        table.Rows[0][columnName] = sumregion;
//                        table.AcceptChanges();
//                    }
//                    //2 cột tổng & phần trăm của region
//                    table.Columns.Add(new DataColumn("Total", typeof(System.String)));
//                    table.Columns.Add(new DataColumn("%", typeof(System.String)));
//                    table.AcceptChanges();
//                    int count_ = 0;
//                    foreach (var m in MonthOfYear)
//                    {
//                        count_++;
//                        var totalcandidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id && (c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year) && c.CandidateSource == esource).ToList().Count;
//                        double percent = 0;
//                        if (candidates.Count > 0)
//                        {
//                            percent = Math.Round(100 * ((double)totalcandidates / (double)candidates.Count), 1, MidpointRounding.AwayFromZero);
//                        }
//                        sumtotalregion += totalcandidates;
//                        sumtpercentregion += (int)percent;
//                        table.Rows[count_]["Total"] = totalcandidates;
//                        table.Rows[count_]["%"] = percent + "%";
//                        table.AcceptChanges();
//                    }
//                    table.Rows[0]["Total"] = sumtotalregion;
//                    table.Rows[0]["%"] = sumtpercentregion + "%";
//                    table.AcceptChanges();

//                    var currentround = Round.ZERO;
//                    // Thêm cột theo round 1 --> 3
//                    for (int i = 1; i <= 3; i++)
//                    {
//                        if (i == 1)
//                        {
//                            currentround = Round.ONE;
//                        }
//                        else
//                        if (i == 2)
//                        {
//                            currentround = Round.TWO;
//                        }
//                        else if (i == 3)
//                        {
//                            currentround = Round.THREE;
//                        }
//                        int totalsumround = 0;
//                        // Thêm cột theo tình trạng 
//                        foreach (var s in interviewStatusList)
//                        {
//                            int _count = 0;
//                            int _sumround = 0;
//                            int sum_total = 0;
//                            table.Columns.Add(new DataColumn(s.Name + "_" + i, typeof(System.String)));
//                            foreach (var m in MonthOfYear)
//                            {
//                                _count++;
//                                var totalcandidates = 0;
//                                //pass
//                                if (s.Id == 5)
//                                {
//                                    if(i == 3)
//                                    {
//                                        totalcandidates = candidates.Where(c => (c.RoundPassed == currentround) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                                    }
//                                    else
//                                    {
//                                        totalcandidates = candidates.Where(c => (c.State == InterviewStatus.PASS && c.RoundPassed == currentround) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                                    }
//                                }
//                                //fail
//                                else if (s.Id == 4)
//                                {
//                                    var newround = Round.ZERO;
//                                    if (currentround == Round.ONE)
//                                    {
//                                        newround = Round.ZERO;
//                                    }
//                                    else if (currentround == Round.TWO)
//                                    {
//                                        newround = Round.ONE;
//                                    }
//                                    else
//                                    {
//                                        newround = Round.TWO;
//                                    }
//                                    totalcandidates = candidates.Where(c => (c.State == InterviewStatus.FAILED && c.RoundPassed == newround) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                                }
//                                //pending
//                                else if (s.Id == 3)
//                                {
//                                    var newround = Round.ZERO;

//                                    if (currentround == Round.ONE)
//                                    {
//                                        newround = Round.ZERO;
//                                    }
//                                    else if (currentround == Round.TWO)
//                                    {
//                                        newround = Round.ONE;
//                                    }
//                                    else
//                                    {
//                                        newround = Round.TWO;
//                                    }
//                                    totalcandidates = candidates.Where(c => (c.State == InterviewStatus.PENDING && c.RoundPassed == newround) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                                }
//                                else
//                                {
//                                    var newround = Round.ZERO;

//                                    if (currentround == Round.ONE)
//                                    {
//                                        newround = Round.ZERO;
//                                    }
//                                    else if (currentround == Round.TWO)
//                                    {
//                                        newround = Round.ONE;
//                                    }
//                                    else
//                                    {
//                                        newround = Round.TWO;
//                                    }
//                                    totalcandidates = candidates.Where(c => ((c.State == InterviewStatus.PASS && c.RoundPassed == currentround) || (c.State == InterviewStatus.PENDING && c.RoundPassed == newround) || (c.State == InterviewStatus.FAILED && c.RoundPassed == newround)) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                                    _sumround -= totalcandidates;
//                                    sum_total += totalcandidates;
//                                }
//                                _sumround += totalcandidates;
//                                table.Rows[_count][s.Name + "_" + i] = totalcandidates;
//                                table.AcceptChanges();
//                            }
//                            totalsumround += _sumround;//tổng hàng đầu tiên
//                            if (s.Id == 10)
//                            {
//                                table.Rows[0][s.Name + "_" + i] = sum_total;
//                            }
//                            else
//                            {
//                                table.Rows[0][s.Name + "_" + i] = _sumround;
//                            }
//                            table.AcceptChanges();
//                        }
//                    }
//                    //add 2 cột cuối
//                    table.Columns.Add(new DataColumn("Convertion (HR/RFM)", typeof(System.String)));
//                    table.Columns.Add(new DataColumn("Convertion (Sourcing/RFM)", typeof(System.String)));
//                    table.AcceptChanges();
//                    int count1_ = 0;
//                    double converthr_sumtotal = 0;
//                    double convertsourcing_sumtotal = 0;
//                    foreach (var m in MonthOfYear)
//                    {
//                        int candidatepass1 = candidates.Where(c => (c.State == InterviewStatus.PASS && c.RoundPassed == Round.ONE) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                        int candidatepass3 = candidates.Where(c => (c.RoundPassed == Round.THREE) && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                        int candidatetotal = candidates.Where(c => c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year && c.CandidateSource == esource).ToList().Count();
//                        double converthr = 0;
//                        double convertsourcing = 0;
//                        if (candidatepass3 != 0)
//                        {
//                            converthr = Math.Round(((double)candidatepass1 / (double)candidatepass3), 0, MidpointRounding.AwayFromZero);
//                            convertsourcing = Math.Round(((double)candidatetotal / (double)candidatepass3), 0, MidpointRounding.AwayFromZero);
//                        }
//                        converthr_sumtotal += converthr;
//                        convertsourcing_sumtotal += convertsourcing;
//                        count1_++;
//                        table.Rows[count1_]["Convertion (HR/RFM)"] = converthr;
//                        table.Rows[count1_]["Convertion (Sourcing/RFM)"] = convertsourcing;
//                        table.AcceptChanges();
//                    }
//                    table.Rows[0]["Convertion (HR/RFM)"] = converthr_sumtotal;
//                    table.Rows[0]["Convertion (Sourcing/RFM)"] = convertsourcing_sumtotal;
//                    table.AcceptChanges();

//                    lstable.Add(table);
//                }

//                //table total - dòng cuối
//                DataTable tabletotal = new DataTable();
//                tabletotal.Columns.Add(new DataColumn("Sources Of Recruiment", typeof(System.String)));
//                tabletotal.AcceptChanges();
//                DataRow _row = tabletotal.NewRow();

//                _row["Sources Of Recruiment"] = "Total";
//                tabletotal.Rows.Add(_row);
//                tabletotal.AcceptChanges();
//                foreach (var r in regions)
//                {
//                    var rid = r.Id;
//                    var columnName = r.RegionName;
//                    tabletotal.Columns.Add(new DataColumn(columnName, typeof(System.String)));
//                    tabletotal.AcceptChanges();
//                    var total_candidate = 0;
//                    foreach (var item in MonthOfYear)
//                    {
//                        var totalcandidates = candidates.Where(c => (c.CityRegion.Region.Id == rid && c.SubmissionDate.Value.Month == item.Month && c.SubmissionDate.Value.Year == item.Year)).ToList().Count;
//                        total_candidate += totalcandidates;
//                    }
//                    _row[columnName] = total_candidate;
//                }
//                tabletotal.Columns.Add(new DataColumn("Total", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("%", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Pass_1", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Fail_1", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Pending_1", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Total_1", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Pass_2", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Fail_2", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Pending_2", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Total_2", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Pass_3", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Fail_3", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Pending_3", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Total_3", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Convertion (HR/RFM)", typeof(System.String)));
//                tabletotal.Columns.Add(new DataColumn("Convertion (Sourcing/RFM)", typeof(System.String)));

//                _row["Total"] = candidates_total;
//                _row["%"] = "100%";
//                var pass1 = 0;
//                var pass2 = 0;
//                var pass3 = 0;
//                var fail1 = 0;
//                var fail2 = 0;
//                var fail3 = 0;
//                var pending1 = 0;
//                var pending2 = 0;
//                var pending3 = 0;
//                foreach (var m in MonthOfYear)
//                {
//                    var pass1_ = candidates.Where(c => c.RoundPassed == Round.ONE && c.State == InterviewStatus.PASS && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var pass2_ = candidates.Where(c => c.RoundPassed == Round.TWO && c.State == InterviewStatus.PASS && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var pass3_ = candidates.Where(c => c.RoundPassed == Round.THREE && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var fail1_ = candidates.Where(c => c.RoundPassed == Round.ZERO && c.State == InterviewStatus.FAILED && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var fail2_ = candidates.Where(c => c.RoundPassed == Round.ONE && c.State == InterviewStatus.FAILED && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var fail3_ = candidates.Where(c => c.RoundPassed == Round.TWO && c.State == InterviewStatus.FAILED && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var pending1_ = candidates.Where(c => c.RoundPassed == Round.ZERO && c.State == InterviewStatus.PENDING && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var pending2_ = candidates.Where(c => c.RoundPassed == Round.ONE && c.State == InterviewStatus.PENDING && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    var pending3_ = candidates.Where(c => c.RoundPassed == Round.TWO && c.State == InterviewStatus.PENDING && c.SubmissionDate.Value.Month == m.Month && c.SubmissionDate.Value.Year == m.Year).ToList().Count();
//                    pass1 += pass1_;
//                    pass2 += pass2_;
//                    pass3 += pass3_;
//                    fail1 += fail1_;
//                    fail2 += fail2_;
//                    fail3 += fail3_;
//                    pending1 += pending1_;
//                    pending2 += pending2_;
//                    pending3 += pending3_;
//                }
//                var total1 = pass1 + fail1 + pending1;
//                var total2 = pass2 + fail2 + pending2;
//                var total3 = pass3 + fail3 + pending3;
//                _row["Pass_1"] = pass1;
//                _row["Fail_1"] = fail1;
//                _row["Pending_1"] = pending1;
//                _row["Total_1"] = total1;
//                _row["Pass_2"] = pass2;
//                _row["Fail_2"] = fail2;
//                _row["Pending_2"] = pending2;
//                _row["Total_2"] = total2;
//                _row["Pass_3"] = pass3;
//                _row["Fail_3"] = fail3;
//                _row["Pending_3"] = pending3;
//                _row["Total_3"] = total3;
//                _row["Convertion (HR/RFM)"] = "0%";
//                _row["Convertion (Sourcing/RFM)"] = "0%";
//                if (pass3 != 0)
//                {
//                    _row["Convertion (HR/RFM)"] = Math.Round(((double)pass1 / (double)pass3), 0, MidpointRounding.AwayFromZero) + "%";
//                    _row["Convertion (Sourcing/RFM)"] = Math.Round(((double)candidates_total / (double)pass3), 0, MidpointRounding.AwayFromZero) + "%";
//                }

//                lstable.Add(tabletotal);
//            }
//            return lstable;
//        }
//        //public static DataTable ReportMonthPassed3Round(EProject project, string[] months)
//        //{
//        //    Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
//        //    var candidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id && c.RoundPassed == Constraint.Round.THREE).ToList();
//        //    var candidates_total = 0;
//        //    if (months.Length > 0)
//        //    {
//        //        foreach (var mon in months)
//        //        {
//        //            if (mon.Length > 0)
//        //            {
//        //                string[] m = mon.Split('/');
//        //                int r_month = int.Parse(m[0]);
//        //                int r_year = int.Parse(m[1]);
//        //                var candidates_ = candidates.Where(c => c.SubmissionDate.Value.Month == r_month && c.SubmissionDate.Value.Year == r_year).ToList().Count();
//        //                candidates_total += candidates_;
//        //            }
//        //        }
//        //    }            //Lấy các khu vực thuộc store của dự án
//        //    var regions = new List<Region>();
//        //    foreach (var m in project.ProjectNewDetail)
//        //    {
//        //        if (!regions.Contains(m.Store.CityRegion.Region))
//        //        {
//        //            regions.Add(m.Store.CityRegion.Region);
//        //        }
//        //    }
//        //    foreach (var m in project.ProjectReplaceDetail)
//        //    {
//        //        if (!regions.Contains(m.Store.CityRegion.Region))
//        //        {
//        //            regions.Add(m.Store.CityRegion.Region);
//        //        }
//        //    }
//        //    //Sắp xếp lại theo tên
//        //    regions = regions.OrderBy(c => c.RegionName).ToList();
//        //    #region month of year
//        //    List<CustomMonth> MonthOfYear = new List<CustomMonth>();
//        //    if (months.Length > 0)
//        //    {
//        //        foreach (var mon in months)
//        //        {
//        //            if (mon.Length > 0)
//        //            {
//        //                string[] m = mon.Split('/');
//        //                int r_month = int.Parse(m[0]);
//        //                int r_year = int.Parse(m[1]);
//        //                CustomMonth customMonth = new CustomMonth();
//        //                customMonth.Month = r_month;
//        //                customMonth.Year = r_year;
//        //                customMonth.Name = "Tháng " + r_month.ToString() + " " + r_year.ToString();
//        //                MonthOfYear.Add(customMonth);
//        //            }
//        //        }
//        //    }
//        //    #endregion

//        //    DataTable table = new DataTable("Candidate Passed 3 Round Report");
//        //    table.Columns.Add(new DataColumn("Sources Of Recruiment", typeof(System.String)));
//        //    table.AcceptChanges();
//        //    foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//        //    {
//        //        DataRow row = table.NewRow();
//        //        row["Sources Of Recruiment"] = EZEnumHelper<EmploymentSource>.GetDisplayValue(esource);
//        //        table.Rows.Add(row);
//        //        table.AcceptChanges();
//        //    }
//        //    DataRow _row = table.NewRow();
//        //    _row["Sources Of Recruiment"] = "Total";
//        //    table.Rows.Add(_row);
//        //    table.AcceptChanges();

//        //    foreach (var item in regions)
//        //    {
//        //        var regioni = item.Id;
//        //        var columnName = item.RegionName;
//        //        table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
//        //        table.AcceptChanges();

//        //        int count = -1;
//        //        int sum = 0;
//        //        foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//        //        {
//        //            count++;
//        //            //tìm ứng viên đúng nguồn
//        //            var total_candidates = 0;
//        //            foreach (var m in MonthOfYear)
//        //            {
//        //                var totalcandidates = candidates.Where(c => (c.SubmissionDate.Value.Year == m.Year && c.SubmissionDate.Value.Month == m.Month) && (c.CityRegion.Region.Id == regioni) && c.CandidateSource == esource).ToList().Count;
//        //                total_candidates += totalcandidates;
//        //            }
//        //            sum += total_candidates;
//        //            table.Rows[count][columnName] = total_candidates;
//        //            table.AcceptChanges();
//        //        }
//        //        table.Rows[count + 1][columnName] = sum;
//        //        table.AcceptChanges();
//        //    }
//        //    table.Columns.Add(new DataColumn("Total", typeof(System.String)));
//        //    table.Columns.Add(new DataColumn("%", typeof(System.String)));
//        //    table.AcceptChanges();
//        //    int count1 = -1;
//        //    int sumtotal = 0;
//        //    double total_pecent = 0;
//        //    foreach (EmploymentSource esource in (EmploymentSource[])Enum.GetValues(typeof(EmploymentSource)))
//        //    {
//        //        count1++;
//        //        double percent = 0;
//        //        int sum1 = 0;
//        //        foreach (var item in regions)
//        //        {
//        //            var regioni = item.Id;
//        //            //tìm ứng viên đúng nguồn
//        //            var total_candidates = 0;
//        //            foreach (var m in MonthOfYear)
//        //            {
//        //                var totalcandidates = candidates.Where(c => (c.SubmissionDate.Value.Year == m.Year && c.SubmissionDate.Value.Month == m.Month) && (c.CityRegion.Region.Id == regioni) && c.CandidateSource == esource).ToList().Count;
//        //                total_candidates += totalcandidates;
//        //            }
//        //            sum1 += total_candidates;
//        //        }
//        //        sumtotal += sum1;
//        //        table.Rows[count1]["Total"] = sum1;
//        //        if(candidates_total != 0)
//        //        {
//        //            percent = Math.Round(100 * ((double)sum1 / (double)candidates_total), 1, MidpointRounding.AwayFromZero);
//        //        }
//        //        table.Rows[count1]["%"] = percent + "%";
//        //        table.AcceptChanges();
//        //        total_pecent += percent;
//        //    }

//        //    table.Rows[count1 + 1]["Total"] = sumtotal;
//        //    table.Rows[count1 + 1]["%"] = "0%";
//        //    if (total_pecent > 0)
//        //    {
//        //        table.Rows[count1 + 1]["%"] = "100%";
//        //    }
//        //    table.AcceptChanges();
           
//        //    return table;
//        //}

//    }
//}
//public class CustomItem
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//}
//public class CustomMonth
//{
//    public int Month { get; set; }
//    public int Year { get; set; }
//    public string Name { get; set; }
//}

