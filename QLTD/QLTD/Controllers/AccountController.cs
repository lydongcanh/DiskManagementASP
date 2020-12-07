using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ehr.Models;
using System.Web.Security;
using Ehr.Auth;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Ehr.Common.Constraint;
using Ehr.Common.Tools;
using Ehr.Bussiness;
using Ehr.Data;
using System.Collections.Generic;

namespace Ehr.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly CustomMembership membership;
        private string keyCookie = ConfigurationManager.AppSettings["cookie"];
        private readonly UnitWork unitWork;
        public AccountController(CustomMembership membership, UnitWork unitWork)
        {
            this.unitWork = unitWork;
            this.membership = membership;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpGet]
        public JsonResult Logins(LoginViewModel loginViewModel)
        {
            try
            {                
                User user = unitWork.User.Get(c => c.Username == loginViewModel.Username & c.Password == loginViewModel.Password).FirstOrDefault();
                if(user == null)
                {
                    var passB = "0" + loginViewModel.Password;
                    var newpassword = passB.Substring(0,passB.Length/2);
                    var userB = unitWork.User.Get(c => c.Username == loginViewModel.Username & c.Password == newpassword).FirstOrDefault();
                    if(userB != null)
                    {
                        if (userB.IsActive == false)
                        {
                            return Json(new { Status = 1, Message = "Account Not Active" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Status = 0, Message = "Success", UserId = userB.Id }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                if (user != null)
                {
                    if(user.IsActive == false)
                    {
                        return Json(new { Status = 1, Message = "Account Not Active" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Status = 0, Message = "Success", UserId = user.Id }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string ReturnUrl = "")
        {
            var userip = EZRetriever.GetIp();
            if (ModelState.IsValid)
            {
                bool isActive = false;
                if (membership.ValidateUser(loginViewModel.Username, loginViewModel.Password, out isActive))
                {
                    if (isActive)
                    {
                        var user = membership.GetUser(loginViewModel.Username, false);
                        if (user.IsActive)
                        {
                            if (user != null)
                            {
                                EZAurthenticateSupport authenSupport = new EZAurthenticateSupport(user);
                                CustomSerializeModel userModel = new CustomSerializeModel()
                                {
                                    FullName = user.FullName,
                                    Image = user.Image,
                                    UserId = user.Id,
                                    Email = user.Username,
                                    RoleName = user.Roles.Select(r => r.RoleName).ToList(),
                                    IsRoot = authenSupport.IsRoot,
                                    PermissionList = authenSupport.PermissionList.Select(p => p.PermissionCode).ToList()
                                };

                                string userData = JsonConvert.SerializeObject(userModel);
                                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                (
                                  1, loginViewModel.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                                );
                                string enTicket = FormsAuthentication.Encrypt(authTicket);
                                if (loginViewModel.IsRemember)
                                {

                                    HttpCookie faCookie = new HttpCookie(keyCookie, enTicket);
                                    FormsAuthentication.SetAuthCookie(loginViewModel.Username, loginViewModel.IsRemember);
                                    Response.Cookies.Add(faCookie);
                                }
                                else
                                {
                                    HttpCookie cookie = Request.Cookies[keyCookie];
                                    if (cookie != null)
                                    {
                                        cookie.Expires = DateTime.Now.AddHours(2);
                                    }
                                    else
                                    {
                                        cookie = new HttpCookie(keyCookie, enTicket);
                                        cookie.Expires = DateTime.Now.AddHours(2);
                                    }
                                    Response.Cookies.Add(cookie);
                                }
                                
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản của bạn đang bị khóa !");
                            return View(loginViewModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn chưa được cấp quyền !");
                        return View(loginViewModel);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Thông tin đăng nhập bị sai !");
            return View(loginViewModel);
        }


        [HttpPost]
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie(keyCookie, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        //[HttpGet]
        //public ActionResult ActivationAccount(string id)
        //{
        //    bool statusAccount = false;
        //    using (AuthenticationDB dbContext = new DataAccess.AuthenticationDB())
        //    {
        //        var userAccount = dbContext.Users.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

        //        if (userAccount != null)
        //        {
        //            userAccount.IsActive = true;
        //            dbContext.SaveChanges();
        //            statusAccount = true;
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Something Wrong !!";
        //        }

        //    }
        //    ViewBag.Status = statusAccount;
        //    return View();
        //}


        //[NonAction]
        //public void VerificationEmail(string email, string activationCode)
        //{
        //    var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

        //    var fromEmail = new MailAddress("gmail@gmail.com", "");
        //    var toEmail = new MailAddress(email);

        //    var fromEmailPassword = "******************";
        //    string subject = "Activation Account !";

        //    string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        //    };

        //        using (var message = new MailMessage(fromEmail, toEmail)
        //{
        //    Subject = subject,
        //            Body = body,
        //            IsBodyHtml = true

        //        })

        //        smtp.Send(message);

        //}

        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Username)
        {
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            var user = unitWork.User.Get(u => u.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordViewModel resetPasswordVM = new ResetPasswordViewModel();
                resetPasswordVM.Code = id;
                return View(resetPasswordVM);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = unitWork.User.Get(s => s.ResetPasswordCode == model.Code).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Utilities.Encrypt(model.Password);
                    user.ResetPasswordCode = "";
                    unitWork.User.Update(user);
                    unitWork.Commit();
                    message = "New password updated successfully";
                }
                else
                {
                    message = "Something invalid";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ConfirmPassword(string id)
        {
            var user = unitWork.User.Get(u => u.ConfirmPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordViewModel resetPasswordVM = new ResetPasswordViewModel();
                resetPasswordVM.Code = id;
                return View(resetPasswordVM);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmPassword(ResetPasswordViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = unitWork.User.Get(s => s.ConfirmPasswordCode == model.Code).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Utilities.Encrypt(model.Password);
                    user.ConfirmPasswordCode = "";
                    unitWork.User.Update(user);
                    unitWork.Commit();
                    message = "New password updated successfully";
                }
                else
                {
                    message = "Something invalid";
                }
            }
            ViewBag.Message = message;
            return View();
        }



        public ActionResult ChangePassword(int? id)
        {
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword()
        {
            return View();
        }

    }
}