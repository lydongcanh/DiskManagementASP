using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ehr.Models;
using Ehr.Bussiness;
using Ehr.Auth;
using Ehr.Common.UI;
using System.Net;
using Ehr.Common.Constraint;

namespace Ehr.Controllers
{
    public class MailConfigController : BaseController
    {
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;

        public MailConfigController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
        }
        
        [HttpGet]
        [PermissionBasedAuthorize("SYSTEM_CFG")]
        public ActionResult Index()
        {
            MailConfig mail = unitWork.MailConfig.Get().FirstOrDefault();
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }

        [HttpPost]
        [PermissionBasedAuthorize("SYSTEM_CFG")]
        public ActionResult Update()
        {
            var id = Request.Form.Get("Id");
            var item = unitWork.MailConfig.GetById(int.Parse(id));
            #region  Old Object
            Ehr.Data.EhrDbContext db = new Data.EhrDbContext();
            var oldObject = (from b in db.MailConfigs
                             select new
                             {
                                 ServerAddress = item.ServerAddress,
                                 Port = item.Port,
                                 UseSSL = item.UseSSL,
                                 EmailSend = item.EmailSend,
                                 EmailCC = item.EmailCC,
                                 Username = item.Username,
                                 Password = item.Password,
                             }).FirstOrDefault();
            #endregion

            item.ServerAddress = Request.Form.Get("txtServerAddress");
            item.Port = Convert.ToInt32(Request.Form.Get("txtPort"));
            string bol = Request.Form.Get("UseSSL");
            if (bol == "on")
            {
                item.UseSSL = true;
            }
            else
            {
                item.UseSSL = false;
            }
            string Default = Request.Form.Get("Default");
            item.Username = Request.Form.Get("txtUsername");
            item.Password = Request.Form.Get("txtPassword");
            item.EmailSend = Request.Form.Get("txtEmailSend");
            item.EmailCC = Request.Form.Get("EmailCC");
            if (item.EmailSend == item.EmailCC)
            {
                return Json(new { success = false, message = "Vui lòng nhập Email CC khác Email gởi !" }, JsonRequestBehavior.AllowGet);
            }

            unitWork.MailConfig.Update(item);
            unitWork.Commit();
            try
            {
                unitWork.Commit();
                #region Audit Trail
                var user = unitWork.User.GetById(this.User.UserId);
                var newMailConfig = unitWork.MailConfig.GetById(int.Parse(id));
                var newObject = (from b in db.MailConfigs
                                 select new
                                 {
                                     ServerAddress = newMailConfig.ServerAddress,
                                     Port = newMailConfig.Port,
                                     UseSSL = newMailConfig.UseSSL,
                                     EmailSend = newMailConfig.EmailSend,
                                     EmailCC = newMailConfig.EmailCC,
                                     Username = newMailConfig.Username,
                                     Password = newMailConfig.Password
                                 }).FirstOrDefault();
                auditTrailBussiness.CreateAuditTrail(AuditActionType.Update, int.Parse(id), "Cấu hình mail", oldObject, newObject, user.Username);
                #endregion
                return Json(new { success = true, message = "Cập nhật cấu hình mail thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}