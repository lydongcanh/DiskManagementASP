using Ehr.Auth;
using Ehr.Bussiness;
using Ehr.Common.Constraint;
using Ehr.Common.UI;
using Ehr.Data;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class CalculatorController : BaseController
    {
        // GET: Questionnaire
        private readonly UnitWork unitWork;
        private readonly AuditTrailBussiness auditTrailBussiness;
        private readonly EhrDbContext db;
        public CalculatorController(UnitWork unitWork, AuditTrailBussiness auditTrailBussiness, EhrDbContext db)
        {
            this.unitWork = unitWork;
            this.auditTrailBussiness = auditTrailBussiness;
            this.db = db;
        }

        /// <summary>
        /// Hàm lưu kháng sinh
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Antimicrobial(AntimicrobialRequired antimicrobialRequired)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Status = 1, Message = "Not Valid" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.AntimicrobialRequireds.Add(antimicrobialRequired);
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Hàm lưu sản phẩm
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Product(ProductRequired productRequired)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Status = 1, Message = "Not Valid" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.ProductRequireds.Add(productRequired);
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

		/// <summary>
		/// Convert IU to mg
		/// </summary>
		/// <param name="antimicrobial"></param>
		/// <returns></returns>
		public double Conversion(string antimicrobial)
		{
			string upperName = antimicrobial.ToUpper();
			switch(upperName)
			{
				case "BACITRACIN":
					return 0.013514;
				case "PENICILLIN":
					return 0.0006;
				case "CHLORTETRACYCLINE":
					return 0.001111;
				case "COLISTIN":
					return 0.000049;									
				case "STREPTOMYCIN":
					return 0.00122;
				case "ERYTHROMYCIN":
					return 0.001087;
				case "GENTAMYCIN":
					return 0.001613;
				case "KANAMYCIN":
					return 0.001256;
				case "NEOMYCIN":
					return 0.001325;				
				case "OXYTETRACYCLINE":
					return 0.001149;
				case "PAROMOMYCIN":
					return 0.001481;
				case "POLYMYCIN":
					return 0.000119;
				case "RIFAMYCIN":
					return 0.001127;
				case "SPIRAMYCIN":
					return 0.000313;
				case "TOBRAMYCIN":
					return 0.001143;
				case "TYLOSIN":
					return 0.001;
				case "TETRACYLINE":
					return 0.001;
			}
			return 0.0;
		}
		double CalByUnit(string unit, double amount, string antimicrobial)
		{
			try
			{
				string tempunit = unit.ToLower();
				double famount = amount;
				if (tempunit.Equals("mg"))
				{
					famount = famount / 1000000;
				}
				else if (tempunit.Equals("g"))
				{
					famount = famount / 1000;
				}
				else if (tempunit.Equals("ml"))
				{
					famount = famount / 1000;
				}
				else if (tempunit.Equals("iu"))
				{
					famount = (famount* Conversion(antimicrobial)) / 1000000;
				}
				return famount;
			}
			catch
			{
				return 0;
			}
			
		}

        /// <summary>
		/// Cập nhật tên các kháng sinh
		/// </summary>
		/// <returns></returns>
		public ActionResult CorrectAB_Name()
        {
            var products = unitWork.ProductInfor.Get();
            foreach (ProductInfor productInfor in products)
            {
                var product = unitWork.Questionnaire.GetById(productInfor.ProductId);
                if (product != null)
                {
                    #region Ghi nhận các thông tin của 4 loại kháng sinh
                    //Ghi nhận các thông tin lưu trữ sau này
                    //kháng sinh thứ 1
                    productInfor.AB1 = product.B2_1__Antimicrobial_1;
                    if (product.B2_1__Antimicrobial_1.Length > 0)
                    {
                        if (product.B2_2__Strength_of_antimicrobial_1 > 0)
                        {
                            productInfor.AB1_Amount = product.B2_2__Strength_of_antimicrobial_1;
                            productInfor.AB1_Unit = product.B2_3__Units_of_antimicrobial_1;
                        }
                        else
                        {
                            double strength = 0;
                            try
                            {
                                strength = double.Parse(product.B2_4__Per_amount_of_product__antimicrobial_1_);
                            }
                            catch { }
                            productInfor.AB1_Amount = strength;
                            productInfor.AB1_Unit = product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
                        }
                    }
                    //kháng sinh thứ 2
                    productInfor.AB2 = product.B3_1__Antimicrobial_2;
                    if (product.B3_1__Antimicrobial_2.Length > 0)
                    {
                        if (product.B3_2__Strength_of_antimicrobial_2 > 0)
                        {
                            productInfor.AB2_Amount = product.B3_2__Strength_of_antimicrobial_2;
                            productInfor.AB2_Unit = product.B3_3__Units_of_antimicrobial_2;
                        }
                        else
                        {
                            double strength = 0;
                            try
                            {
                                strength = double.Parse(product.B3_4__Per_amount_of_product__antimicrobial_2_);
                            }
                            catch { }
                            productInfor.AB2_Amount = strength;
                            productInfor.AB2_Unit = product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
                        }
                    }
                    //kháng sinh thứ 3
                    productInfor.AB3 = product.B4_1__Antimicrobial_3;
                    if (product.B4_1__Antimicrobial_3.Length > 0)
                    {
                        if (product.B4_2__Strength_of_antimicrobial_3 > 0)
                        {
                            productInfor.AB3_Amount = product.B4_2__Strength_of_antimicrobial_3;
                            productInfor.AB3_Unit = product.B4_3__Units_of_antimicrobial_3;
                        }
                        else
                        {
                            double strength = 0;
                            try
                            {
                                strength = double.Parse(product.B4_4__Per_amount_of_product__antimicrobial_3_);
                            }
                            catch { }
                            productInfor.AB3_Amount = strength;
                            productInfor.AB3_Unit = product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
                        }

                    }

                    //kháng sinh thứ 4
                    productInfor.AB4 = product.B5_1__Antimicrobial_4;
                    if (product.B5_1__Antimicrobial_4.Length > 0)
                    {
                        if (product.B4_2__Strength_of_antimicrobial_3 > 0)
                        {
                            productInfor.AB4_Amount = product.B5_2__Strength_of_antimicrobial_4;
                            productInfor.AB4_Unit = product.B5_3__Units_of_antimicrobial_4;
                        }
                        else
                        {
                            double strength = 0;
                            try
                            {
                                strength = double.Parse(product.B5_4__Per_amount_of_product__antimicrobial_4_);
                            }
                            catch { }
                            productInfor.AB4_Amount = strength;
                            productInfor.AB4_Unit = product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
                        }

                    }

                    #endregion                    

                    unitWork.ProductInfor.Update(productInfor);
                }

            }
            unitWork.Commit();
            return View();
        }

		public void CorrectIU()
		{
			var products = unitWork.ProductInfor.Get();
			foreach (ProductInfor productInfor in products)
			{
				productInfor.AntiAmount = CalByUnit(productInfor.AB_Unit,productInfor.AB_Amount,productInfor.Antimicrobial);
				unitWork.ProductInfor.Update(productInfor);
			}
			unitWork.Commit();
		}

        /// <summary>
        /// Cập nhật các giá trị sản phẩm đã lưu
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateProductsCollected()
		{
			//cho các sản phẩm nhận về lượng kháng sinh bằng 0
			var products = unitWork.ProductInfor.Get(p=>p.AB_Amount==0);
			foreach(ProductInfor productInfor in products)
			{
				var product = unitWork.Questionnaire.GetById(productInfor.ProductId);
				if (product != null)
				{
					#region Ghi nhận các thông tin của 4 loại kháng sinh
					//Ghi nhận các thông tin lưu trữ sau này
					//kháng sinh thứ 1
					productInfor.AB1 = product.B2_1__Antimicrobial_1;
					if (product.B2_1__Antimicrobial_1.Length>0)
					{						
						if (product.B2_2__Strength_of_antimicrobial_1 > 0)
						{							
							productInfor.AB1_Amount = product.B2_2__Strength_of_antimicrobial_1;
							productInfor.AB1_Unit = product.B2_3__Units_of_antimicrobial_1;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B2_4__Per_amount_of_product__antimicrobial_1_);
							}
							catch { }							
							productInfor.AB1_Amount = strength;
							productInfor.AB1_Unit = product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
						}
					}
					//kháng sinh thứ 2
					productInfor.AB2 = product.B3_1__Antimicrobial_2;
					if (product.B3_1__Antimicrobial_2.Length>0)
					{						
						if (product.B3_2__Strength_of_antimicrobial_2 > 0)
						{							
							productInfor.AB2_Amount = product.B3_2__Strength_of_antimicrobial_2;
							productInfor.AB2_Unit = product.B3_3__Units_of_antimicrobial_2;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B3_4__Per_amount_of_product__antimicrobial_2_);
							}
							catch { }						
							productInfor.AB2_Amount = strength;
							productInfor.AB2_Unit = product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
						}
					}
					//kháng sinh thứ 3
					productInfor.AB3 = product.B4_1__Antimicrobial_3;
					if (product.B4_1__Antimicrobial_3.Length>0)
					{						
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{							
							productInfor.AB3_Amount = product.B4_2__Strength_of_antimicrobial_3;
							productInfor.AB3_Unit = product.B4_3__Units_of_antimicrobial_3;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B4_4__Per_amount_of_product__antimicrobial_3_);
							}
							catch { }							
							productInfor.AB3_Amount = strength;
							productInfor.AB3_Unit = product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
						}

					}
					
					//kháng sinh thứ 4
					productInfor.AB4 = product.B5_1__Antimicrobial_4;
					if (product.B5_1__Antimicrobial_4.Length>0)
					{						
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{							
							productInfor.AB4_Amount = product.B5_2__Strength_of_antimicrobial_4;
							productInfor.AB4_Unit = product.B5_3__Units_of_antimicrobial_4;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B5_4__Per_amount_of_product__antimicrobial_4_);
							}
							catch { }							
							productInfor.AB4_Amount = strength;
							productInfor.AB4_Unit = product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
						}

					}

					


					#endregion
					if(productInfor.AB1_Amount>0)
					{
						productInfor.Antimicrobial = productInfor.AB1;
						productInfor.AntiAmount = CalByUnit(productInfor.AB1_Unit, productInfor.AB1_Amount, productInfor.Antimicrobial);
						productInfor.AB_Amount = productInfor.AB1_Amount;
						productInfor.AB_Unit = productInfor.AB1_Unit;
					}
					else if (productInfor.AB2_Amount > 0)
					{
						productInfor.Antimicrobial = productInfor.AB2;
						productInfor.AntiAmount = CalByUnit(productInfor.AB2_Unit, productInfor.AB2_Amount, productInfor.Antimicrobial);
						productInfor.AB_Amount = productInfor.AB2_Amount;
						productInfor.AB_Unit = productInfor.AB2_Unit;
					}
					else if (productInfor.AB3_Amount > 0)
					{
						productInfor.Antimicrobial = productInfor.AB3;
						productInfor.AntiAmount = CalByUnit(productInfor.AB3_Unit, productInfor.AB3_Amount, productInfor.Antimicrobial);
						productInfor.AB_Amount = productInfor.AB3_Amount;
						productInfor.AB_Unit = productInfor.AB3_Unit;
					}
					else if (productInfor.AB4_Amount > 0)
					{
						productInfor.Antimicrobial = productInfor.AB4;
						productInfor.AntiAmount = CalByUnit(productInfor.AB4_Unit, productInfor.AB4_Amount, productInfor.Antimicrobial);
						productInfor.AB_Amount = productInfor.AB4_Amount;
						productInfor.AB_Unit = productInfor.AB4_Unit;
					}					

					productInfor.UnitPackage = product.A8__Unit_of_volume_of_product;
					
					unitWork.ProductInfor.Update(productInfor);
				}
				
			}
			unitWork.Commit();
			CorrectIU();
			return View();
		}

		/// <summary>
		/// Hàm lưu thông tin sản phẩm mua
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		public JsonResult ProductInfor_v2(ProductInfor productInfor, string username, string password)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { Status = 1, Message = "Not Valid" }, JsonRequestBehavior.AllowGet);
			}
			try
			{
				var a = unitWork.User.Get(c => c.Username == username && c.Password == password).FirstOrDefault();
				if (a == null)
				{
					return Json(new { Status = 1, Message = "Please login !" }, JsonRequestBehavior.AllowGet);
				}
				if (a.IsActive == false)
				{
					return Json(new { Status = 1, Message = "Account has been locked !" }, JsonRequestBehavior.AllowGet);
				}
				productInfor.DateStamp = DateTime.Now;
				try
				{
					if (productInfor.CollectedDate == null || productInfor.CollectedDate == DateTime.MinValue)
					{
						productInfor.CollectedDate = DateTime.Now;
					}
				}
				catch { }
				//var user = unitWork.User.GetById(this.User.UserId);
				//if(user != null)
				//{
				//    productInfor.UserId = user.Id;
				//}   
				//tính toán cho biến AntiAmount


				var product = unitWork.Questionnaire.GetById(productInfor.ProductId);
				if (product != null)
				{
					#region Ghi nhận các thông tin của 4 loại kháng sinh
					//Ghi nhận các thông tin lưu trữ sau này
					//kháng sinh thứ 1
					productInfor.AB1 = product.B2_1__Antimicrobial_1;
					if (product.B2_1__Antimicrobial_1.Length > 0)
					{
						if (product.B2_2__Strength_of_antimicrobial_1 > 0)
						{
							productInfor.AB1_Amount = product.B2_2__Strength_of_antimicrobial_1;
							productInfor.AB1_Unit = product.B2_3__Units_of_antimicrobial_1;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B2_4__Per_amount_of_product__antimicrobial_1_);
							}
							catch { }
							productInfor.AB1_Amount = strength;
							productInfor.AB1_Unit = product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
						}
					}
					//kháng sinh thứ 2
					productInfor.AB2 = product.B3_1__Antimicrobial_2;
					if (product.B3_1__Antimicrobial_2.Length > 0)
					{
						if (product.B3_2__Strength_of_antimicrobial_2 > 0)
						{
							productInfor.AB2_Amount = product.B3_2__Strength_of_antimicrobial_2;
							productInfor.AB2_Unit = product.B3_3__Units_of_antimicrobial_2;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B3_4__Per_amount_of_product__antimicrobial_2_);
							}
							catch { }
							productInfor.AB2_Amount = strength;
							productInfor.AB2_Unit = product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
						}
					}
					//kháng sinh thứ 3
					productInfor.AB3 = product.B4_1__Antimicrobial_3;
					if (product.B4_1__Antimicrobial_3.Length > 0)
					{
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{
							productInfor.AB3_Amount = product.B4_2__Strength_of_antimicrobial_3;
							productInfor.AB3_Unit = product.B4_3__Units_of_antimicrobial_3;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B4_4__Per_amount_of_product__antimicrobial_3_);
							}
							catch { }
							productInfor.AB3_Amount = strength;
							productInfor.AB3_Unit = product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
						}

					}

					//kháng sinh thứ 4
					productInfor.AB4 = product.B5_1__Antimicrobial_4;
					if (product.B5_1__Antimicrobial_4.Length > 0)
					{
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{
							productInfor.AB4_Amount = product.B5_2__Strength_of_antimicrobial_4;
							productInfor.AB4_Unit = product.B5_3__Units_of_antimicrobial_4;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B5_4__Per_amount_of_product__antimicrobial_4_);
							}
							catch { }
							productInfor.AB4_Amount = strength;
							productInfor.AB4_Unit = product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
						}

					}




					#endregion

					if (productInfor.Antimicrobial.ToLower().Equals(product.B2_1__Antimicrobial_1.ToLower()))
					{
						if (product.B2_2__Strength_of_antimicrobial_1 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B2_3__Units_of_antimicrobial_1, product.B2_2__Strength_of_antimicrobial_1, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B2_2__Strength_of_antimicrobial_1;
							productInfor.AB_Unit = product.B2_3__Units_of_antimicrobial_1;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B2_4__Per_amount_of_product__antimicrobial_1_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
						}

					}
					else if (productInfor.Antimicrobial.ToLower().Equals(product.B3_1__Antimicrobial_2.ToLower()))
					{
						if (product.B3_2__Strength_of_antimicrobial_2 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B3_3__Units_of_antimicrobial_2, product.B3_2__Strength_of_antimicrobial_2, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B3_2__Strength_of_antimicrobial_2;
							productInfor.AB_Unit = product.B3_3__Units_of_antimicrobial_2;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B3_4__Per_amount_of_product__antimicrobial_2_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
						}



					}
					else if (productInfor.Antimicrobial.ToLower().Equals(product.B4_1__Antimicrobial_3.ToLower()))
					{
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B4_3__Units_of_antimicrobial_3, product.B4_2__Strength_of_antimicrobial_3, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B4_2__Strength_of_antimicrobial_3;
							productInfor.AB_Unit = product.B4_3__Units_of_antimicrobial_3;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B4_4__Per_amount_of_product__antimicrobial_3_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
						}


					}
					else if (productInfor.Antimicrobial.ToLower().Equals(product.B5_1__Antimicrobial_4.ToLower()))
					{
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B5_3__Units_of_antimicrobial_4, product.B5_2__Strength_of_antimicrobial_4, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B5_2__Strength_of_antimicrobial_4;
							productInfor.AB_Unit = product.B5_3__Units_of_antimicrobial_4;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B5_4__Per_amount_of_product__antimicrobial_4_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
						}

					}
					else
					{
						//Nếu như tìm không ra kháng sinh nào thì lấy kháng sinh nào có liều lượng
						if (productInfor.AB1_Amount > 0)
						{
							productInfor.Antimicrobial = productInfor.AB1;
							productInfor.AntiAmount = CalByUnit(productInfor.AB1_Unit, productInfor.AB1_Amount, productInfor.Antimicrobial);
							productInfor.AB_Amount = productInfor.AB1_Amount;
							productInfor.AB_Unit = productInfor.AB1_Unit;
						}
						else if (productInfor.AB2_Amount > 0)
						{
							productInfor.Antimicrobial = productInfor.AB2;
							productInfor.AntiAmount = CalByUnit(productInfor.AB2_Unit, productInfor.AB2_Amount, productInfor.Antimicrobial);
							productInfor.AB_Amount = productInfor.AB2_Amount;
							productInfor.AB_Unit = productInfor.AB2_Unit;
						}
						else if (productInfor.AB3_Amount > 0)
						{
							productInfor.Antimicrobial = productInfor.AB3;
							productInfor.AntiAmount = CalByUnit(productInfor.AB3_Unit, productInfor.AB3_Amount, productInfor.Antimicrobial);
							productInfor.AB_Amount = productInfor.AB3_Amount;
							productInfor.AB_Unit = productInfor.AB3_Unit;
						}
						else if (productInfor.AB4_Amount > 0)
						{
							productInfor.Antimicrobial = productInfor.AB4;
							productInfor.AntiAmount = CalByUnit(productInfor.AB4_Unit, productInfor.AB4_Amount, productInfor.Antimicrobial);
							productInfor.AB_Amount = productInfor.AB4_Amount;
							productInfor.AB_Unit = productInfor.AB4_Unit;
						}
					}
					//productInfor.UnitPackage = product.A8__Unit_of_volume_of_product;
					

				}

				db.ProductInfors.Add(productInfor);
				db.SaveChanges();
				return Json(new { Status = 0, Message = "Success" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				return Json(new { Status = 1, Message = "Failed" + e.ToString() }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Hàm lưu thông tin sản phẩm mua
		/// </summary>
		[HttpPost]
        [AllowAnonymous]
        public JsonResult ProductInfor(ProductInfor productInfor,string username ,string password)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Status = 1, Message = "Not Valid" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var a = unitWork.User.Get(c => c.Username == username && c.Password == password).FirstOrDefault();
                if(a == null)
                {
                    return Json(new { Status = 1, Message = "Please login !" }, JsonRequestBehavior.AllowGet);
                }
                if(a.IsActive == false)
                {
                    return Json(new { Status = 1, Message = "Account has been locked !" }, JsonRequestBehavior.AllowGet);
                }
                productInfor.DateStamp = DateTime.Now;
				try
				{
					if (productInfor.CollectedDate == null || productInfor.CollectedDate==DateTime.MinValue) productInfor.CollectedDate = DateTime.Now;
				}
				catch { }
				
				//var user = unitWork.User.GetById(this.User.UserId);
				//if(user != null)
				//{
				//    productInfor.UserId = user.Id;
				//}   
				//tính toán cho biến AntiAmount


				var product = unitWork.Questionnaire.GetById(productInfor.ProductId);
				if(product!=null)
				{
					if (productInfor.Antimicrobial.ToLower().Equals(product.B2_1__Antimicrobial_1.ToLower()))
					{
						if (product.B2_2__Strength_of_antimicrobial_1 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B2_3__Units_of_antimicrobial_1, product.B2_2__Strength_of_antimicrobial_1, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B2_2__Strength_of_antimicrobial_1;
							productInfor.AB_Unit = product.B2_3__Units_of_antimicrobial_1;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B2_4__Per_amount_of_product__antimicrobial_1_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_;
						}

					}
					else if (productInfor.Antimicrobial.ToLower().Equals(product.B3_1__Antimicrobial_2.ToLower()))
					{
						if (product.B3_2__Strength_of_antimicrobial_2 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B3_3__Units_of_antimicrobial_2, product.B3_2__Strength_of_antimicrobial_2, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B3_2__Strength_of_antimicrobial_2;
							productInfor.AB_Unit = product.B3_3__Units_of_antimicrobial_2;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B3_4__Per_amount_of_product__antimicrobial_2_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_;
						}



					}
					else if (productInfor.Antimicrobial.ToLower().Equals(product.B4_1__Antimicrobial_3.ToLower()))
					{
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B4_3__Units_of_antimicrobial_3, product.B4_2__Strength_of_antimicrobial_3, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B4_2__Strength_of_antimicrobial_3;
							productInfor.AB_Unit = product.B4_3__Units_of_antimicrobial_3;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B4_4__Per_amount_of_product__antimicrobial_3_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_;
						}


					}
					else if (productInfor.Antimicrobial.ToLower().Equals(product.B5_1__Antimicrobial_4.ToLower()))
					{
						if (product.B4_2__Strength_of_antimicrobial_3 > 0)
						{
							productInfor.AntiAmount = CalByUnit(product.B5_3__Units_of_antimicrobial_4, product.B5_2__Strength_of_antimicrobial_4, productInfor.Antimicrobial);
							productInfor.AB_Amount = product.B5_2__Strength_of_antimicrobial_4;
							productInfor.AB_Unit = product.B5_3__Units_of_antimicrobial_4;
						}
						else
						{
							double strength = 0;
							try
							{
								strength = double.Parse(product.B5_4__Per_amount_of_product__antimicrobial_4_);
							}
							catch { }
							productInfor.AntiAmount = CalByUnit(product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_, strength, productInfor.Antimicrobial);
							productInfor.AB_Amount = strength;
							productInfor.AB_Unit = product.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_;
						}

					}
					else
					{
						productInfor.AntiAmount = 0;
					}
					productInfor.UnitPackage = product.A8__Unit_of_volume_of_product;
					//Ghi nhận các thông tin lưu trữ sau này
					productInfor.AB1 = product.B2_1__Antimicrobial_1;
					productInfor.AB2 = product.B3_1__Antimicrobial_2;
					productInfor.AB3 = product.B4_1__Antimicrobial_3;
					productInfor.AB4 = product.B5_1__Antimicrobial_4;

					productInfor.AB1_Amount = product.B2_2__Strength_of_antimicrobial_1;
					productInfor.AB2_Amount = product.B3_2__Strength_of_antimicrobial_2;
					productInfor.AB3_Amount = product.B4_2__Strength_of_antimicrobial_3;
					productInfor.AB4_Amount = product.B5_2__Strength_of_antimicrobial_4;

					productInfor.AB1_Unit = product.B2_3__Units_of_antimicrobial_1;
					productInfor.AB2_Unit = product.B3_3__Units_of_antimicrobial_2;
					productInfor.AB3_Unit = product.B4_3__Units_of_antimicrobial_3;
					productInfor.AB4_Unit = product.B5_3__Units_of_antimicrobial_4;

				}			

				db.ProductInfors.Add(productInfor);
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = 1, Message = "Failed" + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #region Report
        /// <summary>
        /// Danh sach san pham ban ra trong thang/nam
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Report(int month, int year, int userid)
        {
            try
            {
                DateTime firtdate = new DateTime(year,month,1,0,0,0);
                DateTime lastdate = DataConverter.LastDateOfMonth(month, year);
                var ProductsIds = (from a in db.ProductInfors where a.UserId == userid && a.DateStamp >= firtdate && a.DateStamp <= lastdate select a.ProductId).Distinct().ToList();
                if (ProductsIds.Count > 0)
                {
                    List<Questionnaire> Products = new List<Questionnaire>();
                    foreach (var proid in ProductsIds)
                    {
                        var pro = unitWork.Questionnaire.GetById(proid);
                        Products.Add(pro);
                    }
                    return Json(new { Status = 0, Message = "Success", Products }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Danh sach san pham ban ra trong khoảng thời gian chọn
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ReportRange(DateTime firtdate, DateTime lastdate, int userid)
        {
            try
            {
                var Products = (from a in db.ProductInfors where  a.UserId == userid && a.DateStamp >= firtdate && a.DateStamp <= lastdate select a).ToList();
                if (Products.Count > 0)
                {
                    return Json(new { Status = 0, Message = "Success", Products }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Danh sach san pham ban ra trong thang/nam
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ReportProduct(int productid, int month, int year,int userid)
        {
            try
            {
                DateTime firtdate = new DateTime(year, month, 1, 0, 0, 0);
                DateTime lastdate = DataConverter.LastDateOfMonth(month, year);

                var Products = unitWork.ProductInfor.Get(c => c.ProductId == productid && c.UserId == userid && c.DateStamp >= firtdate && c.DateStamp <= lastdate).ToList();
               
                if (Products.Count > 0)
                {
                    var product = db.Questionnaires.Where(c => c.Id == productid).Select(r => new
                    {
                        r.Id,
                        r.A1__Product_origin,
                        r.A2__Product_code,
                        r.A3__Product_name,
                        r.A4__Company,
                        r.A5__Type_of_product,
                        r.A6__Other_subtance_in_product,
                        r.A7__Volume_of_product,
                        r.A8__Unit_of_volume_of_product,
                        r.A9__Other_volume_of_product,
                        r.B1__Number_of_antimicrobials_in_product,
                        r.B2_1__Antimicrobial_1,
                        r.B2_2__Strength_of_antimicrobial_1,
                        r.B2_3__Units_of_antimicrobial_1,
                        r.B2_4__Per_amount_of_product__antimicrobial_1_,
                        r.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_,
                        r.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_,
                        r.B2_7__Units_of_product__link_to_question_B2_4_,
                        r.B3_1__Antimicrobial_2,
                        r.B3_2__Strength_of_antimicrobial_2,
                        r.B3_3__Units_of_antimicrobial_2,
                        r.B3_4__Per_amount_of_product__antimicrobial_2_,
                        r.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_,
                        r.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_,
                        r.B3_7__Units_of_product__link_to_question_B3_4_,
                        r.B4_1__Antimicrobial_3,
                        r.B4_2__Strength_of_antimicrobial_3,
                        r.B4_3__Units_of_antimicrobial_3,
                        r.B4_4__Per_amount_of_product__antimicrobial_3_,
                        r.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_,
                        r.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_,
                        r.B4_7__Units_of_product__link_to_question_B4_4_,
                        r.B5_1__Antimicrobial_4,
                        r.B5_2__Strength_of_antimicrobial_4,
                        r.B5_3__Units_of_antimicrobial_4,
                        r.B5_4__Per_amount_of_product__antimicrobial_4_,
                        r.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_,
                        r.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_,
                        r.B5_7__Units_of_product__link_to_question_B5_4_,
                        r.B6__Target_species_x,
                        r.B7__Administration_route
                    });
					double total = 0;
                    foreach (var pro in Products)
                    {
                        total += pro.AmountProduct;
                    }
                    return Json(new { Status = 0, Message = "Success", product , total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Danh sach san pham ban ra trong khoảng thời gian chọn
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ReportProductRange(int productid, DateTime firtdate, DateTime lastdate, int userid)
        {
            try
            {
                var Products = unitWork.ProductInfor.Get(c => c.ProductId == productid && c.UserId == userid && c.DateStamp >= firtdate && c.DateStamp <= lastdate).ToList();

                if (Products.Count > 0)
                {
                    var product = db.Questionnaires.Where(c => c.Id == productid).Select(r => new
                    {
                        r.Id,
                        r.A1__Product_origin,
                        r.A2__Product_code,
                        r.A3__Product_name,
                        r.A4__Company,
                        r.A5__Type_of_product,
                        r.A6__Other_subtance_in_product,
                        r.A7__Volume_of_product,
                        r.A8__Unit_of_volume_of_product,
                        r.A9__Other_volume_of_product,
                        r.B1__Number_of_antimicrobials_in_product,
                        r.B2_1__Antimicrobial_1,
                        r.B2_2__Strength_of_antimicrobial_1,
                        r.B2_3__Units_of_antimicrobial_1,
                        r.B2_4__Per_amount_of_product__antimicrobial_1_,
                        r.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_,
                        r.B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_,
                        r.B2_7__Units_of_product__link_to_question_B2_4_,
                        r.B3_1__Antimicrobial_2,
                        r.B3_2__Strength_of_antimicrobial_2,
                        r.B3_3__Units_of_antimicrobial_2,
                        r.B3_4__Per_amount_of_product__antimicrobial_2_,
                        r.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_,
                        r.B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_,
                        r.B3_7__Units_of_product__link_to_question_B3_4_,
                        r.B4_1__Antimicrobial_3,
                        r.B4_2__Strength_of_antimicrobial_3,
                        r.B4_3__Units_of_antimicrobial_3,
                        r.B4_4__Per_amount_of_product__antimicrobial_3_,
                        r.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_,
                        r.B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_,
                        r.B4_7__Units_of_product__link_to_question_B4_4_,
                        r.B5_1__Antimicrobial_4,
                        r.B5_2__Strength_of_antimicrobial_4,
                        r.B5_3__Units_of_antimicrobial_4,
                        r.B5_4__Per_amount_of_product__antimicrobial_4_,
                        r.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_,
                        r.B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_,
                        r.B5_7__Units_of_product__link_to_question_B5_4_,
                        r.B6__Target_species_x,
                        r.B7__Administration_route
                    });
					double total = 0;
                    foreach (var pro in Products)
                    {
                        total += pro.AmountProduct;
                    }
                    return Json(new { Status = 0, Message = "Success", product, total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Danh sach san pham và số lượng bản mỗi sản phẩm trong khoảng thời gian chọn
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ReportTotal(DateTime firtdate, DateTime lastdate, int userid)
        {
            List<ProductReport> products = new List<ProductReport>();
            try
            {
                var ProductsIds = (from a in db.ProductInfors where a.UserId == userid && a.DateStamp >= firtdate && a.DateStamp <= lastdate select a.ProductId).Distinct().ToList();
                if (ProductsIds.Count > 0)
                {
                    foreach (var proid in ProductsIds)
                    {
                        var prorp = new ProductReport();
						double total = 0;
                        var pro = unitWork.Questionnaire.GetById(proid);
                        var productifor = unitWork.ProductInfor.Get(c => c.ProductId == proid).ToList();
                        if (productifor.Count > 0)
                        {
                            foreach (var proif in productifor)
                            {
                                total += proif.AmountProduct;
                            }
                        }
                        prorp.Productid = proid;
                        prorp.ProductName = pro.A3__Product_name;
                        prorp.total = total;
                        products.Add(prorp);
                    }
                    return Json(new { Status = 0, Message = "Success", products }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        /// <summary>
        /// Lấy tên thú nuôi theo loại thú nuôi
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetPetNameDetailByPet(string PetValue)
        {
            try
            {
                string[] lsPetDefault = { "BUFFALO", "CATTLE", "POULTRY", "PIG", "DOG", "CAT", "GOAT", "QUAIL", "SHEEP", "MUSCOVY_DUCK", "GOOSE", "HORSE", "CHICKEN", "PIGLET", "CALF", "CHICK" };
                if (PetValue == null || PetValue == "")
                {
                    return Json(new { Status = 1, Message = "vui lòng chọn loại thú nuôi" }, JsonRequestBehavior.AllowGet);
                }
                if (!lsPetDefault.Contains(PetValue))
                {
                    return Json(new { Status = 1, Message = "Không tìm thấy loại thú nuôi" }, JsonRequestBehavior.AllowGet);
                }
                var lsPetName = unitWork.Reference.Get(c => c.Pet.Contains(PetValue)).ToList();// lấy tất cả danh sách thú nuôi
                List<string> lsPetNameDetail = new List<string>();
                foreach (var item in lsPetName)
                {
                    if (!lsPetNameDetail.Contains(item.PetNameDetail))
                    {
                        lsPetNameDetail.Add(item.PetNameDetail);
                    }
                }
                return Json(new { Status = 0, Message = "Success", lsPetNameDetail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy danh sách bảng tham chiếu theo tên thú nuôi
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetReference(string PetNameDetail)
        {
            try
            {
                if (PetNameDetail == null || PetNameDetail == "")
                {
                    return Json(new { Status = 1, Message = "Vui lòng chọn thú nuôi !" }, JsonRequestBehavior.AllowGet);
                }
                var lsReference = unitWork.Reference.Get(c => c.PetNameDetail.Contains(PetNameDetail)).ToList();
                if(lsReference == null)
                {
                    return Json(new { Status = 1, Message = "Không tìm thấy thú nuôi nào !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = "Success", lsReference }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy toàn bộ sản phẩm
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Products()
        {
            try
            {
                var products = unitWork.Questionnaire.Get(c => c.State == State.APPROVE).Select(r => new {
                    id=r.Id,                    
                    code=r.A2__Product_code,
                    name=r.A3__Product_name,                  
                    volume=r.A7__Volume_of_product,
                    unit=r.A8__Unit_of_volume_of_product,
                    unit2=r.A9__Other_volume_of_product,                    
                    anti1=r.B2_1__Antimicrobial_1,
					anti2 = r.B3_1__Antimicrobial_2,
					anti3 = r.B4_1__Antimicrobial_3,
					anti4 =r.B5_1__Antimicrobial_4,
					species = r.B6__Target_species_x

                }).ToList();
                if (products != null)
                {
					JsonResult result = Json(new { Status = 0, Message = "Success", ProductList = products }, JsonRequestBehavior.AllowGet);
					result.MaxJsonLength = int.MaxValue;
					return result;
					//return Json(new { Status = 0, Message = "Success", ProductList = products }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Failed Exception" }, JsonRequestBehavior.AllowGet);
            }
        }
        
        /// <summary>
        /// Lấy danh sách loài thú nuôi
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Pets()
        {
            var enumPet = Enum.GetValues(typeof(Pet))
                           .Cast<Pet>()
                           .ToList();
            var ListPet = from s in enumPet
                          select new SelectListItem
                          {
                              Value = s.ToString(),
                              Text = s.GetAttribute<DisplayAttribute>().Name
                          };
            return Json(new { ListPet }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Lấy danh sách sản phẩm theo thú nuôi
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetProductByPet(string Pet)
        {
            try
            {
                if (Pet != "")
                {
                    var Products = unitWork.Questionnaire.Get(c => c.B6__Target_species_x.Contains(Pet)).Select(r => new
                    {
						id = r.Id,
						code = r.A2__Product_code,
						name = r.A3__Product_name,
						volume = r.A7__Volume_of_product,
						unit = r.A8__Unit_of_volume_of_product,
						unit2 = r.A9__Other_volume_of_product,
						anti1 = r.B2_1__Antimicrobial_1,
						anti2 = r.B3_1__Antimicrobial_2,
						anti3 = r.B4_1__Antimicrobial_3,
						anti4 = r.B5_1__Antimicrobial_4,
						species = r.B6__Target_species_x
					}).ToList();
                    if (Products.Count > 0)
                    {
                        JsonResult result = Json(new { Status = 0, Message = "Success", Products }, JsonRequestBehavior.AllowGet);
                        result.MaxJsonLength = int.MaxValue;
                        return result;
                    }
                    else
                    {
                        return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách kháng sinh
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetAllAntimicrobial()
        {
			/*var lsAntimicrobial_B2 = (from b in db.Questionnaires
                                   where b.B2_1__Antimicrobial_1 != "NA" && b.B2_1__Antimicrobial_1 != ""
                                   select b.B2_1__Antimicrobial_1.Trim().ToUpper());
            var lsAntimicrobial_B3 = (from b in db.Questionnaires
                                   where b.B3_1__Antimicrobial_2 != "NA" && b.B3_1__Antimicrobial_2 != ""
                                   select b.B3_1__Antimicrobial_2.Trim().ToUpper());
            var lsAntimicrobial_B4 = (from b in db.Questionnaires
                                   where b.B4_1__Antimicrobial_3 != "NA" && b.B4_1__Antimicrobial_3 != ""
                                   select b.B4_1__Antimicrobial_3.Trim().ToUpper());
            var lsAntimicrobial_B5 = (from b in db.Questionnaires
                                   where b.B5_1__Antimicrobial_4 != "NA" && b.B5_1__Antimicrobial_4 != ""
                                   select b.B5_1__Antimicrobial_4.Trim().ToUpper());
            var lsAntimicrobial = lsAntimicrobial_B2.Union(lsAntimicrobial_B3).Union(lsAntimicrobial_B4).Union(lsAntimicrobial_B5).ToList();*/
			List<string> lsAntimicrobial = new List<string>();
			lsAntimicrobial.Add("AMIKACIN");
			lsAntimicrobial.Add("AMOXICILLIN");
			lsAntimicrobial.Add("AMPICILLIN");
			lsAntimicrobial.Add("APRAMYCIN");
			lsAntimicrobial.Add("AZITHROMYCIN");
			lsAntimicrobial.Add("BACITRACIN");
			lsAntimicrobial.Add("CEFADROXIL");
			lsAntimicrobial.Add("CEFALEXIN");
			lsAntimicrobial.Add("CEFOPERAZONE");
			lsAntimicrobial.Add("CEFOTAXIME");
			lsAntimicrobial.Add("CEFQUINOME");
			lsAntimicrobial.Add("CEFTIOFUR");
			lsAntimicrobial.Add("CEFTRIAXONE");
			lsAntimicrobial.Add("CEFUROXIME");
			lsAntimicrobial.Add("CHLORTETRACYCLINE");
			lsAntimicrobial.Add("CLARITHROMYCINE");
			lsAntimicrobial.Add("CLINDAMYCIN");
			lsAntimicrobial.Add("CLOXACILLIN");
			lsAntimicrobial.Add("COLISTIN");
			lsAntimicrobial.Add("DANOFLOXACINE");
			lsAntimicrobial.Add("DIFLOXACIN");
			lsAntimicrobial.Add("STREPTOMYCIN");
			lsAntimicrobial.Add("DOXYCYCLINE");
			lsAntimicrobial.Add("ENROFLOXACINE");
			lsAntimicrobial.Add("ERYTHROMYCIN");
			lsAntimicrobial.Add("FLAVOMYCIN");
			lsAntimicrobial.Add("FLAVOPHOSPHOLIPOL");
			lsAntimicrobial.Add("FLORFENICOL");
			lsAntimicrobial.Add("FLUMEQUINE");
			lsAntimicrobial.Add("FLUNIXIN");
			lsAntimicrobial.Add("FOSFOMYCIN");
			lsAntimicrobial.Add("GENTAMYCIN");
			lsAntimicrobial.Add("JOSAMYCIN");
			lsAntimicrobial.Add("KANAMYCIN");
			lsAntimicrobial.Add("KITASAMYCIN");
			lsAntimicrobial.Add("LINCOMYCIN");
			lsAntimicrobial.Add("MARBOFLOXACIN");
			lsAntimicrobial.Add("NEOMYCIN");
			lsAntimicrobial.Add("NORFLOXACIN");
			lsAntimicrobial.Add("OXOLINIC ACID");
			lsAntimicrobial.Add("OXYTETRACYCLINE");
			lsAntimicrobial.Add("PENICILLIN");
			lsAntimicrobial.Add("POLYMYCIN");
			lsAntimicrobial.Add("RIFAMYCIN");
			lsAntimicrobial.Add("SARAFLOXACIN");
			lsAntimicrobial.Add("SPECTINOMYCIN");
			lsAntimicrobial.Add("SPIRAMYCIN");
			lsAntimicrobial.Add("STREPTOMYCIN");
			lsAntimicrobial.Add("SULFACHLORPYRIDAZINE");
			lsAntimicrobial.Add("SULFACHLOROPYRAZINE");
			lsAntimicrobial.Add("SULFADIAZINE");
			lsAntimicrobial.Add("SULFADIMERAZINE");
			lsAntimicrobial.Add("SULFADIMETHOXINE");
			lsAntimicrobial.Add("SULFADIMIDINE");
			lsAntimicrobial.Add("SULFADOXINE");
			lsAntimicrobial.Add("SULFAGUANIDINE");
			lsAntimicrobial.Add("SULFAMERAZINE");
			lsAntimicrobial.Add("SULFAMETHAZINE");
			lsAntimicrobial.Add("SULFAMETHOXAZOLE");
			lsAntimicrobial.Add("SULFAMETHOXYPYRIDAZINE");
			lsAntimicrobial.Add("SULFAMONOMETHOXINE");
			lsAntimicrobial.Add("SULFAQUINOXALINE");
			lsAntimicrobial.Add("SULFATHIAZOLE");
			lsAntimicrobial.Add("TETRACYLINE");
			lsAntimicrobial.Add("THIAMPHENICOL");
			lsAntimicrobial.Add("TIAMULIN");
			lsAntimicrobial.Add("TILMICOSIN");
			lsAntimicrobial.Add("TOBRAMYCIN");
			lsAntimicrobial.Add("TRIMETHOPRIM");
			lsAntimicrobial.Add("TULATHROMYCIN");
			lsAntimicrobial.Add("TYLOSIN");
			lsAntimicrobial.Add("TYLVALOSIN");


			return Json(new { Status = 0, Message = "Success", lsAntimicrobial }, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Lấy danh sách sản phẩm bởi kháng sinh và thú nuôi
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetProduct(string Antimicrobial, string Pet)
        {
            var product = db.Questionnaires.Where(c => c.B6__Target_species_x.Contains(Pet));

            var product_1 = product.Where(c => c.B2_1__Antimicrobial_1.Trim().ToUpper() == Antimicrobial.Trim().ToUpper());

            var product_2 = product.Where(c => c.B3_1__Antimicrobial_2.Trim().ToUpper() == Antimicrobial.Trim().ToUpper());

            var product_3 = product.Where(c => c.B4_1__Antimicrobial_3.Trim().ToUpper() == Antimicrobial.Trim().ToUpper());

            var product_4 = product.Where(c => c.B5_1__Antimicrobial_4.Trim().ToUpper() == Antimicrobial.Trim().ToUpper());

            var Products = product_1.Union(product_2).Union(product_3).Union(product_4).ToList();

            if(Products.Count <= 0)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = 0, Message = "Success", Products }, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Lấy thông tin từ sản phẩm 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetProductInfor(int id, string Antimicrobial)
        {
            var products = db.Questionnaires.Where(c => c.Id == id).FirstOrDefault();
            if(products.B2_1__Antimicrobial_1 != "" && products.B2_1__Antimicrobial_1 != "NA" && products.B2_1__Antimicrobial_1.Trim().ToUpper() == Antimicrobial)
            {
               var product = db.Questionnaires.Where(c => c.Id == id).Select(r => new
                {
                   Volume_of_product = r.A7__Volume_of_product,
                   Unit_of_volume_of_product = r.A8__Unit_of_volume_of_product,
                   Strength_of_antimicrobial = r.B2_2__Strength_of_antimicrobial_1,
                   Units_of_antimicrobial = r.B2_3__Units_of_antimicrobial_1,
                   Per_amount_of_product__antimicrobial = r.B2_4__Per_amount_of_product__antimicrobial_1_,
                   Units_of_antimicrobial_link = r.B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_,
                });

                return Json(new { Status = 0, Message = "Success", product }, JsonRequestBehavior.AllowGet);
            }
            
            if(products.B2_1__Antimicrobial_1 != "" && products.B2_1__Antimicrobial_1 != "NA" && products.B3_1__Antimicrobial_2.Trim().ToUpper() == Antimicrobial )
            {
                var product = db.Questionnaires.Where(c => c.Id == id).Select(r => new
                {
                    Volume_of_product = r.A7__Volume_of_product,
                    Unit_of_volume_of_product = r.A8__Unit_of_volume_of_product,
                    Strength_of_antimicrobial = r.B3_2__Strength_of_antimicrobial_2,
                    Units_of_antimicrobial = r.B3_3__Units_of_antimicrobial_2,
                    Per_amount_of_product__antimicrobial = r.B3_4__Per_amount_of_product__antimicrobial_2_,
                    Units_of_antimicrobial_link = r.B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_,
                });
                return Json(new { Status = 0, Message = "Success", product }, JsonRequestBehavior.AllowGet);

            }

            if (products.B2_1__Antimicrobial_1 != "" && products.B2_1__Antimicrobial_1 != "NA" && products.B4_1__Antimicrobial_3.Trim().ToUpper() == Antimicrobial)
            {
                var product = db.Questionnaires.Where(c => c.Id == id).Select(r => new
                {
                    Volume_of_product = r.A7__Volume_of_product,
                    Unit_of_volume_of_product = r.A8__Unit_of_volume_of_product,
                    Strength_of_antimicrobial = r.B4_2__Strength_of_antimicrobial_3,
                    Units_of_antimicrobial = r.B4_3__Units_of_antimicrobial_3,
                    Per_amount_of_product__antimicrobial = r.B4_4__Per_amount_of_product__antimicrobial_3_,
                    Units_of_antimicrobial_link = r.B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_,
                });
                return Json(new { Status = 0, Message = "Success", product }, JsonRequestBehavior.AllowGet);
            }
            
            if(products.B2_1__Antimicrobial_1 != "" && products.B2_1__Antimicrobial_1 != "NA" && products.B5_1__Antimicrobial_4.Trim().ToUpper() == Antimicrobial)
            {
                var product = db.Questionnaires.Where(c => c.Id == id).Select(r => new
                {
                    Volume_of_product = r.A7__Volume_of_product,
                    Unit_of_volume_of_product = r.A8__Unit_of_volume_of_product,
                    Strength_of_antimicrobial = r.B5_2__Strength_of_antimicrobial_4,
                    Units_of_antimicrobial = r.B5_3__Units_of_antimicrobial_4,
                    Per_amount_of_product__antimicrobial = r.B5_4__Per_amount_of_product__antimicrobial_4_,
                    Units_of_antimicrobial_link = r.B5_5__Units_of_antimicrobial_4__link_to_question_5_4_,
                });
                return Json(new { Status = 0, Message = "Success", product }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy trọng lượng vật nuôi
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetBodyWeight(string PetName, int AgeDate, Boolean Isweek)
        {
            if (PetName == "" || AgeDate <= 0)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            var reference = db.References.Where(c => c.PetNameDetail == PetName);
            if (reference.ToList().Count() <= 0)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            if (Isweek == true)
            {
               var refer = reference.Where(c => c.Week == AgeDate).FirstOrDefault();
                if(refer == null)
                {
                    return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                var BodyWeight = refer.Volume_AVG;
                return Json(new { Status = 0, Message = "Success", BodyWeight }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var refer = reference.Where(c => c.Age_Min <= AgeDate && c.Age_Max >= AgeDate).FirstOrDefault();
                if (refer == null)
                {
                    return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                var BodyWeight = refer.Volume_AVG;
                return Json(new { Status = 0, Message = "Success", BodyWeight }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy thông tin từ sản phẩm 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetProductInforByUser(int? userid)
        {
            try
            {
                if(userid > 0)
                {
                    var Product = (from a in db.ProductInfors where a.UserId == userid select a).ToList();
                    var Products = new List<ProductInfors>();
                    foreach (var item in Product)
                    {
                        var date = item.CollectedDate.ToString("yyyy-MM-dd hh:mm:ss");
                        ProductInfors pro = new ProductInfors();
                        pro.Id = item.Id;
                        pro.DateStamp = date;
                        pro.Pet = item.Pet;
                        pro.PetNameDetail = item.PetNameDetail;
                        pro.AmountPet = item.AmountPet;
                        pro.Volume_AVG = item.Volume_AVG;
                        pro.Product = item.Product;
                        pro.Antimicrobial = item.Antimicrobial;
                        pro.Route = item.Route;
                        pro.AmountProduct = item.AmountProduct;
                        pro.Price = item.Price;
                        pro.UserId = item.UserId;
                        pro.ProductId = item.ProductId;
                        pro.VolumePackage = item.VolumePackage;
                        Products.Add(pro);
                    }
                    if (Products.Count > 0)
                    {
                        return Json(new { Status = 0, Message = "Success", Products }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lấy lượng tiêu thụ hằng ngày
        /// Route  1-Ăn , 2-Uống, 3 Tiêm
        /// Isweek  True-Tuần, False-Ngày
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetResult(string PetName, int AgeDate, Boolean Isweek, int Route)
        {
            if (PetName == "" || AgeDate <= 0)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            var reference = db.References.Where(c => c.PetNameDetail == PetName);
            if (Isweek == true)
            {
                reference = reference.Where(c => c.Week == AgeDate);
            }
            else
            {
                reference = reference.Where(c => c.Age_Max >= AgeDate && c.Age_Min <= AgeDate);
            }
            if(reference.ToList().Count() <= 0)
            {
                return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            if (Route == 1 )
            {
                var refe = reference.FirstOrDefault();
                if (refe == null)
                {
                    return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                var Result = refe.Volume_Food_AVG;
                return Json(new { Status = 0, Message = "Success", Result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var refe = reference.FirstOrDefault();
                if (refe == null)
                {
                    return Json(new { Status = 1, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                var Result = refe.Volume_Water_AVG;
                return Json(new { Status = 0, Message = "Success", Result }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Lấy danh sách tham chiếu
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetAllReference()
        {
            try
            {
                var References = db.References.ToList();
                if (References.Count > 0)
                {
                    return Json(new { Status = 0, Message = "Success", References }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
            }
        }



		/// <summary>
		/// Lấy về dữ liệu cho report chart trên Android
		/// </summary>
		[HttpGet]
		[AllowAnonymous]
		public JsonResult AndroidChart(DateTime firtdate, DateTime lastdate, int userid)
		{			
			try
			{
				var products = (from a in db.ProductInfors where a.UserId == userid && a.DateStamp >= firtdate && a.DateStamp <= lastdate select a);
				var result = products
				.GroupBy(x => x.Antimicrobial)
				.Select(g => new
				{
					AntiName = g.Key,
					Total = g.Sum(x => x.AntiAmount > 0 ? x.AntiAmount*x.AmountProduct : 0)
				}).ToList();
				
				if (result.ToList().Count > 0)
				{					
					
					return Json(new { Status = 0, Message = "Success", result }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { Status = 1, Message = "Not Found" }, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception)
			{
				throw;
				return Json(new { Status = 1, Message = "Exception" }, JsonRequestBehavior.AllowGet);
			}
		}
		

		public class ProductReport
        {
            public int Productid
            {
                get; set;
            }
            public string ProductName
            {
                get; set;
            }
            public double total
            {
                get; set;
            }
        }
        public class ProductInfors
        {
            public int Id { get; set; }
            public string DateStamp { get; set; }
            public string Pet { get; set; }
            public string PetNameDetail { get; set; }
            public double AmountPet { get; set; }
            public double Volume_AVG { get; set; }
            public string Product { get; set; }
            public string Antimicrobial { get; set; }
            public int Route { get; set; }
            public double AmountProduct { get; set; }
            public double Price { get; set; }
            public int UserId { get; set; }
            public int ProductId { get; set; }
            public double VolumePackage { get; set; }
        }

    }
}