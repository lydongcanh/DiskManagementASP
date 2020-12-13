using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.Tools;
using Ehr.Common.UI;
using Ehr.Models;
using Ehr.ViewModels;
using OrderLateCharge = Ehr.Models.OrderLateCharge;

namespace Ehr.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UnitWork unitWork;
        public HomeController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public PartialViewResult Search(string searchStrTL)
        {
            Ehr.Data.QLTDDBContext db = new Data.QLTDDBContext();
            if (searchStrTL == "")
            {
                var listResult = unitWork.Rent.Get(x => x.ReceiptDate.Year == DateTime.Now.Year && x.ReceiptDate.Month == DateTime.Now.Month && x.ReceiptDate.Day == DateTime.Now.Day);
                return PartialView("_ListRent", listResult);
            }
            else
            {
                var customer = unitWork.Customer.Get(x => x.Code.ToLower().Contains(searchStrTL.ToLower()) || x.PhoneNumber.ToLower().Contains(searchStrTL.ToLower())).FirstOrDefault();
                ViewBag.Message = "";
                if (customer == null)
                {
                    customer = unitWork.Rent.Get(x => x.RentDetails.Any(a => a.Disk.Code == searchStrTL)).Select(x => x.Customer).LastOrDefault();
                    if(customer == null)
                    {
                        customer = unitWork.DiskHold.Get(x => x.Disk.Code == searchStrTL).Select(x => x.Customer).LastOrDefault();
                        if (customer == null)
                        {
                            customer = unitWork.RentReceipt.Get(x => x.RentDetails.Any(a => a.Disk.Code == searchStrTL)).Select(x => x.Customer).LastOrDefault();
                        }
                    }
                }
                if(customer == null)
                {
                    ViewBag.Message = "Không tìm thấy khách hàng !";
                    return PartialView("_SearchResult");
                }
                ViewBag.Customer = customer;
                ViewBag.Rents = unitWork.Rent.Get(x => x.Customer.Id == customer.Id && (x.Status == RentStatus.PENDING || x.Status == RentStatus.RENTING)).ToList();
                var RentReceipts = unitWork.RentReceipt.Get(x => x.Customer.Id == customer.Id).ToList();
                var lsOrder = new List<LateCharge>();
                if (RentReceipts.Count() > 0)
                {
                    foreach (var item in RentReceipts)
                    {
                        var Order = unitWork.LateCharge.Get(x => x.RentReceipt.Id == item.Id && x.Status == LateChargeStatus.NONE).ToList();
                        lsOrder.AddRange(Order);
                    }
                }
                ViewBag.OrderLates = lsOrder;
                ViewBag.HoldDisks = unitWork.DiskHold.Get(x => x.Customer.Id  == customer.Id).ToList();
                var rentDetails = unitWork.RentDetail.Get(x => x.Rent.Customer.Id == customer.Id).OrderByDescending(c => c.Rent.RentDate).Take(10).ToList();
                var lsHistory = new List<HistoryViewModel>();
                foreach (var c in rentDetails)
                {
                    var history = new HistoryViewModel();
                    history.Id = c.Id;
                    history.Image = c.Disk.DiskTitle.Image;
                    history.Name = c.Disk.DiskTitle.Name;
                    history.DateRent = c.Rent.RentDate;
                    if(c.RentReceipt!= null)
                    {
                        history.DateReceipt = c.RentReceipt.ReceiptDate;
                    }
                    history.State = c.Status;
                    lsHistory.Add(history);
                }
                ViewBag.History = lsHistory;
                return PartialView("_SearchResult");
            }
        }
    }
}