using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class QuestionnaireViewModel
    {
        #region A.General information
        /// <summary>
        /// Ngày thực hiện
        /// </summary>
        public string D_u_th_i_gian { get; set; }
        /// <summary>
        /// Xuất xứ 
        /// </summary>
        public string A1__Product_origin { get; set; }
        /// <summary>
		/// Mã code sản phẩm
		/// </summary>
        public string A2__Product_code { get; set; }
        /// <summary>
		/// Tên sản phẩm
		/// </summary>
        public string A3__Product_name { get; set; }
        /// <summary>
		/// Tên công ty đăng ký sản phẩm
		/// </summary>
        public string A4__Company { get; set; }
        /// <summary>
		/// Loại sản phẩm ( dang bột - dạng dung dịch )
		/// </summary>
        public string A5__Type_of_product { get; set; }
        /// <summary>
		/// Có chứa chất ngoài kháng sinh hay không?
		/// </summary>
        public string A6__Other_subtance_in_product { get; set; }
        /// <summary>
		/// Khối lượng/trọng lượng/thể tích của sản phẩm 
		/// </summary>
        public string A7__Volume_of_product { get; set; }
        /// <summary>
		/// Đơn vị của khối lượng/trọng lượng/thể tích của sản phẩm 
		/// </summary>
        public string A8__Unit_of_volume_of_product { get; set; }
        /// <summary>
		/// Thông tin về khối lượng khác 
		/// </summary>
        public string A9__Other_volume_of_product { get; set; }
        #endregion

        #region B.Information related to antimicrobial
        /// <summary>
		/// Số loại kháng sinh co trong sản phẩm
		/// </summary>
        public int B1__Number_of_antimicrobials_in_product { get; set; }

        #region Antimicrobials 1
        /// <summary>
        /// Antimicrobials_1
        /// </summary>
        public string B2_1__Antimicrobial_1 { get; set; }
        /// <summary>
        /// Thông tin về lượng kháng sinh 1 - chỉ điền số
        /// </summary>
        public double B2_2__Strength_of_antimicrobial_1 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh 1
        /// </summary>
        public string B2_3__Units_of_antimicrobial_1 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng mỗi loại
        /// </summary>
        public string B2_4__Per_amount_of_product__antimicrobial_1_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B2_5__Units_of_antimicrobial_1__link_to_question_B2_4_ { get; set; }
        /// <summary>
        /// khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B2_6__Per_amount_of_product__volume_of_product___link_to_question_B2_4_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B2_7__Units_of_product__link_to_question_B2_4_ { get; set; }
        #endregion
        #region Antimicrobials 2
        /// <summary>
        /// Antimicrobials_2
        /// </summary>
        public string B3_1__Antimicrobial_2 { get; set; }
        /// <summary>
        /// Thông tin về lượng kháng sinh 2 - chỉ điền số
        /// </summary>
        public double B3_2__Strength_of_antimicrobial_2 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh 2
        /// </summary>
        public string B3_3__Units_of_antimicrobial_2 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng mỗi loại
        /// </summary>
        public string B3_4__Per_amount_of_product__antimicrobial_2_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B3_5__Units_of_antimicrobial_2__link_to_question_B3_4_ { get; set; }
        /// <summary>
        /// khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B3_6__Per_amount_of_product__volume_of_product___link_to_question_B3_4_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B3_7__Units_of_product__link_to_question_B3_4_ { get; set; }
        #endregion
        #region Antimicrobials 3
        /// <summary>
        /// Antimicrobials_3
        /// </summary>
        public string B4_1__Antimicrobial_3 { get; set; }
        /// <summary>
        /// Thông tin về lượng kháng sinh 3 - chỉ điền số
        /// </summary>
        public double B4_2__Strength_of_antimicrobial_3 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh 3
        /// </summary>
        public string B4_3__Units_of_antimicrobial_3 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng mỗi loại
        /// </summary>
        public string B4_4__Per_amount_of_product__antimicrobial_3_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B4_5__Units_of_antimicrobial_3__link_to_question_B4_4_ { get; set; }
        /// <summary>
        /// khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B4_6__Per_amount_of_product__volume_of_product___link_to_question_B4_4_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B4_7__Units_of_product__link_to_question_B4_4_ { get; set; }
        #endregion
        #region Antimicrobials 4
        /// <summary>
        /// Antimicrobials_4
        /// </summary>
        public string B5_1__Antimicrobial_4 { get; set; }
        /// <summary>
        /// Thông tin về lượng kháng sinh 4 - chỉ điền số
        /// </summary>
        public double B5_2__Strength_of_antimicrobial_4 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh 4
        /// </summary>
        public string B5_3__Units_of_antimicrobial_4 { get; set; }
        /// <summary>
        /// Đơn vị khối lượng mỗi loại
        /// </summary>
        public string B5_4__Per_amount_of_product__antimicrobial_4_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B5_5__Units_of_antimicrobial_4__link_to_question_5_4_ { get; set; }
        /// <summary>
        /// khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B5_6__Per_amount_of_product__volume_of_product___link_to_question_B5_4_ { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
        /// </summary>
        public string B5_7__Units_of_product__link_to_question_B5_4_ { get; set; }
        #endregion

        /// <summary>
		/// Các loài vật
		/// </summary>
        public string B6__Target_species_x { get; set; }
        /// <summary>
        /// Đường dùng thuốc 
        /// </summary>
        public string B7__Administration_route { get; set; }
        #endregion

        //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho heo
        #region C_ Heo

        #region C_1_ Product preparation (dilution) _pig_prevention purpose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string C1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string C1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string C1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string C1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string C1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string C1_6__Duration_of_usage__min__max_ { get; set; }
        #endregion

        #region C.2 Guidelines referred to bodyweight_pig_prevention purpose
        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string C2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string C2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string C2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string C2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string C2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string C2_6__Duration_of_usage { get; set; }
        #endregion

        #region C_3 Product preparation (dilution) _pig_treatment purpose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string C3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string C3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string C3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string C3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string C3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string C3_6__Duration_of_usage { get; set; }


        #endregion

        #region C_4 Guidelines referred to bodyweight_pig_treatment purpose
        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string C4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string C4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string C4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string C4_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string C4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string C4_6__Duration_of_usage { get; set; }
        #endregion

        #endregion

        //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho thú nhai lại không phân biệt thú lớn và nhỏ
        #region D. Động vật nhai lại

        #region D_1_ Product preparation (dilution) _ruminant_prevention purpose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string D1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string D1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string D1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string D1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string D1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string D1_6__Duration_of_usage { get; set; }

        #endregion

        #region D.2. Guidelines referred to bodyweight_ruminant_prevention purpose
        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string D2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string D2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string D2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string D2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string D2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string D2_6__Duration_of_usage { get; set; }
        #endregion

        #region D.3 Product preparation (dilution) _cattle_treatment purpose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string D3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string D3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string D3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string D3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string D3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string D3_6__Duration_of_usage { get; set; }


        #endregion

        #region D_4 Guidelines referred to bodyweight_ruminant_treatment purpose
        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string D4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string D4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>  
        public string D4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string D4_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string D4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string D4_6__Duration_of_usage { get; set; }
        #endregion

        #endregion

        //Phần này thu nhập các thông tin về cách chuẩn bị sản phẩm kháng sinh sử dụng cho gia cầm nói chung bao gồm gà, vịt, ngan, ngỗng, cút
        #region E. Gia cầm

        #region E.1 Product preparation (dilution) _poultry_prevention purpose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string E1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string E1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string E1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string E1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string E1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string E1_6__Duration_of_usage { get; set; }



        #endregion

        #region E.2 Guidelines referred to bodyweight_poultry_prevention purpose
        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>           
        public string E2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string E2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string E2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string E2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string E2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string E2_6__Duration_of_usage { get; set; }
        #endregion

        #region E.3 Product preparation (dilution) _poultry_treatment purpose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string E3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string E3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string E3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string E3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string E3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string E3_6__Duration_of_usage { get; set; }



        #endregion

        #region E.4 Guidelines referred to bodyweight_poultry_treatment purpose
        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>           
        public string E4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string E4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string E4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string E4_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string E4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string E4_6__Duration_of_usage { get; set; }


        #endregion


        #endregion

        //Phần này bao gồm các thông tin thêm về sản phẩm và người nhập dữ liệu để tiện cho việc kiểm tra tính xác thực của thông tin
        #region F.Further information

        /// <summary>
        /// Điền trang web hoặc vetshop
        /// </summary>
        public string F_1__Source_of_information { get; set; }
        /// <summary>
        /// Ảnh sản phẩm
        /// </summary>
        public string F_2__Picture_of_product { get; set; }
        /// <summary>
        /// Thông tin khác
        /// </summary>
        public string F3__Correction { get; set; }
        /// <summary>
        /// Thông tin người nhập
        /// </summary>
        public string F_4__Person_in_charge { get; set; }
        /// <summary>
        /// thời gian cho việc tìm kiếm thu và nhập thông tin sản phẩm
        /// </summary>
        public string F_5__Working_time { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string F_6__Note { get; set; }

        #endregion

        //Heo con
        #region G.Piglet
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Piglet_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Piglet_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Piglet_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Piglet_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Piglet_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Piglet_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Piglet_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Piglet_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Piglet_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Piglet_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Piglet_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Piglet_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Piglet_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Piglet_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Piglet_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Piglet_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Piglet_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Piglet_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Piglet_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Piglet_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Piglet_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Piglet_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Piglet_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Piglet_4_6__Duration_of_usage { get; set; }


        #endregion

        //Trâu
        #region H.Buffalo
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Buffalo_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Buffalo_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Buffalo_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Buffalo_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Buffalo_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Buffalo_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Buffalo_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Buffalo_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Buffalo_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Buffalo_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Buffalo_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Buffalo_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Buffalo_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Buffalo_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Buffalo_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Buffalo_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Buffalo_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Buffalo_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Buffalo_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Buffalo_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Buffalo_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Buffalo_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Buffalo_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Buffalo_4_6__Duration_of_usage { get; set; }


        #endregion

        //Gia súc
        #region I.Cattle
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Cattle_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Cattle_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Cattle_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Cattle_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Cattle_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cattle_1_6__Duration_of_usages { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Cattle_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Cattle_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Cattle_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Cattle_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Cattle_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cattle_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Cattle_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Cattle_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Cattle_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Cattle_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Cattle_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cattle_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Cattle_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Cattle_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Cattle_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Cattle_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Cattle_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cattle_4_6__Duration_of_usage { get; set; }


        #endregion

        //Dê
        #region J.Goat
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Goat_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Goat_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Goat_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Goat_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Goat_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goat_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Goat_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Goat_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Goat_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Goat_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Goat_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goat_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Goat_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Goat_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Goat_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Goat_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Goat_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goat_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Goat_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Goat_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Goat_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Goat_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Goat_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goat_4_6__Duration_of_usage { get; set; }
        #endregion

        //Dê
        #region K.Sheep
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Sheep_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Sheep_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Sheep_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Sheep_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Sheep_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Sheep_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Sheep_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Sheep_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Sheep_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Sheep_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Sheep_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Sheep_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Sheep_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Sheep_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Sheep_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Sheep_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Sheep_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Sheep_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Sheep_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Sheep_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Sheep_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Sheep_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Sheep_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Sheep_4_6__Duration_of_usage { get; set; }
        #endregion

        //Ngựa
        #region L.Horse
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Horse_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Horse_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Horse_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Horse_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Horse_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Horse_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Horse_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Horse_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Horse_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Horse_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Horse_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Horse_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Horse_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Horse_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Horse_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Horse_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Horse_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Horse_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Horse_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Horse_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Horse_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Horse_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Horse_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Horse_4_6__Duration_of_usage { get; set; }
        #endregion

        //Gà
        #region M.Chicken
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Chicken_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Chicken_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Chicken_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Chicken_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Chicken_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chicken_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Chicken_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Chicken_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Chicken_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Chicken_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Chicken_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chicken_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Chicken_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Chicken_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Chicken_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Chicken_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Chicken_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chicken_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Chicken_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Chicken_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Chicken_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Chicken_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Chicken_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chicken_4_6__Duration_of_usage { get; set; }
        #endregion

        //Vịt
        #region N.Duck
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Duck_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Duck_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Duck_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Duck_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Duck_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Duck_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Duck_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Duck_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Duck_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Duck_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Duck_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Duck_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Duck_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Duck_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Duck_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Duck_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Duck_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Duck_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Duck_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Duck_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Duck_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Duck_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Duck_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Duck_4_6__Duration_of_usage { get; set; }
        #endregion

        //Vịt Xiêm
        #region O.Muscovy_Duck
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Muscovy_Duck_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Muscovy_Duck_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Muscovy_Duck_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Muscovy_Duck_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Muscovy_Duck_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Muscovy_Duck_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Muscovy_Duck_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Muscovy_Duck_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Muscovy_Duck_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Muscovy_Duck_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Muscovy_Duck_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Muscovy_Duck_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Muscovy_Duck_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Muscovy_Duck_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Muscovy_Duck_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Muscovy_Duck_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Muscovy_Duck_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Muscovy_Duck_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Muscovy_Duck_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Muscovy_Duck_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Muscovy_Duck_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Muscovy_Duck_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Muscovy_Duck_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Muscovy_Duck_4_6__Duration_of_usage { get; set; }
        #endregion

        //Cút
        #region P.Quail
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Quail_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Quail_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Quail_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Quail_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Quail_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Quail_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Quail_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Quail_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Quail_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Quail_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Quail_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Quail_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Quail_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Quail_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Quail_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Quail_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Quail_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Quail_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Quail_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Quail_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Quail_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Quail_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Quail_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Quail_4_6__Duration_of_usage { get; set; }
        #endregion

        //Ngỗng
        #region Q.Goose
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Goose_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Goose_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Goose_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Goose_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Goose_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goose_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Goose_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Goose_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Goose_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Goose_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Goose_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goose_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Goose_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Goose_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Goose_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Goose_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Goose_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goose_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Goose_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Goose_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Goose_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Goose_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Goose_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Goose_4_6__Duration_of_usage { get; set; }
        #endregion

        //Chó
        #region R.Dog
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Dog_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Dog_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Dog_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Dog_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Dog_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Dog_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Dog_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Dog_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Dog_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Dog_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Dog_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Dog_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Dog_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Dog_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Dog_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Dog_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Dog_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Dog_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Dog_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Dog_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Dog_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Dog_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Dog_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Dog_4_6__Duration_of_usage { get; set; }
        #endregion

        //Mèo
        #region S.Cat
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Cat_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Cat_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Cat_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Cat_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Cat_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cat_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Cat_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Cat_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Cat_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Cat_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Cat_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cat_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Cat_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Cat_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Cat_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Cat_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Cat_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cat_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Cat_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Cat_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Cat_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Cat_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Cat_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Cat_4_6__Duration_of_usage { get; set; }
        #endregion

        //Bê
        #region T.Calf
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Calf_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Calf_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Calf_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Calf_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Calf_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Calf_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Calf_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Calf_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Calf_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Calf_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Calf_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Calf_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Calf_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Calf_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Calf_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Calf_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Calf_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Calf_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Calf_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Calf_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Calf_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Calf_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Calf_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Calf_4_6__Duration_of_usage { get; set; }
        #endregion

        //Gà con
        #region U.Chick
        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Chick_1_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Chick_1_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Chick_1_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Chick_1_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Chick_1_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chick_1_6__Duration_of_usage { get; set; }


        /// <summary>
        /// Lượng thuốc tối thiểu
        /// </summary>
        public string Chick_2_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Chick_2_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Chick_2_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Chick_2_4__Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Chick_2_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chick_2_6__Duration_of_usage { get; set; }


        /// <summary>
        /// sẩn phẩm pha
        /// </summary>
        public string Chick_3_1__Product_preparation__dilution__Product_amount { get; set; }
        /// <summary>
        /// Lượng thuốc để pha
        /// </summary>
        public string Chick_3_2__Product_preparation_Unit_of_product { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối thiểu để pha thuốc
        /// </summary>
        public string Chick_3_3__Product_preparation_To_be_added_to__min_ { get; set; }
        /// <summary>
        /// Lượng nước hoặc thức ăn tối đa để pha thuốc
        /// </summary>
        public string Chick_3_4__Product_preparation_To_be_added_to__max_ { get; set; }
        /// <summary>
        /// Đơn vị tính lượng nước hoặc thức ăn 
        /// </summary>
        public string Chick_3_5__Product_preparation_Unit_of_water_feed { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chick_3_6__Duration_of_usage { get; set; }



        /// <summary>
        /// Lượng thuốc tối thiểu                                                                             
        /// </summary>                                                                             
        public string Chick_4_1__Product_min { get; set; }
        /// <summary>
        /// Lượng thuốc tối đa
        /// </summary>
        public string Chick_4_2__Product_max { get; set; }
        /// <summary>
        /// đơn vị tính
        /// </summary>
        public string Chick_4_3__Unit_of_product { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối thiểu
        /// </summary>
        public string Chick_4_4_Per_No__Kg_bodyweight_min { get; set; }
        /// <summary>
        /// Trọng lượng mỗi Kg tối đa
        /// </summary>
        public string Chick_4_5__Per_No__Kg_bodyweight_max { get; set; }
        /// <summary>
        /// thông tin về ngày sử dụng Ví dụ như kháng sinh được khuyến cáo sử dụng từ 3 tới 5 ngày thì điền 3-5_
        /// </summary>
        public string Chick_4_6__Duration_of_usage { get; set; }
        #endregion
        
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string State { get; set; }

    }

}