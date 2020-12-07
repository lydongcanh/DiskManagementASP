using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class ProductRequired
    {
        public int Id { get; set; }
        /// <summary>
        /// thú nuôi
        /// </summary>
        public string Pet { get; set; }
        /// <summary>
        /// số lượng thú
        /// </summary>
        public int NumberOfPet { get; set; }
        /// <summary>
        /// số ngày dùng
        /// </summary>
        public int NumberOfDate { get; set; }
        /// <summary>
        /// trọng lượng trung bình của thú nuôi
        /// </summary>
        public double KgOfPet { get; set; }
        /// <summary>
        /// Khối lượng sản phẩm
        /// </summary>
        public double WP { get; set; }
        /// <summary>
        /// Hệ số pha loãng
        /// </summary>
        public double DF { get; set; }
        /// <summary>
        /// Lượng thức ăn tiêu thụ trong 1 ngày
        /// </summary>
        public double DI { get; set; }
        /// <summary>
        /// Lượng thuốc cần mua
        /// </summary>
        public double NumberOfProductRequired { get; set; }

        public int QuestionnaireId { get; set; }
    }
}