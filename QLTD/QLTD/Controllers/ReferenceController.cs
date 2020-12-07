using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Data;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class ReferenceController : Controller
    {
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;
        private readonly EhrDbContext db;
        public ReferenceController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness, EhrDbContext db)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
            this.db = db;
        }
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Pet";
            //Mặc định là từ A-Z
            string nextSort = "";
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "asc";
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
            var properties = typeof(Reference).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Pet"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Loài", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("PetNameDetail"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("Week"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tuần", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("Volume_AVG"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Trọng lượng trung bình", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("Volume_Food_AVG"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thức ăn trung bình", Order = 6, AllowSorting = true });
                }
                if (item.Name.Equals("Volume_Water_AVG"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thức uống trung bình", Order = 7, AllowSorting = true });
                }
                if (item.Name.Equals("Age_Min"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Ngày tuổi", Order = 8, AllowSorting = true });
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
            IQueryable<Reference> referencess = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Pet"))
                    referencess = from c in db.References orderby c.Pet descending select c;
                else if (sortProperty.Equals("PetNameDetail"))
                    referencess = from c in db.References orderby c.PetNameDetail descending select c;
                else if (sortProperty.Equals("Week"))
                    referencess = from c in db.References orderby c.Week descending select c;
                else if (sortProperty.Equals("Volume_AVG"))
                    referencess = from c in db.References orderby c.Volume_AVG descending select c;
                else if (sortProperty.Equals("Volume_Food_AVG"))
                    referencess = from c in db.References orderby c.Volume_Food_AVG descending select c;
                else if (sortProperty.Equals("Volume_Water_AVG"))
                    referencess = from c in db.References orderby c.Volume_Water_AVG descending select c;
            }
            else
            {
                if (sortProperty.Equals("Pet"))
                    referencess = from c in db.References orderby c.Pet ascending select c;
                else if (sortProperty.Equals("PetNameDetail"))
                    referencess = from c in db.References orderby c.PetNameDetail ascending select c;
                else if (sortProperty.Equals("Week"))
                    referencess = from c in db.References orderby c.Week ascending select c;
                else if (sortProperty.Equals("Volume_AVG"))
                    referencess = from c in db.References orderby c.Volume_AVG ascending select c;
                else if (sortProperty.Equals("Volume_Food_AVG"))
                    referencess = from c in db.References orderby c.Volume_Food_AVG ascending select c;
                else if (sortProperty.Equals("Volume_Water_AVG"))
                    referencess = from c in db.References orderby c.Volume_Water_AVG ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                referencess = referencess.Where(c => c.Pet.Contains(searchString) || c.PetNameDetail.Contains(searchString) || c.Week.ToString().Contains(searchString) || c.Volume_AVG.ToString().Contains(searchString) || c.Volume_Food_AVG.ToString().Contains(searchString) || c.Volume_Water_AVG.ToString().Contains(searchString) );
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (referencess != null)
                datasetSize = referencess.Count();
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
            var enumPet = Enum.GetValues(typeof(Pet))
                        .Cast<Pet>()
                        .ToList();
            ViewBag.ListPet = from s in enumPet
                              select new SelectListItem
                              {
                                  Value = s.ToString(),
                                  Text = s.GetAttribute<DisplayAttribute>().Name
                              };
            //Hiển thị trong khoảng chọn
            if (referencess != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(referencess.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(referencess.ToList());
                }

            else
                return View((from c in db.References select c).ToList());
        }
        [HttpGet]
        public ActionResult New()
        {
            var enumPet = Enum.GetValues(typeof(Pet))
                          .Cast<Pet>()
                          .ToList();
            ViewBag.ListPet = from s in enumPet
                              select new SelectListItem
                              {
                                  Value = s.ToString(),
                                  Text = s.GetAttribute<DisplayAttribute>().Name
                              };
            return View();
        }

        [HttpPost]
        [PermissionBasedAuthorize("DATA_INPUT")]
        public ActionResult New(Reference reference)
        {
            if (ModelState.IsValid)
            {
                if (reference == null)
                {
                    return Json(new { success = false, message = "Dữ liệu trống" }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    unitWork.Reference.Insert(reference);
                    unitWork.Commit();
                    return Json(new { success = true, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var enumPet = Enum.GetValues(typeof(Pet))
                       .Cast<Pet>()
                       .ToList();
            ViewBag.ListPet = from s in enumPet
                              select new SelectListItem
                              {
                                  Value = s.ToString(),
                                  Text = s.GetAttribute<DisplayAttribute>().Name
                              };
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = unitWork.Reference.Get(r => r.Id == id).FirstOrDefault();
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        [HttpPost]
        [PermissionBasedAuthorize("DATA_INPUT")]
        public ActionResult Edit(Reference reference)
        {
            if (ModelState.IsValid)
            {
                Ehr.Data.EhrDbContext _context = new Data.EhrDbContext();

                if (reference == null)
                {
                    return Json(new { success = false, message = "Dữ liệu trống" }, JsonRequestBehavior.AllowGet);
                }

                var refe = _context.References.AsNoTracking().Single(c => c.Id == reference.Id);
                refe = reference;
                try
                {
                    _context.Entry<Reference>(refe).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}