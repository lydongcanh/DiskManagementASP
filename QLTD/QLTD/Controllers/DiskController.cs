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
    public class DiskController : BaseController
    {
        private readonly UnitWork unitWork;
        public DiskController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        #region Disk
        [PermissionBasedAuthorize("DISK_MNT")]
        public ActionResult Disk(string sortProperty, string sortOrder, string searchString, int? size, int? page)
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
            var properties = typeof(Disk).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Code"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã đĩa", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("DiskTitle"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên đĩa", Order = 3, AllowSorting = true });
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
            IQueryable<Disk> Disks = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Code"))
                    Disks = from c in db.Disks where c.Status != DiskStatus.DELETED orderby c.Code descending select c;
                else if (sortProperty.Equals("DiskTitle"))
                    Disks = from c in db.Disks where c.Status != DiskStatus.DELETED orderby c.DiskTitle.Name descending select c;
                else if (sortProperty.Equals("Status"))
                    Disks = from c in db.Disks where c.Status != DiskStatus.DELETED orderby c.Status descending select c;
            }
            else
            {
                if (sortProperty.Equals("Code"))
                    Disks = from c in db.Disks where c.Status != DiskStatus.DELETED orderby c.Code ascending select c;
                else if (sortProperty.Equals("DiskTitle"))
                    Disks = from c in db.Disks where c.Status != DiskStatus.DELETED orderby c.DiskTitle.Name ascending select c;
                else if (sortProperty.Equals("Status"))
                    Disks = from c in db.Disks where c.Status != DiskStatus.DELETED orderby c.Status ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                Disks = Disks.Where(c => c.Code.ToString().ToLower().Contains(searchString.ToLower()) || c.DiskTitle.Name.ToString().ToLower().Contains(searchString.ToLower()) );

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (Disks != null)
                datasetSize = Disks.Count();
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
            var enumstatus = Enum.GetValues(typeof(DiskStatus))
                          .Cast<DiskStatus>()
                          .ToList();
            ViewBag.ListStatus = from s in enumstatus
                                 where s != DiskStatus.DELETED
                                 select new SelectListItem
                               {
                                   Value = ((int)s).ToString(),
                                   Text = s.GetAttribute<DisplayAttribute>().Name
                               };

            //Hiển thị trong khoảng chọn
            if (Disks != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(Disks.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(Disks.ToList());
                }

            else
                return View((from c in db.Disks select c).ToList());
        }

        [PermissionBasedAuthorize("DISK_MNT")]
        public JsonResult AddDisk(int? Id, int IdTitle, DiskStatus status)
        {
            try
            {
                var Title = unitWork.DiskTitle.GetById(IdTitle);
                if (Title == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tựa đề !" }, JsonRequestBehavior.AllowGet);
                }
                if (Id == null)
                {
                    var lastdisk = unitWork.Disk.Get().LastOrDefault();
                    var Code = lastdisk.Id + 1;
                    var disk = new Disk();
                    disk.Code = "D-00" + Code.ToString(); ;
                    disk.DiskTitle = Title;
                    disk.Status = status;
                    unitWork.Disk.Insert(disk);
                }
                else
                {
                    var oldisk = unitWork.Disk.GetById(Id);
                    if (oldisk != null)
                    {
                        oldisk.DiskTitle = Title;
                        oldisk.Status = status;
                        unitWork.Disk.Update(oldisk);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy đĩa" }, JsonRequestBehavior.AllowGet);
                    }
                }
                unitWork.Commit();
                return Json(new { success = true, message = "Lưu dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteDisk(int? Id)
        {
            try
            {
                var olddisk = unitWork.Disk.GetById(Id);
                if (olddisk != null)
                {
                    olddisk.Status = DiskStatus.DELETED;
                    unitWork.Disk.Update(olddisk);
                    unitWork.Commit();
                }
                return Json(new { success = true, message = "Xoá đĩa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }
        }



        public bool CheckExist(string Code)
        {
            var check = unitWork.Disk.Get(x => x.Code.ToLower() == Code.ToLower()).FirstOrDefault();
            if (check != null)
                return false;
            return true;
        }
        #endregion


        #region DiskTitle
        [PermissionBasedAuthorize("DISK_MNT")]
        public ActionResult DiskTitle(string sortProperty, string sortOrder, string searchString, int? size, int? page)
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
            var properties = typeof(DiskTitle).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Image"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Hình ảnh", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Code"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã tiêu đề", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("Name"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên tiêu đề", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("DiskType"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Loại", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("Price"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Giá", Order = 6, AllowSorting = true });
                }
                if (item.Name.Equals("LateCharge"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Phí trễ", Order = 7, AllowSorting = true });
                }
                if (item.Name.Equals("Status"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Trạng thái", Order = 8, AllowSorting = true });
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
            IQueryable<DiskTitle> DiskTitles = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Code"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.Code descending select c;
                else if (sortProperty.Equals("Name"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.Name descending select c;
                else if (sortProperty.Equals("DiskType"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.DiskType.Name descending select c;
                else if (sortProperty.Equals("Price"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.Price descending select c;
                else if (sortProperty.Equals("LateCharge"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.LateCharge descending select c;
            }
            else
            {
                if (sortProperty.Equals("Code"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.Code ascending select c;
                else if (sortProperty.Equals("Name"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.Name ascending select c;
                else if (sortProperty.Equals("DiskType"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.DiskType.Name ascending select c;
                else if (sortProperty.Equals("Price"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.Price ascending select c;
                else if (sortProperty.Equals("LateCharge"))
                    DiskTitles = from c in db.DiskTitles where c.Status != TitleStatus.DELETED orderby c.LateCharge ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                DiskTitles = DiskTitles.Where(c => c.Code.ToString().ToLower().Contains(searchString.ToLower()) ||
                c.Name.ToString().ToLower().Contains(searchString.ToLower()) || 
                c.DiskType.Name.ToString().ToLower().Contains(searchString.ToLower()) || 
                c.Price.ToString().ToLower().Contains(searchString.ToLower()) || 
                c.LateCharge.ToString().ToLower().Contains(searchString.ToLower()
                ));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (DiskTitles != null)
                datasetSize = DiskTitles.Count();
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

            ViewBag.DiskTypes = unitWork.DiskType.Get().ToList();
            var enumstatus = Enum.GetValues(typeof(TitleStatus))
                          .Cast<TitleStatus>()
                          .ToList();
            ViewBag.ListStatus = from s in enumstatus
                                 where s != TitleStatus.DELETED
                                 select new SelectListItem
                                 {
                                     Value = ((int)s).ToString(),
                                     Text = s.GetAttribute<DisplayAttribute>().Name
                                 };

            //Hiển thị trong khoảng chọn
            if (DiskTitles != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(DiskTitles.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(DiskTitles.ToList());
                }

            else
                return View((from c in db.Disks select c).ToList());
        }

        [PermissionBasedAuthorize("DISK_MNT")]
        public JsonResult AddDiskTitle(DiskTitle diskTitle)
        {
            try
            {
                var diskid = int.Parse(Request.Form.Get("DiskType"));
                var Type = unitWork.DiskType.GetById(diskid);
                if (Type == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy loại !" }, JsonRequestBehavior.AllowGet);
                }
                if (diskTitle.Id > 0)
                {
                    var oldisktitle = unitWork.DiskTitle.GetById(diskTitle.Id);
                    if (oldisktitle != null)
                    {
                        oldisktitle.Name = diskTitle.Name;
                        oldisktitle.Price = diskTitle.Price;
                        oldisktitle.LateCharge = diskTitle.LateCharge;
                        oldisktitle.Description = diskTitle.Description;
                        oldisktitle.Status = diskTitle.Status;
                        oldisktitle.DiskType = Type;
                        if (Request.Form.Get("Image_img").ToString().Length > 0)
                        {
                            oldisktitle.Image = Request.Form.Get("Image_img");
                        }

                        unitWork.DiskTitle.Update(oldisktitle);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy tiêu đề" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var lastdisk = unitWork.DiskTitle.Get().LastOrDefault();
                    var Code = lastdisk.Id + 1;
                    var disktitle = new DiskTitle();
                    disktitle.Name = diskTitle.Name;
                    disktitle.Price = diskTitle.Price;
                    disktitle.LateCharge = diskTitle.LateCharge;
                    disktitle.Description = diskTitle.Description;
                    disktitle.Status = diskTitle.Status;
                    disktitle.DiskType = Type;
                    disktitle.Code = "TD-00" + Code.ToString();
                    if (Request.Form.Get("Image_img").ToString().Length > 0)
                    {
                        disktitle.Image = Request.Form.Get("Image_img");
                    }
                    unitWork.DiskTitle.Insert(disktitle);
                }
                unitWork.Commit();
                return Json(new { success = true, message = "Lưu dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult DeleteDiskTitle(int? Id)
        {
            try
            {
                var olddisktitle = unitWork.DiskTitle.GetById(Id);
                if (olddisktitle != null)
                {
                    olddisktitle.Status = TitleStatus.DELETED;
                    unitWork.DiskTitle.Update(olddisktitle);
                    unitWork.Commit();
                }
                return Json(new { success = true, message = "Xoá tiêu đề thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool CheckExistTitle(string Code)
        {
            var check = unitWork.DiskTitle.Get(x => x.Code.ToLower() == Code.ToLower()).FirstOrDefault();
            if (check != null)
                return false;
            return true;
        }
        #endregion

        #region DiskType
        [PermissionBasedAuthorize("DISK_MNT")]
        public ActionResult DiskType(string sortProperty, string sortOrder, string searchString, int? size, int? page)
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
            var properties = typeof(DiskType).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Code"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã loại", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Name"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên loại", Order = 3, AllowSorting = true });
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
            IQueryable<DiskType> DiskTypes = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Code"))
                    DiskTypes = from c in db.DiskTypes orderby c.Code descending select c;
                else if (sortProperty.Equals("Name"))
                    DiskTypes = from c in db.DiskTypes orderby c.Name descending select c;
            }
            else
            {
                if (sortProperty.Equals("Code"))
                    DiskTypes = from c in db.DiskTypes orderby c.Code ascending select c;
                else if (sortProperty.Equals("Name"))
                    DiskTypes = from c in db.DiskTypes orderby c.Name ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                DiskTypes = DiskTypes.Where(c => c.Code.ToString().ToLower().Contains(searchString.ToLower()) ||
                c.Name.ToString().ToLower().Contains(searchString.ToLower()
                ));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (DiskTypes != null)
                datasetSize = DiskTypes.Count();
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
            if (DiskTypes != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(DiskTypes.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(DiskTypes.ToList());
                }

            else
                return View((from c in db.DiskTypes select c).ToList());
        }

        [PermissionBasedAuthorize("DISK_MNT")]
        public JsonResult AddDiskType(int? Id, string Name)
        {
            try
            {
                if (Id == null)
                {
                    var lastdisk = unitWork.DiskType.Get().LastOrDefault();
                    var Code = lastdisk.Id + 1;
                    var disktype = new DiskType();
                    disktype.Code = "L-00" + Code.ToString();
                    disktype.Name = Name;
                    unitWork.DiskType.Insert(disktype);
                }
                else
                {
                    var oldtype = unitWork.DiskType.GetById(Id);
                    if (oldtype != null)
                    {
                        oldtype.Name = Name;
                        unitWork.DiskType.Update(oldtype);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy loại" }, JsonRequestBehavior.AllowGet);
                    }
                }
                unitWork.Commit();
                return Json(new { success = true, message = "Lưu dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}