using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class OrtherAB
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên thành phần khác
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Thông tin về lượng thành phần
        /// </summary>
        public double Strength { get; set; }
        /// <summary>
        /// Đơn vị khối lượng thành phần
        /// </summary>
        public UnitVolume Units { get; set; }
        /// <summary>
        /// Lượng thành phần tỷ lệ với lượng sản phẩm
        /// </summary>
        public double PerAmountOfAnti { get; set; }
        /// <summary>
        /// Đơn vị khối lượng thành phần cho mỗi loại
        /// </summary>
        public UnitVolume UnitsOfPerAmountAnti { get; set; }
        /// <summary>
        /// Lượng sản phẩm tương ứng
        /// </summary>
        public double PerAmountOfProduct { get; set; }
        /// <summary>
        /// Đơn vị lượng sản phẩm tương ứng
        /// </summary>
        public UnitVolume UnitsOfPerAmountProduct { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        public virtual Product Product { get; set; }
    }
}