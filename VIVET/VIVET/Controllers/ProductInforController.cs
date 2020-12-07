using Ehr.Bussiness;
using Ehr.Common.UI;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class ProductInforController : BaseController
    {
        private readonly UnitWork unitWork;
        public ProductInforController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        public ActionResult Index()
        {
            ViewBag.CurUser = unitWork.User.Get(x => x.Id == this.User.UserId).FirstOrDefault();
            ViewBag.User = unitWork.User.Get();
            return View();
        }

        // GET: ProductInfor
        public ActionResult Show(string RangeDate, int? accountid, int? province)
        {
            var listProductInfor = new List<ProductInfor>();

            string searchString = "";

            if (Request.Form.Get("searchString") != null)
            {
                searchString = Request.Form.Get("searchString");
            }
            DateTime endDate = DateTime.MinValue;
            DateTime startDate = DateTime.MaxValue;
            if (RangeDate != null)
            {
                string daterange = RangeDate;
                try
                {
                    string temp = daterange.Replace(" ", "");
                    string[] sp = temp.Split('-');
                    if (sp.Length > 1)
                    {
                        startDate = DataConverter.UI2DateTimeRange(sp[0], true);
                        endDate = DataConverter.UI2DateTimeRange(sp[1], false);
                    }
                }
                catch { }
            }
            var productinfor = unitWork.ProductInfor.Get();

            if (RangeDate.Length > 0)
            {
                if (startDate != DateTime.MinValue)
                    productinfor = productinfor.Where(c => c.DateStamp >= startDate);
                if (endDate != DateTime.MinValue)
                    productinfor = productinfor.Where(c => c.DateStamp <= endDate);
            }
            if(province==-1)
            {
                //province hiện tại của user đó
                var usercurrent = unitWork.User.Get(c => c.Id == User.UserId).FirstOrDefault();
                var userprovince = unitWork.User.Get(x => x.Province == usercurrent.Province).Select(x => x.Id);
                if (accountid > 0)
                {
                    if(usercurrent.UserType== Common.Constraint.UserType.LEAD)
                    {
                        productinfor = productinfor.Where(c => userprovince.Any(a => a == accountid));
                    }
                    else
                    {
                        productinfor = productinfor.Where(c => c.UserId==usercurrent.Id);
                    }
                    
                }
            }
            else if (province > 0)
            {
                var userprovince = unitWork.User.Get(x => x.Province == province).Select(x => x.Id);
                if (accountid > 0)
                {
                    productinfor = productinfor.Where(c => userprovince.Any(a => a == accountid));
                }else if(accountid == 0)
                {
                    productinfor = productinfor.Where(x => userprovince.Any(a => a == x.UserId));
                }
            }
            

            if (accountid > 0)
            {
                productinfor = productinfor.Where(c => c.UserId == accountid);
            }

            #region Phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString) && searchString != "")
            {
                productinfor = productinfor.Where(c => (c.Code != null && c.Code.ToLower().Contains(searchString.ToLower().Trim()))
                                                || c.CollectedDate.ToString().ToLower().Contains(searchString.ToLower().Trim())
                                                || c.ProductId.ToString().ToLower().Contains(searchString.ToLower().Trim())
                                                || c.Pet.ToString().ToLower().Contains(searchString.ToLower().Trim())
                                                || c.Antimicrobial.ToString().ToLower().Contains(searchString.ToLower().Trim())
                                                || c.Product.ToString().ToLower().Contains(searchString.ToLower().Trim()));
            }
            #endregion

            listProductInfor = productinfor.OrderByDescending(x => x.CollectedDate).ToList();

            int items = 12;
            int page = 1;
            if (Request.Form.Get("selPage") != null)
            {
                page = int.Parse(Request.Form.Get("selPage").ToString());
            }
            if (Request.Form.Get("numItems") != null)
            {
                items = int.Parse(Request.Form.Get("numItems").ToString());
            }
            int count = productinfor.Count();
            EZPaging ezpage = new EZPaging(page, items, count, "updatePage", "");
            ViewBag.Paging = ezpage.PageModel.HTML;
            ViewBag.Label = ezpage.PageModel.Label;
            if (ezpage.PageModel.StartItem >= 1 && count > 0)
            {
                listProductInfor = listProductInfor.Skip(ezpage.PageModel.StartItem - 1).Take(ezpage.PageModel.StopItem - ezpage.PageModel.StartItem + 1).ToList();
            }
            return PartialView("_ProductInfor", listProductInfor);
        }

        // Load user by province
        public JsonResult LoadUserByProvince(int? province)
        {
            if ( province == null)
            {
                var json = new
                {
                    id = "",
                    text = ""
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }

            var currentuser = unitWork.User.GetById(this.User.UserId);

            if (province != -1)
            {
                if(province == 0)
                {
                    var ps1 = (from f in unitWork.User.Get()
                               select new
                               {
                                   id = f.Id,
                                   text = f.FullName
                               }).AsEnumerable().OrderBy(c => c.text).Distinct().ToList();
                    return Json(ps1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ps2 = (from f in unitWork.User.Get()
                               where f.Province == province
                               select new
                               {
                                   id = f.Id,
                                   text = f.FullName
                               }).AsEnumerable().OrderBy(c => c.text).Distinct().ToList();
                    return Json(ps2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if(currentuser.UserType == Common.Constraint.UserType.LEAD)
                {
                    var ps1 = (from f in unitWork.User.Get()
                               where f.Province == currentuser.Province
                               select new
                               {
                                   id = f.Id,
                                   text = f.FullName
                               }).AsEnumerable().OrderBy(c => c.text).Distinct().ToList();
                    return Json(ps1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ps2 = (from f in unitWork.User.Get()
                               where f.Id == currentuser.Id
                               select new
                               {
                                   id = f.Id,
                                   text = f.FullName
                               }).AsEnumerable().OrderBy(c => c.text).Distinct().ToList();
                    return Json(ps2, JsonRequestBehavior.AllowGet);
                }
            }

            
          
        }
    }
}