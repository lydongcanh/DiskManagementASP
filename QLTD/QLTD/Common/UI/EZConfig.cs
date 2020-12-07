using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Ehr.Common.UI
{
	/// <summary>
	/// Các tham số chung của hệ thống
	/// </summary>
	public class EZConfig
	{
		/// <summary>
		/// Đường dẫn chứa các file upload
		/// </summary>
		public static string UploadPath = ConfigurationManager.AppSettings["uploadPath"];				
	}
}