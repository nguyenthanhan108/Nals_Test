using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.Operation.UserSessionHis
{
    public class UserSessionHisViewModel
    {
        public decimal Ordinal { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}