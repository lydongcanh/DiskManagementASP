using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Models;
using Ehr.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ehr.ViewModels;

namespace Ehr.Controllers
{
    public class QuestionnaireController : BaseController
    {
        // GET: Questionnaire
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;
        private readonly EhrDbContext db;
        public QuestionnaireController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness,EhrDbContext db)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
            this.db = db;
        }
        [PermissionBasedAuthorize("DATA_INPUT")]
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
                    questionnaires = from c in db.Questionnaires where c.CreateBy.Id == user.Id orderby c.D_u_th_i_gian descending select c;
                else if (sortProperty.Equals("A1__Product_origin"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A1__Product_origin descending select c;
                else if (sortProperty.Equals("A2__Product_code"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A2__Product_code descending select c;
                else if (sortProperty.Equals("A3__Product_name"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A3__Product_name descending select c;
                else if (sortProperty.Equals("A4__Company"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A4__Company descending select c;
                else if (sortProperty.Equals("A5__Type_of_product"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A5__Type_of_product descending select c;
                else if (sortProperty.Equals("A7__Volume_of_product"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A7__Volume_of_product descending select c;
                else if (sortProperty.Equals("A8__Unit_of_volume_of_product"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A8__Unit_of_volume_of_product descending select c;
                else if (sortProperty.Equals("State"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.State descending select c;

            }
            else
            {
                if (sortProperty.Equals("D_u_th_i_gian"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.D_u_th_i_gian ascending select c;
                else if (sortProperty.Equals("A1__Product_origin"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A1__Product_origin ascending select c;
                else if (sortProperty.Equals("A2__Product_code"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A2__Product_code ascending select c;
                else if (sortProperty.Equals("A3__Product_name"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A3__Product_name ascending select c;
                else if (sortProperty.Equals("A4__Company"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A4__Company ascending select c;
                else if (sortProperty.Equals("A5__Type_of_product"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A5__Type_of_product ascending select c;
                else if (sortProperty.Equals("A7__Volume_of_product"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A7__Volume_of_product ascending select c;
                else if (sortProperty.Equals("A8__Unit_of_volume_of_product"))
                    questionnaires = from c in db.Questionnaires  where c.CreateBy.Id == user.Id orderby c.A8__Unit_of_volume_of_product ascending select c;
                else if (sortProperty.Equals("State"))
                    questionnaires = from c in db.Questionnaires where c.CreateBy.Id == user.Id orderby c.State ascending select c;
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

        [PermissionBasedAuthorize("DATA_INPUT")]
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
            return View();
        }

        [HttpPost]
        [PermissionBasedAuthorize("DATA_INPUT")]
        public ActionResult Add(Questionnaire questionnaire, List<Pet> B6__Target_species_x, List<Route> B7__Administration_route, string CollectedDate)
        {

            var lspet = "";
            var lsroute = "";
            DateTime? CollectedDates;
            if (B6__Target_species_x != null)
            {
                var count = 0;
                foreach (var item in B6__Target_species_x)
                {
                    count++;
                    if(count == B6__Target_species_x.Count)
                    {
                        lspet += item;
                    }
                    else
                    {
                        lspet += item + ",";
                    }
                }
            }
            if(B7__Administration_route != null)
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
            var user = unitWork.User.GetById(this.User.UserId);
            if (Request.Form.Get("F_2__Picture_of_product").ToString().Length > 0)
            {
                questionnaire.F_2__Picture_of_product = Request.Form.Get("F_2__Picture_of_product");
            }
            questionnaire.B6__Target_species_x = lspet;
            questionnaire.B7__Administration_route = lsroute;
            questionnaire.CreateBy = user;
            questionnaire.State = State.WAIT;
            questionnaire.D_u_th_i_gian = DateTime.Now;
            try
            {
                unitWork.Questionnaire.Insert(questionnaire);
                unitWork.Commit();

                #region 
                #region Enum ToString
                var state = questionnaire.State.ToString();
                var A1__Product_origin = questionnaire.A1__Product_origin.ToString();
                var A5__Type_of_product = questionnaire.A5__Type_of_product.ToString();
                var A6__Other_subtance_in_product = questionnaire.A6__Other_subtance_in_product.ToString();
                var A8__Unit_of_volume_of_product = questionnaire.A8__Unit_of_volume_of_product.ToString();
                var B2_3__Units_of_antimicrobial_1 = questionnaire.B2_3__Units_of_antimicrobial_1.ToString();
                var B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = questionnaire.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_.ToString();
                var B2_7__Units_of_product__link_to_question_B2_4_ = questionnaire.B2_7__Units_of_product__link_to_question_B2_4_.ToString();
                var B3_3__Units_of_antimicrobial_2 = questionnaire.B3_3__Units_of_antimicrobial_2.ToString();
                var B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = questionnaire.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_.ToString();
                var B3_7__Units_of_product__link_to_question_B3_4_ = questionnaire.B3_7__Units_of_product__link_to_question_B3_4_.ToString();
                var B4_3__Units_of_antimicrobial_3 = questionnaire.B4_3__Units_of_antimicrobial_3.ToString();
                var B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = questionnaire.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_.ToString();
                var B4_7__Units_of_product__link_to_question_B4_4_ = questionnaire.B4_7__Units_of_product__link_to_question_B4_4_.ToString();
                var B5_3__Units_of_antimicrobial_4 = questionnaire.B5_3__Units_of_antimicrobial_4.ToString();
                var B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = questionnaire.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_.ToString();
                var B5_7__Units_of_product__link_to_question_B5_4_ = questionnaire.B5_7__Units_of_product__link_to_question_B5_4_.ToString();

                var C1_2__Product_preparation_Unit_of_product = questionnaire.C1_2__Product_preparation_Unit_of_product.ToString();
                var C2_3__Unit_of_product = questionnaire.C2_3__Unit_of_product.ToString();
                var C3_2__Product_preparation_Unit_of_product = questionnaire.C3_2__Product_preparation_Unit_of_product.ToString();
                var C4_3__Unit_of_product = questionnaire.C4_3__Unit_of_product.ToString();

                var D1_2__Product_preparation_Unit_of_product = questionnaire.D1_2__Product_preparation_Unit_of_product.ToString();
                var D2_3__Unit_of_product = questionnaire.D2_3__Unit_of_product.ToString();
                var D3_2__Product_preparation_Unit_of_product = questionnaire.D3_2__Product_preparation_Unit_of_product.ToString();
                var D4_3__Unit_of_product = questionnaire.D4_3__Unit_of_product.ToString();

                var E1_2__Product_preparation_Unit_of_product = questionnaire.E1_2__Product_preparation_Unit_of_product.ToString();
                var E2_3__Unit_of_product = questionnaire.E2_3__Unit_of_product.ToString();
                var E3_2__Product_preparation_Unit_of_product = questionnaire.E3_2__Product_preparation_Unit_of_product.ToString();
                var E4_3__Unit_of_product = questionnaire.E4_3__Unit_of_product.ToString();

                var Piglet_1_2__Product_preparation_Unit_of_product = questionnaire.Piglet_1_2__Product_preparation_Unit_of_product.ToString();
                var Piglet_2_3__Unit_of_product = questionnaire.Piglet_2_3__Unit_of_product.ToString();
                var Piglet_3_2__Product_preparation_Unit_of_product = questionnaire.Piglet_3_2__Product_preparation_Unit_of_product.ToString();
                var Piglet_4_3__Unit_of_product = questionnaire.Piglet_4_3__Unit_of_product.ToString();

                var Buffalo_1_2__Product_preparation_Unit_of_product = questionnaire.Buffalo_1_2__Product_preparation_Unit_of_product.ToString();
                var Buffalo_2_3__Unit_of_product = questionnaire.Buffalo_2_3__Unit_of_product.ToString();
                var Buffalo_3_2__Product_preparation_Unit_of_product = questionnaire.Buffalo_3_2__Product_preparation_Unit_of_product.ToString();
                var Buffalo_4_3__Unit_of_product = questionnaire.Buffalo_4_3__Unit_of_product.ToString();

                var Cattle_1_2__Product_preparation_Unit_of_product = questionnaire.Cattle_1_2__Product_preparation_Unit_of_product.ToString();
                var Cattle_2_3__Unit_of_product = questionnaire.Cattle_2_3__Unit_of_product.ToString();
                var Cattle_3_2__Product_preparation_Unit_of_product = questionnaire.Cattle_3_2__Product_preparation_Unit_of_product.ToString();
                var Cattle_4_3__Unit_of_product = questionnaire.Cattle_4_3__Unit_of_product.ToString();

                var Goat_1_2__Product_preparation_Unit_of_product = questionnaire.Goat_1_2__Product_preparation_Unit_of_product.ToString();
                var Goat_2_3__Unit_of_product = questionnaire.Goat_2_3__Unit_of_product.ToString();
                var Goat_3_2__Product_preparation_Unit_of_product = questionnaire.Goat_3_2__Product_preparation_Unit_of_product.ToString();
                var Goat_4_3__Unit_of_product = questionnaire.Goat_4_3__Unit_of_product.ToString();

                var Sheep_1_2__Product_preparation_Unit_of_product = questionnaire.Sheep_1_2__Product_preparation_Unit_of_product.ToString();
                var Sheep_2_3__Unit_of_product = questionnaire.Sheep_2_3__Unit_of_product.ToString();
                var Sheep_3_2__Product_preparation_Unit_of_product = questionnaire.Sheep_3_2__Product_preparation_Unit_of_product.ToString();
                var Sheep_4_3__Unit_of_product = questionnaire.Sheep_4_3__Unit_of_product.ToString();

                var Horse_1_2__Product_preparation_Unit_of_product = questionnaire.Horse_1_2__Product_preparation_Unit_of_product.ToString();
                var Horse_2_3__Unit_of_product = questionnaire.Horse_2_3__Unit_of_product.ToString();
                var Horse_3_2__Product_preparation_Unit_of_product = questionnaire.Horse_3_2__Product_preparation_Unit_of_product.ToString();
                var Horse_4_3__Unit_of_product = questionnaire.Horse_4_3__Unit_of_product.ToString();

                var Chicken_1_2__Product_preparation_Unit_of_product = questionnaire.Chicken_1_2__Product_preparation_Unit_of_product.ToString();
                var Chicken_2_3__Unit_of_product = questionnaire.Chicken_2_3__Unit_of_product.ToString();
                var Chicken_3_2__Product_preparation_Unit_of_product = questionnaire.Chicken_3_2__Product_preparation_Unit_of_product.ToString();
                var Chicken_4_3__Unit_of_product = questionnaire.Chicken_4_3__Unit_of_product.ToString();

                var Duck_1_2__Product_preparation_Unit_of_product = questionnaire.Duck_1_2__Product_preparation_Unit_of_product.ToString();
                var Duck_2_3__Unit_of_product = questionnaire.Duck_2_3__Unit_of_product.ToString();
                var Duck_3_2__Product_preparation_Unit_of_product = questionnaire.Duck_3_2__Product_preparation_Unit_of_product.ToString();
                var Duck_4_3__Unit_of_product = questionnaire.Duck_4_3__Unit_of_product.ToString();

                var Muscovy_Duck_1_2__Product_preparation_Unit_of_product = questionnaire.Muscovy_Duck_1_2__Product_preparation_Unit_of_product.ToString();
                var Muscovy_Duck_2_3__Unit_of_product = questionnaire.Muscovy_Duck_2_3__Unit_of_product.ToString();
                var Muscovy_Duck_3_2__Product_preparation_Unit_of_product = questionnaire.Muscovy_Duck_3_2__Product_preparation_Unit_of_product.ToString();
                var Muscovy_Duck_4_3__Unit_of_product = questionnaire.Muscovy_Duck_4_3__Unit_of_product.ToString();

                var Quail_1_2__Product_preparation_Unit_of_product = questionnaire.Quail_1_2__Product_preparation_Unit_of_product.ToString();
                var Quail_2_3__Unit_of_product = questionnaire.Quail_2_3__Unit_of_product.ToString();
                var Quail_3_2__Product_preparation_Unit_of_product = questionnaire.Quail_3_2__Product_preparation_Unit_of_product.ToString();
                var Quail_4_3__Unit_of_product = questionnaire.Quail_4_3__Unit_of_product.ToString();

                var Goose_1_2__Product_preparation_Unit_of_product = questionnaire.Goose_1_2__Product_preparation_Unit_of_product.ToString();
                var Goose_2_3__Unit_of_product = questionnaire.Goose_2_3__Unit_of_product.ToString();
                var Goose_3_2__Product_preparation_Unit_of_product = questionnaire.Goose_3_2__Product_preparation_Unit_of_product.ToString();
                var Goose_4_3__Unit_of_product = questionnaire.Goose_4_3__Unit_of_product.ToString();

                var Dog_1_2__Product_preparation_Unit_of_product = questionnaire.Dog_1_2__Product_preparation_Unit_of_product.ToString();
                var Dog_2_3__Unit_of_product = questionnaire.Dog_2_3__Unit_of_product.ToString();
                var Dog_3_2__Product_preparation_Unit_of_product = questionnaire.Dog_3_2__Product_preparation_Unit_of_product.ToString();
                var Dog_4_3__Unit_of_product = questionnaire.Dog_4_3__Unit_of_product.ToString();

                var Cat_1_2__Product_preparation_Unit_of_product = questionnaire.Cat_1_2__Product_preparation_Unit_of_product.ToString();
                var Cat_2_3__Unit_of_product = questionnaire.Cat_2_3__Unit_of_product.ToString();
                var Cat_3_2__Product_preparation_Unit_of_product = questionnaire.Cat_3_2__Product_preparation_Unit_of_product.ToString();
                var Cat_4_3__Unit_of_product = questionnaire.Cat_4_3__Unit_of_product.ToString();

                var Calf_1_2__Product_preparation_Unit_of_product = questionnaire.Calf_1_2__Product_preparation_Unit_of_product.ToString();
                var Calf_2_3__Unit_of_product = questionnaire.Calf_2_3__Unit_of_product.ToString();
                var Calf_3_2__Product_preparation_Unit_of_product = questionnaire.Calf_3_2__Product_preparation_Unit_of_product.ToString();
                var Calf_4_3__Unit_of_product = questionnaire.Calf_4_3__Unit_of_product.ToString();

                var Chick_1_2__Product_preparation_Unit_of_product = questionnaire.Chick_1_2__Product_preparation_Unit_of_product.ToString();
                var Chick_2_3__Unit_of_product = questionnaire.Chick_2_3__Unit_of_product.ToString();
                var Chick_3_2__Product_preparation_Unit_of_product = questionnaire.Chick_3_2__Product_preparation_Unit_of_product.ToString();
                var Chick_4_3__Unit_of_product = questionnaire.Chick_4_3__Unit_of_product.ToString();
				try
				{
					if(questionnaire.A9__Other_volume_of_product==null)
					{
						questionnaire.A9__Other_volume_of_product = "";
					}
				}
				catch { }
                #endregion
                #region newObject
                var newobject = new QuestionnaireViewModel
                {
                    #region A.General information
                    D_u_th_i_gian = questionnaire.D_u_th_i_gian.ToString(),
                    /// <summary>
                    /// Xuất xứ 
                    /// </summary>
                    A1__Product_origin = A1__Product_origin,
                    /// <summary>
                    /// Mã code sản phẩm
                    /// </summary>
                    A2__Product_code = questionnaire.A2__Product_code,
                    /// <summary>
                    /// Tên sản phẩm
                    /// </summary>
                    A3__Product_name = questionnaire.A3__Product_name,
                    /// <summary>
                    /// Tên công ty đăng ký sản phẩm
                    /// </summary>
                    A4__Company = questionnaire.A4__Company,
                    /// <summary>
                    /// Loại sản phẩm ( dang bột - dạng dung dịch )
                    /// </summary>
                    A5__Type_of_product = A5__Type_of_product,
                    /// <summary>
                    /// Có chứa chất ngoài kháng sinh hay không?
                    /// </summary>
                    A6__Other_subtance_in_product = A6__Other_subtance_in_product,
                    /// <summary>
                    /// Khối lượng/trọng lượng/thể tích của sản phẩm 
                    /// </summary>
                    A7__Volume_of_product = questionnaire.A7__Volume_of_product,
                    /// <summary>
                    /// Đơn vị của khối lượng/trọng lượng/thể tích của sản phẩm 
                    /// </summary>
                    A8__Unit_of_volume_of_product = A8__Unit_of_volume_of_product,
                    /// <summary>
                    /// Thông tin về khối lượng khác 
                    /// </summary>
                    A9__Other_volume_of_product = questionnaire.A9__Other_volume_of_product,
                    #endregion

                    #region B.Information related to antimicrobial
                    /// <summary>
                    /// Số loại kháng sinh co trong sản phẩm
                    /// </summary>
                    B1__Number_of_antimicrobials_in_product = questionnaire.B1__Number_of_antimicrobials_in_product,

                    #region Antimicrobials 1
                    /// <summary>
                    /// Antimicrobials_1
                    /// </summary>
                    B2_1__Antimicrobial_1 = questionnaire.B2_1__Antimicrobial_1,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 1 - chỉ điền số
                    /// </summary>
                    B2_2__Strength_of_antimicrobial_1 = questionnaire.B2_2__Strength_of_antimicrobial_1,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 1
                    /// </summary>
                    B2_3__Units_of_antimicrobial_1 = B2_3__Units_of_antimicrobial_1,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B2_4__Per_amount_of_product__antimicrobial_1_ = questionnaire.B2_4__Per_amount_of_product__antimicrobial_1_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ = questionnaire.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_7__Units_of_product__link_to_question_B2_4_ = B2_7__Units_of_product__link_to_question_B2_4_,
                    #endregion
                    #region Antimicrobials 2
                    /// <summary>
                    /// Antimicrobials_2
                    /// </summary>
                    B3_1__Antimicrobial_2 = questionnaire.B3_1__Antimicrobial_2,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 2 - chỉ điền số
                    /// </summary>
                    B3_2__Strength_of_antimicrobial_2 = questionnaire.B3_2__Strength_of_antimicrobial_2,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 2
                    /// </summary>
                    B3_3__Units_of_antimicrobial_2 = B3_3__Units_of_antimicrobial_2,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B3_4__Per_amount_of_product__antimicrobial_2_ = questionnaire.B3_4__Per_amount_of_product__antimicrobial_2_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ = questionnaire.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_7__Units_of_product__link_to_question_B3_4_ = B3_7__Units_of_product__link_to_question_B3_4_,
                    #endregion
                    #region Antimicrobials 3
                    /// <summary>
                    /// Antimicrobials_3
                    /// </summary>
                    B4_1__Antimicrobial_3 = questionnaire.B4_1__Antimicrobial_3,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 3 - chỉ điền số
                    /// </summary>
                    B4_2__Strength_of_antimicrobial_3 = questionnaire.B4_2__Strength_of_antimicrobial_3,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 3
                    /// </summary>
                    B4_3__Units_of_antimicrobial_3 = B4_3__Units_of_antimicrobial_3,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B4_4__Per_amount_of_product__antimicrobial_3_ = questionnaire.B4_4__Per_amount_of_product__antimicrobial_3_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ = questionnaire.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_7__Units_of_product__link_to_question_B4_4_ = B4_7__Units_of_product__link_to_question_B4_4_,
                    #endregion
                    #region Antimicrobials 4
                    /// <summary>
                    /// Antimicrobials_4
                    /// </summary>
                    B5_1__Antimicrobial_4 = questionnaire.B5_1__Antimicrobial_4,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 4 - chỉ điền số
                    /// </summary>
                    B5_2__Strength_of_antimicrobial_4 = questionnaire.B5_2__Strength_of_antimicrobial_4,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 4
                    /// </summary>
                    B5_3__Units_of_antimicrobial_4 = B5_3__Units_of_antimicrobial_4,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B5_4__Per_amount_of_product__antimicrobial_4_ = questionnaire.B5_4__Per_amount_of_product__antimicrobial_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = B5_5__Units_of_antimicrobial_4__link_to_question_5_4_,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ = questionnaire.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_7__Units_of_product__link_to_question_B5_4_ = B5_7__Units_of_product__link_to_question_B5_4_,
                    #endregion

                    /// <summary>
                    /// Các loài vật
                    /// </summary>
                    B6__Target_species_x = lspet,
                    /// <summary>
                    /// Đường dùng thuốc 
                    /// </summary>
                    B7__Administration_route = lsroute,
                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho heo
                    #region C_ Heo

                    #region C_1_ Product preparation (dilution) _pig_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    C1_1__Product_preparation__dilution__Product_amount = questionnaire.C1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    C1_2__Product_preparation_Unit_of_product = C1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    C1_3__Product_preparation_To_be_added_to__min_ = questionnaire.C1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    C1_4__Product_preparation_To_be_added_to__max_ = questionnaire.C1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    C1_5__Product_preparation_Unit_of_water_feed = questionnaire.C1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C1_6__Duration_of_usage__min__max_ = questionnaire.C1_6__Duration_of_usage__min__max_,
                    #endregion

                    #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    C2_1__Product_min = questionnaire.C2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    C2_2__Product_max = questionnaire.C2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    C2_3__Unit_of_product = C2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    C2_4__Per_No__Kg_bodyweight_min = questionnaire.C2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    C2_5__Per_No__Kg_bodyweight_max = questionnaire.C2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C2_6__Duration_of_usage = questionnaire.C2_6__Duration_of_usage,
                    #endregion

                    #region C_3 Product preparation (dilution) _pig_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    C3_1__Product_preparation__dilution__Product_amount = questionnaire.C3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    C3_2__Product_preparation_Unit_of_product = C3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    C3_3__Product_preparation_To_be_added_to__min_ = questionnaire.C3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    C3_4__Product_preparation_To_be_added_to__max_ = questionnaire.C3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    C3_5__Product_preparation_Unit_of_water_feed = questionnaire.C3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C3_6__Duration_of_usage = questionnaire.C3_6__Duration_of_usage,


                    #endregion

                    #region C_4 Guidelines referred to bodyweight_pig_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    C4_1__Product_min = questionnaire.C4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    C4_2__Product_max = questionnaire.C4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    C4_3__Unit_of_product = C4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    C4_4__Per_No__Kg_bodyweight_min = questionnaire.C4_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    C4_5__Per_No__Kg_bodyweight_max = questionnaire.C4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C4_6__Duration_of_usage = questionnaire.C4_6__Duration_of_usage,
                    #endregion

                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho thú nhai lại không phân biệt thú lớn và nhỏ
                    #region D. Động vật nhai lại

                    #region D_1_ Product preparation (dilution) _ruminant_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    D1_1__Product_preparation__dilution__Product_amount = questionnaire.D1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    D1_2__Product_preparation_Unit_of_product = D1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    D1_3__Product_preparation_To_be_added_to__min_ = questionnaire.D1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    D1_4__Product_preparation_To_be_added_to__max_ = questionnaire.D1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    D1_5__Product_preparation_Unit_of_water_feed = questionnaire.D1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D1_6__Duration_of_usage = questionnaire.D1_6__Duration_of_usage,
                    #endregion

                    #region D.2 Guidelines referred to bodyweight_ruminant_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    D2_1__Product_min = questionnaire.D2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    D2_2__Product_max = questionnaire.D2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    D2_3__Unit_of_product = D2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    D2_4__Per_No__Kg_bodyweight_min = questionnaire.D2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    D2_5__Per_No__Kg_bodyweight_max = questionnaire.D2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D2_6__Duration_of_usage = questionnaire.D2_6__Duration_of_usage,
                    #endregion

                    #region D_3 Product preparation (dilution) _ruminant_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    D3_1__Product_preparation__dilution__Product_amount = questionnaire.D3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    D3_2__Product_preparation_Unit_of_product = D3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    D3_3__Product_preparation_To_be_added_to__min_ = questionnaire.D3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    D3_4__Product_preparation_To_be_added_to__max_ = questionnaire.D3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    D3_5__Product_preparation_Unit_of_water_feed = questionnaire.D3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D3_6__Duration_of_usage = questionnaire.D3_6__Duration_of_usage,


                    #endregion

                    #region D_4 Guidelines referred to bodyweight_ruminant_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    D4_1__Product_min = questionnaire.D4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    D4_2__Product_max = questionnaire.D4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    D4_3__Unit_of_product = D4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    D4_4__Per_No__Kg_bodyweight_min = questionnaire.D4_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    D4_5__Per_No__Kg_bodyweight_max = questionnaire.D4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D4_6__Duration_of_usage = questionnaire.D4_6__Duration_of_usage,
                    #endregion

                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho gia cầm nói chung bao gồm gà, vịt, ngan, ngỗng, cút
                    #region E. Gia cầm

                    #region D_1_ Product preparation (dilution) _poultry_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    E1_1__Product_preparation__dilution__Product_amount = questionnaire.E1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    E1_2__Product_preparation_Unit_of_product = E1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    E1_3__Product_preparation_To_be_added_to__min_ = questionnaire.E1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    E1_4__Product_preparation_To_be_added_to__max_ = questionnaire.E1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    E1_5__Product_preparation_Unit_of_water_feed = questionnaire.E1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E1_6__Duration_of_usage = questionnaire.E1_6__Duration_of_usage,
                    #endregion

                    #region D.2 Guidelines referred to bodyweight_poultry_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    E2_1__Product_min = questionnaire.E2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    E2_2__Product_max = questionnaire.E2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    E2_3__Unit_of_product = E2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    E2_4__Per_No__Kg_bodyweight_min = questionnaire.E2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    E2_5__Per_No__Kg_bodyweight_max = questionnaire.E2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E2_6__Duration_of_usage = questionnaire.E2_6__Duration_of_usage,
                    #endregion

                    #region D_3 Product preparation (dilution) _poultry_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    E3_1__Product_preparation__dilution__Product_amount = questionnaire.E3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    E3_2__Product_preparation_Unit_of_product = E3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    E3_3__Product_preparation_To_be_added_to__min_ = questionnaire.E3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    E3_4__Product_preparation_To_be_added_to__max_ = questionnaire.E3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    E3_5__Product_preparation_Unit_of_water_feed = questionnaire.E3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E3_6__Duration_of_usage = questionnaire.E3_6__Duration_of_usage,


                    #endregion

                    #region D_4 Guidelines referred to bodyweight_poultry_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    E4_1__Product_min = questionnaire.E4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    E4_2__Product_max = questionnaire.E4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    E4_3__Unit_of_product = E4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    E4_4__Per_No__Kg_bodyweight_min = questionnaire.E4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    E4_5__Per_No__Kg_bodyweight_max = questionnaire.E4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E4_6__Duration_of_usage = questionnaire.E4_6__Duration_of_usage,

                    #endregion


                    #endregion

                    //Phần này bao gồm các thông tin thêm về sản phẩm và người nhập dữ liệu để tiện cho việc kiểm tra tính xác thực của thông tin
                    #region F.Further information

                    /// <summary>
                    /// Điền trang web hoặc vetshop
                    /// </summary>
                    F_1__Source_of_information = questionnaire.F_1__Source_of_information,
                    /// <summary>
                    /// Ảnh sản phẩm
                    /// </summary>
                    F_2__Picture_of_product = questionnaire.F_2__Picture_of_product,
                    /// <summary>
                    /// Thông tin khác
                    /// </summary>
                    F3__Correction = questionnaire.F3__Correction,
                    /// <summary>
                    /// Thông tin người nhập
                    /// </summary>
                    F_4__Person_in_charge = questionnaire.F_4__Person_in_charge,
                    /// <summary>
                    /// thời gian cho việc tìm kiếm thu và nhập thông tin sản phẩm
                    /// </summary>
                    F_5__Working_time = questionnaire.F_5__Working_time,
                    /// <summary>
                    /// Ghi chú
                    /// </summary>
                    F_6__Note = questionnaire.F_6__Note,

                    #endregion

                    //Heo con
                    #region G.Piglet
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Piglet_1_1__Product_preparation__dilution__Product_amount = questionnaire.Piglet_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Piglet_1_2__Product_preparation_Unit_of_product = Piglet_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Piglet_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Piglet_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Piglet_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Piglet_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Piglet_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Piglet_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_1_6__Duration_of_usage = questionnaire.Piglet_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Piglet_2_1__Product_min = questionnaire.Piglet_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Piglet_2_2__Product_max = questionnaire.Piglet_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Piglet_2_3__Unit_of_product = Piglet_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Piglet_2_4__Per_No__Kg_bodyweight_min = questionnaire.Piglet_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Piglet_2_5__Per_No__Kg_bodyweight_max = questionnaire.Piglet_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_2_6__Duration_of_usage = questionnaire.Piglet_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Piglet_3_1__Product_preparation__dilution__Product_amount = questionnaire.Piglet_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Piglet_3_2__Product_preparation_Unit_of_product = Piglet_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Piglet_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Piglet_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Piglet_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Piglet_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Piglet_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Piglet_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_3_6__Duration_of_usage = questionnaire.Piglet_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Piglet_4_1__Product_min = questionnaire.Piglet_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Piglet_4_2__Product_max = questionnaire.Piglet_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Piglet_4_3__Unit_of_product = Piglet_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Piglet_4_4_Per_No__Kg_bodyweight_min = questionnaire.Piglet_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Piglet_4_5__Per_No__Kg_bodyweight_max = questionnaire.Piglet_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_4_6__Duration_of_usage = questionnaire.Piglet_4_6__Duration_of_usage,


                    #endregion

                    //Trâu
                    #region H.Buffalo
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Buffalo_1_1__Product_preparation__dilution__Product_amount = questionnaire.Buffalo_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Buffalo_1_2__Product_preparation_Unit_of_product = Buffalo_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Buffalo_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Buffalo_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Buffalo_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Buffalo_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Buffalo_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Buffalo_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_1_6__Duration_of_usage = questionnaire.Buffalo_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Buffalo_2_1__Product_min = questionnaire.Buffalo_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Buffalo_2_2__Product_max = questionnaire.Buffalo_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Buffalo_2_3__Unit_of_product = Buffalo_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Buffalo_2_4__Per_No__Kg_bodyweight_min = questionnaire.Buffalo_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Buffalo_2_5__Per_No__Kg_bodyweight_max = questionnaire.Buffalo_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_2_6__Duration_of_usage = questionnaire.Buffalo_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Buffalo_3_1__Product_preparation__dilution__Product_amount = questionnaire.Buffalo_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Buffalo_3_2__Product_preparation_Unit_of_product = Buffalo_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Buffalo_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Buffalo_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Buffalo_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Buffalo_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Buffalo_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Buffalo_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_3_6__Duration_of_usage = questionnaire.Buffalo_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Buffalo_4_1__Product_min = questionnaire.Buffalo_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Buffalo_4_2__Product_max = questionnaire.Buffalo_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Buffalo_4_3__Unit_of_product = Buffalo_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Buffalo_4_4_Per_No__Kg_bodyweight_min = questionnaire.Buffalo_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Buffalo_4_5__Per_No__Kg_bodyweight_max = questionnaire.Buffalo_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_4_6__Duration_of_usage = questionnaire.Buffalo_4_6__Duration_of_usage,



                    #endregion

                    //Gia súc
                    #region I.Cattle
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cattle_1_1__Product_preparation__dilution__Product_amount = questionnaire.Cattle_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cattle_1_2__Product_preparation_Unit_of_product = Cattle_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cattle_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Cattle_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cattle_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Cattle_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cattle_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Cattle_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_1_6__Duration_of_usages = questionnaire.Cattle_1_6__Duration_of_usages,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Cattle_2_1__Product_min = questionnaire.Cattle_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cattle_2_2__Product_max = questionnaire.Cattle_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cattle_2_3__Unit_of_product = Cattle_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cattle_2_4__Per_No__Kg_bodyweight_min = questionnaire.Cattle_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cattle_2_5__Per_No__Kg_bodyweight_max = questionnaire.Cattle_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_2_6__Duration_of_usage = questionnaire.Cattle_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cattle_3_1__Product_preparation__dilution__Product_amount = questionnaire.Cattle_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cattle_3_2__Product_preparation_Unit_of_product = Cattle_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cattle_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Cattle_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cattle_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Cattle_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cattle_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Cattle_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_3_6__Duration_of_usage = questionnaire.Cattle_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Cattle_4_1__Product_min = questionnaire.Cattle_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cattle_4_2__Product_max = questionnaire.Cattle_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cattle_4_3__Unit_of_product = Cattle_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cattle_4_4_Per_No__Kg_bodyweight_min = questionnaire.Cattle_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cattle_4_5__Per_No__Kg_bodyweight_max = questionnaire.Cattle_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_4_6__Duration_of_usage = questionnaire.Cattle_4_6__Duration_of_usage,


                    #endregion

                    //Dê
                    #region J.Goat
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goat_1_1__Product_preparation__dilution__Product_amount = questionnaire.Goat_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goat_1_2__Product_preparation_Unit_of_product = Goat_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goat_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Goat_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goat_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Goat_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goat_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Goat_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_1_6__Duration_of_usage = questionnaire.Goat_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Goat_2_1__Product_min = questionnaire.Goat_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goat_2_2__Product_max = questionnaire.Goat_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goat_2_3__Unit_of_product = Goat_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goat_2_4__Per_No__Kg_bodyweight_min = questionnaire.Goat_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goat_2_5__Per_No__Kg_bodyweight_max = questionnaire.Goat_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_2_6__Duration_of_usage = questionnaire.Goat_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goat_3_1__Product_preparation__dilution__Product_amount = questionnaire.Goat_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goat_3_2__Product_preparation_Unit_of_product = Goat_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goat_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Goat_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goat_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Goat_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goat_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Goat_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_3_6__Duration_of_usage = questionnaire.Goat_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Goat_4_1__Product_min = questionnaire.Goat_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goat_4_2__Product_max = questionnaire.Goat_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goat_4_3__Unit_of_product = Goat_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goat_4_4_Per_No__Kg_bodyweight_min = questionnaire.Goat_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goat_4_5__Per_No__Kg_bodyweight_max = questionnaire.Goat_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_4_6__Duration_of_usage = questionnaire.Goat_4_6__Duration_of_usage,

                    #endregion

                    //Cừu
                    #region K.Sheep
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Sheep_1_1__Product_preparation__dilution__Product_amount = questionnaire.Sheep_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Sheep_1_2__Product_preparation_Unit_of_product = Sheep_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Sheep_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Sheep_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Sheep_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Sheep_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Sheep_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Sheep_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_1_6__Duration_of_usage = questionnaire.Sheep_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Sheep_2_1__Product_min = questionnaire.Sheep_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Sheep_2_2__Product_max = questionnaire.Sheep_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Sheep_2_3__Unit_of_product = Sheep_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Sheep_2_4__Per_No__Kg_bodyweight_min = questionnaire.Sheep_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Sheep_2_5__Per_No__Kg_bodyweight_max = questionnaire.Sheep_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_2_6__Duration_of_usage = questionnaire.Sheep_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Sheep_3_1__Product_preparation__dilution__Product_amount = questionnaire.Sheep_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Sheep_3_2__Product_preparation_Unit_of_product = Sheep_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Sheep_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Sheep_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Sheep_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Sheep_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Sheep_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Sheep_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_3_6__Duration_of_usage = questionnaire.Sheep_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Sheep_4_1__Product_min = questionnaire.Sheep_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Sheep_4_2__Product_max = questionnaire.Sheep_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Sheep_4_3__Unit_of_product = Sheep_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Sheep_4_4_Per_No__Kg_bodyweight_min = questionnaire.Sheep_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Sheep_4_5__Per_No__Kg_bodyweight_max = questionnaire.Sheep_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_4_6__Duration_of_usage = questionnaire.Sheep_4_6__Duration_of_usage,

                    #endregion

                    //Ngựa
                    #region L.Horse
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Horse_1_1__Product_preparation__dilution__Product_amount = questionnaire.Horse_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Horse_1_2__Product_preparation_Unit_of_product = Horse_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Horse_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Horse_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Horse_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Horse_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Horse_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Horse_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_1_6__Duration_of_usage = questionnaire.Horse_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Horse_2_1__Product_min = questionnaire.Horse_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Horse_2_2__Product_max = questionnaire.Horse_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Horse_2_3__Unit_of_product = Horse_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Horse_2_4__Per_No__Kg_bodyweight_min = questionnaire.Horse_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Horse_2_5__Per_No__Kg_bodyweight_max = questionnaire.Horse_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_2_6__Duration_of_usage = questionnaire.Horse_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Horse_3_1__Product_preparation__dilution__Product_amount = questionnaire.Horse_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Horse_3_2__Product_preparation_Unit_of_product = Horse_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Horse_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Horse_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Horse_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Horse_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Horse_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Horse_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_3_6__Duration_of_usage = questionnaire.Horse_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Horse_4_1__Product_min = questionnaire.Horse_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Horse_4_2__Product_max = questionnaire.Horse_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Horse_4_3__Unit_of_product = Horse_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Horse_4_4_Per_No__Kg_bodyweight_min = questionnaire.Horse_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Horse_4_5__Per_No__Kg_bodyweight_max = questionnaire.Horse_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_4_6__Duration_of_usage = questionnaire.Horse_4_6__Duration_of_usage,

                    #endregion

                    //Gà
                    #region M.Chicken
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chicken_1_1__Product_preparation__dilution__Product_amount = questionnaire.Chicken_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chicken_1_2__Product_preparation_Unit_of_product = Chicken_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chicken_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Chicken_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chicken_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Chicken_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chicken_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Chicken_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_1_6__Duration_of_usage = questionnaire.Chicken_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Chicken_2_1__Product_min = questionnaire.Chicken_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chicken_2_2__Product_max = questionnaire.Chicken_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chicken_2_3__Unit_of_product = Chicken_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chicken_2_4__Per_No__Kg_bodyweight_min = questionnaire.Chicken_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chicken_2_5__Per_No__Kg_bodyweight_max = questionnaire.Chicken_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_2_6__Duration_of_usage = questionnaire.Chicken_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chicken_3_1__Product_preparation__dilution__Product_amount = questionnaire.Chicken_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chicken_3_2__Product_preparation_Unit_of_product = Chicken_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chicken_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Chicken_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chicken_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Chicken_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chicken_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Chicken_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_3_6__Duration_of_usage = questionnaire.Chicken_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Chicken_4_1__Product_min = questionnaire.Chicken_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chicken_4_2__Product_max = questionnaire.Chicken_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chicken_4_3__Unit_of_product = Chicken_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chicken_4_4_Per_No__Kg_bodyweight_min = questionnaire.Chicken_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chicken_4_5__Per_No__Kg_bodyweight_max = questionnaire.Chicken_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_4_6__Duration_of_usage = questionnaire.Chicken_4_6__Duration_of_usage,

                    #endregion

                    //Vịt
                    #region N.Duck
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Duck_1_1__Product_preparation__dilution__Product_amount = questionnaire.Duck_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Duck_1_2__Product_preparation_Unit_of_product = Duck_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Duck_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Duck_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Duck_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Duck_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Duck_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Duck_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_1_6__Duration_of_usage = questionnaire.Duck_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Duck_2_1__Product_min = questionnaire.Duck_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Duck_2_2__Product_max = questionnaire.Duck_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Duck_2_3__Unit_of_product = Duck_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Duck_2_4__Per_No__Kg_bodyweight_min = questionnaire.Duck_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Duck_2_5__Per_No__Kg_bodyweight_max = questionnaire.Duck_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_2_6__Duration_of_usage = questionnaire.Duck_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Duck_3_1__Product_preparation__dilution__Product_amount = questionnaire.Duck_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Duck_3_2__Product_preparation_Unit_of_product = Duck_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Duck_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Duck_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Duck_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Duck_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Duck_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Duck_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_3_6__Duration_of_usage = questionnaire.Duck_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Duck_4_1__Product_min = questionnaire.Duck_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Duck_4_2__Product_max = questionnaire.Duck_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Duck_4_3__Unit_of_product = Duck_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Duck_4_4_Per_No__Kg_bodyweight_min = questionnaire.Duck_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Duck_4_5__Per_No__Kg_bodyweight_max = questionnaire.Duck_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_4_6__Duration_of_usage = questionnaire.Duck_4_6__Duration_of_usage,

                    #endregion

                    //Vịt Xiêm
                    #region O.Muscovy_Duck
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount = questionnaire.Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Muscovy_Duck_1_2__Product_preparation_Unit_of_product = Muscovy_Duck_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_1_6__Duration_of_usage = questionnaire.Muscovy_Duck_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Muscovy_Duck_2_1__Product_min = questionnaire.Muscovy_Duck_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Muscovy_Duck_2_2__Product_max = questionnaire.Muscovy_Duck_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Muscovy_Duck_2_3__Unit_of_product = Muscovy_Duck_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min = questionnaire.Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max = questionnaire.Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_2_6__Duration_of_usage = questionnaire.Muscovy_Duck_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount = questionnaire.Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Muscovy_Duck_3_2__Product_preparation_Unit_of_product = Muscovy_Duck_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_3_6__Duration_of_usage = questionnaire.Muscovy_Duck_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Muscovy_Duck_4_1__Product_min = questionnaire.Muscovy_Duck_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Muscovy_Duck_4_2__Product_max = questionnaire.Muscovy_Duck_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Muscovy_Duck_4_3__Unit_of_product = Muscovy_Duck_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min = questionnaire.Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max = questionnaire.Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_4_6__Duration_of_usage = questionnaire.Muscovy_Duck_4_6__Duration_of_usage,

                    #endregion

                    //Cút
                    #region P.Quail
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Quail_1_1__Product_preparation__dilution__Product_amount = questionnaire.Quail_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Quail_1_2__Product_preparation_Unit_of_product = Quail_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Quail_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Quail_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Quail_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Quail_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Quail_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Quail_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_1_6__Duration_of_usage = questionnaire.Quail_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Quail_2_1__Product_min = questionnaire.Quail_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Quail_2_2__Product_max = questionnaire.Quail_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Quail_2_3__Unit_of_product = Quail_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Quail_2_4__Per_No__Kg_bodyweight_min = questionnaire.Quail_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Quail_2_5__Per_No__Kg_bodyweight_max = questionnaire.Quail_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_2_6__Duration_of_usage = questionnaire.Quail_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Quail_3_1__Product_preparation__dilution__Product_amount = questionnaire.Quail_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Quail_3_2__Product_preparation_Unit_of_product = Quail_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Quail_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Quail_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Quail_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Quail_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Quail_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Quail_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_3_6__Duration_of_usage = questionnaire.Quail_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Quail_4_1__Product_min = questionnaire.Quail_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Quail_4_2__Product_max = questionnaire.Quail_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Quail_4_3__Unit_of_product = Quail_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Quail_4_4_Per_No__Kg_bodyweight_min = questionnaire.Quail_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Quail_4_5__Per_No__Kg_bodyweight_max = questionnaire.Quail_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_4_6__Duration_of_usage = questionnaire.Quail_4_6__Duration_of_usage,

                    #endregion

                    //Ngỗng
                    #region Q.Goose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goose_1_1__Product_preparation__dilution__Product_amount = questionnaire.Goose_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goose_1_2__Product_preparation_Unit_of_product = Goose_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goose_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Goose_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goose_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Goose_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goose_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Goose_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_1_6__Duration_of_usage = questionnaire.Goose_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Goose_2_1__Product_min = questionnaire.Goose_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goose_2_2__Product_max = questionnaire.Goose_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goose_2_3__Unit_of_product = Goose_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goose_2_4__Per_No__Kg_bodyweight_min = questionnaire.Goose_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goose_2_5__Per_No__Kg_bodyweight_max = questionnaire.Goose_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_2_6__Duration_of_usage = questionnaire.Goose_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goose_3_1__Product_preparation__dilution__Product_amount = questionnaire.Goose_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goose_3_2__Product_preparation_Unit_of_product = Goose_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goose_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Goose_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goose_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Goose_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goose_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Goose_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_3_6__Duration_of_usage = questionnaire.Goose_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Goose_4_1__Product_min = questionnaire.Goose_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goose_4_2__Product_max = questionnaire.Goose_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goose_4_3__Unit_of_product = Goose_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goose_4_4_Per_No__Kg_bodyweight_min = questionnaire.Goose_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goose_4_5__Per_No__Kg_bodyweight_max = questionnaire.Goose_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_4_6__Duration_of_usage = questionnaire.Goose_4_6__Duration_of_usage,

                    #endregion

                    //Chó
                    #region R.Dog
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Dog_1_1__Product_preparation__dilution__Product_amount = questionnaire.Dog_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Dog_1_2__Product_preparation_Unit_of_product = Dog_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Dog_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Dog_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Dog_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Dog_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Dog_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Dog_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_1_6__Duration_of_usage = questionnaire.Dog_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Dog_2_1__Product_min = questionnaire.Dog_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Dog_2_2__Product_max = questionnaire.Dog_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Dog_2_3__Unit_of_product = Dog_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Dog_2_4__Per_No__Kg_bodyweight_min = questionnaire.Dog_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Dog_2_5__Per_No__Kg_bodyweight_max = questionnaire.Dog_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_2_6__Duration_of_usage = questionnaire.Dog_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Dog_3_1__Product_preparation__dilution__Product_amount = questionnaire.Dog_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Dog_3_2__Product_preparation_Unit_of_product = Dog_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Dog_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Dog_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Dog_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Dog_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Dog_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Dog_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_3_6__Duration_of_usage = questionnaire.Dog_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Dog_4_1__Product_min = questionnaire.Dog_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Dog_4_2__Product_max = questionnaire.Dog_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Dog_4_3__Unit_of_product = Dog_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Dog_4_4_Per_No__Kg_bodyweight_min = questionnaire.Dog_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Dog_4_5__Per_No__Kg_bodyweight_max = questionnaire.Dog_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_4_6__Duration_of_usage = questionnaire.Dog_4_6__Duration_of_usage,

                    #endregion

                    //Mèo
                    #region S.Cat
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cat_1_1__Product_preparation__dilution__Product_amount = questionnaire.Cat_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cat_1_2__Product_preparation_Unit_of_product = Cat_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cat_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Cat_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cat_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Cat_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cat_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Cat_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_1_6__Duration_of_usage = questionnaire.Cat_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Cat_2_1__Product_min = questionnaire.Cat_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cat_2_2__Product_max = questionnaire.Cat_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cat_2_3__Unit_of_product = Cat_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cat_2_4__Per_No__Kg_bodyweight_min = questionnaire.Cat_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cat_2_5__Per_No__Kg_bodyweight_max = questionnaire.Cat_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_2_6__Duration_of_usage = questionnaire.Cat_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cat_3_1__Product_preparation__dilution__Product_amount = questionnaire.Cat_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cat_3_2__Product_preparation_Unit_of_product = Cat_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cat_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Cat_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cat_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Cat_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cat_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Cat_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_3_6__Duration_of_usage = questionnaire.Cat_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Cat_4_1__Product_min = questionnaire.Cat_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cat_4_2__Product_max = questionnaire.Cat_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cat_4_3__Unit_of_product = Cat_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cat_4_4_Per_No__Kg_bodyweight_min = questionnaire.Cat_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cat_4_5__Per_No__Kg_bodyweight_max = questionnaire.Cat_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_4_6__Duration_of_usage = questionnaire.Cat_4_6__Duration_of_usage,

                    #endregion

                    //Bê
                    #region T.Calf
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Calf_1_1__Product_preparation__dilution__Product_amount = questionnaire.Calf_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Calf_1_2__Product_preparation_Unit_of_product = Calf_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Calf_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Calf_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Calf_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Calf_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Calf_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Calf_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_1_6__Duration_of_usage = questionnaire.Calf_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Calf_2_1__Product_min = questionnaire.Calf_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Calf_2_2__Product_max = questionnaire.Calf_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Calf_2_3__Unit_of_product = Calf_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Calf_2_4__Per_No__Kg_bodyweight_min = questionnaire.Calf_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Calf_2_5__Per_No__Kg_bodyweight_max = questionnaire.Calf_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_2_6__Duration_of_usage = questionnaire.Calf_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Calf_3_1__Product_preparation__dilution__Product_amount = questionnaire.Calf_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Calf_3_2__Product_preparation_Unit_of_product = Calf_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Calf_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Calf_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Calf_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Calf_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Calf_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Calf_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_3_6__Duration_of_usage = questionnaire.Calf_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Calf_4_1__Product_min = questionnaire.Calf_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Calf_4_2__Product_max = questionnaire.Calf_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Calf_4_3__Unit_of_product = Calf_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Calf_4_4_Per_No__Kg_bodyweight_min = questionnaire.Calf_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Calf_4_5__Per_No__Kg_bodyweight_max = questionnaire.Calf_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_4_6__Duration_of_usage = questionnaire.Calf_4_6__Duration_of_usage,

                    #endregion

                    //Gà con
                    #region U.Chick
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chick_1_1__Product_preparation__dilution__Product_amount = questionnaire.Chick_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chick_1_2__Product_preparation_Unit_of_product = Chick_1_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chick_1_3__Product_preparation_To_be_added_to__min_ = questionnaire.Chick_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chick_1_4__Product_preparation_To_be_added_to__max_ = questionnaire.Chick_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chick_1_5__Product_preparation_Unit_of_water_feed = questionnaire.Chick_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_1_6__Duration_of_usage = questionnaire.Chick_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Chick_2_1__Product_min = questionnaire.Chick_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chick_2_2__Product_max = questionnaire.Chick_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chick_2_3__Unit_of_product = Chick_2_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chick_2_4__Per_No__Kg_bodyweight_min = questionnaire.Chick_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chick_2_5__Per_No__Kg_bodyweight_max = questionnaire.Chick_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_2_6__Duration_of_usage = questionnaire.Chick_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chick_3_1__Product_preparation__dilution__Product_amount = questionnaire.Chick_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chick_3_2__Product_preparation_Unit_of_product = Chick_3_2__Product_preparation_Unit_of_product,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chick_3_3__Product_preparation_To_be_added_to__min_ = questionnaire.Chick_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chick_3_4__Product_preparation_To_be_added_to__max_ = questionnaire.Chick_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chick_3_5__Product_preparation_Unit_of_water_feed = questionnaire.Chick_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_3_6__Duration_of_usage = questionnaire.Chick_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Chick_4_1__Product_min = questionnaire.Chick_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chick_4_2__Product_max = questionnaire.Chick_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chick_4_3__Unit_of_product = Chick_4_3__Unit_of_product,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chick_4_4_Per_No__Kg_bodyweight_min = questionnaire.Chick_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chick_4_5__Per_No__Kg_bodyweight_max = questionnaire.Chick_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_4_6__Duration_of_usage = questionnaire.Chick_4_6__Duration_of_usage,

                    #endregion

                    State = state
                };
                #endregion
                #region OldObject
                var oldobject = new QuestionnaireViewModel
                {
                    #region A.General information
                    /// <summary>
                    /// Ngày thực hiện
                    /// </summary>
                    D_u_th_i_gian = "",
                    /// <summary>
                    /// Xuất xứ 
                    /// </summary>
                    A1__Product_origin = "",
                    /// <summary>
                    /// Mã code sản phẩm
                    /// </summary>
                    A2__Product_code = "",
                    /// <summary>
                    /// Tên sản phẩm
                    /// </summary>
                    A3__Product_name = "",
                    /// <summary>
                    /// Tên công ty đăng ký sản phẩm
                    /// </summary>
                    A4__Company = "",
                    /// <summary>
                    /// Loại sản phẩm ( dang bột - dạng dung dịch )
                    /// </summary>
                    A5__Type_of_product = "",
                    /// <summary>
                    /// Có chứa chất ngoài kháng sinh hay không?
                    /// </summary>
                    A6__Other_subtance_in_product = "",
                    /// <summary>
                    /// Khối lượng/trọng lượng/thể tích của sản phẩm 
                    /// </summary>
                    A7__Volume_of_product = "",
                    /// <summary>
                    /// Đơn vị của khối lượng/trọng lượng/thể tích của sản phẩm 
                    /// </summary>
                    A8__Unit_of_volume_of_product = "",
                    /// <summary>
                    /// Thông tin về khối lượng khác 
                    /// </summary>
                    A9__Other_volume_of_product = "",
                    #endregion

                    #region B.Information related to antimicrobial
                    /// <summary>
                    /// Số loại kháng sinh co trong sản phẩm
                    /// </summary>
                    B1__Number_of_antimicrobials_in_product = 0,

                    #region Antimicrobials 1
                    /// <summary>
                    /// Antimicrobials_1
                    /// </summary>
                    B2_1__Antimicrobial_1 = "",
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 1 - chỉ điền số
                    /// </summary>
                    B2_2__Strength_of_antimicrobial_1 = 0,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 1
                    /// </summary>
                    B2_3__Units_of_antimicrobial_1 = "",
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B2_4__Per_amount_of_product__antimicrobial_1_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = "",
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_7__Units_of_product__link_to_question_B2_4_ = "",
                    #endregion
                    #region Antimicrobials 2
                    /// <summary>
                    /// Antimicrobials_2
                    /// </summary>
                    B3_1__Antimicrobial_2 = "",
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 2 - chỉ điền số
                    /// </summary>
                    B3_2__Strength_of_antimicrobial_2 = 0,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 2
                    /// </summary>
                    B3_3__Units_of_antimicrobial_2 = "",
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B3_4__Per_amount_of_product__antimicrobial_2_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = "",
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_7__Units_of_product__link_to_question_B3_4_ = "",
                    #endregion
                    #region Antimicrobials 3
                    /// <summary>
                    /// Antimicrobials_3
                    /// </summary>
                    B4_1__Antimicrobial_3 = "",
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 3 - chỉ điền số
                    /// </summary>
                    B4_2__Strength_of_antimicrobial_3 = 0,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 3
                    /// </summary>
                    B4_3__Units_of_antimicrobial_3 = "",
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B4_4__Per_amount_of_product__antimicrobial_3_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = "",
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_7__Units_of_product__link_to_question_B4_4_ = "",
                    #endregion
                    #region Antimicrobials 4
                    /// <summary>
                    /// Antimicrobials_4
                    /// </summary>
                    B5_1__Antimicrobial_4 = "",
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 4 - chỉ điền số
                    /// </summary>
                    B5_2__Strength_of_antimicrobial_4 = 0,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 4
                    /// </summary>
                    B5_3__Units_of_antimicrobial_4 = "",
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B5_4__Per_amount_of_product__antimicrobial_4_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = "",
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ = "",
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_7__Units_of_product__link_to_question_B5_4_ = "",
                    #endregion

                    /// <summary>
                    /// Các loài vật
                    /// </summary>
                    B6__Target_species_x = "",
                    /// <summary>
                    /// Đường dùng thuốc 
                    /// </summary>
                    B7__Administration_route = "",
                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho heo
                    #region C_ Heo

                    #region C_1_ Product preparation (dilution) _pig_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    C1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    C1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    C1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    C1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    C1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C1_6__Duration_of_usage__min__max_ = "",
                    #endregion

                    #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    C2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    C2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    C2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    C2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    C2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C2_6__Duration_of_usage = "",
                    #endregion

                    #region C_3 Product preparation (dilution) _pig_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    C3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    C3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    C3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    C3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    C3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C3_6__Duration_of_usage = "",


                    #endregion

                    #region C_4 Guidelines referred to bodyweight_pig_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    C4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    C4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    C4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    C4_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    C4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C4_6__Duration_of_usage = "",
                    #endregion

                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho thú nhai lại không phân biệt thú lớn và nhỏ
                    #region D. Động vật nhai lại

                    #region D_1_ Product preparation (dilution) _ruminant_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    D1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    D1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    D1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    D1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    D1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D1_6__Duration_of_usage = "",

                    #endregion

                    #region D.2. Guidelines referred to bodyweight_ruminant_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    D2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    D2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    D2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    D2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    D2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D2_6__Duration_of_usage = "",
                    #endregion

                    #region D.3 Product preparation (dilution) _cattle_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    D3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    D3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    D3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    D3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    D3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D3_6__Duration_of_usage = "",


                    #endregion

                    #region D_4 Guidelines referred to bodyweight_ruminant_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    D4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    D4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>  
                    D4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    D4_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    D4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D4_6__Duration_of_usage = "",
                    #endregion

                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho gia cầm nói chung bao gồm gà, vịt, ngan, ngỗng, cút
                    #region E. Gia cầm

                    #region E.1 Product preparation (dilution) _poultry_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    E1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    E1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    E1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    E1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    E1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E1_6__Duration_of_usage = "",



                    #endregion

                    #region E.2 Guidelines referred to bodyweight_poultry_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>           
                    E2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    E2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    E2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    E2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    E2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E2_6__Duration_of_usage = "",
                    #endregion

                    #region E.3 Product preparation (dilution) _poultry_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    E3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    E3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    E3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    E3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    E3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E3_6__Duration_of_usage = "",



                    #endregion

                    #region E.4 Guidelines referred to bodyweight_poultry_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>           
                    E4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    E4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    E4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    E4_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    E4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E4_6__Duration_of_usage = "",


                    #endregion


                    #endregion

                    //Phần này bao gồm các thông tin thêm về sản phẩm và người nhập dữ liệu để tiện cho việc kiểm tra tính xác thực của thông tin
                    #region F.Further information

                    /// <summary>
                    /// Điền trang web hoặc vetshop
                    /// </summary>
                    F_1__Source_of_information = "",
                    /// <summary>
                    /// Ảnh sản phẩm
                    /// </summary>
                    F_2__Picture_of_product = "",
                    /// <summary>
                    /// Thông tin khác
                    /// </summary>
                    F3__Correction = "",
                    /// <summary>
                    /// Thông tin người nhập
                    /// </summary>
                    F_4__Person_in_charge = "",
                    /// <summary>
                    /// thời gian cho việc tìm kiếm thu và nhập thông tin sản phẩm
                    /// </summary>
                    F_5__Working_time = "",
                    /// <summary>
                    /// Ghi chú
                    /// </summary>
                    F_6__Note = "",

                    #endregion

                    //Heo con
                    #region G.Piglet
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Piglet_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Piglet_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Piglet_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Piglet_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Piglet_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Piglet_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Piglet_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Piglet_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Piglet_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Piglet_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Piglet_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Piglet_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Piglet_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Piglet_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Piglet_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Piglet_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Piglet_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Piglet_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Piglet_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Piglet_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_4_6__Duration_of_usage = "",


                    #endregion

                    //Trâu
                    #region H.Buffalo
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Buffalo_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Buffalo_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Buffalo_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Buffalo_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Buffalo_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Buffalo_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Buffalo_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Buffalo_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Buffalo_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Buffalo_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Buffalo_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Buffalo_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Buffalo_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Buffalo_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Buffalo_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Buffalo_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Buffalo_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Buffalo_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Buffalo_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Buffalo_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_4_6__Duration_of_usage = "",


                    #endregion

                    //Gia súc
                    #region I.Cattle
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cattle_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cattle_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cattle_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cattle_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cattle_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_1_6__Duration_of_usages = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Cattle_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cattle_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cattle_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cattle_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cattle_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cattle_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cattle_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cattle_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cattle_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cattle_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Cattle_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cattle_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cattle_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cattle_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cattle_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_4_6__Duration_of_usage = "",


                    #endregion

                    //Dê
                    #region J.Goat
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goat_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goat_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goat_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goat_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goat_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Goat_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goat_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goat_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goat_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goat_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goat_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goat_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goat_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goat_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goat_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Goat_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goat_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goat_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goat_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goat_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_4_6__Duration_of_usage = "",
                    #endregion

                    //Dê
                    #region K.Sheep
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Sheep_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Sheep_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Sheep_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Sheep_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Sheep_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Sheep_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Sheep_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Sheep_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Sheep_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Sheep_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Sheep_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Sheep_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Sheep_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Sheep_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Sheep_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Sheep_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Sheep_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Sheep_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Sheep_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Sheep_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_4_6__Duration_of_usage = "",
                    #endregion

                    //Ngựa
                    #region L.Horse
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Horse_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Horse_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Horse_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Horse_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Horse_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Horse_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Horse_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Horse_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Horse_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Horse_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Horse_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Horse_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Horse_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Horse_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Horse_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Horse_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Horse_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Horse_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Horse_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Horse_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_4_6__Duration_of_usage = "",
                    #endregion

                    //Gà
                    #region M.Chicken
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chicken_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chicken_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chicken_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chicken_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chicken_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Chicken_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chicken_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chicken_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chicken_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chicken_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chicken_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chicken_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chicken_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chicken_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chicken_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Chicken_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chicken_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chicken_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chicken_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chicken_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_4_6__Duration_of_usage = "",
                    #endregion

                    //Vịt
                    #region N.Duck
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Duck_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Duck_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Duck_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Duck_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Duck_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Duck_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Duck_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Duck_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Duck_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Duck_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Duck_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Duck_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Duck_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Duck_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Duck_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Duck_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Duck_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Duck_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Duck_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Duck_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_4_6__Duration_of_usage = "",
                    #endregion

                    //Vịt Xiêm
                    #region O.Muscovy_Duck
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Muscovy_Duck_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Muscovy_Duck_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Muscovy_Duck_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Muscovy_Duck_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Muscovy_Duck_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Muscovy_Duck_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Muscovy_Duck_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Muscovy_Duck_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_4_6__Duration_of_usage = "",
                    #endregion

                    //Cút
                    #region P.Quail
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Quail_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Quail_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Quail_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Quail_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Quail_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Quail_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Quail_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Quail_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Quail_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Quail_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Quail_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Quail_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Quail_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Quail_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Quail_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Quail_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Quail_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Quail_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Quail_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Quail_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_4_6__Duration_of_usage = "",
                    #endregion

                    //Ngỗng
                    #region Q.Goose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goose_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goose_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goose_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goose_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goose_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Goose_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goose_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goose_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goose_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goose_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goose_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goose_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goose_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goose_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goose_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Goose_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goose_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goose_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goose_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goose_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_4_6__Duration_of_usage = "",
                    #endregion

                    //Chó
                    #region R.Dog
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Dog_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Dog_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Dog_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Dog_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Dog_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Dog_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Dog_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Dog_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Dog_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Dog_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Dog_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Dog_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Dog_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Dog_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Dog_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Dog_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Dog_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Dog_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Dog_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Dog_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_4_6__Duration_of_usage = "",
                    #endregion

                    //Mèo
                    #region S.Cat
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cat_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cat_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cat_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cat_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cat_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Cat_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cat_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cat_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cat_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cat_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cat_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cat_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cat_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cat_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cat_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Cat_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cat_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cat_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cat_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cat_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_4_6__Duration_of_usage = "",
                    #endregion

                    //Bê
                    #region T.Calf
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Calf_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Calf_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Calf_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Calf_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Calf_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Calf_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Calf_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Calf_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Calf_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Calf_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Calf_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Calf_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Calf_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Calf_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Calf_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Calf_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Calf_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Calf_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Calf_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Calf_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_4_6__Duration_of_usage = "",
                    #endregion

                    //Gà con
                    #region U.Chick
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chick_1_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chick_1_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chick_1_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chick_1_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chick_1_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_1_6__Duration_of_usage = "",


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Chick_2_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chick_2_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chick_2_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chick_2_4__Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chick_2_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_2_6__Duration_of_usage = "",


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chick_3_1__Product_preparation__dilution__Product_amount = "",
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chick_3_2__Product_preparation_Unit_of_product = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chick_3_3__Product_preparation_To_be_added_to__min_ = "",
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chick_3_4__Product_preparation_To_be_added_to__max_ = "",
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chick_3_5__Product_preparation_Unit_of_water_feed = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_3_6__Duration_of_usage = "",



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Chick_4_1__Product_min = "",
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chick_4_2__Product_max = "",
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chick_4_3__Unit_of_product = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chick_4_4_Per_No__Kg_bodyweight_min = "",
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chick_4_5__Per_No__Kg_bodyweight_max = "",
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_4_6__Duration_of_usage = "",
                    #endregion

                    State = ""
                };
                #endregion
                auditTrailBussiness.CreateAuditTrail(AuditActionType.Create, questionnaire.Id, "Dữ liệu", oldobject, newobject, user.Username);
                #endregion
                return Json(new { success = true, message = "Lưu dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [PermissionBasedAuthorize("DATA_INPUT")]
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
            Questionnaire questionnaire = unitWork.Questionnaire.Get(r => r.Id == id && r.CreateBy.Id == user.Id).FirstOrDefault();
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
            if (questionnaire.B7__Administration_route != "" && questionnaire.B7__Administration_route != null)
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
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            return View(questionnaire);
        }

        [HttpPost]
        [PermissionBasedAuthorize("DATA_INPUT")]
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
            #region Auditrail Oldobject
            #region Enum ToString
            var state = oldquestionnaire.State.ToString();
			try
			{
				if (questionnaire.A9__Other_volume_of_product == null)
				{
					questionnaire.A9__Other_volume_of_product = "";
				}
			}
			catch { }
			var A1__Product_origin = oldquestionnaire.A1__Product_origin.ToString();
            var A5__Type_of_product = oldquestionnaire.A5__Type_of_product.ToString();
            var A6__Other_subtance_in_product = oldquestionnaire.A6__Other_subtance_in_product.ToString();
            var A8__Unit_of_volume_of_product = oldquestionnaire.A8__Unit_of_volume_of_product.ToString();
            var B2_3__Units_of_antimicrobial_1 = oldquestionnaire.B2_3__Units_of_antimicrobial_1.ToString();
            var B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = oldquestionnaire.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_.ToString();
            var B2_7__Units_of_product__link_to_question_B2_4_ = oldquestionnaire.B2_7__Units_of_product__link_to_question_B2_4_.ToString();
            var B3_3__Units_of_antimicrobial_2 = oldquestionnaire.B3_3__Units_of_antimicrobial_2.ToString();
            var B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = oldquestionnaire.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_.ToString();
            var B3_7__Units_of_product__link_to_question_B3_4_ = oldquestionnaire.B3_7__Units_of_product__link_to_question_B3_4_.ToString();
            var B4_3__Units_of_antimicrobial_3 = oldquestionnaire.B4_3__Units_of_antimicrobial_3.ToString();
            var B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = oldquestionnaire.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_.ToString();
            var B4_7__Units_of_product__link_to_question_B4_4_ = oldquestionnaire.B4_7__Units_of_product__link_to_question_B4_4_.ToString();
            var B5_3__Units_of_antimicrobial_4 = oldquestionnaire.B5_3__Units_of_antimicrobial_4.ToString();
            var B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = oldquestionnaire.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_.ToString();
            var B5_7__Units_of_product__link_to_question_B5_4_ = oldquestionnaire.B5_7__Units_of_product__link_to_question_B5_4_.ToString();

            var C1_2__Product_preparation_Unit_of_product = oldquestionnaire.C1_2__Product_preparation_Unit_of_product.ToString();
            var C2_3__Unit_of_product = oldquestionnaire.C2_3__Unit_of_product.ToString();
            var C3_2__Product_preparation_Unit_of_product = oldquestionnaire.C3_2__Product_preparation_Unit_of_product.ToString();
            var C4_3__Unit_of_product = oldquestionnaire.C4_3__Unit_of_product.ToString();

            var D1_2__Product_preparation_Unit_of_product = oldquestionnaire.D1_2__Product_preparation_Unit_of_product.ToString();
            var D2_3__Unit_of_product = oldquestionnaire.D2_3__Unit_of_product.ToString();
            var D3_2__Product_preparation_Unit_of_product = oldquestionnaire.D3_2__Product_preparation_Unit_of_product.ToString();
            var D4_3__Unit_of_product = oldquestionnaire.D4_3__Unit_of_product.ToString();

            var E1_2__Product_preparation_Unit_of_product = oldquestionnaire.E1_2__Product_preparation_Unit_of_product.ToString();
            var E2_3__Unit_of_product = oldquestionnaire.E2_3__Unit_of_product.ToString();
            var E3_2__Product_preparation_Unit_of_product = oldquestionnaire.E3_2__Product_preparation_Unit_of_product.ToString();
            var E4_3__Unit_of_product = oldquestionnaire.E4_3__Unit_of_product.ToString();

            var Piglet_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Piglet_1_2__Product_preparation_Unit_of_product.ToString();
            var Piglet_2_3__Unit_of_product = oldquestionnaire.Piglet_2_3__Unit_of_product.ToString();
            var Piglet_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Piglet_3_2__Product_preparation_Unit_of_product.ToString();
            var Piglet_4_3__Unit_of_product = oldquestionnaire.Piglet_4_3__Unit_of_product.ToString();

            var Buffalo_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Buffalo_1_2__Product_preparation_Unit_of_product.ToString();
            var Buffalo_2_3__Unit_of_product = oldquestionnaire.Buffalo_2_3__Unit_of_product.ToString();
            var Buffalo_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Buffalo_3_2__Product_preparation_Unit_of_product.ToString();
            var Buffalo_4_3__Unit_of_product = oldquestionnaire.Buffalo_4_3__Unit_of_product.ToString();

            var Cattle_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Cattle_1_2__Product_preparation_Unit_of_product.ToString();
            var Cattle_2_3__Unit_of_product = oldquestionnaire.Cattle_2_3__Unit_of_product.ToString();
            var Cattle_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Cattle_3_2__Product_preparation_Unit_of_product.ToString();
            var Cattle_4_3__Unit_of_product = oldquestionnaire.Cattle_4_3__Unit_of_product.ToString();

            var Goat_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Goat_1_2__Product_preparation_Unit_of_product.ToString();
            var Goat_2_3__Unit_of_product = oldquestionnaire.Goat_2_3__Unit_of_product.ToString();
            var Goat_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Goat_3_2__Product_preparation_Unit_of_product.ToString();
            var Goat_4_3__Unit_of_product = oldquestionnaire.Goat_4_3__Unit_of_product.ToString();

            var Sheep_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Sheep_1_2__Product_preparation_Unit_of_product.ToString();
            var Sheep_2_3__Unit_of_product = oldquestionnaire.Sheep_2_3__Unit_of_product.ToString();
            var Sheep_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Sheep_3_2__Product_preparation_Unit_of_product.ToString();
            var Sheep_4_3__Unit_of_product = oldquestionnaire.Sheep_4_3__Unit_of_product.ToString();

            var Horse_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Horse_1_2__Product_preparation_Unit_of_product.ToString();
            var Horse_2_3__Unit_of_product = oldquestionnaire.Horse_2_3__Unit_of_product.ToString();
            var Horse_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Horse_3_2__Product_preparation_Unit_of_product.ToString();
            var Horse_4_3__Unit_of_product = oldquestionnaire.Horse_4_3__Unit_of_product.ToString();

            var Chicken_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Chicken_1_2__Product_preparation_Unit_of_product.ToString();
            var Chicken_2_3__Unit_of_product = oldquestionnaire.Chicken_2_3__Unit_of_product.ToString();
            var Chicken_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Chicken_3_2__Product_preparation_Unit_of_product.ToString();
            var Chicken_4_3__Unit_of_product = oldquestionnaire.Chicken_4_3__Unit_of_product.ToString();

            var Duck_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Duck_1_2__Product_preparation_Unit_of_product.ToString();
            var Duck_2_3__Unit_of_product = oldquestionnaire.Duck_2_3__Unit_of_product.ToString();
            var Duck_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Duck_3_2__Product_preparation_Unit_of_product.ToString();
            var Duck_4_3__Unit_of_product = oldquestionnaire.Duck_4_3__Unit_of_product.ToString();

            var Muscovy_Duck_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Muscovy_Duck_1_2__Product_preparation_Unit_of_product.ToString();
            var Muscovy_Duck_2_3__Unit_of_product = oldquestionnaire.Muscovy_Duck_2_3__Unit_of_product.ToString();
            var Muscovy_Duck_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Muscovy_Duck_3_2__Product_preparation_Unit_of_product.ToString();
            var Muscovy_Duck_4_3__Unit_of_product = oldquestionnaire.Muscovy_Duck_4_3__Unit_of_product.ToString();

            var Quail_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Quail_1_2__Product_preparation_Unit_of_product.ToString();
            var Quail_2_3__Unit_of_product = oldquestionnaire.Quail_2_3__Unit_of_product.ToString();
            var Quail_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Quail_3_2__Product_preparation_Unit_of_product.ToString();
            var Quail_4_3__Unit_of_product = oldquestionnaire.Quail_4_3__Unit_of_product.ToString();

            var Goose_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Goose_1_2__Product_preparation_Unit_of_product.ToString();
            var Goose_2_3__Unit_of_product = oldquestionnaire.Goose_2_3__Unit_of_product.ToString();
            var Goose_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Goose_3_2__Product_preparation_Unit_of_product.ToString();
            var Goose_4_3__Unit_of_product = oldquestionnaire.Goose_4_3__Unit_of_product.ToString();

            var Dog_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Dog_1_2__Product_preparation_Unit_of_product.ToString();
            var Dog_2_3__Unit_of_product = oldquestionnaire.Dog_2_3__Unit_of_product.ToString();
            var Dog_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Dog_3_2__Product_preparation_Unit_of_product.ToString();
            var Dog_4_3__Unit_of_product = oldquestionnaire.Dog_4_3__Unit_of_product.ToString();

            var Cat_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Cat_1_2__Product_preparation_Unit_of_product.ToString();
            var Cat_2_3__Unit_of_product = oldquestionnaire.Cat_2_3__Unit_of_product.ToString();
            var Cat_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Cat_3_2__Product_preparation_Unit_of_product.ToString();
            var Cat_4_3__Unit_of_product = oldquestionnaire.Cat_4_3__Unit_of_product.ToString();

            var Calf_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Calf_1_2__Product_preparation_Unit_of_product.ToString();
            var Calf_2_3__Unit_of_product = oldquestionnaire.Calf_2_3__Unit_of_product.ToString();
            var Calf_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Calf_3_2__Product_preparation_Unit_of_product.ToString();
            var Calf_4_3__Unit_of_product = oldquestionnaire.Calf_4_3__Unit_of_product.ToString();

            var Chick_1_2__Product_preparation_Unit_of_product = oldquestionnaire.Chick_1_2__Product_preparation_Unit_of_product.ToString();
            var Chick_2_3__Unit_of_product = oldquestionnaire.Chick_2_3__Unit_of_product.ToString();
            var Chick_3_2__Product_preparation_Unit_of_product = oldquestionnaire.Chick_3_2__Product_preparation_Unit_of_product.ToString();
            var Chick_4_3__Unit_of_product = oldquestionnaire.Chick_4_3__Unit_of_product.ToString();
            #endregion
            #region oldObject
            var oldobject = new QuestionnaireViewModel
            {
                #region A.General information
                D_u_th_i_gian = oldquestionnaire.D_u_th_i_gian.ToString(),
                /// <summary>
                /// Xuất xứ 
                /// </summary>
                A1__Product_origin = A1__Product_origin,
                /// <summary>
                /// Mã code sản phẩm
                /// </summary>
                A2__Product_code = oldquestionnaire.A2__Product_code,
                /// <summary>
                /// Tên sản phẩm
                /// </summary>
                A3__Product_name = oldquestionnaire.A3__Product_name,
                /// <summary>
                /// Tên công ty đăng ký sản phẩm
                /// </summary>
                A4__Company = oldquestionnaire.A4__Company,
                /// <summary>
                /// Loại sản phẩm ( dang bột - dạng dung dịch )
                /// </summary>
                A5__Type_of_product = A5__Type_of_product,
                /// <summary>
                /// Có chứa chất ngoài kháng sinh hay không?
                /// </summary>
                A6__Other_subtance_in_product = A6__Other_subtance_in_product,
                /// <summary>
                /// Khối lượng/trọng lượng/thể tích của sản phẩm 
                /// </summary>
                A7__Volume_of_product = oldquestionnaire.A7__Volume_of_product,
                /// <summary>
                /// Đơn vị của khối lượng/trọng lượng/thể tích của sản phẩm 
                /// </summary>
                A8__Unit_of_volume_of_product = A8__Unit_of_volume_of_product,
                /// <summary>
                /// Thông tin về khối lượng khác 
                /// </summary>
                A9__Other_volume_of_product = oldquestionnaire.A9__Other_volume_of_product,
                #endregion

                #region B.Information related to antimicrobial
                /// <summary>
                /// Số loại kháng sinh co trong sản phẩm
                /// </summary>
                B1__Number_of_antimicrobials_in_product = oldquestionnaire.B1__Number_of_antimicrobials_in_product,

                #region Antimicrobials 1
                /// <summary>
                /// Antimicrobials_1
                /// </summary>
                B2_1__Antimicrobial_1 = oldquestionnaire.B2_1__Antimicrobial_1,
                /// <summary>
                /// Thông tin về lượng kháng sinh 1 - chỉ điền số
                /// </summary>
                B2_2__Strength_of_antimicrobial_1 = oldquestionnaire.B2_2__Strength_of_antimicrobial_1,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh 1
                /// </summary>
                B2_3__Units_of_antimicrobial_1 = B2_3__Units_of_antimicrobial_1,
                /// <summary>
                /// Đơn vị khối lượng mỗi loại
                /// </summary>
                B2_4__Per_amount_of_product__antimicrobial_1_ = oldquestionnaire.B2_4__Per_amount_of_product__antimicrobial_1_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_,
                /// <summary>
                /// khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ = oldquestionnaire.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B2_7__Units_of_product__link_to_question_B2_4_ = B2_7__Units_of_product__link_to_question_B2_4_,
                #endregion
                #region Antimicrobials 2
                /// <summary>
                /// Antimicrobials_2
                /// </summary>
                B3_1__Antimicrobial_2 = oldquestionnaire.B3_1__Antimicrobial_2,
                /// <summary>
                /// Thông tin về lượng kháng sinh 2 - chỉ điền số
                /// </summary>
                B3_2__Strength_of_antimicrobial_2 = oldquestionnaire.B3_2__Strength_of_antimicrobial_2,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh 2
                /// </summary>
                B3_3__Units_of_antimicrobial_2 = B3_3__Units_of_antimicrobial_2,
                /// <summary>
                /// Đơn vị khối lượng mỗi loại
                /// </summary>
                B3_4__Per_amount_of_product__antimicrobial_2_ = oldquestionnaire.B3_4__Per_amount_of_product__antimicrobial_2_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_,
                /// <summary>
                /// khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ = oldquestionnaire.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B3_7__Units_of_product__link_to_question_B3_4_ = B3_7__Units_of_product__link_to_question_B3_4_,
                #endregion
                #region Antimicrobials 3
                /// <summary>
                /// Antimicrobials_3
                /// </summary>
                B4_1__Antimicrobial_3 = oldquestionnaire.B4_1__Antimicrobial_3,
                /// <summary>
                /// Thông tin về lượng kháng sinh 3 - chỉ điền số
                /// </summary>
                B4_2__Strength_of_antimicrobial_3 = oldquestionnaire.B4_2__Strength_of_antimicrobial_3,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh 3
                /// </summary>
                B4_3__Units_of_antimicrobial_3 = B4_3__Units_of_antimicrobial_3,
                /// <summary>
                /// Đơn vị khối lượng mỗi loại
                /// </summary>
                B4_4__Per_amount_of_product__antimicrobial_3_ = oldquestionnaire.B4_4__Per_amount_of_product__antimicrobial_3_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_,
                /// <summary>
                /// khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ = oldquestionnaire.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B4_7__Units_of_product__link_to_question_B4_4_ = B4_7__Units_of_product__link_to_question_B4_4_,
                #endregion
                #region Antimicrobials 4
                /// <summary>
                /// Antimicrobials_4
                /// </summary>
                B5_1__Antimicrobial_4 = oldquestionnaire.B5_1__Antimicrobial_4,
                /// <summary>
                /// Thông tin về lượng kháng sinh 4 - chỉ điền số
                /// </summary>
                B5_2__Strength_of_antimicrobial_4 = oldquestionnaire.B5_2__Strength_of_antimicrobial_4,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh 4
                /// </summary>
                B5_3__Units_of_antimicrobial_4 = B5_3__Units_of_antimicrobial_4,
                /// <summary>
                /// Đơn vị khối lượng mỗi loại
                /// </summary>
                B5_4__Per_amount_of_product__antimicrobial_4_ = oldquestionnaire.B5_4__Per_amount_of_product__antimicrobial_4_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = B5_5__Units_of_antimicrobial_4__link_to_question_5_4_,
                /// <summary>
                /// khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ = oldquestionnaire.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_,
                /// <summary>
                /// Đơn vị khối lượng kháng sinh cho mỗi loại
                /// </summary>
                B5_7__Units_of_product__link_to_question_B5_4_ = B5_7__Units_of_product__link_to_question_B5_4_,
                #endregion

                /// <summary>
                /// Các loài vật
                /// </summary>
                B6__Target_species_x = oldquestionnaire.B6__Target_species_x,
                /// <summary>
                /// Đường dùng thuốc 
                /// </summary>
                B7__Administration_route = oldquestionnaire.B7__Administration_route,
                #endregion

                //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho heo
                #region C_ Heo

                #region C_1_ Product preparation (dilution) _pig_prevention purpose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                C1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.C1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                C1_2__Product_preparation_Unit_of_product = C1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                C1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.C1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                C1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.C1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                C1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.C1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                C1_6__Duration_of_usage__min__max_ = oldquestionnaire.C1_6__Duration_of_usage__min__max_,
                #endregion

                #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                C2_1__Product_min = oldquestionnaire.C2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                C2_2__Product_max = oldquestionnaire.C2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                C2_3__Unit_of_product = C2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                C2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.C2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                C2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.C2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                C2_6__Duration_of_usage = oldquestionnaire.C2_6__Duration_of_usage,
                #endregion

                #region C_3 Product preparation (dilution) _pig_treatment purpose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                C3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.C3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                C3_2__Product_preparation_Unit_of_product = C3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                C3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.C3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                C3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.C3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                C3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.C3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                C3_6__Duration_of_usage = oldquestionnaire.C3_6__Duration_of_usage,


                #endregion

                #region C_4 Guidelines referred to bodyweight_pig_treatment purpose
                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                C4_1__Product_min = oldquestionnaire.C4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                C4_2__Product_max = oldquestionnaire.C4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                C4_3__Unit_of_product = C4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                C4_4__Per_No__Kg_bodyweight_min = oldquestionnaire.C4_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                C4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.C4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                C4_6__Duration_of_usage = oldquestionnaire.C4_6__Duration_of_usage,
                #endregion

                #endregion

                //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho thú nhai lại không phân biệt thú lớn và nhỏ
                #region D. Động vật nhai lại

                #region D_1_ Product preparation (dilution) _ruminant_prevention purpose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                D1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.D1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                D1_2__Product_preparation_Unit_of_product = D1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                D1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.D1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                D1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.D1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                D1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.D1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                D1_6__Duration_of_usage = oldquestionnaire.D1_6__Duration_of_usage,
                #endregion

                #region D.2 Guidelines referred to bodyweight_ruminant_prevention purpose
                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                D2_1__Product_min = oldquestionnaire.D2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                D2_2__Product_max = oldquestionnaire.D2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                D2_3__Unit_of_product = D2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                D2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.D2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                D2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.D2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                D2_6__Duration_of_usage = oldquestionnaire.D2_6__Duration_of_usage,
                #endregion

                #region D_3 Product preparation (dilution) _ruminant_treatment purpose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                D3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.D3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                D3_2__Product_preparation_Unit_of_product = D3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                D3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.D3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                D3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.D3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                D3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.D3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                D3_6__Duration_of_usage = oldquestionnaire.D3_6__Duration_of_usage,


                #endregion

                #region D_4 Guidelines referred to bodyweight_ruminant_treatment purpose
                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                D4_1__Product_min = oldquestionnaire.D4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                D4_2__Product_max = oldquestionnaire.D4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                D4_3__Unit_of_product = D4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                D4_4__Per_No__Kg_bodyweight_min = oldquestionnaire.D4_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                D4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.D4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                D4_6__Duration_of_usage = oldquestionnaire.D4_6__Duration_of_usage,
                #endregion

                #endregion

                //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho gia cầm nói chung bao gồm gà, vịt, ngan, ngỗng, cút
                #region E. Gia cầm

                #region D_1_ Product preparation (dilution) _poultry_prevention purpose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                E1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.E1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                E1_2__Product_preparation_Unit_of_product = E1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                E1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.E1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                E1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.E1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                E1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.E1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                E1_6__Duration_of_usage = oldquestionnaire.E1_6__Duration_of_usage,
                #endregion

                #region D.2 Guidelines referred to bodyweight_poultry_prevention purpose
                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                E2_1__Product_min = oldquestionnaire.E2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                E2_2__Product_max = oldquestionnaire.E2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                E2_3__Unit_of_product = E2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                E2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.E2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                E2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.E2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                E2_6__Duration_of_usage = oldquestionnaire.E2_6__Duration_of_usage,
                #endregion

                #region D_3 Product preparation (dilution) _poultry_treatment purpose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                E3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.E3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                E3_2__Product_preparation_Unit_of_product = E3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                E3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.E3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                E3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.E3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                E3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.E3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                E3_6__Duration_of_usage = oldquestionnaire.E3_6__Duration_of_usage,


                #endregion

                #region D_4 Guidelines referred to bodyweight_poultry_treatment purpose
                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                E4_1__Product_min = oldquestionnaire.E4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                E4_2__Product_max = oldquestionnaire.E4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                E4_3__Unit_of_product = E4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                E4_4__Per_No__Kg_bodyweight_min = oldquestionnaire.E4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                E4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.E4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                E4_6__Duration_of_usage = oldquestionnaire.E4_6__Duration_of_usage,

                #endregion


                #endregion

                //Phần này bao gồm các thông tin thêm về sản phẩm và người nhập dữ liệu để tiện cho việc kiểm tra tính xác thực của thông tin
                #region F.Further information

                /// <summary>
                /// Điền trang web hoặc vetshop
                /// </summary>
                F_1__Source_of_information = oldquestionnaire.F_1__Source_of_information,
                /// <summary>
                /// Ảnh sản phẩm
                /// </summary>
                F_2__Picture_of_product = oldquestionnaire.F_2__Picture_of_product,
                /// <summary>
                /// Thông tin khác
                /// </summary>
                F3__Correction = oldquestionnaire.F3__Correction,
                /// <summary>
                /// Thông tin người nhập
                /// </summary>
                F_4__Person_in_charge = oldquestionnaire.F_4__Person_in_charge,
                /// <summary>
                /// thời gian cho việc tìm kiếm thu và nhập thông tin sản phẩm
                /// </summary>
                F_5__Working_time = oldquestionnaire.F_5__Working_time,
                /// <summary>
                /// Ghi chú
                /// </summary>
                F_6__Note = oldquestionnaire.F_6__Note,

                #endregion

                //Heo con
                #region G.Piglet
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Piglet_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Piglet_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Piglet_1_2__Product_preparation_Unit_of_product = Piglet_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Piglet_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Piglet_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Piglet_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Piglet_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Piglet_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Piglet_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Piglet_1_6__Duration_of_usage = oldquestionnaire.Piglet_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Piglet_2_1__Product_min = oldquestionnaire.Piglet_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Piglet_2_2__Product_max = oldquestionnaire.Piglet_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Piglet_2_3__Unit_of_product = Piglet_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Piglet_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Piglet_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Piglet_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Piglet_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Piglet_2_6__Duration_of_usage = oldquestionnaire.Piglet_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Piglet_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Piglet_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Piglet_3_2__Product_preparation_Unit_of_product = Piglet_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Piglet_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Piglet_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Piglet_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Piglet_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Piglet_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Piglet_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Piglet_3_6__Duration_of_usage = oldquestionnaire.Piglet_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Piglet_4_1__Product_min = oldquestionnaire.Piglet_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Piglet_4_2__Product_max = oldquestionnaire.Piglet_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Piglet_4_3__Unit_of_product = Piglet_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Piglet_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Piglet_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Piglet_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Piglet_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Piglet_4_6__Duration_of_usage = oldquestionnaire.Piglet_4_6__Duration_of_usage,


                #endregion

                //Trâu
                #region H.Buffalo
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Buffalo_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Buffalo_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Buffalo_1_2__Product_preparation_Unit_of_product = Buffalo_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Buffalo_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Buffalo_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Buffalo_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Buffalo_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Buffalo_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Buffalo_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Buffalo_1_6__Duration_of_usage = oldquestionnaire.Buffalo_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Buffalo_2_1__Product_min = oldquestionnaire.Buffalo_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Buffalo_2_2__Product_max = oldquestionnaire.Buffalo_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Buffalo_2_3__Unit_of_product = Buffalo_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Buffalo_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Buffalo_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Buffalo_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Buffalo_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Buffalo_2_6__Duration_of_usage = oldquestionnaire.Buffalo_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Buffalo_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Buffalo_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Buffalo_3_2__Product_preparation_Unit_of_product = Buffalo_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Buffalo_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Buffalo_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Buffalo_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Buffalo_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Buffalo_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Buffalo_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Buffalo_3_6__Duration_of_usage = oldquestionnaire.Buffalo_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Buffalo_4_1__Product_min = oldquestionnaire.Buffalo_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Buffalo_4_2__Product_max = oldquestionnaire.Buffalo_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Buffalo_4_3__Unit_of_product = Buffalo_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Buffalo_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Buffalo_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Buffalo_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Buffalo_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Buffalo_4_6__Duration_of_usage = oldquestionnaire.Buffalo_4_6__Duration_of_usage,



                #endregion

                //Gia súc
                #region I.Cattle
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Cattle_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cattle_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Cattle_1_2__Product_preparation_Unit_of_product = Cattle_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Cattle_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cattle_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Cattle_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cattle_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Cattle_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cattle_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cattle_1_6__Duration_of_usages = oldquestionnaire.Cattle_1_6__Duration_of_usages,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Cattle_2_1__Product_min = oldquestionnaire.Cattle_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Cattle_2_2__Product_max = oldquestionnaire.Cattle_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Cattle_2_3__Unit_of_product = Cattle_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Cattle_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Cattle_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Cattle_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cattle_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cattle_2_6__Duration_of_usage = oldquestionnaire.Cattle_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Cattle_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cattle_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Cattle_3_2__Product_preparation_Unit_of_product = Cattle_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Cattle_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cattle_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Cattle_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cattle_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Cattle_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cattle_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cattle_3_6__Duration_of_usage = oldquestionnaire.Cattle_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Cattle_4_1__Product_min = oldquestionnaire.Cattle_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Cattle_4_2__Product_max = oldquestionnaire.Cattle_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Cattle_4_3__Unit_of_product = Cattle_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Cattle_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Cattle_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Cattle_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cattle_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cattle_4_6__Duration_of_usage = oldquestionnaire.Cattle_4_6__Duration_of_usage,


                #endregion

                //Dê
                #region J.Goat
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Goat_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goat_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Goat_1_2__Product_preparation_Unit_of_product = Goat_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Goat_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goat_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Goat_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goat_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Goat_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goat_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goat_1_6__Duration_of_usage = oldquestionnaire.Goat_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Goat_2_1__Product_min = oldquestionnaire.Goat_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Goat_2_2__Product_max = oldquestionnaire.Goat_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Goat_2_3__Unit_of_product = Goat_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Goat_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Goat_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Goat_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goat_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goat_2_6__Duration_of_usage = oldquestionnaire.Goat_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Goat_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goat_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Goat_3_2__Product_preparation_Unit_of_product = Goat_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Goat_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goat_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Goat_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goat_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Goat_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goat_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goat_3_6__Duration_of_usage = oldquestionnaire.Goat_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Goat_4_1__Product_min = oldquestionnaire.Goat_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Goat_4_2__Product_max = oldquestionnaire.Goat_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Goat_4_3__Unit_of_product = Goat_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Goat_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Goat_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Goat_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goat_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goat_4_6__Duration_of_usage = oldquestionnaire.Goat_4_6__Duration_of_usage,

                #endregion

                //Cừu
                #region K.Sheep
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Sheep_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Sheep_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Sheep_1_2__Product_preparation_Unit_of_product = Sheep_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Sheep_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Sheep_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Sheep_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Sheep_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Sheep_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Sheep_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Sheep_1_6__Duration_of_usage = oldquestionnaire.Sheep_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Sheep_2_1__Product_min = oldquestionnaire.Sheep_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Sheep_2_2__Product_max = oldquestionnaire.Sheep_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Sheep_2_3__Unit_of_product = Sheep_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Sheep_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Sheep_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Sheep_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Sheep_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Sheep_2_6__Duration_of_usage = oldquestionnaire.Sheep_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Sheep_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Sheep_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Sheep_3_2__Product_preparation_Unit_of_product = Sheep_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Sheep_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Sheep_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Sheep_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Sheep_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Sheep_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Sheep_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Sheep_3_6__Duration_of_usage = oldquestionnaire.Sheep_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Sheep_4_1__Product_min = oldquestionnaire.Sheep_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Sheep_4_2__Product_max = oldquestionnaire.Sheep_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Sheep_4_3__Unit_of_product = Sheep_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Sheep_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Sheep_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Sheep_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Sheep_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Sheep_4_6__Duration_of_usage = oldquestionnaire.Sheep_4_6__Duration_of_usage,

                #endregion

                //Ngựa
                #region L.Horse
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Horse_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Horse_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Horse_1_2__Product_preparation_Unit_of_product = Horse_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Horse_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Horse_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Horse_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Horse_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Horse_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Horse_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Horse_1_6__Duration_of_usage = oldquestionnaire.Horse_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Horse_2_1__Product_min = oldquestionnaire.Horse_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Horse_2_2__Product_max = oldquestionnaire.Horse_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Horse_2_3__Unit_of_product = Horse_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Horse_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Horse_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Horse_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Horse_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Horse_2_6__Duration_of_usage = oldquestionnaire.Horse_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Horse_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Horse_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Horse_3_2__Product_preparation_Unit_of_product = Horse_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Horse_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Horse_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Horse_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Horse_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Horse_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Horse_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Horse_3_6__Duration_of_usage = oldquestionnaire.Horse_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Horse_4_1__Product_min = oldquestionnaire.Horse_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Horse_4_2__Product_max = oldquestionnaire.Horse_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Horse_4_3__Unit_of_product = Horse_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Horse_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Horse_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Horse_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Horse_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Horse_4_6__Duration_of_usage = oldquestionnaire.Horse_4_6__Duration_of_usage,

                #endregion

                //Gà
                #region M.Chicken
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Chicken_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chicken_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Chicken_1_2__Product_preparation_Unit_of_product = Chicken_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Chicken_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chicken_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Chicken_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chicken_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Chicken_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chicken_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chicken_1_6__Duration_of_usage = oldquestionnaire.Chicken_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Chicken_2_1__Product_min = oldquestionnaire.Chicken_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Chicken_2_2__Product_max = oldquestionnaire.Chicken_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Chicken_2_3__Unit_of_product = Chicken_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Chicken_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Chicken_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Chicken_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chicken_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chicken_2_6__Duration_of_usage = oldquestionnaire.Chicken_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Chicken_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chicken_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Chicken_3_2__Product_preparation_Unit_of_product = Chicken_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Chicken_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chicken_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Chicken_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chicken_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Chicken_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chicken_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chicken_3_6__Duration_of_usage = oldquestionnaire.Chicken_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Chicken_4_1__Product_min = oldquestionnaire.Chicken_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Chicken_4_2__Product_max = oldquestionnaire.Chicken_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Chicken_4_3__Unit_of_product = Chicken_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Chicken_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Chicken_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Chicken_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chicken_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chicken_4_6__Duration_of_usage = oldquestionnaire.Chicken_4_6__Duration_of_usage,

                #endregion

                //Vịt
                #region N.Duck
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Duck_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Duck_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Duck_1_2__Product_preparation_Unit_of_product = Duck_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Duck_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Duck_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Duck_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Duck_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Duck_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Duck_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Duck_1_6__Duration_of_usage = oldquestionnaire.Duck_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Duck_2_1__Product_min = oldquestionnaire.Duck_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Duck_2_2__Product_max = oldquestionnaire.Duck_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Duck_2_3__Unit_of_product = Duck_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Duck_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Duck_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Duck_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Duck_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Duck_2_6__Duration_of_usage = oldquestionnaire.Duck_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Duck_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Duck_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Duck_3_2__Product_preparation_Unit_of_product = Duck_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Duck_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Duck_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Duck_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Duck_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Duck_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Duck_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Duck_3_6__Duration_of_usage = oldquestionnaire.Duck_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Duck_4_1__Product_min = oldquestionnaire.Duck_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Duck_4_2__Product_max = oldquestionnaire.Duck_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Duck_4_3__Unit_of_product = Duck_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Duck_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Duck_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Duck_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Duck_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Duck_4_6__Duration_of_usage = oldquestionnaire.Duck_4_6__Duration_of_usage,

                #endregion

                //Vịt Xiêm
                #region O.Muscovy_Duck
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Muscovy_Duck_1_2__Product_preparation_Unit_of_product = Muscovy_Duck_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Muscovy_Duck_1_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Muscovy_Duck_2_1__Product_min = oldquestionnaire.Muscovy_Duck_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Muscovy_Duck_2_2__Product_max = oldquestionnaire.Muscovy_Duck_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Muscovy_Duck_2_3__Unit_of_product = Muscovy_Duck_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Muscovy_Duck_2_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Muscovy_Duck_3_2__Product_preparation_Unit_of_product = Muscovy_Duck_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Muscovy_Duck_3_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Muscovy_Duck_4_1__Product_min = oldquestionnaire.Muscovy_Duck_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Muscovy_Duck_4_2__Product_max = oldquestionnaire.Muscovy_Duck_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Muscovy_Duck_4_3__Unit_of_product = Muscovy_Duck_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Muscovy_Duck_4_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_4_6__Duration_of_usage,

                #endregion

                //Cút
                #region P.Quail
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Quail_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Quail_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Quail_1_2__Product_preparation_Unit_of_product = Quail_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Quail_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Quail_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Quail_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Quail_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Quail_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Quail_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Quail_1_6__Duration_of_usage = oldquestionnaire.Quail_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Quail_2_1__Product_min = oldquestionnaire.Quail_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Quail_2_2__Product_max = oldquestionnaire.Quail_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Quail_2_3__Unit_of_product = Quail_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Quail_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Quail_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Quail_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Quail_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Quail_2_6__Duration_of_usage = oldquestionnaire.Quail_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Quail_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Quail_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Quail_3_2__Product_preparation_Unit_of_product = Quail_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Quail_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Quail_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Quail_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Quail_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Quail_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Quail_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Quail_3_6__Duration_of_usage = oldquestionnaire.Quail_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Quail_4_1__Product_min = oldquestionnaire.Quail_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Quail_4_2__Product_max = oldquestionnaire.Quail_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Quail_4_3__Unit_of_product = Quail_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Quail_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Quail_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Quail_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Quail_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Quail_4_6__Duration_of_usage = oldquestionnaire.Quail_4_6__Duration_of_usage,

                #endregion

                //Ngỗng
                #region Q.Goose
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Goose_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goose_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Goose_1_2__Product_preparation_Unit_of_product = Goose_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Goose_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goose_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Goose_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goose_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Goose_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goose_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goose_1_6__Duration_of_usage = oldquestionnaire.Goose_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Goose_2_1__Product_min = oldquestionnaire.Goose_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Goose_2_2__Product_max = oldquestionnaire.Goose_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Goose_2_3__Unit_of_product = Goose_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Goose_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Goose_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Goose_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goose_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goose_2_6__Duration_of_usage = oldquestionnaire.Goose_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Goose_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goose_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Goose_3_2__Product_preparation_Unit_of_product = Goose_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Goose_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goose_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Goose_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goose_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Goose_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goose_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goose_3_6__Duration_of_usage = oldquestionnaire.Goose_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Goose_4_1__Product_min = oldquestionnaire.Goose_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Goose_4_2__Product_max = oldquestionnaire.Goose_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Goose_4_3__Unit_of_product = Goose_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Goose_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Goose_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Goose_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goose_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Goose_4_6__Duration_of_usage = oldquestionnaire.Goose_4_6__Duration_of_usage,

                #endregion

                //Chó
                #region R.Dog
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Dog_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Dog_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Dog_1_2__Product_preparation_Unit_of_product = Dog_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Dog_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Dog_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Dog_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Dog_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Dog_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Dog_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Dog_1_6__Duration_of_usage = oldquestionnaire.Dog_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Dog_2_1__Product_min = oldquestionnaire.Dog_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Dog_2_2__Product_max = oldquestionnaire.Dog_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Dog_2_3__Unit_of_product = Dog_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Dog_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Dog_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Dog_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Dog_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Dog_2_6__Duration_of_usage = oldquestionnaire.Dog_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Dog_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Dog_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Dog_3_2__Product_preparation_Unit_of_product = Dog_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Dog_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Dog_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Dog_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Dog_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Dog_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Dog_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Dog_3_6__Duration_of_usage = oldquestionnaire.Dog_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Dog_4_1__Product_min = oldquestionnaire.Dog_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Dog_4_2__Product_max = oldquestionnaire.Dog_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Dog_4_3__Unit_of_product = Dog_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Dog_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Dog_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Dog_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Dog_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Dog_4_6__Duration_of_usage = oldquestionnaire.Dog_4_6__Duration_of_usage,

                #endregion

                //Mèo
                #region S.Cat
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Cat_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cat_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Cat_1_2__Product_preparation_Unit_of_product = Cat_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Cat_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cat_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Cat_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cat_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Cat_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cat_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cat_1_6__Duration_of_usage = oldquestionnaire.Cat_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Cat_2_1__Product_min = oldquestionnaire.Cat_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Cat_2_2__Product_max = oldquestionnaire.Cat_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Cat_2_3__Unit_of_product = Cat_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Cat_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Cat_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Cat_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cat_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cat_2_6__Duration_of_usage = oldquestionnaire.Cat_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Cat_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cat_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Cat_3_2__Product_preparation_Unit_of_product = Cat_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Cat_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cat_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Cat_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cat_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Cat_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cat_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cat_3_6__Duration_of_usage = oldquestionnaire.Cat_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Cat_4_1__Product_min = oldquestionnaire.Cat_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Cat_4_2__Product_max = oldquestionnaire.Cat_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Cat_4_3__Unit_of_product = Cat_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Cat_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Cat_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Cat_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cat_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Cat_4_6__Duration_of_usage = oldquestionnaire.Cat_4_6__Duration_of_usage,

                #endregion

                //Bê
                #region T.Calf
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Calf_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Calf_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Calf_1_2__Product_preparation_Unit_of_product = Calf_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Calf_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Calf_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Calf_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Calf_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Calf_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Calf_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Calf_1_6__Duration_of_usage = oldquestionnaire.Calf_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Calf_2_1__Product_min = oldquestionnaire.Calf_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Calf_2_2__Product_max = oldquestionnaire.Calf_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Calf_2_3__Unit_of_product = Calf_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Calf_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Calf_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Calf_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Calf_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Calf_2_6__Duration_of_usage = oldquestionnaire.Calf_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Calf_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Calf_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Calf_3_2__Product_preparation_Unit_of_product = Calf_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Calf_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Calf_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Calf_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Calf_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Calf_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Calf_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Calf_3_6__Duration_of_usage = oldquestionnaire.Calf_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Calf_4_1__Product_min = oldquestionnaire.Calf_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Calf_4_2__Product_max = oldquestionnaire.Calf_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Calf_4_3__Unit_of_product = Calf_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Calf_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Calf_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Calf_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Calf_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Calf_4_6__Duration_of_usage = oldquestionnaire.Calf_4_6__Duration_of_usage,

                #endregion

                //Gà con
                #region U.Chick
                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Chick_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chick_1_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Chick_1_2__Product_preparation_Unit_of_product = Chick_1_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Chick_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chick_1_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Chick_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chick_1_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Chick_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chick_1_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chick_1_6__Duration_of_usage = oldquestionnaire.Chick_1_6__Duration_of_usage,


                /// <summary>
                /// Lượng thuốc tối thiểu
                /// </summary>
                Chick_2_1__Product_min = oldquestionnaire.Chick_2_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Chick_2_2__Product_max = oldquestionnaire.Chick_2_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Chick_2_3__Unit_of_product = Chick_2_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Chick_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Chick_2_4__Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Chick_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chick_2_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chick_2_6__Duration_of_usage = oldquestionnaire.Chick_2_6__Duration_of_usage,


                /// <summary>
                /// sẩn phẩm pha
                /// </summary>
                Chick_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chick_3_1__Product_preparation__dilution__Product_amount,
                /// <summary>
                /// Lượng thuốc để pha
                /// </summary>
                Chick_3_2__Product_preparation_Unit_of_product = Chick_3_2__Product_preparation_Unit_of_product,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                /// </summary>
                Chick_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chick_3_3__Product_preparation_To_be_added_to__min_,
                /// <summary>
                /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                /// </summary>
                Chick_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chick_3_4__Product_preparation_To_be_added_to__max_,
                /// <summary>
                /// Đơn vị tính lượng nước hoặc thức ăn 
                /// </summary>
                Chick_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chick_3_5__Product_preparation_Unit_of_water_feed,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chick_3_6__Duration_of_usage = oldquestionnaire.Chick_3_6__Duration_of_usage,



                /// <summary>
                /// Lượng thuốc tối thiểu                                                                             
                /// </summary>                                                                             
                Chick_4_1__Product_min = oldquestionnaire.Chick_4_1__Product_min,
                /// <summary>
                /// Lượng thuốc tối đa
                /// </summary>
                Chick_4_2__Product_max = oldquestionnaire.Chick_4_2__Product_max,
                /// <summary>
                /// đơn vị tính
                /// </summary>
                Chick_4_3__Unit_of_product = Chick_4_3__Unit_of_product,
                /// <summary>
                /// Trọng lượng mỗi Kg tối thiểu
                /// </summary>
                Chick_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Chick_4_4_Per_No__Kg_bodyweight_min,
                /// <summary>
                /// Trọng lượng mỗi Kg tối đa
                /// </summary>
                Chick_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chick_4_5__Per_No__Kg_bodyweight_max,
                /// <summary>
                /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                /// </summary>
                Chick_4_6__Duration_of_usage = oldquestionnaire.Chick_4_6__Duration_of_usage,

                #endregion

                State = state
            };
            #endregion
            #endregion
            if (questionnaire.F_2__Picture_of_product != null)
            {
                oldquestionnaire.F_2__Picture_of_product = questionnaire.F_2__Picture_of_product;
            }
            oldquestionnaire = questionnaire;
            oldquestionnaire.B6__Target_species_x = lspet;
            oldquestionnaire.B7__Administration_route = lsroute;
            oldquestionnaire.State = State.WAIT;
            oldquestionnaire.D_u_th_i_gian = DateTime.Now;
            try
            {
                db.Entry<Questionnaire>(oldquestionnaire).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var user = unitWork.User.GetById(this.User.UserId);
                #region Auditrail Newobject
                #region Enum ToString
                var state1 = oldquestionnaire.State.ToString();
                var A1__Product_origin1 = oldquestionnaire.A1__Product_origin.ToString();
                var A5__Type_of_product1 = oldquestionnaire.A5__Type_of_product.ToString();
                var A6__Other_subtance_in_product1 = oldquestionnaire.A6__Other_subtance_in_product.ToString();
                var A8__Unit_of_volume_of_product1 = oldquestionnaire.A8__Unit_of_volume_of_product.ToString();
                var B2_3__Units_of_antimicrobial_11 = oldquestionnaire.B2_3__Units_of_antimicrobial_1.ToString();
                var B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_1 = oldquestionnaire.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_.ToString();
                var B2_7__Units_of_product__link_to_question_B2_4_1 = oldquestionnaire.B2_7__Units_of_product__link_to_question_B2_4_.ToString();
                var B3_3__Units_of_antimicrobial_21 = oldquestionnaire.B3_3__Units_of_antimicrobial_2.ToString();
                var B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_1 = oldquestionnaire.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_.ToString();
                var B3_7__Units_of_product__link_to_question_B3_4_1 = oldquestionnaire.B3_7__Units_of_product__link_to_question_B3_4_.ToString();
                var B4_3__Units_of_antimicrobial_31 = oldquestionnaire.B4_3__Units_of_antimicrobial_3.ToString();
                var B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_1 = oldquestionnaire.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_.ToString();
                var B4_7__Units_of_product__link_to_question_B4_4_1 = oldquestionnaire.B4_7__Units_of_product__link_to_question_B4_4_.ToString();
                var B5_3__Units_of_antimicrobial_41 = oldquestionnaire.B5_3__Units_of_antimicrobial_4.ToString();
                var B5_5__Units_of_antimicrobial_4__link_to_question_5_4_1 = oldquestionnaire.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_.ToString();
                var B5_7__Units_of_product__link_to_question_B5_4_1 = oldquestionnaire.B5_7__Units_of_product__link_to_question_B5_4_.ToString();

                var C1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.C1_2__Product_preparation_Unit_of_product.ToString();
                var C2_3__Unit_of_product1 = oldquestionnaire.C2_3__Unit_of_product.ToString();
                var C3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.C3_2__Product_preparation_Unit_of_product.ToString();
                var C4_3__Unit_of_product1 = oldquestionnaire.C4_3__Unit_of_product.ToString();

                var D1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.D1_2__Product_preparation_Unit_of_product.ToString();
                var D2_3__Unit_of_product1 = oldquestionnaire.D2_3__Unit_of_product.ToString();
                var D3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.D3_2__Product_preparation_Unit_of_product.ToString();
                var D4_3__Unit_of_product1 = oldquestionnaire.D4_3__Unit_of_product.ToString();

                var E1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.E1_2__Product_preparation_Unit_of_product.ToString();
                var E2_3__Unit_of_product1 = oldquestionnaire.E2_3__Unit_of_product.ToString();
                var E3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.E3_2__Product_preparation_Unit_of_product.ToString();
                var E4_3__Unit_of_product1 = oldquestionnaire.E4_3__Unit_of_product.ToString();

                var Piglet_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Piglet_1_2__Product_preparation_Unit_of_product.ToString();
                var Piglet_2_3__Unit_of_product1 = oldquestionnaire.Piglet_2_3__Unit_of_product.ToString();
                var Piglet_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Piglet_3_2__Product_preparation_Unit_of_product.ToString();
                var Piglet_4_3__Unit_of_product1 = oldquestionnaire.Piglet_4_3__Unit_of_product.ToString();

                var Buffalo_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Buffalo_1_2__Product_preparation_Unit_of_product.ToString();
                var Buffalo_2_3__Unit_of_product1 = oldquestionnaire.Buffalo_2_3__Unit_of_product.ToString();
                var Buffalo_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Buffalo_3_2__Product_preparation_Unit_of_product.ToString();
                var Buffalo_4_3__Unit_of_product1 = oldquestionnaire.Buffalo_4_3__Unit_of_product.ToString();

                var Cattle_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Cattle_1_2__Product_preparation_Unit_of_product.ToString();
                var Cattle_2_3__Unit_of_product1 = oldquestionnaire.Cattle_2_3__Unit_of_product.ToString();
                var Cattle_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Cattle_3_2__Product_preparation_Unit_of_product.ToString();
                var Cattle_4_3__Unit_of_product1 = oldquestionnaire.Cattle_4_3__Unit_of_product.ToString();

                var Goat_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Goat_1_2__Product_preparation_Unit_of_product.ToString();
                var Goat_2_3__Unit_of_product1 = oldquestionnaire.Goat_2_3__Unit_of_product.ToString();
                var Goat_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Goat_3_2__Product_preparation_Unit_of_product.ToString();
                var Goat_4_3__Unit_of_product1 = oldquestionnaire.Goat_4_3__Unit_of_product.ToString();

                var Sheep_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Sheep_1_2__Product_preparation_Unit_of_product.ToString();
                var Sheep_2_3__Unit_of_product1 = oldquestionnaire.Sheep_2_3__Unit_of_product.ToString();
                var Sheep_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Sheep_3_2__Product_preparation_Unit_of_product.ToString();
                var Sheep_4_3__Unit_of_product1 = oldquestionnaire.Sheep_4_3__Unit_of_product.ToString();

                var Horse_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Horse_1_2__Product_preparation_Unit_of_product.ToString();
                var Horse_2_3__Unit_of_product1 = oldquestionnaire.Horse_2_3__Unit_of_product.ToString();
                var Horse_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Horse_3_2__Product_preparation_Unit_of_product.ToString();
                var Horse_4_3__Unit_of_product1 = oldquestionnaire.Horse_4_3__Unit_of_product.ToString();

                var Chicken_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Chicken_1_2__Product_preparation_Unit_of_product.ToString();
                var Chicken_2_3__Unit_of_product1 = oldquestionnaire.Chicken_2_3__Unit_of_product.ToString();
                var Chicken_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Chicken_3_2__Product_preparation_Unit_of_product.ToString();
                var Chicken_4_3__Unit_of_product1 = oldquestionnaire.Chicken_4_3__Unit_of_product.ToString();

                var Duck_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Duck_1_2__Product_preparation_Unit_of_product.ToString();
                var Duck_2_3__Unit_of_product1 = oldquestionnaire.Duck_2_3__Unit_of_product.ToString();
                var Duck_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Duck_3_2__Product_preparation_Unit_of_product.ToString();
                var Duck_4_3__Unit_of_product1 = oldquestionnaire.Duck_4_3__Unit_of_product.ToString();

                var Muscovy_Duck_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Muscovy_Duck_1_2__Product_preparation_Unit_of_product.ToString();
                var Muscovy_Duck_2_3__Unit_of_product1 = oldquestionnaire.Muscovy_Duck_2_3__Unit_of_product.ToString();
                var Muscovy_Duck_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Muscovy_Duck_3_2__Product_preparation_Unit_of_product.ToString();
                var Muscovy_Duck_4_3__Unit_of_product1 = oldquestionnaire.Muscovy_Duck_4_3__Unit_of_product.ToString();

                var Quail_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Quail_1_2__Product_preparation_Unit_of_product.ToString();
                var Quail_2_3__Unit_of_product1 = oldquestionnaire.Quail_2_3__Unit_of_product.ToString();
                var Quail_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Quail_3_2__Product_preparation_Unit_of_product.ToString();
                var Quail_4_3__Unit_of_product1 = oldquestionnaire.Quail_4_3__Unit_of_product.ToString();

                var Goose_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Goose_1_2__Product_preparation_Unit_of_product.ToString();
                var Goose_2_3__Unit_of_product1 = oldquestionnaire.Goose_2_3__Unit_of_product.ToString();
                var Goose_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Goose_3_2__Product_preparation_Unit_of_product.ToString();
                var Goose_4_3__Unit_of_product1 = oldquestionnaire.Goose_4_3__Unit_of_product.ToString();

                var Dog_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Dog_1_2__Product_preparation_Unit_of_product.ToString();
                var Dog_2_3__Unit_of_product1 = oldquestionnaire.Dog_2_3__Unit_of_product.ToString();
                var Dog_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Dog_3_2__Product_preparation_Unit_of_product.ToString();
                var Dog_4_3__Unit_of_product1 = oldquestionnaire.Dog_4_3__Unit_of_product.ToString();

                var Cat_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Cat_1_2__Product_preparation_Unit_of_product.ToString();
                var Cat_2_3__Unit_of_product1 = oldquestionnaire.Cat_2_3__Unit_of_product.ToString();
                var Cat_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Cat_3_2__Product_preparation_Unit_of_product.ToString();
                var Cat_4_3__Unit_of_product1 = oldquestionnaire.Cat_4_3__Unit_of_product.ToString();

                var Calf_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Calf_1_2__Product_preparation_Unit_of_product.ToString();
                var Calf_2_3__Unit_of_product1 = oldquestionnaire.Calf_2_3__Unit_of_product.ToString();
                var Calf_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Calf_3_2__Product_preparation_Unit_of_product.ToString();
                var Calf_4_3__Unit_of_product1 = oldquestionnaire.Calf_4_3__Unit_of_product.ToString();

                var Chick_1_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Chick_1_2__Product_preparation_Unit_of_product.ToString();
                var Chick_2_3__Unit_of_product1 = oldquestionnaire.Chick_2_3__Unit_of_product.ToString();
                var Chick_3_2__Product_preparation_Unit_of_product1 = oldquestionnaire.Chick_3_2__Product_preparation_Unit_of_product.ToString();
                var Chick_4_3__Unit_of_product1 = oldquestionnaire.Chick_4_3__Unit_of_product.ToString();
                #endregion
                #region newObject
                var newobject = new QuestionnaireViewModel
                {
                    #region A.General information
                    D_u_th_i_gian = oldquestionnaire.D_u_th_i_gian.ToString(),
                    /// <summary>
                    /// Xuất xứ 
                    /// </summary>
                    A1__Product_origin = A1__Product_origin1,
                    /// <summary>
                    /// Mã code sản phẩm
                    /// </summary>
                    A2__Product_code = oldquestionnaire.A2__Product_code,
                    /// <summary>
                    /// Tên sản phẩm
                    /// </summary>
                    A3__Product_name = oldquestionnaire.A3__Product_name,
                    /// <summary>
                    /// Tên công ty đăng ký sản phẩm
                    /// </summary>
                    A4__Company = oldquestionnaire.A4__Company,
                    /// <summary>
                    /// Loại sản phẩm ( dang bột - dạng dung dịch )
                    /// </summary>
                    A5__Type_of_product = A5__Type_of_product1,
                    /// <summary>
                    /// Có chứa chất ngoài kháng sinh hay không?
                    /// </summary>
                    A6__Other_subtance_in_product = A6__Other_subtance_in_product1,
                    /// <summary>
                    /// Khối lượng/trọng lượng/thể tích của sản phẩm 
                    /// </summary>
                    A7__Volume_of_product = oldquestionnaire.A7__Volume_of_product,
                    /// <summary>
                    /// Đơn vị của khối lượng/trọng lượng/thể tích của sản phẩm 
                    /// </summary>
                    A8__Unit_of_volume_of_product = A8__Unit_of_volume_of_product1,
                    /// <summary>
                    /// Thông tin về khối lượng khác 
                    /// </summary>
                    A9__Other_volume_of_product = oldquestionnaire.A9__Other_volume_of_product,
                    #endregion

                    #region B.Information related to antimicrobial
                    /// <summary>
                    /// Số loại kháng sinh co trong sản phẩm
                    /// </summary>
                    B1__Number_of_antimicrobials_in_product = oldquestionnaire.B1__Number_of_antimicrobials_in_product,

                    #region Antimicrobials 1
                    /// <summary>
                    /// Antimicrobials_1
                    /// </summary>
                    B2_1__Antimicrobial_1 = oldquestionnaire.B2_1__Antimicrobial_1,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 1 - chỉ điền số
                    /// </summary>
                    B2_2__Strength_of_antimicrobial_1 = oldquestionnaire.B2_2__Strength_of_antimicrobial_1,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 1
                    /// </summary>
                    B2_3__Units_of_antimicrobial_1 = B2_3__Units_of_antimicrobial_11,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B2_4__Per_amount_of_product__antimicrobial_1_ = oldquestionnaire.B2_4__Per_amount_of_product__antimicrobial_1_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ = B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_1,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ = oldquestionnaire.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B2_7__Units_of_product__link_to_question_B2_4_ = B2_7__Units_of_product__link_to_question_B2_4_1,
                    #endregion
                    #region Antimicrobials 2
                    /// <summary>
                    /// Antimicrobials_2
                    /// </summary>
                    B3_1__Antimicrobial_2 = oldquestionnaire.B3_1__Antimicrobial_2,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 2 - chỉ điền số
                    /// </summary>
                    B3_2__Strength_of_antimicrobial_2 = oldquestionnaire.B3_2__Strength_of_antimicrobial_2,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 2
                    /// </summary>
                    B3_3__Units_of_antimicrobial_2 = B3_3__Units_of_antimicrobial_21,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B3_4__Per_amount_of_product__antimicrobial_2_ = oldquestionnaire.B3_4__Per_amount_of_product__antimicrobial_2_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ = B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_1,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ = oldquestionnaire.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B3_7__Units_of_product__link_to_question_B3_4_ = B3_7__Units_of_product__link_to_question_B3_4_1,
                    #endregion
                    #region Antimicrobials 3
                    /// <summary>
                    /// Antimicrobials_3
                    /// </summary>
                    B4_1__Antimicrobial_3 = oldquestionnaire.B4_1__Antimicrobial_3,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 3 - chỉ điền số
                    /// </summary>
                    B4_2__Strength_of_antimicrobial_3 = oldquestionnaire.B4_2__Strength_of_antimicrobial_3,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 3
                    /// </summary>
                    B4_3__Units_of_antimicrobial_3 = B4_3__Units_of_antimicrobial_31,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B4_4__Per_amount_of_product__antimicrobial_3_ = oldquestionnaire.B4_4__Per_amount_of_product__antimicrobial_3_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ = B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_1,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ = oldquestionnaire.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B4_7__Units_of_product__link_to_question_B4_4_ = B4_7__Units_of_product__link_to_question_B4_4_1,
                    #endregion
                    #region Antimicrobials 4
                    /// <summary>
                    /// Antimicrobials_4
                    /// </summary>
                    B5_1__Antimicrobial_4 = oldquestionnaire.B5_1__Antimicrobial_4,
                    /// <summary>
                    /// Thông tin về lượng kháng sinh 4 - chỉ điền số
                    /// </summary>
                    B5_2__Strength_of_antimicrobial_4 = oldquestionnaire.B5_2__Strength_of_antimicrobial_4,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh 4
                    /// </summary>
                    B5_3__Units_of_antimicrobial_4 = B5_3__Units_of_antimicrobial_41,
                    /// <summary>
                    /// Đơn vị khối lượng mỗi loại
                    /// </summary>
                    B5_4__Per_amount_of_product__antimicrobial_4_ = oldquestionnaire.B5_4__Per_amount_of_product__antimicrobial_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ = B5_5__Units_of_antimicrobial_4__link_to_question_5_4_1,
                    /// <summary>
                    /// khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ = oldquestionnaire.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_,
                    /// <summary>
                    /// Đơn vị khối lượng kháng sinh cho mỗi loại
                    /// </summary>
                    B5_7__Units_of_product__link_to_question_B5_4_ = B5_7__Units_of_product__link_to_question_B5_4_1,
                    #endregion

                    /// <summary>
                    /// Các loài vật
                    /// </summary>
                    B6__Target_species_x = oldquestionnaire.B6__Target_species_x,
                    /// <summary>
                    /// Đường dùng thuốc 
                    /// </summary>
                    B7__Administration_route = oldquestionnaire.B7__Administration_route,
                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho heo
                    #region C_ Heo

                    #region C_1_ Product preparation (dilution) _pig_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    C1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.C1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    C1_2__Product_preparation_Unit_of_product = C1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    C1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.C1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    C1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.C1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    C1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.C1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C1_6__Duration_of_usage__min__max_ = oldquestionnaire.C1_6__Duration_of_usage__min__max_,
                    #endregion

                    #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    C2_1__Product_min = oldquestionnaire.C2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    C2_2__Product_max = oldquestionnaire.C2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    C2_3__Unit_of_product = C2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    C2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.C2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    C2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.C2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C2_6__Duration_of_usage = oldquestionnaire.C2_6__Duration_of_usage,
                    #endregion

                    #region C_3 Product preparation (dilution) _pig_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    C3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.C3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    C3_2__Product_preparation_Unit_of_product = C3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    C3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.C3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    C3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.C3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    C3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.C3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C3_6__Duration_of_usage = oldquestionnaire.C3_6__Duration_of_usage,


                    #endregion

                    #region C_4 Guidelines referred to bodyweight_pig_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    C4_1__Product_min = oldquestionnaire.C4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    C4_2__Product_max = oldquestionnaire.C4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    C4_3__Unit_of_product = C4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    C4_4__Per_No__Kg_bodyweight_min = oldquestionnaire.C4_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    C4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.C4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    C4_6__Duration_of_usage = oldquestionnaire.C4_6__Duration_of_usage,
                    #endregion

                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho thú nhai lại không phân biệt thú lớn và nhỏ
                    #region D. Động vật nhai lại

                    #region D_1_ Product preparation (dilution) _ruminant_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    D1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.D1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    D1_2__Product_preparation_Unit_of_product = D1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    D1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.D1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    D1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.D1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    D1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.D1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D1_6__Duration_of_usage = oldquestionnaire.D1_6__Duration_of_usage,
                    #endregion

                    #region D.2 Guidelines referred to bodyweight_ruminant_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    D2_1__Product_min = oldquestionnaire.D2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    D2_2__Product_max = oldquestionnaire.D2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    D2_3__Unit_of_product = D2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    D2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.D2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    D2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.D2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D2_6__Duration_of_usage = oldquestionnaire.D2_6__Duration_of_usage,
                    #endregion

                    #region D_3 Product preparation (dilution) _ruminant_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    D3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.D3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    D3_2__Product_preparation_Unit_of_product = D3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    D3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.D3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    D3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.D3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    D3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.D3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D3_6__Duration_of_usage = oldquestionnaire.D3_6__Duration_of_usage,


                    #endregion

                    #region D_4 Guidelines referred to bodyweight_ruminant_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    D4_1__Product_min = oldquestionnaire.D4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    D4_2__Product_max = oldquestionnaire.D4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    D4_3__Unit_of_product = D4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    D4_4__Per_No__Kg_bodyweight_min = oldquestionnaire.D4_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    D4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.D4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    D4_6__Duration_of_usage = oldquestionnaire.D4_6__Duration_of_usage,
                    #endregion

                    #endregion

                    //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho gia cầm nói chung bao gồm gà, vịt, ngan, ngỗng, cút
                    #region E. Gia cầm

                    #region D_1_ Product preparation (dilution) _poultry_prevention purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    E1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.E1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    E1_2__Product_preparation_Unit_of_product = E1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    E1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.E1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    E1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.E1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    E1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.E1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E1_6__Duration_of_usage = oldquestionnaire.E1_6__Duration_of_usage,
                    #endregion

                    #region D.2 Guidelines referred to bodyweight_poultry_prevention purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    E2_1__Product_min = oldquestionnaire.E2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    E2_2__Product_max = oldquestionnaire.E2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    E2_3__Unit_of_product = E2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    E2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.E2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    E2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.E2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E2_6__Duration_of_usage = oldquestionnaire.E2_6__Duration_of_usage,
                    #endregion

                    #region D_3 Product preparation (dilution) _poultry_treatment purpose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    E3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.E3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    E3_2__Product_preparation_Unit_of_product = E3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    E3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.E3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    E3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.E3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    E3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.E3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E3_6__Duration_of_usage = oldquestionnaire.E3_6__Duration_of_usage,


                    #endregion

                    #region D_4 Guidelines referred to bodyweight_poultry_treatment purpose
                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    E4_1__Product_min = oldquestionnaire.E4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    E4_2__Product_max = oldquestionnaire.E4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    E4_3__Unit_of_product = E4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    E4_4__Per_No__Kg_bodyweight_min = oldquestionnaire.E4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    E4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.E4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    E4_6__Duration_of_usage = oldquestionnaire.E4_6__Duration_of_usage,

                    #endregion


                    #endregion

                    //Phần này bao gồm các thông tin thêm về sản phẩm và người nhập dữ liệu để tiện cho việc kiểm tra tính xác thực của thông tin
                    #region F.Further information

                    /// <summary>
                    /// Điền trang web hoặc vetshop
                    /// </summary>
                    F_1__Source_of_information = oldquestionnaire.F_1__Source_of_information,
                    /// <summary>
                    /// Ảnh sản phẩm
                    /// </summary>
                    F_2__Picture_of_product = oldquestionnaire.F_2__Picture_of_product,
                    /// <summary>
                    /// Thông tin khác
                    /// </summary>
                    F3__Correction = oldquestionnaire.F3__Correction,
                    /// <summary>
                    /// Thông tin người nhập
                    /// </summary>
                    F_4__Person_in_charge = oldquestionnaire.F_4__Person_in_charge,
                    /// <summary>
                    /// thời gian cho việc tìm kiếm thu và nhập thông tin sản phẩm
                    /// </summary>
                    F_5__Working_time = oldquestionnaire.F_5__Working_time,
                    /// <summary>
                    /// Ghi chú
                    /// </summary>
                    F_6__Note = oldquestionnaire.F_6__Note,

                    #endregion

                    //Heo con
                    #region G.Piglet
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Piglet_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Piglet_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Piglet_1_2__Product_preparation_Unit_of_product = Piglet_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Piglet_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Piglet_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Piglet_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Piglet_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Piglet_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Piglet_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_1_6__Duration_of_usage = oldquestionnaire.Piglet_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Piglet_2_1__Product_min = oldquestionnaire.Piglet_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Piglet_2_2__Product_max = oldquestionnaire.Piglet_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Piglet_2_3__Unit_of_product = Piglet_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Piglet_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Piglet_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Piglet_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Piglet_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_2_6__Duration_of_usage = oldquestionnaire.Piglet_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Piglet_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Piglet_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Piglet_3_2__Product_preparation_Unit_of_product = Piglet_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Piglet_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Piglet_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Piglet_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Piglet_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Piglet_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Piglet_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_3_6__Duration_of_usage = oldquestionnaire.Piglet_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Piglet_4_1__Product_min = oldquestionnaire.Piglet_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Piglet_4_2__Product_max = oldquestionnaire.Piglet_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Piglet_4_3__Unit_of_product = Piglet_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Piglet_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Piglet_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Piglet_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Piglet_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Piglet_4_6__Duration_of_usage = oldquestionnaire.Piglet_4_6__Duration_of_usage,


                    #endregion

                    //Trâu
                    #region H.Buffalo
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Buffalo_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Buffalo_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Buffalo_1_2__Product_preparation_Unit_of_product = Buffalo_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Buffalo_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Buffalo_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Buffalo_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Buffalo_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Buffalo_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Buffalo_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_1_6__Duration_of_usage = oldquestionnaire.Buffalo_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Buffalo_2_1__Product_min = oldquestionnaire.Buffalo_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Buffalo_2_2__Product_max = oldquestionnaire.Buffalo_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Buffalo_2_3__Unit_of_product = Buffalo_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Buffalo_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Buffalo_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Buffalo_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Buffalo_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_2_6__Duration_of_usage = oldquestionnaire.Buffalo_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Buffalo_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Buffalo_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Buffalo_3_2__Product_preparation_Unit_of_product = Buffalo_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Buffalo_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Buffalo_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Buffalo_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Buffalo_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Buffalo_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Buffalo_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_3_6__Duration_of_usage = oldquestionnaire.Buffalo_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Buffalo_4_1__Product_min = oldquestionnaire.Buffalo_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Buffalo_4_2__Product_max = oldquestionnaire.Buffalo_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Buffalo_4_3__Unit_of_product = Buffalo_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Buffalo_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Buffalo_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Buffalo_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Buffalo_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Buffalo_4_6__Duration_of_usage = oldquestionnaire.Buffalo_4_6__Duration_of_usage,



                    #endregion

                    //Gia súc
                    #region I.Cattle
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cattle_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cattle_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cattle_1_2__Product_preparation_Unit_of_product = Cattle_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cattle_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cattle_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cattle_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cattle_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cattle_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cattle_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_1_6__Duration_of_usages = oldquestionnaire.Cattle_1_6__Duration_of_usages,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Cattle_2_1__Product_min = oldquestionnaire.Cattle_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cattle_2_2__Product_max = oldquestionnaire.Cattle_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cattle_2_3__Unit_of_product = Cattle_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cattle_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Cattle_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cattle_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cattle_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_2_6__Duration_of_usage = oldquestionnaire.Cattle_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cattle_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cattle_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cattle_3_2__Product_preparation_Unit_of_product = Cattle_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cattle_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cattle_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cattle_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cattle_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cattle_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cattle_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_3_6__Duration_of_usage = oldquestionnaire.Cattle_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Cattle_4_1__Product_min = oldquestionnaire.Cattle_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cattle_4_2__Product_max = oldquestionnaire.Cattle_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cattle_4_3__Unit_of_product = Cattle_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cattle_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Cattle_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cattle_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cattle_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cattle_4_6__Duration_of_usage = oldquestionnaire.Cattle_4_6__Duration_of_usage,


                    #endregion

                    //Dê
                    #region J.Goat
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goat_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goat_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goat_1_2__Product_preparation_Unit_of_product = Goat_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goat_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goat_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goat_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goat_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goat_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goat_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_1_6__Duration_of_usage = oldquestionnaire.Goat_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Goat_2_1__Product_min = oldquestionnaire.Goat_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goat_2_2__Product_max = oldquestionnaire.Goat_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goat_2_3__Unit_of_product = Goat_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goat_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Goat_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goat_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goat_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_2_6__Duration_of_usage = oldquestionnaire.Goat_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goat_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goat_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goat_3_2__Product_preparation_Unit_of_product = Goat_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goat_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goat_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goat_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goat_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goat_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goat_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_3_6__Duration_of_usage = oldquestionnaire.Goat_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Goat_4_1__Product_min = oldquestionnaire.Goat_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goat_4_2__Product_max = oldquestionnaire.Goat_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goat_4_3__Unit_of_product = Goat_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goat_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Goat_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goat_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goat_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goat_4_6__Duration_of_usage = oldquestionnaire.Goat_4_6__Duration_of_usage,

                    #endregion

                    //Cừu
                    #region K.Sheep
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Sheep_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Sheep_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Sheep_1_2__Product_preparation_Unit_of_product = Sheep_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Sheep_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Sheep_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Sheep_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Sheep_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Sheep_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Sheep_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_1_6__Duration_of_usage = oldquestionnaire.Sheep_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Sheep_2_1__Product_min = oldquestionnaire.Sheep_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Sheep_2_2__Product_max = oldquestionnaire.Sheep_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Sheep_2_3__Unit_of_product = Sheep_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Sheep_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Sheep_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Sheep_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Sheep_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_2_6__Duration_of_usage = oldquestionnaire.Sheep_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Sheep_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Sheep_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Sheep_3_2__Product_preparation_Unit_of_product = Sheep_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Sheep_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Sheep_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Sheep_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Sheep_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Sheep_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Sheep_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_3_6__Duration_of_usage = oldquestionnaire.Sheep_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Sheep_4_1__Product_min = oldquestionnaire.Sheep_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Sheep_4_2__Product_max = oldquestionnaire.Sheep_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Sheep_4_3__Unit_of_product = Sheep_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Sheep_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Sheep_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Sheep_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Sheep_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Sheep_4_6__Duration_of_usage = oldquestionnaire.Sheep_4_6__Duration_of_usage,

                    #endregion

                    //Ngựa
                    #region L.Horse
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Horse_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Horse_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Horse_1_2__Product_preparation_Unit_of_product = Horse_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Horse_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Horse_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Horse_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Horse_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Horse_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Horse_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_1_6__Duration_of_usage = oldquestionnaire.Horse_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Horse_2_1__Product_min = oldquestionnaire.Horse_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Horse_2_2__Product_max = oldquestionnaire.Horse_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Horse_2_3__Unit_of_product = Horse_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Horse_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Horse_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Horse_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Horse_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_2_6__Duration_of_usage = oldquestionnaire.Horse_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Horse_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Horse_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Horse_3_2__Product_preparation_Unit_of_product = Horse_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Horse_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Horse_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Horse_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Horse_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Horse_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Horse_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_3_6__Duration_of_usage = oldquestionnaire.Horse_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Horse_4_1__Product_min = oldquestionnaire.Horse_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Horse_4_2__Product_max = oldquestionnaire.Horse_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Horse_4_3__Unit_of_product = Horse_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Horse_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Horse_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Horse_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Horse_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Horse_4_6__Duration_of_usage = oldquestionnaire.Horse_4_6__Duration_of_usage,

                    #endregion

                    //Gà
                    #region M.Chicken
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chicken_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chicken_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chicken_1_2__Product_preparation_Unit_of_product = Chicken_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chicken_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chicken_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chicken_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chicken_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chicken_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chicken_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_1_6__Duration_of_usage = oldquestionnaire.Chicken_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Chicken_2_1__Product_min = oldquestionnaire.Chicken_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chicken_2_2__Product_max = oldquestionnaire.Chicken_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chicken_2_3__Unit_of_product = Chicken_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chicken_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Chicken_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chicken_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chicken_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_2_6__Duration_of_usage = oldquestionnaire.Chicken_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chicken_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chicken_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chicken_3_2__Product_preparation_Unit_of_product = Chicken_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chicken_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chicken_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chicken_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chicken_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chicken_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chicken_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_3_6__Duration_of_usage = oldquestionnaire.Chicken_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Chicken_4_1__Product_min = oldquestionnaire.Chicken_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chicken_4_2__Product_max = oldquestionnaire.Chicken_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chicken_4_3__Unit_of_product = Chicken_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chicken_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Chicken_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chicken_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chicken_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chicken_4_6__Duration_of_usage = oldquestionnaire.Chicken_4_6__Duration_of_usage,

                    #endregion

                    //Vịt
                    #region N.Duck
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Duck_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Duck_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Duck_1_2__Product_preparation_Unit_of_product = Duck_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Duck_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Duck_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Duck_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Duck_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Duck_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Duck_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_1_6__Duration_of_usage = oldquestionnaire.Duck_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Duck_2_1__Product_min = oldquestionnaire.Duck_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Duck_2_2__Product_max = oldquestionnaire.Duck_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Duck_2_3__Unit_of_product = Duck_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Duck_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Duck_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Duck_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Duck_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_2_6__Duration_of_usage = oldquestionnaire.Duck_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Duck_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Duck_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Duck_3_2__Product_preparation_Unit_of_product = Duck_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Duck_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Duck_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Duck_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Duck_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Duck_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Duck_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_3_6__Duration_of_usage = oldquestionnaire.Duck_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Duck_4_1__Product_min = oldquestionnaire.Duck_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Duck_4_2__Product_max = oldquestionnaire.Duck_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Duck_4_3__Unit_of_product = Duck_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Duck_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Duck_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Duck_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Duck_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Duck_4_6__Duration_of_usage = oldquestionnaire.Duck_4_6__Duration_of_usage,

                    #endregion

                    //Vịt Xiêm
                    #region O.Muscovy_Duck
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Muscovy_Duck_1_2__Product_preparation_Unit_of_product = Muscovy_Duck_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_1_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Muscovy_Duck_2_1__Product_min = oldquestionnaire.Muscovy_Duck_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Muscovy_Duck_2_2__Product_max = oldquestionnaire.Muscovy_Duck_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Muscovy_Duck_2_3__Unit_of_product = Muscovy_Duck_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_2_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Muscovy_Duck_3_2__Product_preparation_Unit_of_product = Muscovy_Duck_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_3_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Muscovy_Duck_4_1__Product_min = oldquestionnaire.Muscovy_Duck_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Muscovy_Duck_4_2__Product_max = oldquestionnaire.Muscovy_Duck_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Muscovy_Duck_4_3__Unit_of_product = Muscovy_Duck_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Muscovy_Duck_4_6__Duration_of_usage = oldquestionnaire.Muscovy_Duck_4_6__Duration_of_usage,

                    #endregion

                    //Cút
                    #region P.Quail
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Quail_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Quail_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Quail_1_2__Product_preparation_Unit_of_product = Quail_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Quail_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Quail_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Quail_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Quail_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Quail_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Quail_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_1_6__Duration_of_usage = oldquestionnaire.Quail_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Quail_2_1__Product_min = oldquestionnaire.Quail_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Quail_2_2__Product_max = oldquestionnaire.Quail_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Quail_2_3__Unit_of_product = Quail_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Quail_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Quail_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Quail_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Quail_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_2_6__Duration_of_usage = oldquestionnaire.Quail_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Quail_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Quail_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Quail_3_2__Product_preparation_Unit_of_product = Quail_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Quail_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Quail_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Quail_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Quail_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Quail_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Quail_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_3_6__Duration_of_usage = oldquestionnaire.Quail_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Quail_4_1__Product_min = oldquestionnaire.Quail_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Quail_4_2__Product_max = oldquestionnaire.Quail_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Quail_4_3__Unit_of_product = Quail_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Quail_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Quail_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Quail_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Quail_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Quail_4_6__Duration_of_usage = oldquestionnaire.Quail_4_6__Duration_of_usage,

                    #endregion

                    //Ngỗng
                    #region Q.Goose
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goose_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goose_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goose_1_2__Product_preparation_Unit_of_product = Goose_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goose_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goose_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goose_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goose_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goose_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goose_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_1_6__Duration_of_usage = oldquestionnaire.Goose_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Goose_2_1__Product_min = oldquestionnaire.Goose_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goose_2_2__Product_max = oldquestionnaire.Goose_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goose_2_3__Unit_of_product = Goose_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goose_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Goose_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goose_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goose_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_2_6__Duration_of_usage = oldquestionnaire.Goose_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Goose_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Goose_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Goose_3_2__Product_preparation_Unit_of_product = Goose_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Goose_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Goose_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Goose_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Goose_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Goose_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Goose_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_3_6__Duration_of_usage = oldquestionnaire.Goose_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Goose_4_1__Product_min = oldquestionnaire.Goose_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Goose_4_2__Product_max = oldquestionnaire.Goose_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Goose_4_3__Unit_of_product = Goose_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Goose_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Goose_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Goose_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Goose_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Goose_4_6__Duration_of_usage = oldquestionnaire.Goose_4_6__Duration_of_usage,

                    #endregion

                    //Chó
                    #region R.Dog
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Dog_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Dog_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Dog_1_2__Product_preparation_Unit_of_product = Dog_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Dog_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Dog_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Dog_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Dog_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Dog_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Dog_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_1_6__Duration_of_usage = oldquestionnaire.Dog_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Dog_2_1__Product_min = oldquestionnaire.Dog_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Dog_2_2__Product_max = oldquestionnaire.Dog_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Dog_2_3__Unit_of_product = Dog_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Dog_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Dog_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Dog_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Dog_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_2_6__Duration_of_usage = oldquestionnaire.Dog_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Dog_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Dog_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Dog_3_2__Product_preparation_Unit_of_product = Dog_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Dog_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Dog_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Dog_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Dog_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Dog_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Dog_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_3_6__Duration_of_usage = oldquestionnaire.Dog_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Dog_4_1__Product_min = oldquestionnaire.Dog_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Dog_4_2__Product_max = oldquestionnaire.Dog_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Dog_4_3__Unit_of_product = Dog_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Dog_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Dog_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Dog_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Dog_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Dog_4_6__Duration_of_usage = oldquestionnaire.Dog_4_6__Duration_of_usage,

                    #endregion

                    //Mèo
                    #region S.Cat
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cat_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cat_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cat_1_2__Product_preparation_Unit_of_product = Cat_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cat_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cat_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cat_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cat_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cat_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cat_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_1_6__Duration_of_usage = oldquestionnaire.Cat_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Cat_2_1__Product_min = oldquestionnaire.Cat_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cat_2_2__Product_max = oldquestionnaire.Cat_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cat_2_3__Unit_of_product = Cat_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cat_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Cat_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cat_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cat_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_2_6__Duration_of_usage = oldquestionnaire.Cat_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Cat_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Cat_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Cat_3_2__Product_preparation_Unit_of_product = Cat_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Cat_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Cat_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Cat_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Cat_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Cat_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Cat_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_3_6__Duration_of_usage = oldquestionnaire.Cat_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Cat_4_1__Product_min = oldquestionnaire.Cat_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Cat_4_2__Product_max = oldquestionnaire.Cat_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Cat_4_3__Unit_of_product = Cat_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Cat_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Cat_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Cat_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Cat_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Cat_4_6__Duration_of_usage = oldquestionnaire.Cat_4_6__Duration_of_usage,

                    #endregion

                    //Bê
                    #region T.Calf
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Calf_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Calf_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Calf_1_2__Product_preparation_Unit_of_product = Calf_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Calf_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Calf_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Calf_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Calf_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Calf_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Calf_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_1_6__Duration_of_usage = oldquestionnaire.Calf_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Calf_2_1__Product_min = oldquestionnaire.Calf_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Calf_2_2__Product_max = oldquestionnaire.Calf_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Calf_2_3__Unit_of_product = Calf_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Calf_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Calf_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Calf_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Calf_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_2_6__Duration_of_usage = oldquestionnaire.Calf_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Calf_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Calf_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Calf_3_2__Product_preparation_Unit_of_product = Calf_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Calf_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Calf_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Calf_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Calf_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Calf_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Calf_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_3_6__Duration_of_usage = oldquestionnaire.Calf_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Calf_4_1__Product_min = oldquestionnaire.Calf_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Calf_4_2__Product_max = oldquestionnaire.Calf_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Calf_4_3__Unit_of_product = Calf_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Calf_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Calf_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Calf_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Calf_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Calf_4_6__Duration_of_usage = oldquestionnaire.Calf_4_6__Duration_of_usage,

                    #endregion

                    //Gà con
                    #region U.Chick
                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chick_1_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chick_1_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chick_1_2__Product_preparation_Unit_of_product = Chick_1_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chick_1_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chick_1_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chick_1_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chick_1_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chick_1_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chick_1_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_1_6__Duration_of_usage = oldquestionnaire.Chick_1_6__Duration_of_usage,


                    /// <summary>
                    /// Lượng thuốc tối thiểu
                    /// </summary>
                    Chick_2_1__Product_min = oldquestionnaire.Chick_2_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chick_2_2__Product_max = oldquestionnaire.Chick_2_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chick_2_3__Unit_of_product = Chick_2_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chick_2_4__Per_No__Kg_bodyweight_min = oldquestionnaire.Chick_2_4__Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chick_2_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chick_2_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_2_6__Duration_of_usage = oldquestionnaire.Chick_2_6__Duration_of_usage,


                    /// <summary>
                    /// sẩn phẩm pha
                    /// </summary>
                    Chick_3_1__Product_preparation__dilution__Product_amount = oldquestionnaire.Chick_3_1__Product_preparation__dilution__Product_amount,
                    /// <summary>
                    /// Lượng thuốc để pha
                    /// </summary>
                    Chick_3_2__Product_preparation_Unit_of_product = Chick_3_2__Product_preparation_Unit_of_product1,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
                    /// </summary>
                    Chick_3_3__Product_preparation_To_be_added_to__min_ = oldquestionnaire.Chick_3_3__Product_preparation_To_be_added_to__min_,
                    /// <summary>
                    /// Lượng nước hoặc thức ăn tối đa để pha thuốc
                    /// </summary>
                    Chick_3_4__Product_preparation_To_be_added_to__max_ = oldquestionnaire.Chick_3_4__Product_preparation_To_be_added_to__max_,
                    /// <summary>
                    /// Đơn vị tính lượng nước hoặc thức ăn 
                    /// </summary>
                    Chick_3_5__Product_preparation_Unit_of_water_feed = oldquestionnaire.Chick_3_5__Product_preparation_Unit_of_water_feed,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_3_6__Duration_of_usage = oldquestionnaire.Chick_3_6__Duration_of_usage,



                    /// <summary>
                    /// Lượng thuốc tối thiểu                                                                             
                    /// </summary>                                                                             
                    Chick_4_1__Product_min = oldquestionnaire.Chick_4_1__Product_min,
                    /// <summary>
                    /// Lượng thuốc tối đa
                    /// </summary>
                    Chick_4_2__Product_max = oldquestionnaire.Chick_4_2__Product_max,
                    /// <summary>
                    /// đơn vị tính
                    /// </summary>
                    Chick_4_3__Unit_of_product = Chick_4_3__Unit_of_product1,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối thiểu
                    /// </summary>
                    Chick_4_4_Per_No__Kg_bodyweight_min = oldquestionnaire.Chick_4_4_Per_No__Kg_bodyweight_min,
                    /// <summary>
                    /// Trọng lượng mỗi Kg tối đa
                    /// </summary>
                    Chick_4_5__Per_No__Kg_bodyweight_max = oldquestionnaire.Chick_4_5__Per_No__Kg_bodyweight_max,
                    /// <summary>
                    /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
                    /// </summary>
                    Chick_4_6__Duration_of_usage = oldquestionnaire.Chick_4_6__Duration_of_usage,

                    #endregion

                    State = state1
                };
                #endregion
                #endregion
                auditTrailBussiness.CreateAuditTrail(AuditActionType.Update, questionnaire.Id, "Dữ liệu", oldobject, newobject, user.Username);
                return Json(new { success = true, message = "Lưu dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
        }

        [PermissionBasedAuthorize("DATA_INPUT")]
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
    }
}