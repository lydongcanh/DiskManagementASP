using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ehr.Bussiness;
using Ehr.Models;
using Ehr.Common.Constraint;
using System.Data.Entity;
using Ehr.Common.UI;
using Ehr.ViewModels;
using Ehr.Auth;

namespace Ehr.Controllers
{
    public class RoleController : BaseController
    {
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;

        public RoleController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
        }
            // GET: Role
        [HttpGet]
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            //Sắp xếp mặc định theo Tên khách hàng
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "RoleName";
            //Mặc định là từ A-Z
            string nextSort = "";
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "asc";
            if (sortOrder.Equals("asc") || sortOrder.Equals("none")) nextSort = "desc";
            if (sortOrder.Equals("desc")) nextSort = "asc";
            #region Tạo danh sách cho hiển thị số dòng trên lưới
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
            #endregion

            #region  xây dựng header cho lưới
            string header = "";
            //các cột cần hiển thị
            List<EZGridColumn> columns = new List<EZGridColumn>();
            
            var properties = typeof(Role).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("RoleName"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên nhóm quyền", Order = 1, AllowSorting = true });
                }
                if (item.Name.Equals("RoleStatus"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Trạng thái", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("IsRoot"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Quyền cao nhất", Order = 3, AllowSorting = true });
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
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            #region Phần sắp xếp
            //Lấy dataset rỗng
            IQueryable<Role> roless = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("RoleName"))
                    roless = from c in db.Roles orderby c.RoleName descending select c;
                else if (sortProperty.Equals("RoleStatus"))
                    roless = from c in db.Roles orderby c.RoleStatus descending select c;
                if (sortProperty.Equals("IsRoot"))
                    roless = from c in db.Roles orderby c.IsRoot descending select c;
                
            }
            else
            {
                if (sortProperty.Equals("RoleName"))
                    roless = from c in db.Roles orderby c.RoleName ascending select c;
                else if (sortProperty.Equals("RoleStatus"))
                    roless = from c in db.Roles orderby c.RoleStatus ascending select c;
                if (sortProperty.Equals("IsRoot"))
                    roless = from c in db.Roles orderby c.IsRoot ascending select c;
                
            }
            #endregion
            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                roless = roless.Where(c => c.RoleName.Contains(searchString));
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 1000);
           //Đếm tổng số trang
           int datasetSize = 0;
			if(roless != null)
				datasetSize = roless.Count();
            int checkTotal = (int)(datasetSize / pageSize) + 1;
            //Nếu trang yêu cầu vượt qua tổng số trang thì thiết lập là tổng số trang
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

            if (roless != null)
               if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(roless.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(roless.ToList());
                }

            else
                return View((from c in db.Roles select c).ToList());

            
        }

        [HttpPost]
        [PermissionBasedAuthorize("USER_MNT")]
        public ActionResult Role_Add(Role newRole)
        {

            List<string> list = new List<string>();
            List<Permission> list_per = new List<Permission>();
            List<Role> list_role = new List<Role>();
            YesNo isroot = (Request.Form.Get("IsRoot") != null) ? (Request.Form.Get("IsRoot").ToString().Equals("on") ? YesNo.YES : YesNo.NO) : YesNo.NO;
            RoleStatus status = (Request.Form.Get("RoleStatus") != null) ? (Request.Form.Get("RoleStatus").ToString().Equals("on") ? RoleStatus.ACTIVATED : RoleStatus.NOTACTIVATED) : RoleStatus.NOTACTIVATED;
            foreach (var i in unitWork.Permission.Get().ToList())
            {
                string name="Permission"+i.Id;
                list.Add(Request.Form.Get(name));
            }
            newRole.RoleStatus = status;
            newRole.IsRoot = isroot;
            
                if (newRole.IsRoot.ToString()=="YES")
                {
                    
                    foreach (var item_Permession in unitWork.Permission.Get().ToList())
                    {
                        item_Permession.Roles.Add(newRole);
                    }
                }
                else
                {
                    
                   foreach(var i in list)// lấy i( là id được lấy từ checkbox) 
                    {
                        if(i!=null)//nếu i > 0 
                        {
                            foreach (var i2 in unitWork.Permission.Get().ToList()) //tìm trong bảng permission
                            {
                                if(i2.Id.ToString()==i)//id nào bằng i thì add id của permission đó và list_per
                                {
                                    list_per.Add(i2);
                                    //thêm role vừa tạo vào i2
                                }
                            }
                        }
                    }
                    newRole.Permissions = list_per; // danh sách permission của Role = list_per này
                }
            
            unitWork.Role.Insert(newRole);
            unitWork.Commit();
            #region Audit Trail
            var user = unitWork.User.GetById(this.User.UserId);

            string lsPer = "";

            foreach (var item in newRole.Permissions)
            {
                lsPer += " - " + item.PermissionName;
            }

            RoleViewModel newObject = new RoleViewModel()
            {
                Id = newRole.Id,
                RoleName = newRole.RoleName,
                RoleStatus = newRole.RoleStatus,
                IsRoot = newRole.IsRoot,
                Permissions = lsPer,
            };
            RoleViewModel oldObject = new RoleViewModel();
            auditTrailBussiness.CreateAuditTrail(AuditActionType.Create, newRole.Id, "Vai trò", oldObject, newObject, user.Username);
            #endregion

            return RedirectToAction("Index","Role");
        
            
        }

        #region
        //[HttpPost]
        //public ActionResult Role_Update(string id, Role newRole)
        //{
        //    //chưa xét quyền admin  


        //    List<string> list = new List<string>();
        //    List<Permission> list_per = new List<Permission>();
        //    foreach (var i in unitWork.Permission.Get().ToList())
        //    {
        //        string name = "Permission" + i.Id;
        //        list.Add(Request.Form.Get(name));
        //    }
        //    //lấy role theo id
        //    var roles = unitWork.Role.GetById(int.Parse(id));
        //    var listper = unitWork.Permission.Get().ToList();
        //    // xóa permission cũ
        //    foreach (var item in listper)
        //    {
        //        foreach (var item2 in item.Roles.ToList())
        //        {
        //            if (item2.Id == roles.Id)
        //                item.Roles.Remove(item2);

        //        }
        //    }
        //    //add add role vào permission có trong list
        //    foreach (var i in unitWork.Permission.Get().ToList())
        //    {
        //        foreach (var item in list)
        //        {
        //            if (item != null)
        //            {
        //                if (i.Id == int.Parse(item))
        //                {
        //                    list_per.Add(i); break;
        //                }
        //            }

        //        }
        //    }
        //    foreach (var i in unitWork.Permission.Get().ToList())
        //    {
        //        foreach (var item in list)
        //        {
        //            if (item != null)
        //            {
        //                if (i.Id == int.Parse(item))
        //                {
        //                    i.Roles.Add(roles);
        //                }
        //            }

        //        }



        //    }
        //    roles.Permissions = list_per;
        //    unitWork.Role.Update(roles);
        //    unitWork.Commit();
        //    return View("Index", unitWork.Role.Get().ToList());
        //}
        #endregion
        /// <summary>
        /// Hàm xử lý cập nhật lại các permission cho role
        /// </summary>
        /// <param name="selected_permissions"></param>
        /// <param name="role"></param>
        void UpdateRolePermission(List<Permission> selected_permissions, Role role)
        {
            //chưa check addmin
            var currentPermissions = role.Permissions;
            foreach (var permission in unitWork.Permission.Get().ToList())
            {
                if (selected_permissions.Contains(permission))
                {
                    if (!role.Permissions.Contains(permission))
                    {
                        role.Permissions.Add(permission);
                    }
                }
                else
                {
                    if (role.Permissions.Contains(permission))
                    {
                        role.Permissions.Remove(permission);
                    }
                }
            }

        }
        [PermissionBasedAuthorize("USER_MNT")]
        public ActionResult Role_Detail( string id)
        {
            var idd = int.Parse(Request.QueryString["id"]);
            var model_ = unitWork.Role.Get(s => s.Id == idd).ToList();
            return View(model_);
        }
        [HttpPost]
        [PermissionBasedAuthorize("USER_MNT")]
        public ActionResult Role_Update(string id, Role newRole)
        {
            //string temp = Request["RoleStatus"].ToString ( );			
            //int roleID = (Request.QueryString["id"] != null) ? int.Parse ( Request.QueryString["id"].ToString ( ) ) : 0;
            List<string> list = new List<string>();
            List<Permission> list_per = new List<Permission>();
            YesNo isroot = (Request.Form.Get("IsRoot") != null) ? (Request.Form.Get("IsRoot").ToString().Equals("on") ? YesNo.YES : YesNo.NO) : YesNo.NO;
            newRole.IsRoot = isroot;
            if (newRole.IsRoot.ToString() == "YES")
            {
                list_per = unitWork.Permission.Get().ToList();
            }
            else
            {
                foreach (var i in unitWork.Permission.Get().ToList())
                {
                    string name = "Permission" + i.Id;
                    if (Request.Form.Get(name) != null)
                        list_per.Add(unitWork.Permission.GetById(int.Parse(Request.Form.Get(name))));
                }
            }
                
            
            RoleStatus status = (Request.Form.Get("RoleStatus") != null) ? (Request.Form.Get("RoleStatus").ToString().Equals("on") ? RoleStatus.ACTIVATED : RoleStatus.NOTACTIVATED) : RoleStatus.NOTACTIVATED;
            //lấy role theo id			
            var roles = unitWork.Role.GetById(int.Parse(id));

            #region old role
            string lsOldPer = "";

            foreach (var item in roles.Permissions)
            {
                lsOldPer += " - " + item.PermissionName;
            }

            RoleViewModel oldObject = new RoleViewModel()
            {
                Id = roles.Id,
                RoleName = roles.RoleName,
                RoleStatus = roles.RoleStatus,
                IsRoot = roles.IsRoot,
                Permissions = lsOldPer
            };
            #endregion
            UpdateRolePermission(list_per, roles);
            roles.RoleName = newRole.RoleName;
            roles.IsRoot = isroot;
            roles.RoleStatus = status;
            unitWork.Role.Update(roles);
            unitWork.Commit();

            #region Audit Trail
            var newroles = unitWork.Role.GetById(int.Parse(id));
            var user = unitWork.User.GetById(this.User.UserId);

            string lsPer = "";

            foreach (var item in newroles.Permissions)
            {
                lsPer += " - " + item.PermissionName;
            }

            RoleViewModel newObject = new RoleViewModel()
            {
                Id = newroles.Id,
                RoleName = newroles.RoleName,
                RoleStatus = newroles.RoleStatus,
                IsRoot = newroles.IsRoot,
                Permissions = lsPer,
            };
           
            auditTrailBussiness.CreateAuditTrail(AuditActionType.Update, newRole.Id, "Vai trò", oldObject, newObject, user.Username);
            #endregion
            return RedirectToAction("Index", unitWork.User.Get().ToList());
        }

        [HttpGet]
        [PermissionBasedAuthorize("USER_MNT")]
        public ActionResult Role_Update(string id)
        {
            
            var roles=unitWork.Role.Get().Where(r=>r.Id.ToString()==Request.QueryString["id"]);
            if(roles==null)
                return RedirectToAction("Index", "Role");
            else
            return View(roles);
        }
    }
}