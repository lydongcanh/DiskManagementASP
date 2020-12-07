using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Common.UI
{
    /// <summary>
    /// Lớp để model lại cho hiển thị các stores
    /// </summary>
    public class StoreListing
    {
        public string StoreName
        {
            get; set;
        }
        public long StoreID
        {
            get; set;
        }
        public string CityName
        {
            get; set;
        }
        public bool SelectedItem
        {
            get; set;
        }
    }
}