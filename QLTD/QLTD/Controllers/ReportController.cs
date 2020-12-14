using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Data;
using Ehr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class ReportController : BaseController
    {
        private readonly UnitWork unitWork;
        public ReportController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [PermissionBasedAuthorize("DISK_MNT")]
        public ActionResult All(string sortProperty, string sortOrder, string searchString, int? size, int? page)
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

            QLTDDBContext db = new QLTDDBContext();
            //Lấy dataset rỗng
            var lsReport = new List<ReportViewModel>();

            var lscus = unitWork.Customer.Get();
            foreach (var item in lscus)
            {
                var rp = new ReportViewModel();
                rp.Code = item.Code;
                rp.Name = item.Name;
                rp.PhoneNumber = item.PhoneNumber;
                rp.Address = item.Address;
                var getall = unitWork.RentDetail.Get(X => X.Disk.Status == DiskStatus.RENTING && X.Rent.Customer.Id == item.Id);
                var numlate = unitWork.RentDetail.Get(X => X.Disk.Status == DiskStatus.RENTING && X.Rent.Customer.Id == item.Id && X.Rent.ReceiptDate.Date < DateTime.Now).Count();
                var disk = getall.Select(x => x.Disk.DiskTitle.Name).Distinct().OrderBy(c => c).ToList();
                foreach (var item1 in disk)
                {
                    rp.Disk += " - " + item1;
                }
                lsReport.Add(rp);
            }

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (lsReport != null)
                datasetSize = lsReport.Count();
            int checkTotal = (int)(datasetSize / pageSize) + 1;
            // Nếu trang yêu cầu vượt qua tổng số trang thì thiết lập là tổng số trang
            if (page > checkTotal) page = checkTotal;
            string _url = "searchString=" + searchString + "&sortProperty=" + sortProperty + "&sortOrder=" + sortOrder;
            EZPaging ezpage = new EZPaging((page ?? 1), pageSize, datasetSize, _url);
            #endregion

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
            if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
            {
                return View(lsReport.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
            }
            else
            {
                return View(lsReport);
            }
        }

    }
}