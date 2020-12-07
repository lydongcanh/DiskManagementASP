using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class Reference
    {
        public int Id { get; set; }

        /// <summary>
        /// Loại thú nuôi
        /// </summary>
        public string Pet { get; set; }

        /// <summary>
        /// Tên thú nuôi
        /// </summary>
        public string PetNameDetail { get; set; }

        /// <summary>
        /// Tuần tuổi
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        ///Trọng lượng min (g/con)
        /// </summary>
        public int Volume_Min { get; set; }

        /// <summary>
        /// Trọng lượng max (g/con)
        /// </summary>
        public int Volume_Max { get; set; }

        /// <summary>
        /// Trọng lượng trung bình (g/con)
        /// </summary>
        public double Volume_AVG { get; set; }

        /// <summary>
        /// Lượng ăn min (g/con/ngày)
        /// </summary>
        public int Volume_Food_Min { get; set; }

        /// <summary>
        /// Lượng ăn max (g/con/ngày)
        /// </summary>
        public int Volume_Food_Max { get; set; }

        /// <summary>
        /// Lượng ăn tb (g/con/ngày)
        /// </summary>
        public double Volume_Food_AVG { get; set; }

        /// <summary>
        /// Lượng nước hấp thu min (ml/ con/ngày)
        /// </summary>
        public int Volume_Water_Min { get; set; }

        /// <summary>
        /// Lượng nước hấp thu max (ml/ con/ngày)
        /// </summary>
        public int Volume_Water_Max { get; set; }

        /// <summary>
        /// Lượng nước hấp thu tb (ml/con/ngày)
        /// </summary>
        public double Volume_Water_AVG { get; set; }

        /// <summary>
        /// Ngày tuổi nhỏ nhất
        /// </summary>
        public int Age_Min { get; set; }

        /// <summary>
        /// Ngày tuổi nhỏ nhất
        /// </summary>
        public int Age_Max { get; set; }
    }
}