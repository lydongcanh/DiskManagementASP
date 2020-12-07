using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class AnimalInfor
    {
        public int Id { get; set; }
        /// <summary>
        /// Thú nuôi
        /// </summary>
        public virtual Animal Animal { get; set; }

        #region Phòng bệnh
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string PB_Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public UnitVolume PB_Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public double PB_Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public double PB_Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string PB_Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string PB_Duration_of_usages { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public double PB_Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public double PB_Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public UnitVolume PB_Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public double PB_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public double PB_Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string PB_Duration_of_usage { get; set; }

        #endregion
        #region Trị bệnh
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string TB_Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public UnitVolume TB_Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public double TB_Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public double TB_Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string TB_Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string TB_Duration_of_usage_MM { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public double TB_Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public double TB_Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public UnitVolume TB_Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public double TB_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public double TB_Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string TB_Duration_of_usage { get; set; }
        #endregion
        public virtual Product Product { get; set; }
    }
}