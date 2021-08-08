using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.Cache
{
    public class MessageModel
    {
        public string type { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string valueEN { get; set; }

        public string func { get; set; }
    }
}