using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class AntimicrobialRequired
    {
        public int Id { get; set; }
        /// <summary>
        /// thú nuôi
        /// </summary>
        public string Pet { get; set; }
        /// <summary>
        /// Trọng lượng trung bình mỗi thú
        /// </summary>
        public double BodyWeight { get; set; }
        /// <summary>
        /// số lượng thú
        /// </summary>
        public int NumberOfPet { get; set; } 
        /// <summary>
        /// Số lượng gói
        /// </summary>
        public int NumberOfPackage { get; set; }
        /// <summary>
        /// trọng lượng mỗi gói
        /// </summary>
        public double VolumnPackage { get; set; }
        /// <summary>
        /// Lượng thuốc mua = WP
        /// </summary>
        public double APA { get; set; }
        /// <summary>
        /// Lượng kháng sinh
        /// </summary>
        public double SA { get; set; }
        /// <summary>
        /// AAA = APA * SA
        /// </summary>
        public double AAA { get; set; }
        /// <summary>
        /// ABW = NumberOfPet x BodyWeight
        /// </summary>
        public double ABW { get; set; }
        /// <summary>
        /// AAA / ABW
        /// </summary>
        public double AAA_ABW { get; set; }
        /// <summary>
        /// lượng nước pha / lượng sản phẩm pha
        /// </summary>
        public double DF { get; set; }
        /// <summary>
        /// Lượng thức ăn tiêu thụ trong 1 ngày
        /// </summary>
        public double DI { get; set; }
        /// <summary>
        /// tổng lượng sản phẩm = (WP)APA * DF 
        /// </summary>
        public double TA { get; set; }
        /// <summary>
        /// Lượng kháng sinh cần mua = SA * (DI/TA)
        /// </summary>
        public double ADDKg { get; set; }

        public int QuestionnaireId { get; set; }
    }
}