using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly UnitWork unitWork;
        public CustomerController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        // GET: Customer
        [PermissionBasedAuthorize("CUS_MNT")]
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
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
            var properties = typeof(Customer).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Code"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã khách hàng", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Name"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên khách hàng", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("PhoneNumber"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Số điện thoại", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("Address"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Địa chỉ", Order = 5, AllowSorting = true });
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
            IQueryable<Customer> Customers = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Code"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.Code descending select c;
                if (sortProperty.Equals("Name"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.Name descending select c;
                else if (sortProperty.Equals("PhoneNumber"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.PhoneNumber descending select c;
                else if (sortProperty.Equals("Address"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.Address descending select c;
            }
            else
            {
                if (sortProperty.Equals("Code"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.Code ascending select c;
                if (sortProperty.Equals("Name"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.Name ascending select c;
                else if (sortProperty.Equals("PhoneNumber"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.PhoneNumber ascending select c;
                else if (sortProperty.Equals("Address"))
                    Customers = from c in db.Customers where c.Status == CustomerStatus.ACTIVE orderby c.Address ascending select c;
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                Customers = Customers.Where(c => c.Code.ToString().ToLower().Contains(searchString.ToLower()) ||c.Name.ToString().ToLower().Contains(searchString.ToLower()) || c.PhoneNumber.ToString().ToLower().Contains(searchString.ToLower())|| c.Address.ToString().ToLower().Contains(searchString.ToLower()));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (Customers != null)
                datasetSize = Customers.Count();
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
            if (Customers != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(Customers.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(Customers.ToList());
                }

            else
                return View((from c in db.Customers select c).ToList());
        }

        [PermissionBasedAuthorize("CUS_MNT")]
        public JsonResult AddCustomer(int? Id,string Code ,string Name,string PhoneNumber,string Address)
        {
            try
            {
                var checkex = CheckExist(Code);
                if (checkex == false)
                {
                    return Json(new { success = false, message = "Mã khách hàng đã tồn tại !" }, JsonRequestBehavior.AllowGet);
                }
                if (Id == null)
                {
                    var customer = new Customer();
                    customer.Code = Code;
                    customer.Name = Name;
                    customer.PhoneNumber = PhoneNumber;
                    customer.Address = Address;
                    customer.Status = CustomerStatus.ACTIVE;
                    unitWork.Customer.Insert(customer);
                }
                else
                {
                    var oldcustomer = unitWork.Customer.GetById(Id);
                    if (oldcustomer != null)
                    {
                        oldcustomer.Code = Code;
                        oldcustomer.Name = Name;
                        oldcustomer.PhoneNumber = PhoneNumber;
                        oldcustomer.Address = Address;
                        oldcustomer.Status = CustomerStatus.ACTIVE;
                        unitWork.Customer.Update(oldcustomer);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy khách hàng" }, JsonRequestBehavior.AllowGet);
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

        [PermissionBasedAuthorize("CUS_MNT")]
        public JsonResult DeleteCustomer(int? Id)
        {
            try
            {
                var oldcustomer = unitWork.Customer.GetById(Id);
                if (oldcustomer != null)
                {
                    oldcustomer.Status = CustomerStatus.INACTIVE;
                    unitWork.Customer.Update(oldcustomer);
                    unitWork.Commit();
                }
                return Json(new { success = true, message = "Xoá khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool CheckExist(string Code)
        {
            var check = unitWork.Customer.Get(x => x.Name.ToLower() == Code.ToLower()).FirstOrDefault();
            if (check != null)
                return false;
            return true;
        }
    }
}