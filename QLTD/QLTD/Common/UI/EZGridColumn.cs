using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Common.UI
{
    /// <summary>
    /// Lớp để trình bày model của một cột trong lưới
    /// </summary>
    public class EZGridColumn
    {
        /// <summary>
        /// Tên cột (Biến)
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// Văn bản hiển thị cho cột
        /// </summary>
        public string Text
        {
            get; set;
        }
        /// <summary>
        /// Thứ tự cột trong lưới
        /// </summary>
        public int Order
        {
            get; set;
        }
        /// <summary>
        /// Cho phép sắp xếp hay không
        /// </summary>
        public bool AllowSorting
        {
            get; set;
        }
        /// <summary>
        /// Cho phép hiển thị hay không
        /// </summary>
        public bool AllowDisplay
        {
            get; set;
        }
    }
}