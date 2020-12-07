﻿using Ehr.Common.Constraint;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class AntimiViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên kháng sinh
        /// </summary>
        public int AntimiId { get; set; }
        public string AntimiName { get; set; }
        /// <summary>
        /// Thông tin về lượng kháng sinh 1 - chỉ điền số
        /// </summary>
        public double Strength { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh 1
        /// </summary>
        public UnitVolume Units { get; set; }
        /// <summary>
        /// Lượng kháng sinh tỷ lệ với lượng sản phẩm
        /// </summary>
        public double PerAmountOfAnti { get; set; }
        /// <summary>
        /// Đơn vị khối lượng kháng sinh cho mỗi loại
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

        public int ProductId { get; set; }
    }
}