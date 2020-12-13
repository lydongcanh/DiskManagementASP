using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Data;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class HoldDiskController : Controller
    {
        private readonly UnitWork unitWork;
        public HoldDiskController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        // GET: HoldDisk
        public ActionResult Index()
        {
            ViewBag.Customer = unitWork.Customer.Get(x => x.Status != CustomerStatus.INACTIVE);
            ViewBag.DiskTitles = unitWork.DiskTitle.Get(x => x.Status == TitleStatus.PENDING);
            return View();
        }
        public JsonResult LoadPhoneById(int? Id)
        {
            QLTDDBContext db = new QLTDDBContext();
            if (Id == null)
            {
                var json = new
                {
                    id = "",
                    text = ""
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            var customer = unitWork.Customer.GetById(Id);
            if(customer == null)
            {
                var json = new
                {
                    id = "",
                    text = ""
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PhoneNumber = customer.PhoneNumber }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult AddHoldDisk(DateTime HoldDate, int? customerId, int? diskId)
        {
            try
            {
                if (customerId == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy khách hàng !" }, JsonRequestBehavior.AllowGet);
                }
                if (diskId == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đĩa !" }, JsonRequestBehavior.AllowGet);
                }
                var customer = unitWork.Customer.GetById(customerId);
                var disk = unitWork.Disk.GetById(diskId);
                var newholddisk = new DiskHold();
                newholddisk.HoldDate = HoldDate;
                newholddisk.Customer = customer;
                newholddisk.Disk = disk;
                unitWork.DiskHold.Insert(newholddisk);
                unitWork.Commit();
                disk.Status = DiskStatus.HOLDING;
                unitWork.Disk.Update(disk);
                unitWork.Commit();

                return Json(new { success = true, message = "Lưu phiếu đặt đĩa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra !" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}