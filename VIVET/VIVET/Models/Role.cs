using Ehr.Common.Constraint;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ehr.Models
{
    public class Role
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
			get;set;
		}
		/// <summary>
		/// Tạm thời để không bị lỗi
		/// </summary>
		[Column("Role")]
        public UserRole IntRole { get; set; }

		/// <summary>
		/// Các user sở hữu role này
		/// </summary>
        public virtual ICollection<User> Users { get; set; }
		/// <summary>
		/// Các permission mà role này sở hữu
		/// </summary>
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}