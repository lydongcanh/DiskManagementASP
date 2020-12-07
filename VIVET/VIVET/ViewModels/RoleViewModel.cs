using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên của vai trò
        /// </summary>
        public string RoleName { get; set; }
        //[Column("Role")]
        //public UserRole IntRole { get; set; }
        /// <summary>
        /// Trạng thái kích hoạt của vai trò
        /// </summary>
        public RoleStatus RoleStatus
        {
            get; set;
        }
        /// <summary>
        /// Xem role này có đặc quyền cao nhất không (Nếu là Yes thì không cần phải xét quyền)
        /// </summary>
        public YesNo IsRoot
        {
            get; set;
        }

        /// <summary>
        /// Các permission mà role này sở hữu
        /// </summary>
        public string Permissions { get; set; }
    }
}