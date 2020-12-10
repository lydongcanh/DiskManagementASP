using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Data;
using Ehr.Models;
using Ehr.ViewModels;
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
            Ehr.Data.QLTDDBContext db = new Data.QLTDDBContext();

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
            ViewBag.Customer = unitWork.Customer.Get();
            ViewBag.DiskTitles = unitWork.DiskTitle.Get();
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


        [PermissionBasedAuthorize("RENT_DISK")]
        public JsonResult GetRent(int? Id)
        {
            try
            {
                if(Id != null)
                {
                    var Rent = unitWork.Rent.GetById(Id);
                    var rentview = new RentViewModel();
                    rentview.Id = Rent.Id;
                    rentview.Code = Rent.Code;
                    rentview.RentLenght = Rent.RentLenght;
                    rentview.RentDate = DataConverter.DateTime2UI_Full(Rent.RentDate);
                    rentview.CustomerId = Rent.Customer.Id;
                    rentview.CustomerName = Rent.Customer.Name;

                    var lsrentviewdetail = new List<RentDetailViewModel>();
                    foreach (var item in Rent.RentDetails)
                    {
                        var rentviewdetail = new RentDetailViewModel();
                        rentviewdetail.Id = item.Id;
                        rentviewdetail.DiskId = item.Disk.Id;
                        rentviewdetail.DiskCode = item.Disk.Code;
                        rentviewdetail.DiskName = item.Disk.DiskTitle.Name;
                        rentviewdetail.TypeName = item.Disk.DiskTitle.DiskType.Name;
                        rentviewdetail.Prices = item.Disk.DiskTitle.Price;
                        lsrentviewdetail.Add(rentviewdetail);
                    }

                    if (Rent != null)
                    {
                        return Json(new { success = true ,Data = rentview, RentDetails = lsrentviewdetail } , JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Không tìm thấy phiếu thuê" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Không tìm thấy phiếu thuê" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult LoadDiskByTitle(int? Id, List<RentDetailViewModel> rents)
        {
            QLTDDBContext db = new QLTDDBContext();
            var lsrentid = new List<int>();
            if(rents!= null)
            {
                lsrentid = rents.Select(x => x.DiskId).ToList();
            }
            if (Id == null)
            {
                var json = new
                {
                    id = "",
                    text = ""
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }

            var disk = unitWork.Disk.Get(x => x.DiskTitle.Id == Id).Where(x => !lsrentid.Contains(x.Id) && x.Status == DiskStatus.WAITING).Select(
            c => new
            {
                id = c.Id,
                text = c.Code
            }
            ).Distinct().OrderBy(c => c.text).ToList();
            return Json(disk, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDisk(int? Id)
        {
            try
            {
                var item = unitWork.Disk.GetById(Id);
                var rentviewdetail = new RentDetailViewModel();
                rentviewdetail.DiskId = item.Id;
                rentviewdetail.DiskCode = item.Code;
                rentviewdetail.DiskName = item.DiskTitle.Name;
                rentviewdetail.TypeName = item.DiskTitle.DiskType.Name;
                rentviewdetail.Prices = item.DiskTitle.Price;
                return Json(new { Data = rentviewdetail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult AddRent(Rent rent,int? customerId, List<RentDetailViewModel> rents)
        {
            try
            {
                if(rent == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu thuê !" }, JsonRequestBehavior.AllowGet);
                }
                if(customerId == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy khách hàng !" }, JsonRequestBehavior.AllowGet);
                }
                if(rents == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đĩa thuê !" }, JsonRequestBehavior.AllowGet);
                }
                var customer = unitWork.Customer.GetById(customerId);
                if (rent.Id > 0)
                {
                    var oldrent = unitWork.Rent.GetById(rent.Id);
                    oldrent.Code = rent.Code;
                    oldrent.RentLenght = rent.RentLenght;
                    oldrent.RentDate = rent.RentDate;
                    oldrent.Customer = customer;
                    unitWork.Rent.Update(oldrent);
                    unitWork.Commit();
                }
                else
                {
                    rent.Customer = customer;
                    unitWork.Rent.Insert(rent);
                    unitWork.Commit();
                    foreach (var item in rents)
                    {
                        var rentdetail = new RentDetail();
                        var disk = unitWork.Disk.GetById(item.DiskId);
                        rentdetail.Disk = disk;
                        rentdetail.Rent = rent;
                        unitWork.RentDetail.Insert(rentdetail);
                        unitWork.Commit();
                        disk.Status = DiskStatus.RENTING;
                        unitWork.Disk.Update(disk);
                        unitWork.Commit();
                    }
                }
                return Json(new { success = true, message = "Lưu phiếu thuê thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}