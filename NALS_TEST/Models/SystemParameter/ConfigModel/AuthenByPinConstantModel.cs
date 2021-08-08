using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.SystemParameter.ConfigModel
{
    public class AuthenByPinConstantModel
    {
        public const string AuthenByPinConstantCode = "AUTHEN_BY_PIN";
        public static Dictionary<string,string> Dicts = new Dictionary<string, string>()
        {
            {"OFF","OFF"}, {"ON","ON"}
        }; 
    }
   
}