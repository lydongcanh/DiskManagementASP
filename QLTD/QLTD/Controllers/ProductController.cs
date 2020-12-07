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
    public class ProductController : BaseController
    {
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;
        private readonly EhrDbContext db;
        public ProductController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness, EhrDbContext db)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
            this.db = db;
        }

        // GET: Product
        [PermissionBasedAuthorize("DATA_INPUT")]
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            var user = unitWork.User.GetById(this.User.UserId);
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "ProductCode";
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
            var properties = typeof(Product).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("ProductCode"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã sản phẩm", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("ProductName"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên sản phẩm", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("ProductOrigin"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Xuất xứ", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("Company"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Công ty", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("TypeOfProduct"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Loại sản phẩm", Order = 6, AllowSorting = true });
                }
                if (item.Name.Equals("Antimicrobials"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Kháng sinh", Order = 7, AllowSorting = true });
                }
                if (item.Name.Equals("AnimalInfors"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thú nuôi", Order = 8, AllowSorting = true });
                }
                if (item.Name.Equals("OrtherABs"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thành phần khác", Order = 9, AllowSorting = true });
                }
                if (item.Name.Equals("State"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Trạng thái", Order = 10, AllowSorting = true });
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
            IQueryable<Product> Products = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("ProductCode"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.ProductCode descending select c;
                else if (sortProperty.Equals("ProductName"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.ProductName descending select c;
                else if (sortProperty.Equals("ProductOrigin"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.ProductOrigin descending select c;
                else if (sortProperty.Equals("Company"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.Company descending select c;
                else if (sortProperty.Equals("TypeOfProduct"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.TypeOfProduct descending select c;
                else if (sortProperty.Equals("Antimicrobials"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.Antimicrobials descending select c;
                else if (sortProperty.Equals("AnimalInfors"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.AnimalInfors descending select c;
                else if (sortProperty.Equals("OrtherABs"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.OrtherABs descending select c;
                else if (sortProperty.Equals("State"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.State descending select c;

            }
            else
            {
                if (sortProperty.Equals("ProductCode"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.ProductCode ascending select c;
                else if (sortProperty.Equals("ProductName"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.ProductName ascending select c;
                else if (sortProperty.Equals("ProductOrigin"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.ProductOrigin ascending select c;
                else if (sortProperty.Equals("Company"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.Company ascending select c;
                else if (sortProperty.Equals("TypeOfProduct"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.TypeOfProduct ascending select c;
                else if (sortProperty.Equals("Antimicrobials"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.Antimicrobials ascending select c;
                else if (sortProperty.Equals("AnimalInfors"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.AnimalInfors ascending select c;
                else if (sortProperty.Equals("OrtherABs"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.OrtherABs ascending select c;
                else if (sortProperty.Equals("State"))
                    Products = from c in db.Products where c.CreateBy.Id == user.Id orderby c.State ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                Products = Products.Where(c => c.ProductCode.ToString().Contains(searchString) || c.ProductName.ToString().Contains(searchString) 
                || c.Company.Contains(searchString));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (Products != null)
                datasetSize = Products.Count();
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
            if (Products != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(Products.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(Products.ToList());
                }

            else
                return View((from c in db.Products select c).ToList());
        }

        public ActionResult Add()
        {
            var enumPet = Enum.GetValues(typeof(Pet))
                             .Cast<Pet>()
                             .ToList();
            var enumOrigin = Enum.GetValues(typeof(Origin))
                           .Cast<Origin>()
                           .ToList();
            var enumType = Enum.GetValues(typeof(ProductType))
                           .Cast<ProductType>()
                           .ToList();
            var enumRoute = Enum.GetValues(typeof(Route))
                           .Cast<Route>()
                           .ToList();
            var enumRank = Enum.GetValues(typeof(UnitVolume))
                            .Cast<UnitVolume>()
                            .ToList();
            var enumYesNo = Enum.GetValues(typeof(YesNo))
                           .Cast<YesNo>()
                           .ToList();
            var enumAnti = Enum.GetValues(typeof(Antimicrobial))
                           .Cast<Antimicrobial>()
                           .ToList();
            ViewBag.ListRank = from s in enumRank
                               select new SelectListItem
                               {
                                   Value = ((int)s).ToString(),
                                   Text = s.GetAttribute<DisplayAttribute>().Name
                               };
            ViewBag.ListPet = from s in enumPet
                              select new SelectListItem
                              {
                                  Value = ((int)s).ToString(),
                                  Text = s.GetAttribute<DisplayAttribute>().Name
                              };
            ViewBag.ListRoute = from s in enumRoute
                                select new SelectListItem
                                {
                                    Value = ((int)s).ToString(),
                                    Text = s.GetAttribute<DisplayAttribute>().Name
                                };
            ViewBag.ListOrigin = from s in enumOrigin
                                 select new SelectListItem
                                 {
                                     Value = ((int)s).ToString(),
                                     Text = s.GetAttribute<DisplayAttribute>().Name
                                 };
            ViewBag.ListType = from s in enumType
                               select new SelectListItem
                               {
                                   Value = ((int)s).ToString(),
                                   Text = s.GetAttribute<DisplayAttribute>().Name
                               };
            ViewBag.ListYesNo = from s in enumYesNo
                                select new SelectListItem
                                {
                                    Value = ((int)s).ToString(),
                                    Text = s.GetAttribute<DisplayAttribute>().Name
                                };
            ViewBag.ListAnti = from s in enumAnti
                               select new SelectListItem
                               {
                                   Value = s.ToString()
                               };
            ViewBag.ListAntimi = unitWork.Antimi.Get();
            ViewBag.ListAnimal = unitWork.Animal.Get();
            return View();
        }

        public ActionResult AddProduct(Product product, List<AnimalInfor> animals, List<AntimicroBial> antimicroBials, List<OrtherAB> ortherABs)
        {
            try
            {
                if (product != null)
                {
                    var user = unitWork.User.GetById(this.User.UserId);
                    product.DateStamp = DateTime.Now;
                    product.CreateBy = user;
                    product.State = State.WAIT;
                    unitWork.Product.Insert(product);
                    unitWork.Commit();
                    if (antimicroBials.Count > 0)
                    {
                        foreach (var item in antimicroBials)
                        {
                            item.Product = product;
                            unitWork.AntimicroBial.Insert(item);
                            unitWork.Commit();
                        }
                    }
                    if (ortherABs.Count > 0)
                    {

                        foreach (var item in ortherABs)
                        {
                            item.Product = product;
                            unitWork.OrtherAB.Insert(item);
                            unitWork.Commit();
                        }
                    }
                    if (animals.Count > 0)
                    {

                        foreach (var item in animals)
                        {
                            item.Product = product;
                            unitWork.AnimalInfor.Insert(item);
                            unitWork.Commit();
                        }
                    }
                    return Json(new { success = true, message = "Thêm sản phẩm thành công !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm sản phẩm !" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAntimiByProductId(int? Id)
        {
            var lsAnimi = unitWork.AntimicroBial.Get(x => x.Product.Id == Id).ToList();
            var lsAntimiview = new List<AntimiViewModel>();
            foreach (var item in lsAnimi)
            {
                var antimi = new AntimiViewModel();
                antimi.Id = item.Id;
                antimi.AntimiId = item.Antimi.Id;
                antimi.AntimiName = item.Antimi.Name;
                antimi.Strength = item.Strength;
                antimi.Units = item.Units;
                antimi.PerAmountOfAnti = item.PerAmountOfAnti;
                antimi.UnitsOfPerAmountAnti = item.UnitsOfPerAmountAnti;
                antimi.PerAmountOfProduct = item.PerAmountOfProduct;
                antimi.UnitsOfPerAmountProduct = item.UnitsOfPerAmountProduct;
                antimi.Note = item.Note;
                antimi.ProductId = item.Product.Id;
                lsAntimiview.Add(antimi);
            }
            return new JsonResult
            {
                Data = lsAntimiview,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetAnimalByProductId(int? Id)
        {
            var lsOrther = unitWork.OrtherAB.Get(x => x.Product.Id == Id).ToList();
            var lsOrtherView = new List<OrtherABViewModel>();
            foreach (var item in lsOrther)
            {
                var ortherab = new OrtherABViewModel();
                ortherab.Id = item.Id;
                ortherab.Name = item.Name;
                ortherab.Strength = item.Strength;
                ortherab.Units = item.Units;
                ortherab.PerAmountOfAnti = item.PerAmountOfAnti;
                ortherab.UnitsOfPerAmountAnti = item.UnitsOfPerAmountAnti;
                ortherab.PerAmountOfProduct = item.PerAmountOfProduct;
                ortherab.UnitsOfPerAmountProduct = item.UnitsOfPerAmountProduct;
                ortherab.Note = item.Note;
                ortherab.ProductId = item.Product.Id;
                lsOrtherView.Add(ortherab);
            }
            return new JsonResult
            {
                Data = lsOrtherView,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetOrtherABByProductId(int? Id)
        {
            var lsOrther = unitWork.OrtherAB.Get(x => x.Product.Id == Id).ToList();
            var lsOrtherView = new List<OrtherABViewModel>();
            foreach (var item in lsOrther)
            {
                var ortherab = new OrtherABViewModel();
                ortherab.Id = item.Id;
                ortherab.Name = item.Name;
                ortherab.Strength = item.Strength;
                ortherab.Units = item.Units;
                ortherab.PerAmountOfAnti = item.PerAmountOfAnti;
                ortherab.UnitsOfPerAmountAnti = item.UnitsOfPerAmountAnti;
                ortherab.PerAmountOfProduct = item.PerAmountOfProduct;
                ortherab.UnitsOfPerAmountProduct = item.UnitsOfPerAmountProduct;
                ortherab.Note = item.Note;
                ortherab.ProductId = item.Product.Id;
                lsOrtherView.Add(ortherab);
            }
            return new JsonResult
            {
                Data = lsOrtherView,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}