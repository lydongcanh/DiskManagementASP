using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class RentDiskController : BaseController
    {
        private readonly UnitWork unitWork;
        public RentDiskController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        // GET: RentDisk
        [PermissionBasedAuthorize("RENT_DISK")]
        public ActionResult Rent(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            var user = unitWork.User.GetById(this.User.UserId);
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Code";
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
            //Lấy tất cả thuộc tính của city ra
            var properties = typeof(Rent).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Code"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã phiếu thuê", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Customer"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Khách hàng", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("RentDate"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Ngày thuê", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("Status"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Trạng thái", Order = 4, AllowSorting = true });
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
            IQueryable<Rent> Rents = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Code"))
                    Rents = from c in db.Rents orderby c.Code descending select c;
                else if (sortProperty.Equals("Customer"))
                    Rents = from c in db.Rents orderby c.Customer.Name descending select c;
                else if (sortProperty.Equals("RentDate"))
                    Rents = from c in db.Rents orderby c.RentDate descending select c;
                else if (sortProperty.Equals("Status"))
                    Rents = from c in db.Rents orderby c.Status descending select c;
            }
            else
            {
                if (sortProperty.Equals("Code"))
                    Rents = from c in db.Rents orderby c.Code ascending select c;
                else if (sortProperty.Equals("Customer"))
                    Rents = from c in db.Rents orderby c.Customer.Name ascending select c;
                else if (sortProperty.Equals("RentDate"))
                    Rents = from c in db.Rents orderby c.RentDate ascending select c;
                else if (sortProperty.Equals("Status"))
                    Rents = from c in db.Rents orderby c.Status ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                Rents = Rents.Where(c => c.Code.ToString().ToLower().Contains(searchString.ToLower())
                || c.RentDate.ToString().ToLower().Contains(searchString.ToLower())
                || c.Customer.Name.ToString().ToLower().Contains(searchString.ToLower())
                );

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (Rents != null)
                datasetSize = Rents.Count();
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
            ViewBag.DiskTitles = unitWork.DiskTitle.Get().ToList();
            var enumstatus = Enum.GetValues(typeof(RentStatus))
                          .Cast<RentStatus>()
                          .ToList();
            ViewBag.ListStatus = from s in enumstatus
                                 select new SelectListItem
                                 {
                                     Value = ((int)s).ToString(),
                                     Text = s.GetAttribute<DisplayAttribute>().Name
                                 };

            //Hiển thị trong khoảng chọn
            if (Rents != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(Rents.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(Rents.ToList());
                }

            else
                return View((from c in db.Rents select c).ToList());
        }
    }
}