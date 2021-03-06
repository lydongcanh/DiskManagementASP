﻿using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class Customer
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// trạng thái
        /// </summary>
        public CustomerStatus Status { get; set; }
    }
}