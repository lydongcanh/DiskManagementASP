using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Ehr.Controllers
{
    public class AuditTrailController : Controller
    {
        private readonly UnitWork unitWork;

        public AuditTrailController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        #region Auditrail
        [PermissionBasedAuthorize("HISTORY")]
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "DateTimeStamp";
            //Mặc định là từ A-Z
            string nextSort = "";
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "desc";
            if (sortOrder.Equals("asc") | sortOrder.Equals("none")) nextSort = "desc";
            if (sortOrder.Equals("desc")) nextSort = "asc";


            //Tạo danh sách cho hiển thị số dòng trên lưới
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "35", Value = "35" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }

            #region  xây dựng header cho lưới
            string header = "";
            //các cột cần hiển thị
            List<EZGridColumn> columns = new List<EZGridColumn>();
            //Lấy tất cả thuộc tính 
            var properties = typeof(AuditTrail).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("DateTimeStamp"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thời gian", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Username"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Người thực hiện", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("AuditActionType"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thao tác", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("KeyFieldID"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã bảng", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("DataModel"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Bảng", Order = 6, AllowSorting = true });
                }
               
            }
            //sắp xếp lại các cột theo thứ tự
            columns = columns.OrderBy(c => c.Order).ToList();

            foreach (var col in columns)
            {
                if (col.AllowSorting)
                {
                    string basedUrl = "page=" + page + "&size=" + size + "&searchString=" + searchString + "&sortProperty=" + col.Name + "&sortOrder=";
                    if (sortOrder.Equals("desc") && sortProperty.Equals(col.Name))
                    {
                        header += "<th class='sorting_desc'><a style='display: block;' href='?" + basedUrl +
                            nextSort + "'>" + col.Text + "</th></a></th>";
                    }
                    else if (sortOrder.Equals("asc") && sortProperty.Equals(col.Name))
                    {
                        header += "<th class='sorting_asc'><a style='display: block;' href='?" + basedUrl +
                            nextSort + "'>" + col.Text + "</a></th>";
                    }
                    else
                    {
                        header += "<th class='sorting'><a  style='display: block;' href='?" + basedUrl +
                           nextSort + "'>" + col.Text + "</a></th>";
                    }
                }
                else header += "<th>" + col.Text + "</th>";
            }
            #endregion



            #region Phần sắp xếp
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();

            //Lấy dataset rỗng
            IQueryable<AuditTrail> auditTrails = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("DateTimeStamp"))
                    auditTrails = from c in db.AuditTrails orderby c.DateTimeStamp descending select c;
                else if (sortProperty.Equals("Username"))
                    auditTrails = from c in db.AuditTrails orderby c.Username descending select c;
                else if (sortProperty.Equals("AuditActionType"))
                    auditTrails = from c in db.AuditTrails orderby c.AuditActionType descending select c;
                else if (sortProperty.Equals("KeyFieldID"))
                    auditTrails = from c in db.AuditTrails orderby c.KeyFieldID descending select c;
                else if (sortProperty.Equals("DataModel"))
                    auditTrails = from c in db.AuditTrails orderby c.DataModel descending select c;
            }
            else
            {
                if (sortProperty.Equals("DateTimeStamp"))
                    auditTrails = from c in db.AuditTrails orderby c.DateTimeStamp ascending select c;
                else if (sortProperty.Equals("Username"))
                    auditTrails = from c in db.AuditTrails orderby c.Username ascending select c;
                else if (sortProperty.Equals("AuditActionType"))
                    auditTrails = from c in db.AuditTrails orderby c.AuditActionType ascending select c;
                else if (sortProperty.Equals("KeyFieldID"))
                    auditTrails = from c in db.AuditTrails orderby c.KeyFieldID ascending select c;
                else if (sortProperty.Equals("DataModel"))
                    auditTrails = from c in db.AuditTrails orderby c.DataModel ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                auditTrails = auditTrails.Where(c => c.DateTimeStamp.ToString().Contains(searchString) 
                || c.Username.ToString().Contains(searchString) || c.KeyFieldID.ToString().Contains(searchString) 
                || c.DataModel.Contains(searchString) || c.AuditActionType.ToString().Contains(searchString) 
                || c.Id.ToString().Contains(searchString));
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (auditTrails != null)
                datasetSize = auditTrails.Count();
            int checkTotal = (int)(datasetSize / pageSize) + 1;
            // Nếu trang yêu cầu vượt qua tổng số trang thì thiết lập là tổng số trang
            if (page > checkTotal) page = checkTotal;
            string _url = "searchString=" + searchString + "&sortProperty=" + sortProperty + "&sortOrder=" + sortOrder;
            EZPaging ezpage = new EZPaging((page ?? 1), pageSize, datasetSize, _url);
            #endregion

            // Thiết lập các ViewBag
            ViewBag.GridHeader = header;// Header của lưới			
            ViewBag.searchValue = searchString;// Chuỗi tìm kiếm
            ViewBag.sortProperty = sortProperty;//Chọn cột sắp xếp
            ViewBag.sortOrder = nextSort;//Kiểu sắp xếp tiếp theo
            ViewBag.page = page;// Trang hiện tại
            ViewBag.size = items; //Danh sách cho chọn trang
            ViewBag.currentSize = size; //Số mục trên 1 trang được chọn
            ViewBag.pageSize = pageSize;//Số mục trên 1 trang để cache lại
            ViewBag.paging = ezpage.PageModel.HTML;//HTML cho đoạn phân trang
            ViewBag.fromItem = ezpage.PageModel.StartItem;
            //Hiển thị trong khoảng chọn
            if (auditTrails != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(auditTrails.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(auditTrails.ToList());
                }

            else
                return View((from c in db.AuditTrails select c).ToList());
        }

        [PermissionBasedAuthorize("HISTORY")]
        public JsonResult Detail(int Id)
        {
            AuditTrailBussiness AB = new AuditTrailBussiness(unitWork);

            var AuditTrail = AB.GetAudit(Id);

            return Json(AuditTrail, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Login AuditTrail
        [PermissionBasedAuthorize("HISTORY")]
        public ActionResult Login(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "DateTimeStamp";
            //Mặc định là từ A-Z
            string nextSort = "";
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "desc";
            if (sortOrder.Equals("asc") | sortOrder.Equals("none")) nextSort = "desc";
            if (sortOrder.Equals("desc")) nextSort = "asc";


            //Tạo danh sách cho hiển thị số dòng trên lưới
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "35", Value = "35" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }

            #region  xây dựng header cho lưới
            string header = "";
            //các cột cần hiển thị
            List<EZGridColumn> columns = new List<EZGridColumn>();
            //Lấy tất cả thuộc tính 
            var properties = typeof(LoginAuditTrail).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("DateTimeStamp"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thời gian", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Username"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Người thực hiện", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("Status"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Trạng thái", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("IpAddress"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Địa chỉ IP", Order = 5, AllowSorting = true });
                }

            }
            //sắp xếp lại các cột theo thứ tự
            columns = columns.OrderBy(c => c.Order).ToList();

            foreach (var col in columns)
            {
                if (col.AllowSorting)
                {
                    string basedUrl = "page=" + page + "&size=" + size + "&searchString=" + searchString + "&sortProperty=" + col.Name + "&sortOrder=";
                    if (sortOrder.Equals("desc") && sortProperty.Equals(col.Name))
                    {
                        header += "<th class='sorting_desc'><a style='display: block;' href='?" + basedUrl +
                            nextSort + "'>" + col.Text + "</th></a></th>";
                    }
                    else if (sortOrder.Equals("asc") && sortProperty.Equals(col.Name))
                    {
                        header += "<th class='sorting_asc'><a style='display: block;' href='?" + basedUrl +
                            nextSort + "'>" + col.Text + "</a></th>";
                    }
                    else
                    {
                        header += "<th class='sorting'><a  style='display: block;' href='?" + basedUrl +
                           nextSort + "'>" + col.Text + "</a></th>";
                    }
                }
                else header += "<th>" + col.Text + "</th>";
            }
            #endregion



            #region Phần sắp xếp
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();

            //Lấy dataset rỗng
            IQueryable<LoginAuditTrail> loginAuditTrails = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("DateTimeStamp"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.DateTimeStamp descending select c;
                else if (sortProperty.Equals("Username"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.Username descending select c;
                else if (sortProperty.Equals("Status"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.Status descending select c;
                else if (sortProperty.Equals("IpAddress"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.IpAddress descending select c;
            }
            else
            {
                if (sortProperty.Equals("DateTimeStamp"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.DateTimeStamp ascending select c;
                else if (sortProperty.Equals("Username"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.Username ascending select c;
                else if (sortProperty.Equals("Status"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.Status ascending select c;
                else if (sortProperty.Equals("IpAddress"))
                    loginAuditTrails = from c in db.LoginAuditTrails orderby c.IpAddress ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                loginAuditTrails = loginAuditTrails.Where(c => c.DateTimeStamp.ToString().Contains(searchString)
                || c.Username.ToString().Contains(searchString) || c.Status.ToString().Contains(searchString)
                || c.IpAddress.Contains(searchString));
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (loginAuditTrails != null)
                datasetSize = loginAuditTrails.Count();
            int checkTotal = (int)(datasetSize / pageSize) + 1;
            // Nếu trang yêu cầu vượt qua tổng số trang thì thiết lập là tổng số trang
            if (page > checkTotal) page = checkTotal;
            string _url = "searchString=" + searchString + "&sortProperty=" + sortProperty + "&sortOrder=" + sortOrder;
            EZPaging ezpage = new EZPaging((page ?? 1), pageSize, datasetSize, _url);
            #endregion

            // Thiết lập các ViewBag
            ViewBag.GridHeader = header;// Header của lưới			
            ViewBag.searchValue = searchString;// Chuỗi tìm kiếm
            ViewBag.sortProperty = sortProperty;//Chọn cột sắp xếp
            ViewBag.sortOrder = nextSort;//Kiểu sắp xếp tiếp theo
            ViewBag.page = page;// Trang hiện tại
            ViewBag.size = items; //Danh sách cho chọn trang
            ViewBag.currentSize = size; //Số mục trên 1 trang được chọn
            ViewBag.pageSize = pageSize;//Số mục trên 1 trang để cache lại
            ViewBag.paging = ezpage.PageModel.HTML;//HTML cho đoạn phân trang
            ViewBag.fromItem = ezpage.PageModel.StartItem;
            
            //Hiển thị trong khoảng chọn
            if (loginAuditTrails != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(loginAuditTrails.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(loginAuditTrails.ToList());
                }

            else
                return View((from c in db.LoginAuditTrails select c).ToList());
        }
        [PermissionBasedAuthorize("HISTORY")]
        public JsonResult LoginDetail(int? id)
        {
            if (id == null)
                return Json("", JsonRequestBehavior.AllowGet);
            var loginaudittrail = unitWork.LoginAuditTrail.GetById(id.Value);
            var status = "";
            if (loginaudittrail.Status == YesNo.YES)
            {
                status = "Thành công";
            }
            else
            {
                status = "Thất bại";
            }
            var result = new
            {
                Id = loginaudittrail.Id,
                DateTimeStamp = loginaudittrail.DateTimeStamp,
                Username = loginaudittrail.Username,
                IpAddress = loginaudittrail.IpAddress,
                Status = status,
                Note = loginaudittrail.Note,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}