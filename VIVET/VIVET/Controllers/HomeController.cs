using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Tools;
using Ehr.Common.UI;
using Ehr.Models;
using Ehr.ViewModels;

namespace Ehr.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UnitWork unitWork;
        public HomeController(UnitWork unitWork)
        {
            //đếm dự án
   //         var projectCount = unitWork.EProject.Get().Count();
   //         ViewBag.ProjectCount = projectCount;
   //         var candidateCount = unitWork.Candidate.Get().Count();
			//var contractCount = 0;
			//if(candidateCount > 0)
			//{
			//	contractCount = unitWork.Candidate.Get ( c => c.ContractDate != null ).Count ( ) / candidateCount;
			//	ViewBag.CandidateCountRate = candidateCount / candidateCount;
			//}
			//else
			//{
			//	ViewBag.CandidateCountRate = 0;
			//}
   //         ViewBag.CandidateCount = candidateCount;
   //         ViewBag.AuditCount = unitWork.EFormAuditTrail.Get().Count();
            this.unitWork = unitWork;
        }
        public ActionResult Index()
        {
            return View();
        }
		

		public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}