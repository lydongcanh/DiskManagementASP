//using Ehr.Common.Constraint;
//using Ehr.Common.Tools;
//using Ehr.Models;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;

//namespace Ehr.Common.UI
//{
//    public class ReportDetailMonthHelper
//    {
//        public static List<DataTable> ReportMonthConvertChannel(EProject project, DateTime date)
//        {
//            int year = date.Year;
//            int month = date.Month;
//            List<DataTable> lstable = new List<DataTable>();
//            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
//            var candidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id).ToList();
//            candidates = candidates.Where(c => (c.SubmissionDate.Value.Year == year && c.SubmissionDate.Value.Date.Month <= month)).ToList();
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
//            List<CustomItem> MonthOfYear = new List<CustomItem>();
//            MonthOfYear.Add(new CustomItem { Id = 1, Name = "Jan" });
//            MonthOfYear.Add(new CustomItem { Id = 2, Name = "Fed" });
//            MonthOfYear.Add(new CustomItem { Id = 3, Name = "Mar" });
//            MonthOfYear.Add(new CustomItem { Id = 4, Name = "Apr" });
//            MonthOfYear.Add(new CustomItem { Id = 5, Name = "May" });
//            MonthOfYear.Add(new CustomItem { Id = 6, Name = "Jun" });
//            MonthOfYear.Add(new CustomItem { Id = 7, Name = "Jul" });
//            MonthOfYear.Add(new CustomItem { Id = 8, Name = "Aug" });
//            MonthOfYear.Add(new CustomItem { Id = 9, Name = "Sep" });
//            MonthOfYear.Add(new CustomItem { Id = 10, Name = "Oct" });
//            MonthOfYear.Add(new CustomItem { Id = 11, Name = "Nov" });
//            MonthOfYear.Add(new CustomItem { Id = 12, Name = "Dec" });
//            #endregion
//            #region Interview Status 
//            List<CustomItem> interviewStatusList = new List<CustomItem>();
//            interviewStatusList.Add(new CustomItem { Id = 5, Name = "Pass" });
//            interviewStatusList.Add(new CustomItem { Id = 4, Name = "Fail" });
//            interviewStatusList.Add(new CustomItem { Id = 3, Name = "Pending" });
//            interviewStatusList.Add(new CustomItem { Id = 10, Name = "Total" });
//            #endregion
//            // một ensouce là 1 datatable
//            foreach (var reg in regions)
//            {
//                DataTable table = new DataTable("ConvertChannel");
//                table.Columns.Add(new DataColumn("Số nhân viên cần tuyển", typeof(System.String)));
//                table.Columns.Add(new DataColumn("Số hồ sơ cần có", typeof(System.String)));
//                table.Columns.Add(new DataColumn("Actual CV", typeof(System.String)));
//                table.Columns.Add(new DataColumn("Unused Lead of Month", typeof(System.String)));
//                table.Columns.Add(new DataColumn("Khu vực chưa tuyển dụng", typeof(System.String)));
//                table.AcceptChanges();
//                DataRow row = table.NewRow();
//                row["Sources Of Recruiment"] = reg.RegionName;
//                table.Rows.Add(row);
//                table.AcceptChanges();
//                foreach (var newm in MonthOfYear)
//                {
//                    DataRow row_m = table.NewRow();
//                    row_m["Sources Of Recruiment"] = newm.Name;
//                    table.Rows.Add(row_m);
//                    table.AcceptChanges();
//                    DataRow row_n = table.NewRow();
//                    row_n["Sources Of Recruiment"] = "Up to " + newm.Name;
//                    table.Rows.Add(row_m);
//                    table.AcceptChanges();
//                }
//                table.AcceptChanges();

//                var sumtotalregion = 0;
//                double sumtpercentregion = 0;
//                // Thêm cột theo khu vực
//                int sum_numberofrequest = 0;
//                int sum_requestnumber = 0;
//                int sum_actualcv = 0;
//                int sum_unusedlead = 0;
//                int sum_waitcandidates = 0;
//                int count = 0;
//                foreach (var m in MonthOfYear)
//                {
//                    count++;
//                    int upto_actualcv = 0;
//                    int upto_unusedlead = 0;
//                    // số nhân viên cần tuyển
//                    var numberofrequest = project.ProjectNewDetail.Where(c => c.Store.CityRegion.Id == reg.Id).Sum(c => c.NumberOfRequested) + project.ProjectReplaceDetail.Where(c => c.Store.CityRegion.Id == reg.Id).Sum(c => c.NumberOfRequested);
//                    // số nhân viên cần có
//                    var requestnumber = numberofrequest * 10;
//                    // số nhân viên cần có
//                    var actualcv = candidates.Where(c => c.CityRegion.Id == reg.Id && c.SubmissionDate.Value.Month == m.Id).ToList().Count();
//                    // số nhân viên State = New -- Do New chua co de tam RESIGN
//                    var unusedlead = candidates.Where(c => c.CityRegion.Id == reg.Id && c.SubmissionDate.Value.Month == m.Id && c.State == InterviewStatus.RESIGN).ToList().Count();
//                    // số nhân viên chưa tuyển dụng
//                    var waitcandidates = candidates.Where(c => c.CityRegion.Id == reg.Id && c.SubmissionDate.Value.Month == m.Id && c.State == InterviewStatus.WAIT).ToList().Count();

//                    if(m.Id == 1)
//                    {
//                        upto_actualcv += actualcv;
//                    }
//                    else
//                    {
//                        upto_actualcv = upto_unusedlead + actualcv;
//                    }
//                    upto_unusedlead += actualcv;

//                    sum_numberofrequest += numberofrequest;
//                    sum_requestnumber += requestnumber;
//                    sum_actualcv += actualcv;
//                    sum_unusedlead += unusedlead;
//                    sum_waitcandidates += waitcandidates;

//                    //sumregion += totalcandidates;
//                    table.Rows[count]["Số nhân viên cần tuyển"] = numberofrequest;
//                    table.Rows[count + 1]["Số nhân viên cần tuyển"] = sum_numberofrequest;
//                    table.Rows[count]["Số hồ sơ cần có"] = requestnumber;
//                    table.Rows[count + 1]["Số hồ sơ cần có"] = sum_requestnumber;
//                    table.Rows[count]["Actual CV"] = actualcv;
//                    table.Rows[count + 1]["Actual CV"] = upto_actualcv;
//                    table.Rows[count]["Unused Lead of Month"] = unusedlead;
//                    table.Rows[count + 1]["Unused Lead of Month"] = upto_unusedlead;
//                    table.Rows[count]["Khu vực chưa tuyển dụng"] = waitcandidates;
//                    table.Rows[count + 1]["Khu vực chưa tuyển dụng"] = sum_waitcandidates;
//                    table.AcceptChanges();
//                }
//                table.Rows[0]["Số nhân viên cần tuyển"] = sum_numberofrequest;
//                table.Rows[0]["Số hồ sơ cần có"] = sum_requestnumber;
//                table.Rows[0]["Actual CV"] = sum_actualcv;
//                table.Rows[0]["Unused Lead of Month"] = sum_unusedlead;
//                table.Rows[0]["Khu vực chưa tuyển dụng"] = sum_waitcandidates;

//                var totalcandidateregion = candidates.Where(c => (c.CityRegion.Region.Id == regioni) && c.CandidateSource == esource).ToList().Count;
//                // côt tổng đâu tiên
//                table.Rows[0][columnName] = totalcandidateregion;
//                table.AcceptChanges();
//                table.Columns.Add(new DataColumn("Total", typeof(System.String)));
//                table.Columns.Add(new DataColumn("%", typeof(System.String)));
//                table.AcceptChanges();
//                int count_ = 0;
//                foreach (var m in MonthOfYear)
//                {
//                    count_++;
//                    var totalcandidates = db.Candidates.Where(c => c.Form.Project.Id == project.Id && (c.SubmissionDate.Value.Year == year && c.SubmissionDate.Value.Month == m.Id) && c.CandidateSource == esource).ToList().Count;
//                    double percent = 0;
//                    if (candidates.Count > 0)
//                    {
//                        percent = Math.Round(100 * ((double)totalcandidates / (double)candidates.Count), 1, MidpointRounding.AwayFromZero);
//                    }
//                    sumtotalregion += totalcandidates;
//                    sumtpercentregion += (int)percent;
//                    table.Rows[count_]["Total"] = totalcandidates;
//                    table.Rows[count_]["%"] = percent + "%";
//                    table.AcceptChanges();
//                }
//                table.Rows[0]["Total"] = sumtotalregion;
//                table.Rows[0]["%"] = sumtpercentregion + "%";
//                table.AcceptChanges();

//                var currentround = Round.ZERO;
//                // Thêm cột theo round 1 --> 3
//                for (int i = 1; i <= 3; i++)
//                {
//                    if (i == 1)
//                    {
//                        currentround = Round.ONE;
//                    }
//                    else
//                    if (i == 2)
//                    {
//                        currentround = Round.TWO;
//                    }
//                    else if (i == 3)
//                    {
//                        currentround = Round.THREE;
//                    }
//                    int totalsumround = 0;
//                    // Thêm cột theo tình trạng 
//                    foreach (var s in interviewStatusList)
//                    {
//                        int _count = 0;
//                        int _sumround = 0;
//                        int sum_total = 0;
//                        table.Columns.Add(new DataColumn(s.Name + "_" + i, typeof(System.String)));
//                        foreach (var m in MonthOfYear.OrderBy(c => c.Id))
//                        {
//                            _count++;
//                            var totalcandidates = 0;
//                            //pass
//                            if (s.Id == 5)
//                            {
//                                totalcandidates = candidates.Where(c => (c.State == InterviewStatus.PASS && c.RoundPassed == currentround) && c.SubmissionDate.Value.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                            }
//                            //fail
//                            else if (s.Id == 4)
//                            {
//                                var newround = Round.ZERO;
//                                if (currentround == Round.ONE)
//                                {
//                                    newround = Round.ZERO;
//                                }
//                                else if (currentround == Round.TWO)
//                                {
//                                    newround = Round.ONE;
//                                }
//                                else
//                                {
//                                    newround = Round.TWO;
//                                }
//                                totalcandidates = candidates.Where(c => (c.State == InterviewStatus.FAILED && c.RoundPassed == newround) && c.SubmissionDate.Value.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                            }
//                            //pending
//                            else if (s.Id == 3)
//                            {
//                                var newround = Round.ZERO;

//                                if (currentround == Round.ONE)
//                                {
//                                    newround = Round.ZERO;
//                                }
//                                else if (currentround == Round.TWO)
//                                {
//                                    newround = Round.ONE;
//                                }
//                                else
//                                {
//                                    newround = Round.TWO;
//                                }
//                                totalcandidates = candidates.Where(c => (c.State == InterviewStatus.PENDING && c.RoundPassed == newround) && c.SubmissionDate.Value.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                            }
//                            else
//                            {
//                                var newround = Round.ZERO;

//                                if (currentround == Round.ONE)
//                                {
//                                    newround = Round.ZERO;
//                                }
//                                else if (currentround == Round.TWO)
//                                {
//                                    newround = Round.ONE;
//                                }
//                                else
//                                {
//                                    newround = Round.TWO;
//                                }
//                                totalcandidates = candidates.Where(c => ((c.State == InterviewStatus.PASS && c.RoundPassed == currentround) || (c.State == InterviewStatus.PENDING && c.RoundPassed == newround) || (c.State == InterviewStatus.FAILED && c.RoundPassed == newround)) && c.SubmissionDate.Value.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                                _sumround -= totalcandidates;
//                                sum_total += totalcandidates;
//                            }
//                            _sumround += totalcandidates;
//                            table.Rows[_count][s.Name + "_" + i] = totalcandidates;
//                            table.AcceptChanges();
//                        }
//                        totalsumround += _sumround;//tổng hàng đầu tiên
//                        if (s.Id == 10)
//                        {
//                            table.Rows[0][s.Name + "_" + i] = sum_total;
//                        }
//                        else
//                        {
//                            table.Rows[0][s.Name + "_" + i] = _sumround;
//                        }
//                        table.AcceptChanges();
//                    }
//                }
//                //add 2 cột cuối
//                table.Columns.Add(new DataColumn("Convertion (HR/RFM)", typeof(System.String)));
//                table.Columns.Add(new DataColumn("Convertion (Sourcing/RFM)", typeof(System.String)));
//                table.AcceptChanges();
//                int count1_ = 0;
//                double converthr_sumtotal = 0;
//                double convertsourcing_sumtotal = 0;
//                foreach (var m in MonthOfYear.OrderBy(c => c.Id))
//                {
//                    int candidatepass1 = candidates.Where(c => (c.State == InterviewStatus.PASS && c.RoundPassed == Round.ONE) && c.SubmissionDate.Value.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                    int candidatepass3 = candidates.Where(c => (c.State == InterviewStatus.PASS && c.RoundPassed == Round.THREE) && c.SubmissionDate.Value.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                    int candidatetotal = candidates.Where(c => c.SubmissionDate.Value.Date.Month == m.Id && c.CandidateSource == esource).ToList().Count();
//                    double converthr = 0;
//                    double convertsourcing = 0;
//                    if (candidatepass3 != 0)
//                    {
//                        converthr = Math.Round(((double)candidatepass1 / (double)candidatepass3), 0, MidpointRounding.AwayFromZero);
//                        convertsourcing = Math.Round(((double)candidatetotal / (double)candidatepass3), 0, MidpointRounding.AwayFromZero);
//                    }
//                    converthr_sumtotal += converthr;
//                    convertsourcing_sumtotal += convertsourcing;
//                    count1_++;
//                    table.Rows[count1_]["Convertion (HR/RFM)"] = converthr;
//                    table.Rows[count1_]["Convertion (Sourcing/RFM)"] = convertsourcing;
//                    table.AcceptChanges();
//                }
//                table.Rows[0]["Convertion (HR/RFM)"] = converthr_sumtotal;
//                table.Rows[0]["Convertion (Sourcing/RFM)"] = convertsourcing_sumtotal;
//                table.AcceptChanges();

//                lstable.Add(table);
//            }

//            //table total - dòng cuối
//            DataTable tabletotal = new DataTable();
//            tabletotal.Columns.Add(new DataColumn("Sources Of Recruiment", typeof(System.String)));
//            tabletotal.AcceptChanges();
//            DataRow _row = tabletotal.NewRow();

//            _row["Sources Of Recruiment"] = "Total";
//            tabletotal.Rows.Add(_row);
//            tabletotal.AcceptChanges();
//            foreach (var r in regions)
//            {
//                var rid = r.Id;
//                var columnName = r.RegionName;
//                tabletotal.Columns.Add(new DataColumn(columnName, typeof(System.String)));
//                tabletotal.AcceptChanges();
//                var totalcandidates = candidates.Where(c => (c.CityRegion.Region.Id == rid)).ToList().Count;
//                _row[columnName] = totalcandidates;
//            }
//            tabletotal.Columns.Add(new DataColumn("Total", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("%", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Pass_1", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Fail_1", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Pending_1", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Total_1", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Pass_2", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Fail_2", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Pending_2", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Total_2", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Pass_3", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Fail_3", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Pending_3", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Total_3", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Convertion (HR/RFM)", typeof(System.String)));
//            tabletotal.Columns.Add(new DataColumn("Convertion (Sourcing/RFM)", typeof(System.String)));

//            _row["Total"] = candidates.Count();
//            _row["%"] = "100%";
//            var pass1 = candidates.Where(c => c.RoundPassed == Round.ONE && c.State == InterviewStatus.PASS).ToList().Count();
//            var pass2 = candidates.Where(c => c.RoundPassed == Round.TWO && c.State == InterviewStatus.PASS).ToList().Count();
//            var pass3 = candidates.Where(c => c.RoundPassed == Round.THREE && c.State == InterviewStatus.PASS).ToList().Count();
//            var fail1 = candidates.Where(c => c.RoundPassed == Round.ZERO && c.State == InterviewStatus.FAILED).ToList().Count();
//            var fail2 = candidates.Where(c => c.RoundPassed == Round.ONE && c.State == InterviewStatus.FAILED).ToList().Count();
//            var fail3 = candidates.Where(c => c.RoundPassed == Round.TWO && c.State == InterviewStatus.FAILED).ToList().Count();
//            var pending1 = candidates.Where(c => c.RoundPassed == Round.ZERO && c.State == InterviewStatus.PENDING).ToList().Count();
//            var pending2 = candidates.Where(c => c.RoundPassed == Round.ONE && c.State == InterviewStatus.PENDING).ToList().Count();
//            var pending3 = candidates.Where(c => c.RoundPassed == Round.TWO && c.State == InterviewStatus.PENDING).ToList().Count();
//            var total1 = pass1 + fail1 + pending1;
//            var total2 = pass2 + fail2 + pending2;
//            var total3 = pass3 + fail3 + pending3;
//            _row["Pass_1"] = pass1;
//            _row["Fail_1"] = fail1;
//            _row["Pending_1"] = pending1;
//            _row["Total_1"] = total1;
//            _row["Pass_2"] = pass2;
//            _row["Fail_2"] = fail2;
//            _row["Pending_2"] = pending2;
//            _row["Total_2"] = total2;
//            _row["Pass_3"] = pass3;
//            _row["Fail_3"] = fail3;
//            _row["Pending_3"] = pending3;
//            _row["Total_3"] = total3;
//            if (pass3 != 0)
//            {
//                _row["Convertion (HR/RFM)"] = Math.Round(((double)pass1 / (double)pass3), 0, MidpointRounding.AwayFromZero);
//                _row["Convertion (Sourcing/RFM)"] = Math.Round(((double)candidates.Count / (double)pass3), 0, MidpointRounding.AwayFromZero);
//            }
//            _row["Convertion (HR/RFM)"] = "0%";
//            _row["Convertion (Sourcing/RFM)"] = "0%";

//            lstable.Add(tabletotal);
//            return lstable;
//        }

//    }
//}