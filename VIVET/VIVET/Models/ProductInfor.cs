using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class ProductInfor
    {
        public int Id { get; set; }
		/// <summary>
		/// Ngày lưu  thông tin
		/// </summary>
		public DateTime DateStamp { get; set; }
        /// <summary>
        /// Ngày thu thập thông tin
        /// </summary>
        public DateTime CollectedDate { get; set; }
        /// <summary>
        /// Tên kháng sinh chính mà khách chọn để mua
        /// </summary>
        public string Antimicrobial { get; set; }
		/// <summary>
		/// Tên vật nuôi
		/// </summary>
		public string Pet { get; set; }
		public string PetNameDetail { get; set; }
		/// <summary>
		/// Số lượng vật nuôi
		/// </summary>
		public double AmountPet { get; set; }
		/// <summary>
		/// Trọng lượng trung bình vật nuôi
		/// </summary>
		public double Volume_AVG { get; set; }
		/// <summary>
		/// Trọng lượng trung bình vật nuôi
		/// </summary>
		public string Volume_AVG_Unit { get; set; }
		/// <summary>
		/// Mã sản phẩm
		/// </summary>
		public string Code
        {
            get;set;
        }
		/// <summary>
		/// ID sản phẩm của hệ thống
		/// </summary>
		public int ProductId { get; set; }
		/// <summary>
		/// Sản phẩm
		/// </summary>
		public string Product { get; set; }
		/// <summary>
		/// Số lượng sản phẩm
		/// </summary>
		public double AmountProduct { get; set; }
		/// <summary>
		/// Đơn vị số lượng sản phẩm (Ví dụ: 1 gói, 2 thùng, 3 chai...)
		/// </summary>
		public string QuantityUnit
		{
			get;set;
		}
		/// <summary>
		/// Trọng lượng mỗi đơn vị sản phẩm
		/// </summary>
		public double VolumePackage { get; set; }
		/// <summary>
		/// Đơn vị trọng lượng sản phẩm
		/// </summary>
		public string UnitPackage
		{
			get; set;
		}
		/// <summary>
		/// Có đơn thuốc hay không
		/// </summary>
		public bool IsPrescription
		{
			get; set;
		}
		/// <summary>
		/// Người nhập
		/// </summary>
		public int UserId { get; set; }
		/// <summary>
		/// Lượng kháng sinh cho 1 sản phẩm (Đơn vị kg) - Để báo cáo
		/// </summary>
		public double AntiAmount
		{
			get; set;
		}
		/// <summary>
		/// Đơn vị thực tế lượng kháng sinh
		/// </summary>
		public string AB_Unit
		{
			get;set;
		}
		/// <summary>
		/// Lượng kháng sinh thực tế - Truy từ hệ thống
		/// </summary>
		public double AB_Amount
		{
			get;set;
		}
		public int Route { get; set; }         
        public double Price { get; set; }
            		

							
		public string AB1
		{
			get;set;
		}
		public double AB1_Amount
		{
			get;set;
		}
		public string AB1_Unit
		{
			get;set;
		}
		public string AB2
		{
			get; set;
		}
		public double AB2_Amount
		{
			get; set;
		}
		public string AB2_Unit
		{
			get; set;
		}
		public string AB3
		{
			get; set;
		}
		public double AB3_Amount
		{
			get; set;
		}
		public string AB3_Unit
		{
			get; set;
		}
		public string AB4
		{
			get; set;
		}
		public double AB4_Amount
		{
			get; set;
		}
		public string AB4_Unit
		{
			get; set;
		}
	}
}