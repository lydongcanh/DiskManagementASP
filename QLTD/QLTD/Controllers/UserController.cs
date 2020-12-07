using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ehr.Bussiness;
using Ehr.Models;
using Ehr.Common.Constraint;
using System.Data.Entity.Migrations;
using Ehr.Data;
using System.Data.Entity;
using Ehr.Common.UI;
using Ehr.Auth;
using System.IO;
using Ehr.Common.Tools;
using Ehr.ViewModels;
using System.Configuration;
using System.Web.Security;

namespace Ehr.Controllers
{
    public class UserController : BaseController
    {
        // GET: User


        private readonly UnitWork unitWork;

        public UserController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

		[HttpGet]
        public ActionResult ResetPWD(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            //Sắp xếp mặc định theo Tên khách hàng
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Username";
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

            var properties = typeof(User).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Username"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên người dùng", Order = 1, AllowSorting = true });
                }
                if (item.Name.Equals("FullName"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Họ và tên", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Address"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Đia chỉ", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("IsActive"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Kích hoạt?", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("EmployeeType"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Nhóm nhân viên", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("Department"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Phòng ban", Order = 6, AllowSorting = true });
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
            IQueryable<User> usr = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Username"))
                    usr = from c in db.Users orderby c.Username descending select c;
                else if (sortProperty.Equals("FullName"))
                    usr = from c in db.Users orderby c.FullName descending select c;
                else if (sortProperty.Equals("Address"))
                    usr = from c in db.Users orderby c.Address descending select c;
                else if (sortProperty.Equals("IsActive"))
                    usr = from c in db.Users orderby c.IsActive descending select c;


            }
            else
            {
                if (sortProperty.Equals("Username"))
                    usr = from c in db.Users orderby c.Username ascending select c;
                else if (sortProperty.Equals("FullName"))
                    usr = from c in db.Users orderby c.FullName ascending select c;
                else if (sortProperty.Equals("Address"))
                    usr = from c in db.Users orderby c.Address ascending select c;
                else if (sortProperty.Equals("isActive"))
                    usr = from c in db.Users orderby c.IsActive ascending select c;

            }
            #endregion
            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                usr = usr.Where(c => c.Username.Contains(searchString));
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 1000);
            //Đếm tổng số trang
            int datasetSize = 0;
            if (usr != null)
                datasetSize = usr.Count();
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

            if (usr != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(usr.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(usr.ToList());
                }

            else
                return View((from c in db.Users select c).ToList());



        }

        [HttpGet]
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            //Sắp xếp mặc định theo Tên khách hàng
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Username";
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

            var properties = typeof(User).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Username"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên người dùng", Order = 1, AllowSorting = true });
                }
                if (item.Name.Equals("FullName"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Họ và tên", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Address"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Đia chỉ", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("IsActive"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Kích hoạt?", Order = 4, AllowSorting = true });

                }
                if (item.Name.Equals("EmployeeType"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Nhóm nhân viên", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("Department"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Phòng ban", Order = 6, AllowSorting = true });
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
            IQueryable<User> usr = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Username"))
                    usr = from c in db.Users orderby c.Username descending select c;
                else if (sortProperty.Equals("FullName"))
                    usr = from c in db.Users orderby c.FullName descending select c;
                else if (sortProperty.Equals("Address"))
                    usr = from c in db.Users orderby c.Address descending select c;
                else if (sortProperty.Equals("IsActive"))
                    usr = from c in db.Users orderby c.IsActive descending select c;


            }
            else
            {
                if (sortProperty.Equals("Username"))
                    usr = from c in db.Users orderby c.Username ascending select c;
                else if (sortProperty.Equals("FullName"))
                    usr = from c in db.Users orderby c.FullName ascending select c;
                else if (sortProperty.Equals("Address"))
                    usr = from c in db.Users orderby c.Address ascending select c;
                else if (sortProperty.Equals("isActive"))
                    usr = from c in db.Users orderby c.IsActive ascending select c;

            }
            #endregion
            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                usr = usr.Where(c => c.Username.Contains(searchString));
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 1000);
            //Đếm tổng số trang
            int datasetSize = 0;
            if (usr != null)
                datasetSize = usr.ToList().Count;
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

            if (usr != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(usr.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(usr.ToList());
                }

            else
                return View((from c in db.Users select c).ToList());



        }
        [HttpPost]
        [PermissionBasedAuthorize("USER_MNT")]
        public ActionResult User_Add(User newUser, HttpPostedFileBase imageFile)
        {
            string message = "";

            var checkuser = unitWork.User.Get(s => s.Username.ToLower() == newUser.Username.ToLower()).FirstOrDefault();
            if (checkuser != null)
            {
                message = "Đã có username này";
                ViewBag.Message = message;
                return RedirectToAction("Index", "User");
            }

            if (imageFile != null && imageFile.ContentLength > 0)
            {
                if (imageFile.ContentLength > 4194304) // 4mb
                {
                    message = "File quá lớn";
                    ViewBag.Message = message;
                    return RedirectToAction("Index", "User");
                }

            }
            if (imageFile != null && !imageFile.ContentType.Contains("image"))
            {
                message = "Không phải ảnh";
                ViewBag.Message = message;
                return RedirectToAction("Index", "User");
            }
            if (imageFile == null)
            {
                newUser.Image = "user-default.jpg";
            }
            else
            {
                EZFileController ezfile = new EZFileController();
                EZFileInfo fileInfo = ezfile.GetFinalName(EZConfig.UploadPath, Path.GetExtension(imageFile.FileName));
                string relativePath = fileInfo.DirectoryContain + "//" + fileInfo.FileName;
                imageFile.SaveAs(EZConfig.UploadPath + "//" + relativePath);
                newUser.Image = relativePath;
            }


            List<Role> list_role = new List<Role>();
            Ehr.Common.UI.EZFileController upass = new Ehr.Common.UI.EZFileController();
            string password = upass.UniqueName();
            newUser.Password = Utilities.Encrypt(password);

            foreach (var item in unitWork.Role.Get().ToList())
            {
                string name = "Role" + item.Id;
                if (Request.Form.Get(name) != null)
                    list_role.Add(unitWork.Role.GetById(int.Parse(Request.Form.Get(name))));
            }
            newUser.IsActive = true;
            newUser.Roles = list_role;
            unitWork.User.Insert(newUser);
            unitWork.Commit();
            SendVerifyConfirm(newUser);

            string lsroles = "";
            foreach (var item in list_role)
            {
                lsroles += " - " + item.RoleName;
            }

            var user = unitWork.User.GetById(this.User.UserId);

            var usertype = newUser.UserType.ToString();
            ViewBag.Message = message;
            return RedirectToAction("Index", "User");
        }

        private void SendVerifyConfirm(User user)
        {
            unitWork.User.Update(user);
            unitWork.Commit();
        }

        [HttpGet]
        public ActionResult User_Update(string id)
        {
            var NoId = int.Parse(Request.QueryString["id"]);
            var user = unitWork.User.Get(u => u.Id == NoId).FirstOrDefault();
            return View(user);
        }
        [HttpPost]
        [PermissionBasedAuthorize("USER_MNT")]
        public ActionResult User_Update(User updateUser)
        {
            List<Role> list_rol = new List<Role>();// để lưu danh sách sau khi chỉnh sửa 
            //thêm role vào danh sách 
            foreach (var i in unitWork.Role.Get().ToList())
            {
                string name = "Role" + i.Id;
                if (Request.Form.Get(name) != null)
                {
                    list_rol.Add(i);
                }
            }
            var usr = unitWork.User.GetById(updateUser.Id);
            string oldroles = "";
            foreach (var item in usr.Roles)
            {
                oldroles += " - " + item.RoleName;
            }
            var usertype = usr.UserType.ToString();
            UserViewModel oldObject = new UserViewModel()
            {
                Id = usr.Id,
                Username = usr.Username,
                Password = usr.Password,
                IsActive = usr.IsActive,
                FullName = usr.FullName,
                Image = usr.Image,
                Email = usr.Email,
                PhoneNumber = usr.PhoneNumber,
                Address = usr.Address,
                Gender = usr.Gender,
                Experience = usr.Experience,
                IsCentral = usr.IsCentral,
                Province = usr.Province,
                UserType = usertype,
                Roles = oldroles,
            };

            usr.IsActive = updateUser.IsActive;
            usr.Address = updateUser.Address;
            usr.Email = updateUser.Email;
            usr.PhoneNumber = updateUser.PhoneNumber;
            usr.FullName = updateUser.FullName;
            usr.Gender = updateUser.Gender;
            usr.Experience = updateUser.Experience;
            usr.IsCentral = updateUser.IsCentral;
            usr.Province = updateUser.Province;
            usr.UserType = updateUser.UserType;


            //update lại role chứa user
            foreach (var i in unitWork.Role.Get().ToList())
            {
                if (list_rol.Contains(i))//role này có trong list role sau khi chỉnh sửa thì 
                {
                    if (!usr.Roles.Contains(i))// thì nếu role này không có trong list role ban đầu thì thêm role này vào còn có rồi thì next
                    {
                        usr.Roles.Add(i);
                    }
                }
                else//nếu role i này không có trong list role vừa chình sửa thì
                {
                    if (usr.Roles.Contains(i))// nesu role i này có trong list role ban đầu của user thì 
                    {
                        usr.Roles.Remove(i);//xóa nó khỏi danh sách
                    }
                }

            }
            unitWork.User.Update(usr);
            unitWork.Commit();

            var newUser = unitWork.User.GetById(updateUser.Id);
            var user = unitWork.User.GetById(this.User.UserId);
            string newroles = "";
            foreach (var item in list_rol)
            {
                newroles += " - " + item.RoleName;
            }
            var newusertype = usr.UserType.ToString();
            UserViewModel newObject = new UserViewModel()
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Password = newUser.Password,
                IsActive = newUser.IsActive,
                FullName = newUser.FullName,
                Image = newUser.Image,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Address = newUser.Address,
                Gender = newUser.Gender,
                Experience = newUser.Experience,
                IsCentral = usr.IsCentral,
                Province = usr.Province,
                UserType = newusertype,
                Roles = newroles
            };
          

            return RedirectToAction("Index", unitWork.User.Get().ToList());

        }

        [HttpGet]
        public ActionResult User_Detail(string id)
        {
            int NoId = int.Parse(Request.QueryString["id"]);
            var usr = unitWork.User.Get(u => u.Id == NoId).FirstOrDefault();
            return View(usr);
        }

        public ActionResult UpdateProfile(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = unitWork.User.Get(s => s.Id == id.Value).FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Home");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(User updateUser, HttpPostedFileBase ImageUpdate)
        {
            string message = "";
            if (ImageUpdate != null && ImageUpdate.ContentLength > 0)
            {
                if (ImageUpdate.ContentLength > 4194304) // 4mb
                {
                    message = "File quá lớn";
                    ViewBag.Message = message;
                    return View(updateUser);
                }

            }
            if (ImageUpdate != null && !ImageUpdate.ContentType.Contains("image"))
            {
                message = "Không phải ảnh";
                ViewBag.Message = message;
                return View(updateUser);
            }
            var user = unitWork.User.GetById(updateUser.Id);
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            var oldObject = (from b in db.Users
                             select new
                             {
                                 FullName = user.FullName,
                                 Image = user.Image,
                                 Email = user.Email,
                                 Address = user.Address,
                                 PhoneNumber = user.PhoneNumber,
                                 Gender = user.Gender,
                                 Experience = user.Experience,
                             }).FirstOrDefault();

            user.Address = updateUser.Address;
            user.Email = updateUser.Email;
            user.PhoneNumber = updateUser.PhoneNumber;
            user.FullName = updateUser.FullName;
            user.Gender = updateUser.Gender;
            user.Experience = updateUser.Experience;

            if (ImageUpdate != null)
			{
				EZFileController ezfile = new EZFileController ( );
				EZFileInfo fileInfo = ezfile.GetFinalName ( EZConfig.UploadPath,Path.GetExtension ( ImageUpdate.FileName ) );
				string relativePath = fileInfo.DirectoryContain + "//" + fileInfo.FileName;
				ImageUpdate.SaveAs ( EZConfig.UploadPath + "//" + relativePath );
				user.Image = relativePath;
			}

            unitWork.User.Update(user);
            unitWork.Commit();

            var currentuser = unitWork.User.GetById(this.User.UserId);
            var newUser = unitWork.User.GetById(updateUser.Id);

            var newObject = (from b in db.Users
                             select new
                             {
                                 FullName = newUser.FullName,
                                 Image = newUser.Image,
                                 Email = newUser.Email,
                                 Address = newUser.Address,
                                 PhoneNumber = newUser.PhoneNumber,
                                 Gender = newUser.Gender,
                                 Experience = newUser.Experience,
                             }).FirstOrDefault();

            string keyCookie = ConfigurationManager.AppSettings["cookie"];
            HttpCookie cookie = new HttpCookie(keyCookie, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        [HttpPost]
        [PermissionBasedAuthorize("USER_MNT")]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordReset(string userid,string NewPassword,string ConfirmedPassword)
        {	
            string idd = Request.Form.Get("id");
            var usr = unitWork.User.GetById(int.Parse(userid));
            if(NewPassword!=""&&ConfirmedPassword!="")
            {
                if (NewPassword == ConfirmedPassword)
                {
                    
                    usr.Password = Utilities.Encrypt(NewPassword);
                    unitWork.User.Update(usr);
                    unitWork.Commit();
                    return PartialView("Message",new Common.UI.AjaxStatus ( ) {Status= Common.UI.PostedStatus.OK,Message="Đổi mật khẩu thành công!" } );
                }
                else return PartialView("Message",new Common.UI.AjaxStatus ( ) {Status= Common.UI.PostedStatus.FAILED,Message="Đổi mật khẩu thất bại!" } );;
            }
            
            else return PartialView("Message",new Common.UI.AjaxStatus ( ) {Status= Common.UI.PostedStatus.FAILED,Message="Đổi mật khẩu thất bại!" } );;
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Success()
        {
            ViewBag.Message = TempData["Message"] as string;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User newUser, HttpPostedFileBase imageFile)
        {
            try
            {
                var checkuser = unitWork.User.Get(s => s.Username.ToLower() == newUser.Username.ToLower()).FirstOrDefault();
                if (checkuser != null)
                {
                    TempData["Message"] = "1";
                    return RedirectToAction("Success", "User");
                }

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    if (imageFile.ContentLength > 4194304) // 4mb
                    {
                        TempData["Message"] = "2";
                        return RedirectToAction("Success", "User");
                    }

                }
                if (imageFile != null && !imageFile.ContentType.Contains("image"))
                {
                    TempData["Message"] = "3";
                    return RedirectToAction("Success", "User");

                }
                if (imageFile == null)
                {
                    newUser.Image = "user-default.jpg";
                }
                else
                {
                    EZFileController ezfile = new EZFileController();
                    EZFileInfo fileInfo = ezfile.GetFinalName(EZConfig.UploadPath, Path.GetExtension(imageFile.FileName));
                    string relativePath = fileInfo.DirectoryContain + "//" + fileInfo.FileName;
                    imageFile.SaveAs(EZConfig.UploadPath + "//" + relativePath);
                    newUser.Image = relativePath;
                }


                List<Role> list_role = new List<Role>();
                Ehr.Common.UI.EZFileController upass = new Ehr.Common.UI.EZFileController();
                string password = upass.UniqueName();
                newUser.Password = Utilities.Encrypt(password);

                list_role.Add(unitWork.Role.Get(x => x.RoleName.Trim() == "Người sử dụng").FirstOrDefault());
                newUser.IsActive = true;
                newUser.Roles = list_role;
                newUser.IsActive = false;
                unitWork.User.Insert(newUser);
                unitWork.Commit();
                
                TempData["Message"] = "0";
                return RedirectToAction("Success", "User");
            }
            catch (Exception)
            {
                TempData["Message"] = "-1";
                return RedirectToAction("Success", "User");
            }
        }

        [HttpPost]
        [PermissionBasedAuthorize("USER_MNT")]
        public JsonResult ApproveUser(int? userid)
        {
            try
            {
                var user = unitWork.User.GetById(userid);
                if(user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng !" }, JsonRequestBehavior.AllowGet);
                }
                user.IsActive = true;
                unitWork.User.Update(user);
                unitWork.Commit();
                SendVerifyConfirm(user);
                return Json(new { success = true, message = "Duyệt thành công !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Duyệt không thành công !" }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        [HttpGet]
        public ActionResult Approve(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            //Sắp xếp mặc định theo Tên khách hàng
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Username";
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

            var properties = typeof(User).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("Username"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên người dùng", Order = 1, AllowSorting = true });
                }
                if (item.Name.Equals("FullName"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Họ và tên", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("Address"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Đia chỉ", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("IsActive"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Kích hoạt?", Order = 4, AllowSorting = true });

                }
                if (item.Name.Equals("EmployeeType"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Nhóm nhân viên", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("Department"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = " Phòng ban", Order = 6, AllowSorting = true });
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
            IQueryable<User> usr = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("Username"))
                    usr = from c in db.Users orderby c.Username descending select c;
                else if (sortProperty.Equals("FullName"))
                    usr = from c in db.Users orderby c.FullName descending select c;
                else if (sortProperty.Equals("Address"))
                    usr = from c in db.Users orderby c.Address descending select c;
                else if (sortProperty.Equals("IsActive"))
                    usr = from c in db.Users orderby c.IsActive descending select c;


            }
            else
            {
                if (sortProperty.Equals("Username"))
                    usr = from c in db.Users orderby c.Username ascending select c;
                else if (sortProperty.Equals("FullName"))
                    usr = from c in db.Users orderby c.FullName ascending select c;
                else if (sortProperty.Equals("Address"))
                    usr = from c in db.Users orderby c.Address ascending select c;
                else if (sortProperty.Equals("isActive"))
                    usr = from c in db.Users orderby c.IsActive ascending select c;

            }

            usr = usr.Where(x => x.IsActive == false);
            #endregion
            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                usr = usr.Where(c => c.Username.Contains(searchString));
            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 1000);
            //Đếm tổng số trang
            int datasetSize = 0;
            if (usr != null)
                datasetSize = usr.Count();
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

            if (usr != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(usr.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(usr.ToList());
                }

            else
                return View((from c in db.Users where c.IsActive == false select c).ToList());



        }

    }
}