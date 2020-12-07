using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.Tools;
using Ehr.Common.UI;
using Ehr.Models;

namespace Ehr.Controllers
{
    public class ResourceController : BaseController
    {
        private readonly UnitWork unitWork;

        public ResourceController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        // GET: Resource
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EZDOC(string appx)
        {
            string filename = appx;
            string filepath = EZConfig.UploadPath + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
        public byte[] ImageToByteArray(string path)
        {
            try
            {
                return System.IO.File.ReadAllBytes(path);
            }
            catch
            {
                return System.IO.File.ReadAllBytes(Server.MapPath("~/Content/img/profile.png"));
            }
        }
        public ActionResult EZIMG(string appx)
        {
            string filename = appx;
            string filepath = EZConfig.UploadPath + filename;
            return this.File(ImageToByteArray(filepath), "image/png", "image.png");
        }
        [AllowAnonymous]
        public ActionResult EZIMGA(string appx)
        {
            string filename = appx;
            string filepath = EZConfig.UploadPath + filename;
            return this.File(ImageToByteArray(filepath), "image/png", "image.png");
        }

        public ActionResult EX_DOWNLOAD_PRODUCTINFOR(string _rangedate, string _account, string _province)
        {
            ETConverter etc = new ETConverter();

            int account = int.Parse(_account);
            int province = int.Parse(_province);
            var listProductInfor = new List<ProductInfor>();


            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now;
            if (_rangedate != null && _rangedate != "undefined")
            {
                string daterange = _rangedate;
                try
                {
                    string temp = daterange.Replace(" ", "");
                    string[] sp = temp.Split('-');
                    if (sp.Length > 1)
                    {
                        startDate = DataConverter.UI2DateTimeRange(sp[0], true);
                        endDate = DataConverter.UI2DateTimeRange(sp[1], false);
                    }
                }
                catch { }
            }

            var productinfor = unitWork.ProductInfor.Get();

            if (_rangedate.Length > 0)
            {
                if (startDate != DateTime.MinValue)
                    productinfor = productinfor.Where(c => c.CollectedDate >= startDate);
                if (endDate != DateTime.MinValue)
                    productinfor = productinfor.Where(c => c.CollectedDate <= endDate);
            }

            if (province > 0)
            {
                var userprovince = unitWork.User.Get(x => x.Province == province).Select(x => x.Id);
                if (account > 0)
                {
                    productinfor = productinfor.Where(c => userprovince.Any(a => a == account));
                }
            }

            if (account > 0)
            {
                productinfor = productinfor.Where(c => c.UserId == account);
            }
            listProductInfor = productinfor.ToList();

            DataTable dt = new DataTable();
            dt = etc.ConvertPro(listProductInfor);
            string fileName = "ProductInfor.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "ProductInfor");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        //     [HttpGet]
        //     [PermissionBasedAuthorize("FORM_SCR")]
        //     /// <summary>
        //     /// Trả về master file Excel theo project
        //     /// </summary>
        //     /// <param name="pid">Project ID</param>
        //     /// <returns>Trả về master file Excel theo project</returns>
        //     public ActionResult EX_DOWNLOAD_FORM(string form_id)
        //     {
        //         int id = int.Parse(form_id);
        //         var candidates = unitWork.Candidate.Get(c => c.Form.Id == id);

        //         DataTable dt = new DataTable();
        //         ETConverter etc = new ETConverter();
        //         dt = etc.ConvertCand(candidates.ToList());
        //         string fileName = "Candidates_Byform_" + form_id + ".xlsx";
        //         using (XLWorkbook wb = new XLWorkbook())
        //         {
        //             wb.Worksheets.Add(dt, "Candidates");
        //             using (MemoryStream stream = new MemoryStream())
        //             {
        //                 wb.SaveAs(stream);
        //                 return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //             }
        //         }
        //     }

        //     [HttpGet]
        //     [PermissionBasedAuthorize("FORM_SCR")]
        //     /// <summary>
        //     /// Trả về master file Excel theo project
        //     /// </summary>
        //     /// <param name="pid">Project ID</param>
        //     /// <returns>Trả về master file Excel theo project</returns>
        //     public ActionResult EX_DOWNLOAD_SCREENING(string proj, string appVac, string appStatus, string _region, string _city, string _status, string rangeDate)
        //     {
        //         ETConverter etc = new ETConverter();
        //         List<Candidate> finalList = new List<Candidate>();
        //         ProfileFiltering pf = new ProfileFiltering();
        //         int approvalStatus = int.Parse(appStatus);
        //         int vacancy = int.Parse(appVac);
        //         int project = int.Parse(proj);

        //         int region = 0;
        //         if (_region != null)
        //         {
        //             region = int.Parse(_region);
        //         }
        //         int city = 0;
        //         if (_city != null)
        //         {
        //             city = int.Parse(_city);
        //         }

        //         int status = 0;
        //         if (_status != null)
        //         {
        //             status = int.Parse(_status);
        //         }

        //         string daterange = "";
        //         DateTime from = DateTime.MinValue;
        //         DateTime to = DateTime.MinValue;
        //         if (rangeDate != null)
        //         {
        //             daterange = (rangeDate);
        //             try
        //             {
        //                 string temp = daterange.Replace(" ", "");
        //                 string[] sp = temp.Split('-');
        //                 if (sp.Length > 1)
        //                 {
        //                     from = DataConverter.UI2DateTimeRange(sp[0], true);
        //                     to = DataConverter.UI2DateTimeRange(sp[1], false);
        //                 }
        //             }
        //             catch { }
        //         }

        //         var candidates = unitWork.Candidate.Get();
        //         if (project > 0)
        //         {
        //             candidates = unitWork.Candidate.Get(c => c.Form.Project.Id.Equals(project));
        //         }
        //         else
        //         {
        //             candidates = unitWork.Candidate.Get(c => c.Form.Project.ProjectMembers.Any(p => p.Id == User.UserId));
        //         }
        //         if (appVac != null)
        //         {
        //             vacancy = int.Parse(appVac);
        //             if (vacancy != 0)
        //             {
        //                 //lọc candidates theo vacancy
        //                 candidates = candidates.Where(c => c.Position.Id.Equals(vacancy));
        //             }
        //         }
        //         //Lọc theo Region
        //         if (region > 0)
        //         {
        //             candidates = candidates.Where(c => c.CityRegion.Region.Id.Equals(region));
        //         }
        //         //Lọc theo city
        //         if (city > 0)
        //         {
        //             candidates = candidates.Where(c => c.CityRegion.City.Id.Equals(city));
        //         }
        //         if (status > 0)
        //         {
        //             candidates = candidates.Where(c => c.State.Equals((InterviewStatus)Enum.Parse(typeof(InterviewStatus), status.ToString(), true)));
        //         }
        //         if (daterange.Length > 0)
        //         {
        //             if (from != DateTime.MinValue)
        //                 candidates = candidates.Where(c => c.SubmissionDate >= from);
        //             if (to != DateTime.MinValue)
        //                 candidates = candidates.Where(c => c.SubmissionDate <= to);
        //         }
        //         if (approvalStatus != 2)
        //         {
        //             //lọc theo kiểu hồ sơ
        //             switch (approvalStatus)
        //             {
        //                 case 0://tất cả trừ hồ sơ xóa, đã duyệt hoặc bị loại
        //                     candidates = candidates.Where(c => c.Approval != ApprovalStatus.DELETED);
        //                     break;
        //                 case 1://tất cả hồ sơ không đạt
        //                     candidates = candidates.Where(c => c.Approval == ApprovalStatus.REJECTED);
        //                     break;
        //                 case 3://tất cả hồ sơ đã xóa
        //                     candidates = candidates.Where(c => c.Approval == ApprovalStatus.DELETED);
        //                     break;
        //                 case 5://đã duyệt
        //                     candidates = candidates.Where(c => c.Approval == ApprovalStatus.OK);
        //                     break;
        //                 case 6://chưa tuyển dụng
        //                     candidates = candidates.Where(c => c.Approval == ApprovalStatus.WAIT);
        //                     break;
        //                 case 4://chưa duyệt bao gồm trùng và spam
        //                        //candidates = candidates.Where ( c => c.Approval == ApprovalStatus.OK && c.Approval != ApprovalStatus.DELETED );
        //                     candidates = candidates.Where(c => c.Approval == ApprovalStatus.PENDING);
        //                     break;
        //             }
        //             finalList = candidates.ToList();
        //         }
        //         else
        //         {
        //             var duplicates = candidates.Where(c => c.Approval != ApprovalStatus.DELETED && c.Approval != ApprovalStatus.REJECTED && c.Approval != ApprovalStatus.OK)
        //                         .GroupBy(c => new { c.IP, c.FullName, c.PhoneNumber })
        //                         .Select(g => new { Qty = g.Count(), First = g.OrderBy(c => c.Id).ToList() })
        //                         .Where(t => t.Qty > 1)
        //                         .Select(p => new
        //                         {
        //                             id = p.First
        //                         }).ToList();

        //             List<List<Candidate>> list = new List<List<Candidate>>();
        //             foreach (var cad in duplicates)
        //             {
        //                 list.Add(cad.id);
        //             }
        //             finalList = pf.Cast2List(list);
        //         }

        //         DataTable dt = new DataTable();
        //         dt = etc.ConvertCand(finalList);
        //         string fileName = "Candidates.xlsx";
        //         using (XLWorkbook wb = new XLWorkbook())
        //         {
        //             wb.Worksheets.Add(dt, "Candidates");
        //             using (MemoryStream stream = new MemoryStream())
        //             {
        //                 wb.SaveAs(stream);
        //                 return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //             }
        //         }
        //     }

        //     [HttpGet]
        //     [PermissionBasedAuthorize("PROJECT_MNT")]
        //     public ActionResult EX_DOWNLOAD_NEW_PROJECTS(int projectid, int position, int region, int status)
        //     {
        //         ETConverter etc = new ETConverter();
        //         var projects = unitWork.EProject.Get();
        //         if (projectid > 0)
        //         {
        //             projects = projects.Where(s => s.Id == projectid);
        //         }
        //         if (status > -1)
        //         {
        //             projects = projects.Where(s => s.StatusActive == EZEnumConverter.ProjectStatus(status.ToString()));
        //         }
        //         /*if(position > 0)
        //{
        //	projects = projects.Where ( s => s.Vacancies.Contains
        //}*/
        //         DataTable dt = new DataTable();
        //         dt = etc.ConvertProject(projects.ToList());
        //         string fileName = "Projects.xlsx";
        //         using (XLWorkbook wb = new XLWorkbook())
        //         {
        //             wb.Worksheets.Add(dt, "Projects");
        //             using (MemoryStream stream = new MemoryStream())
        //             {
        //                 wb.SaveAs(stream);
        //                 return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //             }
        //         }
        //     }


    }
}