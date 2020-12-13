using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public DateTime DateRent { get; set; }
        public DateTime DateReceipt { get; set; }
        public RentDetailState State { get; set; }
    }
}