using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.Customer
{
    public class CustomerModels
    {
        public int Ordinal { get; set; }
        public string MobileId { get; set; }
        public string MobileNo { get; set; }
        public string Role { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string ServiceCode { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
    }
}