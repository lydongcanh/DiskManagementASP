using ClosedXML.Excel;
using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.Tools;
using Ehr.Common.UI;
using Ehr.Data;
using Ehr.Models;
using Ehr.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class QuestionnaireManageController : BaseController
    {
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;
        private readonly EhrDbContext db;
        public QuestionnaireManageController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness, EhrDbContext db)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
            this.db = db;
        }
        // GET: QuestionnaireManage
        [PermissionBasedAuthorize("DATA_MNT")]
        public ActionResult Index(string sortProperty, string sortOrder, string searchString, int? size, int? page)
        {
            var user = unitWork.User.GetById(this.User.UserId);
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "D_u_th_i_gian";
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
            var properties = typeof(Questionnaire).GetProperties();
            foreach (var item in properties)
            {
                if (item.Name.Equals("D_u_th_i_gian"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Thời gian thực hiện", Order = 2, AllowSorting = true });
                }
                if (item.Name.Equals("A1__Product_origin"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Xuất xứ", Order = 3, AllowSorting = true });
                }
                if (item.Name.Equals("A2__Product_code"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Mã sản phẩm", Order = 4, AllowSorting = true });
                }
                if (item.Name.Equals("A3__Product_name"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Tên sản phẩm", Order = 5, AllowSorting = true });
                }
                if (item.Name.Equals("A4__Company"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Công ty", Order = 6, AllowSorting = true });
                }
                if (item.Name.Equals("A5__Type_of_product"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Loại sản phẩm", Order = 7, AllowSorting = true });
                }
                if (item.Name.Equals("A7__Volume_of_product"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Khối lượng sản phẩm", Order = 8, AllowSorting = true });
                }
                if (item.Name.Equals("A8__Unit_of_volume_of_product"))
                {
                    columns.Add(new EZGridColumn() { Name = item.Name, Text = "Đơn vị", Order = 9, AllowSorting = true });
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
            IQueryable<Questionnaire> questionnaires = null;
            if (sortOrder.Equals("desc"))
            {
                if (sortProperty.Equals("D_u_th_i_gian"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.D_u_th_i_gian descending select c;
                else if (sortProperty.Equals("A1__Product_origin"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A1__Product_origin descending select c;
                else if (sortProperty.Equals("A2__Product_code"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A2__Product_code descending select c;
                else if (sortProperty.Equals("A3__Product_name"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A3__Product_name descending select c;
                else if (sortProperty.Equals("A4__Company"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A4__Company descending select c;
                else if (sortProperty.Equals("A5__Type_of_product"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A5__Type_of_product descending select c;
                else if (sortProperty.Equals("A7__Volume_of_product"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A7__Volume_of_product descending select c;
                else if (sortProperty.Equals("A8__Unit_of_volume_of_product"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A8__Unit_of_volume_of_product descending select c;
                else if (sortProperty.Equals("State"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.State descending select c;

            }
            else
            {
                if (sortProperty.Equals("D_u_th_i_gian"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.D_u_th_i_gian ascending select c;
                else if (sortProperty.Equals("A1__Product_origin"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A1__Product_origin ascending select c;
                else if (sortProperty.Equals("A2__Product_code"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A2__Product_code ascending select c;
                else if (sortProperty.Equals("A3__Product_name"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A3__Product_name ascending select c;
                else if (sortProperty.Equals("A4__Company"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A4__Company ascending select c;
                else if (sortProperty.Equals("A5__Type_of_product"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A5__Type_of_product ascending select c;
                else if (sortProperty.Equals("A7__Volume_of_product"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A7__Volume_of_product ascending select c;
                else if (sortProperty.Equals("A8__Unit_of_volume_of_product"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.A8__Unit_of_volume_of_product ascending select c;
                else if (sortProperty.Equals("State"))
                    questionnaires = from c in db.Questionnaires where c.State == State.APPROVE orderby c.State ascending select c;
            }
            if (!user.IsCentral)
            {
                questionnaires = questionnaires.Where(c => c.CreateBy.Province == user.Province);
            }
            #endregion

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                questionnaires = questionnaires.Where(c => c.D_u_th_i_gian.ToString().Contains(searchString) || c.A1__Product_origin.ToString().Contains(searchString) || c.A2__Product_code.Contains(searchString) || c.A3__Product_name.Contains(searchString) || c.A4__Company.Contains(searchString)
                || c.A5__Type_of_product.ToString().Contains(searchString) || c.A7__Volume_of_product.Contains(searchString) || c.A8__Unit_of_volume_of_product.ToString().Contains(searchString) || c.State.ToString().Contains(searchString));

            }
            #endregion

            #region Phần phân trang		
            //Trang mặc định
            if (page == null) page = 1;
            //Số dòng mặc định
            int pageSize = (size ?? 10);
            // Đếm tổng số trang

            int datasetSize = 0;
            if (questionnaires != null)
                datasetSize = questionnaires.Count();
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
            if (questionnaires != null)
                if (ezpage.PageModel.StartItem >= 1 && datasetSize > 0)
                {
                    return View(questionnaires.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList());
                }
                else
                {
                    return View(questionnaires.ToList());
                }

            else
                return View((from c in db.Questionnaires select c).ToList());
        }
        [PermissionBasedAuthorize("DATA_MNT")]
        public ActionResult Detail(int? id)
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
            ViewBag.ListRank = from s in enumRank
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

            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var questionnaire = unitWork.Questionnaire.Get(r => r.Id == id).FirstOrDefault();

            var lsPet = questionnaire.B6__Target_species_x.Split(',').ToArray();
            var lsPetSelect = new List<Pet>();
            foreach (var item in lsPet)
            {
                Pet pet = (Pet)Enum.Parse(typeof(Pet), item, true);
                lsPetSelect.Add(pet);
            }
            var lsRoute = questionnaire.B7__Administration_route.Split(',').ToArray();
            var lsRouteSelect = new List<Route>();
            foreach (var item in lsRoute)
            {
                Route route = (Route)Enum.Parse(typeof(Route), item, true);
                lsRouteSelect.Add(route);
            }

            ViewBag.ListPet = from s in enumPet
                              select new SelectListItem
                              {
                                  Value = ((int)s).ToString(),
                                  Text = s.GetAttribute<DisplayAttribute>().Name,
                                  Selected = (lsPetSelect.Contains(s))
                              };
            ViewBag.ListRoute = from s in enumRoute
                                select new SelectListItem
                                {
                                    Value = ((int)s).ToString(),
                                    Text = s.GetAttribute<DisplayAttribute>().Name,
                                    Selected = (lsRouteSelect.Contains(s))
                                };

            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            return View(questionnaire);
        }
        public ActionResult Import(HttpPostedFileBase optImportFile)
        {
            var user = unitWork.User.GetById(User.UserId);
            if (optImportFile != null)
            {
                EZFileController ezfile = new EZFileController();
                EZFileInfo fileInfo = ezfile.GetFinalName(EZConfig.UploadPath, Path.GetExtension(optImportFile.FileName));
                string relativePath = fileInfo.DirectoryContain + "//" + fileInfo.FileName;
                string finalPath = EZConfig.UploadPath + "//" + relativePath;
                optImportFile.SaveAs(finalPath);

                EZImporter import = new EZImporter();
				//import.ImportQuestionnaire(finalPath, user);
				import.UpdateQuestionnaire(finalPath, user,unitWork);
			}
            return RedirectToAction("Index", "QuestionnaireManage");
        }

        [PermissionBasedAuthorize("DATA_MNT")]
        public ActionResult Update(int? id)
        {
            var user = unitWork.User.GetById(this.User.UserId);
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
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = unitWork.Questionnaire.Get(r => r.Id == id).FirstOrDefault();
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            if (questionnaire.B6__Target_species_x != "" && questionnaire.B6__Target_species_x != null)
            {
                var lsPet = questionnaire.B6__Target_species_x.Split(',').ToArray();
                var lsPetSelect = new List<Pet>();
                foreach (var item in lsPet)
                {
                    Pet pet = (Pet)Enum.Parse(typeof(Pet), item, true);
                    lsPetSelect.Add(pet);

                }
                ViewBag.ListPet = from s in enumPet
                                  select new SelectListItem
                                  {
                                      Value = ((int)s).ToString(),
                                      Text = s.GetAttribute<DisplayAttribute>().Name,
                                      Selected = (lsPetSelect.Contains(s))
                                  };
            }
            if(questionnaire.B7__Administration_route != "" && questionnaire.B7__Administration_route != null)
            {
                var lsRoute = questionnaire.B7__Administration_route.Split(',').ToArray();
                var lsRouteSelect = new List<Route>();
                foreach (var item in lsRoute)
                {
                    Route route = (Route)Enum.Parse(typeof(Route), item, true);
                    lsRouteSelect.Add(route);
                }

                ViewBag.ListRoute = from s in enumRoute
                                    select new SelectListItem
                                    {
                                        Value = ((int)s).ToString(),
                                        Text = s.GetAttribute<DisplayAttribute>().Name,
                                        Selected = (lsRouteSelect.Contains(s))
                                    };

            }
            return View(questionnaire);
        }

        [HttpPost]
        [PermissionBasedAuthorize("DATA_MNT")]
        public ActionResult Update(Questionnaire questionnaire, List<Pet> B6__Target_species_x, List<Route> B7__Administration_route, string CollectedDate)
        {
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            var lspet = "";
            var lsroute = "";
            DateTime? CollectedDates;
            if (B6__Target_species_x != null)
            {
                var count = 0;
                foreach (var item in B6__Target_species_x)
                {
                    count++;
                    if (count == B6__Target_species_x.Count)
                    {
                        lspet += item;
                    }
                    else
                    {
                        lspet += item + ",";
                    }
                }
            }
            if (B7__Administration_route != null)
            {
                var count = 0;
                foreach (var item in B7__Administration_route)
                {
                    count++;
                    if (count == B7__Administration_route.Count)
                    {
                        lsroute += item;
                    }
                    else
                    {
                        lsroute += item + ",";
                    }
                }
            }
            if (CollectedDate == "" || CollectedDate == "-")
            {
                questionnaire.CollectedDate = DateTime.Now;
            }
            else
            {
                CollectedDates = DataConverter.UI2DateTimeOrNull(CollectedDate);
                questionnaire.CollectedDate = (DateTime)CollectedDates;
            }

            var oldquestionnaire = db.Questionnaires.AsNoTracking().Single(c => c.Id == questionnaire.Id);
            if (questionnaire.F_2__Picture_of_product != null)
            {
                oldquestionnaire.F_2__Picture_of_product = questionnaire.F_2__Picture_of_product;
            }
            oldquestionnaire = questionnaire;
            oldquestionnaire.State = State.APPROVE;
            oldquestionnaire.B6__Target_species_x = lspet;
            oldquestionnaire.B7__Administration_route = lsroute;
            oldquestionnaire.D_u_th_i_gian = DateTime.Now;
            try
            {
                db.Entry<Questionnaire>(oldquestionnaire).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var user = unitWork.User.GetById(this.User.UserId);
                return Json(new { success = true, message = "Lưu dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
        }
        [PermissionBasedAuthorize("DATA_MNT")]
        public ActionResult Export()
        {
            var questionnaires = unitWork.Questionnaire.Get();
            var user = unitWork.User.GetById(this.User.UserId);
            if (!user.IsCentral)
            {
                questionnaires = questionnaires.Where(c => c.CreateBy != null && c.CreateBy.Province == user.Province);
            }
            var questionfinal = questionnaires.ToList();
            ETConverter etc = new ETConverter();
            EhrDbContext db = new EhrDbContext();
            DataSet ds = etc.ConvertQuestionnaires(questionfinal);
            var date = DateTime.Now.Date.Day + "_" + DateTime.Now.Date.Month + "_" + DateTime.Now.Date.Year;
            string fileName = "database_products_" + date + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                MemoryStream stream = new MemoryStream();
                foreach (DataTable table in ds.Tables)
                {
                    wb.Worksheets.Add(table, table.TableName);
                    wb.SaveAs(stream);
                }
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}