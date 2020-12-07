using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ehr.Models;

namespace Ehr.Common.Constraint
{
	/// <summary>
	/// Lớp hỗ trợ xử lý truy xuất quyền
	/// </summary>
	public class EZAurthenticateSupport
	{		
		/// <summary>
		///	 Instructor của lớp
		/// </summary>
		/// <param name="_account">Truyền dữ liệu của user cần xét vào</param>
		public EZAurthenticateSupport ( User _account)
		{
			if(_account != null)
			{
				Account = _account;
				IsRoot = 0;//mặc định là không có quyền tối cao
				//lấy ra các permission user có
				List<Permission> permissions = new List<Permission> ( );
				foreach(Role role in _account.Roles)
				{
					if(role.IsRoot == YesNo.YES)
						IsRoot = 1;
					if(role.RoleStatus == RoleStatus.ACTIVATED)
					{
						List<Permission> _sub_perms = role.Permissions.ToList ( );
						permissions = permissions.Union ( _sub_perms ).Distinct ( ).ToList ( );
					}
				}
				PermissionList = permissions;
			}			
		}
		/// <summary>
		/// Người dùng đang cần xét quyền
		/// </summary>
		public User Account
		{
			get;set;
		}
		/// <summary>
		/// Danh sách các quyền mà người này có
		/// </summary>
		public List<Permission> PermissionList
		{
			get;set;
		}
		/// <summary>
		/// Xem người này có quyền tối cao hay không: 1- có
		/// </summary>
		public int IsRoot
		{
			get;set;
		}
		/// <summary>
		/// Kiểm tra quyền user theo mã quyền
		/// </summary>
		/// <param name="permissionCode">Mã quyền</param>
		/// <returns></returns>
		public bool CheckRight ( string permissionCode )
		{
			if(PermissionList != null)
			{
				return (PermissionList.Where(permission => permission.PermissionCode.Equals(permissionCode)).ToList().Count>0);
			}
			return false;
		}
		/// <summary>
		/// Kiểm tra quyền user theo đối tượng quyền
		/// </summary>
		/// <param name="permissionCode">Mã quyền</param>
		/// <returns></returns>
		public bool CheckRight ( Permission permission )
		{
			if(PermissionList != null)
			{
				return (PermissionList.Where(perm => perm.Id.Equals(permission.Id)).ToList().Count>0);
			}
			return false;
		}
	}
}