using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Models;
namespace Ehr.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;
        public CategoryController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
        }

        #region Animals
        [PermissionBasedAuthorize("DATA_INPUT")]
        public ActionResult Animals (string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            var user = unitWork.User.GetById(this.User.UserId);
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Name";
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
            var properties = typeof(Animal).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Name"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thú nuôi", Order = 2, AllowSorting = true });
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
            IQueryable<Animal> Animals = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Name"))
                    Animals = from c in db.Animals orderby c.Name descending select c;
            }
            else
            {
                if (sortProperty.Equals("Name"))
                    Animals = from c in db.Animals orderby c.Name ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                Animals = Animals.Where(c => c.Name.ToString().Contains(searchString));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (Animals != null)
                datasetSize = Animals.Count();
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
            if (Animals != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(Animals.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(Animals.ToList());
                }

            else
                return View((from c in db.Animals select c).ToList());
        }

        public JsonResult AddAnimal(int? Id, string Name)
        {
            try
            {
                var checkex = CheckExistAnimal(Name);
                if (checkex == false)
                {
                    return Json(new { success = false, message = "Thú nuôi đã có trên hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                if (Id == null)
                {
                    var animal = new Animal();
                    animal.Name = Name;
                    unitWork.Animal.Insert(animal);
                }
                else
                {
                    var oldnimal = unitWork.Animal.GetById(Id);
                    if(oldnimal != null)
                    {
                        oldnimal.Name = Name;
                        unitWork.Animal.Update(oldnimal);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy thú nuôi" }, JsonRequestBehavior.AllowGet);
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

        public bool CheckExistAnimal(string Name)
        {
            var check = unitWork.Animal.Get(x => x.Name.ToLower() == Name.ToLower()).FirstOrDefault();
            if(check != null)
                return false;
            return true;
        }
        #endregion

        [PermissionBasedAuthorize("DATA_INPUT")]
        public ActionResult Antimis(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            var user = unitWork.User.GetById(this.User.UserId);
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Name";
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
            var properties = typeof(Antimi).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Name"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Kháng sinh", Order = 2, AllowSorting = true });
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
            IQueryable<Antimi> Antimis = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Name"))
                    Antimis = from c in db.Antimis orderby c.Name descending select c;
            }
            else
            {
                if (sortProperty.Equals("Name"))
                    Antimis = from c in db.Antimis orderby c.Name ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                Antimis = Antimis.Where(c => c.Name.ToString().Contains(searchString));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (Antimis != null)
                datasetSize = Antimis.Count();
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
            if (Antimis != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(Antimis.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(Antimis.ToList());
                }

            else
                return View((from c in db.Antimis select c).ToList());
        }


        public JsonResult AddAntimi(int? Id, string Name)
        {
            try
            {
                var checkex = CheckExistAntimi(Name);
                if (checkex == false)
                {
                    return Json(new { success = false, message = "Kháng sinh đã có trên hệ thống !" }, JsonRequestBehavior.AllowGet);
                }
                if (Id == null)
                {
                    var antimi = new Antimi();
                    antimi.Name = Name;
                    unitWork.Antimi.Insert(antimi);
                }
                else
                {
                    var oldantimi = unitWork.Antimi.GetById(Id);
                    if (oldantimi != null)
                    {
                        oldantimi.Name = Name;
                        unitWork.Antimi.Update(oldantimi);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy kháng sinh" }, JsonRequestBehavior.AllowGet);
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


        public bool CheckExistAntimi(string Name)
        {
            var check = unitWork.Antimi.Get(x => x.Name.ToLower() == Name.ToLower()).FirstOrDefault();
            if (check != null)
                return false;
            return true;
        }
    }
}







