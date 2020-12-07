//using Ehr.Common.Constraint;
//using Ehr.Data;
//using Ehr.Models;
//using Ehr.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web;

//namespace Ehr.Common.UI
//{
//    public class MonthDetailReportHelper
//    {
//        public List<List<Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>>> Graph_1(string idproject, string valueoption, string option, string year)
//        {
//            var result = new List<List<Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>>>();
//            var db = new EhrDbContext();

//            var idprj = int.Parse(idproject);
//            var opt = int.Parse(option);
//            var value = int.Parse(valueoption);
//            var yr = int.Parse(year);


//            var prj = db.EProjects.FirstOrDefault(s => s.Id == idprj);
//            //var rg1 = prj.ProjectNewDetail.Select(s => s.Store.CityRegion);
//            //var rg2 = prj.ProjectReplaceDetail.Select(s => s.Store.CityRegion);
//            //var rgs = rg1.Union(rg2).Distinct().ToList();

//            var regionCandidates = new List<Dictionary<int, List<Candidate>>>();
//            if (opt == 1)
//            {
//                regionCandidates = db.Forms.Where(f => f.Project.Id == idprj
//                && ((f.Project.StartingDate.Month == value && f.Project.StartingDate.Year == yr) || (f.Project.EndingDate.Month == value && f.Project.EndingDate.Year == yr)))
//                .AsEnumerable()
//                .SelectMany(s => s.Candidates)
//                .GroupBy(s => s.CityRegion.Region.Id)
//                .OrderBy(s => s.Key)
//                .Select(s => new Dictionary<int, List<Candidate>>
//                {
//                    {s.Key, s.ToList()}
//                }).ToList();
//            }
//            else if (opt == 2)
//            {
//                if (value == 1)
//                {
//                    regionCandidates = db.Forms.Where(f => f.Project.Id == idprj
//                                  && ((f.Project.StartingDate.Month >= 1 && f.Project.StartingDate.Year == yr) || (f.Project.EndingDate.Month <= 3 && f.Project.EndingDate.Year == yr)))
//                                  .AsEnumerable()
//                                  .SelectMany(s => s.Candidates)
//                                  .GroupBy(s => s.CityRegion.Region.Id)
//                                  .OrderBy(s => s.Key)
//                                  .Select(s => new Dictionary<int, List<Candidate>>
//                                  {
//                                       {s.Key, s.ToList()}
//                                  }).ToList();
//                }

//                if (value == 2)
//                {
//                    regionCandidates = db.Forms.Where(f => f.Project.Id == idprj
//                                  && ((f.Project.StartingDate.Month >= 4 && f.Project.StartingDate.Year == yr) || (f.Project.EndingDate.Month <= 6 && f.Project.EndingDate.Year == yr)))
//                                  .AsEnumerable()
//                                  .SelectMany(s => s.Candidates)
//                                  .GroupBy(s => s.CityRegion.Region.Id)
//                                  .OrderBy(s => s.Key)
//                                  .Select(s => new Dictionary<int, List<Candidate>>
//                                  {
//                                       {s.Key, s.ToList()}
//                                  }).ToList();
//                }

//                if (value == 3)
//                {
//                    regionCandidates = db.Forms.Where(f => f.Project.Id == idprj
//                                  && ((f.Project.StartingDate.Month >= 7 && f.Project.StartingDate.Year == yr) || (f.Project.EndingDate.Month <= 9 && f.Project.EndingDate.Year == yr)))
//                                  .AsEnumerable()
//                                  .SelectMany(s => s.Candidates)
//                                  .GroupBy(s => s.CityRegion.Region.Id)
//                                  .OrderBy(s => s.Key)
//                                  .Select(s => new Dictionary<int, List<Candidate>>
//                                  {
//                                       {s.Key, s.ToList()}
//                                  }).ToList();
//                }

//                if (value == 4)
//                {
//                    regionCandidates = db.Forms.Where(f => f.Project.Id == idprj
//                                  && ((f.Project.StartingDate.Month >= 10 && f.Project.StartingDate.Year == yr) || (f.Project.EndingDate.Month <= 12 && f.Project.EndingDate.Year == yr)))
//                                  .AsEnumerable()
//                                  .SelectMany(s => s.Candidates)
//                                  .GroupBy(s => s.CityRegion.Region.Id)
//                                  .OrderBy(s => s.Key)
//                                  .Select(s => new Dictionary<int, List<Candidate>>
//                                  {
//                                       {s.Key, s.ToList()}
//                                  }).ToList();
//                }

//            }
//            else
//            {
//                regionCandidates = db.Forms.Where(f => f.Project.Id == idprj
//                                                && ((f.Project.StartingDate.Year == yr) || (f.Project.EndingDate.Year == yr)))
//                                                .AsEnumerable()
//                                                .SelectMany(s => s.Candidates)
//                                                .GroupBy(s => s.CityRegion.Region.Id)
//                                                .OrderBy(s => s.Key)
//                                                .Select(s => new Dictionary<int, List<Candidate>>
//                                                {
//                                       {s.Key, s.ToList()}
//                                                }).ToList();
//            }




//            var list = new Dictionary<int, List<Candidate>>();
//            foreach (var item in regionCandidates)
//            {
//                foreach (var v in item)
//                {
//                    list.Add(v.Key, v.Value);

//                }
//            }
//            var clone = list.ToDictionary(e => e.Key, e => e.Value);
//            var numberOfRequest = new List<int>();
//            foreach (var item in list)
//            {
//                // Row đầu tiên là tổng
//                var regionTuples = new List<Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>>();
//                var candidates = item.Value.ToList();
//                var newprj = 0;
//                var replaceprj = 0;
//                if (opt == 1)
//                {
//                    candidates = candidates.Where(s => s.SubmissionDate?.Month == value && s.SubmissionDate?.Year == yr).ToList();
//                    newprj = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == value && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == value && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                    replaceprj = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == value && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == value && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                }

//                if (opt == 2)
//                {
//                    if (value == 1)
//                    {
//                        candidates = candidates.Where(s => s.SubmissionDate?.Month >= 1 && s.SubmissionDate?.Month <= 3 && s.SubmissionDate?.Year == yr).ToList();
//                        newprj = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 1 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 3 && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        replaceprj = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 1 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 3 && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                    }
//                    if (value == 2)
//                    {
//                        candidates = candidates.Where(s => s.SubmissionDate?.Month >= 4 && s.SubmissionDate?.Month <= 6 && s.SubmissionDate?.Year == yr).ToList();
//                        newprj = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 4 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 6 && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        replaceprj = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 4 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 6 && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);

//                    }
//                    if (value == 3)
//                    {
//                        candidates = candidates.Where(s => s.SubmissionDate?.Month >= 7 && s.SubmissionDate?.Month <= 9 && s.SubmissionDate?.Year == yr).ToList();
//                        newprj = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 7 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 9 && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        replaceprj = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 7 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 9 && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                    }
//                    if (value == 4)
//                    {
//                        candidates = candidates.Where(s => s.SubmissionDate?.Month >= 10 && s.SubmissionDate?.Month <= 12 && s.SubmissionDate?.Year == yr).ToList();
//                        newprj = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 10 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 12 && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        replaceprj = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month >= 10 && p.StartingDate?.Year == yr) || (p.EndingDate?.Month <= 12 && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                    }
//                }
//                if (opt == 3)
//                {
//                    candidates = candidates.Where(s => s.SubmissionDate?.Year == yr).ToList();
//                    for (int i = 1; i <= 12; i++)
//                    {
//                        newprj += prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == i && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == i && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        replaceprj += prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == i && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == i && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                    }
//                }


//                clone[item.Key] = candidates;


//                var request = newprj + replaceprj;

//                var candidateCVScan = candidates.Where(s => s.Round1_Date!=null);
//                var candidateFirstRound = candidates.Where(s => s.Round2_Date!=null);
//                var candidateSecondRound = candidates.Where(s => s.Round3_Date!=null);


//                #region hàng đầu tiên của một region
//                var regionConversion = new RegionConversion();
//                regionConversion.RegionName = db.Regions.FirstOrDefault(s => s.Id == item.Key).RegionName;
//                regionConversion.NumberOfActual = candidates.Count;
//                regionConversion.NumberOfRequest = request;
//                regionConversion.NumberOfRequired = request * 10;
//                regionConversion.UnsedLeadOfMonth = 0;//chưa tính
//                regionConversion.NumberOfNotRecruit = 0;//chưa tính

//                var cvScan = new CVScan();
//                cvScan.Fail = candidateCVScan.Where(s => s.Result_R1== Result.FAILED).Count();
//                cvScan.Pass = candidateCVScan.Where(s => s.Result_R1== Result.PASSED).Count();
//                cvScan.Pending = candidateCVScan.Where(s => s.Result_R1== Result.WAIT).Count();
//                cvScan.Total = cvScan.Fail + cvScan.Pass + cvScan.Pending;

//                var fRound = new FirstRound();
//                fRound.Fail = candidateFirstRound.Where(s => s.Result_R2== Result.FAILED).Count();
//                fRound.Pass = candidateFirstRound.Where(s => s.Result_R2== Result.PASSED).Count();
//                fRound.Pending = candidateFirstRound.Where(s => s.Result_R2== Result.WAIT).Count();
//                fRound.Total = fRound.Fail + fRound.Pass + cvScan.Pending;

//                var sRound = new SecondRound();
//                sRound.Fail = candidateSecondRound.Where(s => s.Result_R3== Result.FAILED).Count();
//                sRound.Pass = candidateSecondRound.Where(s => s.Result_R3== Result.PASSED).Count();
//                sRound.Pending = candidateSecondRound.Where(s => s.Result_R3== Result.WAIT).Count();
//                sRound.Total = sRound.Fail + sRound.Pass + sRound.Pending;

//                var conversion = new Conversion();
//                if (sRound.Pass > 0)
//                {
//                    conversion.HR = Math.Round(((double)cvScan.Pass / sRound.Pass), 1);
//                    conversion.Sourcing = Math.Round(((double)regionConversion.NumberOfActual / sRound.Pass), 1);
//                }

//                var tuple = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversion, cvScan, fRound, sRound, conversion);
//                regionTuples.Add(tuple);

//                #endregion

//                #region các hàng còn lại theo tháng
//                // Tính ra các tháng
//                if (opt == 1)
//                {
//                    var regionConversionDetail = new RegionConversion
//                    {
//                        NumberOfActual = regionConversion.NumberOfActual,
//                        NumberOfNotRecruit = regionConversion.NumberOfNotRecruit,
//                        NumberOfRequest = regionConversion.NumberOfRequest,
//                        NumberOfRequired = regionConversion.NumberOfRequired,
//                        RegionName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value),
//                        UnsedLeadOfMonth = regionConversion.UnsedLeadOfMonth
//                    };

//                    var regionConversionDetailUpto = new RegionConversion
//                    {
//                        NumberOfActual = regionConversion.NumberOfActual,
//                        NumberOfNotRecruit = regionConversion.NumberOfNotRecruit,
//                        NumberOfRequest = regionConversion.NumberOfRequest,
//                        NumberOfRequired = regionConversion.NumberOfRequired,
//                        RegionName = "Up to " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value),
//                        UnsedLeadOfMonth = regionConversion.UnsedLeadOfMonth
//                    };
//                    var tupleDetail = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversionDetail, cvScan, fRound, sRound, conversion);
//                    var tupleDetailUpto = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversionDetailUpto, cvScan, fRound, sRound, conversion);
//                    regionTuples.Add(tupleDetail);
//                    regionTuples.Add(tupleDetailUpto);
//                }
//                else if (opt == 2)
//                {
//                    for (int monthInQuarter = value * 3 - 2; monthInQuarter <= value * 3; monthInQuarter++)
//                    {
//                        var cands = candidates.Where(s => s.SubmissionDate?.Month == monthInQuarter).ToList();
//                        var newprjQ = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == monthInQuarter && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == monthInQuarter && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        var replaceprjQ = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == monthInQuarter && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == monthInQuarter && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                        var requestQ = newprjQ + replaceprjQ;

//                        var candidateCVScanQ = cands.Where(s => s.Round1_Date!=null);
//                        var candidateFirstRoundQ = cands.Where(s => s.Round2_Date!=null);
//                        var candidateSecondRoundQ = cands.Where(s => s.Round3_Date!=null);


//                        var regionConversionQ = new RegionConversion();
//                        regionConversionQ.RegionName = db.Regions.FirstOrDefault(s => s.Id == item.Key).RegionName;
//                        regionConversionQ.NumberOfActual = cands.Count;
//                        regionConversionQ.NumberOfRequest = requestQ;
//                        regionConversionQ.NumberOfRequired = requestQ * 10;
//                        regionConversionQ.UnsedLeadOfMonth = 0;//chưa tính
//                        regionConversionQ.NumberOfNotRecruit = 0;//chưa tính

//                        var cvScanQ = new CVScan();
//                        cvScanQ.Fail = candidateCVScanQ.Where(s => s.Result_R1== Result.FAILED).Count();
//                        cvScanQ.Pass = candidateCVScanQ.Where(s => s.Result_R1== Result.PASSED).Count();
//                        cvScanQ.Pending = candidateCVScanQ.Where(s => s.Result_R1== Result.WAIT).Count();
//                        cvScanQ.Total = cvScanQ.Fail + cvScanQ.Pass + cvScanQ.Pending;

//                        var fRoundQ = new FirstRound();
//                        fRoundQ.Fail = candidateFirstRoundQ.Where(s => s.Result_R2== Result.FAILED).Count();
//                        fRoundQ.Pass = candidateFirstRoundQ.Where(s => s.Result_R2== Result.PASSED).Count();
//                        fRoundQ.Pending = candidateFirstRoundQ.Where(s => s.Result_R2== Result.WAIT).Count();
//                        fRoundQ.Total = fRoundQ.Fail + fRoundQ.Pass + fRoundQ.Pending;

//                        var sRoundQ = new SecondRound();
//                        sRoundQ.Fail = candidateSecondRoundQ.Where(s => s.Result_R3== Result.FAILED).Count();
//                        sRoundQ.Pass = candidateSecondRoundQ.Where(s => s.Result_R3== Result.PASSED).Count();
//                        sRoundQ.Pending = candidateSecondRoundQ.Where(s => s.Result_R3== Result.WAIT).Count();
//                        sRoundQ.Total = sRoundQ.Fail + sRoundQ.Pass + sRoundQ.Pending;

//                        var conversionQ = new Conversion();
//                        if (sRoundQ.Pass > 0)
//                        {
//                            conversionQ.HR = Math.Round(((double)cvScanQ.Pass / sRoundQ.Pass), 1);
//                            conversionQ.Sourcing = Math.Round(((double)regionConversionQ.NumberOfActual / sRoundQ.Pass), 1);
//                        }

//                        var regionConversionDetail = new RegionConversion
//                        {
//                            NumberOfActual = regionConversionQ.NumberOfActual,//
//                            NumberOfNotRecruit = regionConversionQ.NumberOfNotRecruit,//
//                            NumberOfRequest = regionConversionQ.NumberOfRequest,//
//                            NumberOfRequired = regionConversionQ.NumberOfRequired,//
//                            RegionName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthInQuarter),
//                            UnsedLeadOfMonth = regionConversionQ.UnsedLeadOfMonth//
//                        };

//                        var regionConversionDetailUpto = new RegionConversion
//                        {
//                            NumberOfActual = regionConversionQ.NumberOfActual,
//                            NumberOfNotRecruit = regionConversionQ.NumberOfNotRecruit,
//                            NumberOfRequest = regionConversionQ.NumberOfRequest,
//                            NumberOfRequired = regionConversionQ.NumberOfRequired,
//                            RegionName = "Up to " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthInQuarter),
//                            UnsedLeadOfMonth = regionConversionQ.UnsedLeadOfMonth
//                        };
//                        var tupleDetail = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversionDetail, cvScanQ, fRoundQ, sRoundQ, conversionQ);
//                        var tupleDetailUpto = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversionDetailUpto, cvScanQ, fRoundQ, sRoundQ, conversionQ);
//                        regionTuples.Add(tupleDetail);
//                        regionTuples.Add(tupleDetailUpto);
//                    }
//                }
//                else
//                {
//                    for (int i = 1; i <= 12; i++)
//                    {
//                        var cands = candidates.Where(s => s.SubmissionDate?.Month == i).ToList();
//                        var newprjQ = prj.ProjectNewDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == i && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == i && p.EndingDate?.Year == yr)).ToList().Sum(s => s.NumberOfRequested);
//                        var replaceprjQ = prj.ProjectReplaceDetail.Where(p => p.Store.CityRegion.Region.Id == item.Key && (p.StartingDate?.Month == i && p.StartingDate?.Year == yr) || (p.EndingDate?.Month == i && p.EndingDate?.Year == yr)).Sum(s => s.NumberOfRequested);
//                        var requestQ = newprjQ + replaceprjQ;

//                        var candidateCVScanQ = cands.Where(s => s.Round1_Date!=null);
//                        var candidateFirstRoundQ = cands.Where(s => s.Round2_Date!=null);
//                        var candidateSecondRoundQ = cands.Where(s => s.Round3_Date!=null);


//                        var regionConversionQ = new RegionConversion();
//                        regionConversionQ.RegionName = db.Regions.FirstOrDefault(s => s.Id == item.Key).RegionName;
//                        regionConversionQ.NumberOfActual = cands.Count;
//                        regionConversionQ.NumberOfRequest = requestQ;
//                        regionConversionQ.NumberOfRequired = requestQ * 10;
//                        regionConversionQ.UnsedLeadOfMonth = 0;//chưa tính
//                        regionConversionQ.NumberOfNotRecruit = 0;//chưa tính

//                        var cvScanQ = new CVScan();
//                        cvScanQ.Fail = candidateCVScanQ.Where(s => s.Result_R1== Result.FAILED).Count();
//                        cvScanQ.Pass = candidateCVScanQ.Where(s => s.Result_R1== Result.PASSED).Count();
//                        cvScanQ.Pending = candidateCVScanQ.Where(s => s.Result_R1== Result.WAIT).Count();
//                        cvScanQ.Total = cvScanQ.Fail + cvScanQ.Pass + cvScanQ.Pending;

//                        var fRoundQ = new FirstRound();
//                        fRoundQ.Fail = candidateFirstRoundQ.Where(s => s.Result_R2== Result.FAILED).Count();
//                        fRoundQ.Pass = candidateFirstRoundQ.Where(s => s.Result_R2== Result.PASSED).Count();
//                        fRoundQ.Pending = candidateFirstRoundQ.Where(s => s.Result_R2== Result.WAIT).Count();
//                        fRoundQ.Total = fRoundQ.Fail + fRoundQ.Pass + fRoundQ.Pending;

//                        var sRoundQ = new SecondRound();
//                        sRoundQ.Fail = candidateSecondRoundQ.Where(s => s.Result_R3== Result.FAILED).Count();
//                        sRoundQ.Pass = candidateSecondRoundQ.Where(s => s.Result_R3== Result.PASSED).Count();
//                        sRoundQ.Pending = candidateSecondRoundQ.Where(s => s.Result_R3== Result.WAIT).Count();
//                        sRoundQ.Total = sRoundQ.Fail + sRoundQ.Pass + sRoundQ.Pending;

//                        var conversionQ = new Conversion();
//                        if (sRoundQ.Pass > 0)
//                        {
//                            conversionQ.HR = Math.Round(((double)cvScanQ.Pass / sRoundQ.Pass), 1);
//                            conversionQ.Sourcing = Math.Round(((double)regionConversionQ.NumberOfActual / sRoundQ.Pass), 1);
//                        }

//                        var regionConversionDetail = new RegionConversion
//                        {
//                            NumberOfActual = regionConversionQ.NumberOfActual,//
//                            NumberOfNotRecruit = regionConversionQ.NumberOfNotRecruit,//
//                            NumberOfRequest = regionConversionQ.NumberOfRequest,//
//                            NumberOfRequired = regionConversionQ.NumberOfRequired,//
//                            RegionName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
//                            UnsedLeadOfMonth = regionConversionQ.UnsedLeadOfMonth//
//                        };

//                        var regionConversionDetailUpto = new RegionConversion
//                        {
//                            NumberOfActual = regionConversionQ.NumberOfActual,
//                            NumberOfNotRecruit = regionConversionQ.NumberOfNotRecruit,
//                            NumberOfRequest = regionConversionQ.NumberOfRequest,
//                            NumberOfRequired = regionConversionQ.NumberOfRequired,
//                            RegionName = "Up to " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
//                            UnsedLeadOfMonth = regionConversionQ.UnsedLeadOfMonth
//                        };
//                        var tupleDetail = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversionDetail, cvScanQ, fRoundQ, sRoundQ, conversionQ);
//                        var tupleDetailUpto = new Tuple<RegionConversion, CVScan, FirstRound, SecondRound, Conversion>(regionConversionDetailUpto, cvScanQ, fRoundQ, sRoundQ, conversionQ);
//                        regionTuples.Add(tupleDetail);
//                        regionTuples.Add(tupleDetailUpto);
//                    }
//                }
//                #endregion


//                result.Add(regionTuples);
//            }
//            return result;
//        }

        
//    }
//}