using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class Product
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
        /// Xuất xứ 
        /// </summary>
        public Origin ProductOrigin { get; set; }
        /// <summary>
		/// Mã code sản phẩm
		/// </summary>
        public string ProductCode { get; set; }
        /// <summary>
		/// Tên sản phẩm
		/// </summary>
        public string ProductName { get; set; }
        /// <summary>
		/// Tên công ty đăng ký sản phẩm
		/// </summary>
        public string Company { get; set; }
        /// <summary>
		/// Loại sản phẩm ( dang bột - dạng dung dịch )
		/// </summary>
        public ProductType TypeOfProduct { get; set; }
        /// <summary>
		/// Có chứa chất ngoài kháng sinh hay không?
		/// </summary>
        public YesNo Other_Subtance_In_Product { get; set; }
        /// <summary>
		/// Khối lượng/trọng lượng/thể tích của sản phẩm 
		/// </summary>
        public string Volume_Of_product { get; set; }
        /// <summary>
		/// Đơn vị của khối lượng/trọng lượng/thể tích của sản phẩm 
		/// </summary>
        public UnitVolume Unit_Of_Volume_Of_Product { get; set; }
        /// <summary>
		/// Thông tin về khối lượng khác 
		/// </summary>
        public string Other_Volume_Of_Product { get; set; }
        /// <summary>
		/// Có chưa kháng sinh hay không?
		/// </summary>
        public YesNo IsAntimicrobial { get; set; }
        /// <summary>
		/// Có chứa chất ma túy không?
		/// </summary>
        public YesNo IsDope { get; set; }
        /// <summary>
        /// Điền trang web hoặc vetshop
        /// </summary>
        public string Source_of_information { get; set; }
        /// <summary>
        /// Ảnh sản phẩm
        /// </summary>
        public string Picture_of_product { get; set; }
        /// <summary>
        /// Thông tin người nhập
        /// </summary>
        public string Person_in_charge { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Người nhập
        /// </summary>
        public User CreateBy { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public State State { get; set; }
        public virtual ICollection<AntimicroBial> Antimicrobials { get; set; }
        public virtual ICollection<OrtherAB> OrtherABs { get; set; }
        public virtual ICollection<AnimalInfor> AnimalInfors { get; set; }
    }
}