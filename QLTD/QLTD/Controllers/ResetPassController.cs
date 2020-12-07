using Ehr.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ehr.Models;
using System.IO;
using Ehr.Auth;
using Ehr.Common.UI;


namespace Ehr.Controllers
{
    public class ResetPassController : Controller
    {
        private readonly UnitWork unitWork;

        public ResetPassController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        // GET: ResetPass
   
        public ActionResult Index()
        {
            string Usr = Request.Form.Get("txtUser");
            if (Usr == null || Usr == "")
                return View();
            else
            {
                var Usr_list = unitWork.User.Get().Where(u => u.Username.ToLower().Contains(Usr.ToLower())).ToList();

                if (Usr_list == null || Usr_list.Count() < 1)
                {
                    ViewBag.mess = "Không Tìm Thấy UserName";
                    return View();
                }
                else
                {
                    
                    return View("Pass_Reset",Usr_list);
                }
            }
            
            
        }

       
        [HttpGet]

        public ActionResult Pass_Reset(string id)
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Pass_Reset()
        {
            string pass1 = Request.Form.Get("Pass1");
            string pass2 = Request.Form.Get("Pass2");
            string idd = Request.Form.Get("id");
            var usr = unitWork.User.GetById(int.Parse(idd));
            if(pass1!=""&&pass2!="")
            {
                if (pass1 == pass2)
                {
                    
                    usr.Password = Utilities.Encrypt(pass2);
                    unitWork.User.Update(usr);
                    unitWork.Commit();
                    return View("index");
                }
                else return View();
            }
            
            else return View();
        }
            
            
    }
}