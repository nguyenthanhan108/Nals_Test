//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.OracleClient;
//using System.Linq;
//using System.Web;
//using DBConnection;
//using MBVABBank.Helpers;
//using MBVABBank.Models.SystemAdministrator.UserModel;
//using MBVABBank.Models.UserModel;
//using Newtonsoft.Json.Linq;

//namespace MBVABBank.Repository.SystemAdminstrator.UserStore
//{
//    public class UserIntegrateBIDVService : IUserIntegrateBIDV
//    {
//        private OracleDB db = new OracleDB();
//        public UserInforBidv LoginBIDV(string userName, string password, string ip)
//        {
//            try
//            {
//                //var _loginService = new OrderServiceService();
//                string appVersion = System.Configuration.ConfigurationManager.AppSettings["appVersion"];
//                string appCode = System.Configuration.ConfigurationManager.AppSettings["appCode"];
//                string pfxFile = System.Configuration.ConfigurationManager.AppSettings["pfxFile"];
//                string pfxPass = System.Configuration.ConfigurationManager.AppSettings["pfxPass"]; //123456
//                string cerFile = System.Configuration.ConfigurationManager.AppSettings["cerfile"];
//                //NLogHelper.Logger.Info(appVersion +"," + appCode + "," + pfxFile + "," + pfxPass + "," + cerFile);
//                //var request =
//                //"<DOC><MESSAGECODE>1001</MESSAGECODE><CERTNAME>00102</CERTNAME><XML><REF>2011032909491010418</REF><DATA><USERNAME>33807</USERNAME><PASSWORD>fvOi8fPVaoxsJdCvvfBvytY7o036ZNz0LYNYt+sY6xjo1aK0fKg311yZRtlrAY6kB47ZV4B+U3O4CYTwHM12rPJ0GnDF5f3oSoOm10K+qcoLgHptc8Pu0XpcrAa+xPsXy8JO6994BzE/SXcO5OIT66mMZOpgHNunHFV0n3SKD1g=</PASSWORD><APPCODE>KMTT</APPCODE><APPVERSION>1.0</APPVERSION><IP>127.0.0.1</IP></DATA></XML><SIGNATURE>oSBLNj0oYNeJKJU3Nu8DnVJCOTd93pASS3p6es3gwHuSe3788ta3asEuOadTBTTQxAPzXCASGzSqe9j4+xxVCSLaPMvkaP5Iwazr9PSOP1joSod9RO/fjDvzqSdDjF+Xn7BQxNQSidHL+J3C/crHA1ty69/+2KZ5Zgt0k8wUbJg=</SIGNATURE></DOC>";
//                var request = MessagesProcessor.CreateSendMsg(userName, password, ip, appVersion, appCode, pfxFile,
//                    pfxPass, cerFile);
//                NLogHelper.Logger.Info(string.Format("LoginBIDV request: {0}", request));
//                var mesg = _loginService.submit(request);
//                //var mesg = CommonHelper.ConvertXmlFileToJson(@"C:\Users\Administrator\Desktop\new3.xml");
//                NLogHelper.Logger.Info(string.Format("LoginBIDV mesg: {0}", mesg));
//                if (mesg == null) return new UserInforBidv()
//                {
//                    Check = false,
//                    LoginMessage = "Tài khoản hoặc mật khẩu không đúng."
//                };
//                var obj = CommonHelper.ConvertXmlStringToJson(mesg);
//                var model = GetUserInforBidv(obj, userName);
//                if (model == null)
//                {
//                    return new UserInforBidv()
//                    {
//                        Check = false,
//                        LoginMessage = "Tài khoản hoặc mật khẩu không đúng."
//                    };
//                }
//                //var model = GetUserInforBidv(mesg, userName);
//                return model;
//            }
//            catch (Exception ex)
//            {
//                NLogHelper.Logger.Error(string.Format("LoginBIDV: {0}", ex));
//                return new UserInforBidv()
//                {
//                    Check = false,
//                    LoginMessage = "Tài khoản hoặc mật khẩu không đúng."
//                };
//            }
//        }

//        public bool CreateUser(string userName, string fullName, string branch, string role, string userCreate)
//        {
//            try
//            {
//                var sql = CommonHelper.GetProcedurePkgLoginBIDV("BO_USER_Insert");
//                var oracParams = new[]
//                {
//                    new OracleParameter("p_out", OracleType.Cursor),
//                    new OracleParameter("p_username", OracleType.VarChar),
//                    new OracleParameter("p_fullname", OracleType.VarChar),
//                    new OracleParameter("p_branch_code", OracleType.VarChar),
//                    new OracleParameter("p_role", OracleType.VarChar), 
//                    new OracleParameter("p_created_user", OracleType.VarChar),
//                    new OracleParameter("p_user_id", OracleType.Number)
//                };
//                oracParams[0].Direction = ParameterDirection.Output;
//                oracParams[1].Value = userName;
//                oracParams[2].Value = fullName;
//                oracParams[3].Value = branch;
//                oracParams[4].Value = role;
//                oracParams[5].Value = userCreate;
//                oracParams[6].Value = Convert.ToDecimal(userName);
//                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
//                return dt.Rows[0][0].ToString() == "1";
//            }
//            catch (Exception ex)
//            {
//                NLogHelper.Logger.Error(string.Format("CreateUser: {0}", ex.ToString()));
//                return false;
//            }
//        }

//        public void InsertSessionHis(string userId, string ip, int timeOut)
//        {
//            try
//            {
//                var sql = CommonHelper.GetProcedurePkgLoginBIDV("BO_USER_InsertSessionHis");
//                var oracParams = new[]
//                {
//                    new OracleParameter("p_user_id", OracleType.Number),
//                    new OracleParameter("p_ip", OracleType.VarChar),
//                    new OracleParameter("p_timeout", OracleType.Number)
//                };
//                oracParams[0].Value = Convert.ToDecimal(userId);
//                oracParams[1].Value = ip;
//                oracParams[2].Value = timeOut;
//                db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
//            }
//            catch (Exception ex)
//            {
//                NLogHelper.Logger.Error(string.Format("InsertSessionHis: {0}", ex));
//            }
//        }

//        private UserInforBidv GetUserInforBidv(string json, string userName)
//        {
//            try
//            {
//                var obj = JObject.Parse(json);
//                NLogHelper.Logger.Info(obj);
//                var data = obj["DOC"]["XML"];
//                var result = data["RESULT"].ToString();
//                if (result != "000")
//                    return new UserInforBidv()
//                    {
//                        Check = false,
//                        LoginMessage = data["ERRORDETAIL"].ToString()
//                    };
//                var model = new UserInforBidv()
//                {
//                    BranchCode = data["BRN"].ToString(),
//                   // BranchName = ReplaceTextHelper.ReplaceForInforFromBidv(data["BRANCH"].ToString()).Replace("\\", ""),
//                    UserName = userName,
//                    //Department = ReplaceTextHelper.ReplaceForInforFromBidv(data["DEPARTMENT"].ToString().Replace("\\", "")),
//                    //FullName = ReplaceTextHelper.ReplaceForInforFromBidv(data["FULLNAME"].ToString()).Replace("\\", ""),
//                    Check = true
//                };
//                var listFunc = data["LIST"]["ROW"];
//                //model.RoleName = data["LIST"]["ROW"][0]["FUNCLINK"].ToString();
//                model.RoleName = GetRoleName(listFunc);
//                return model;
//            }
//            catch (Exception ex)
//            {
//                NLogHelper.Logger.Error(string.Format("GetUserInforBidv: {0}", ex));
//                return null;
//            }

//        }

//        private string GetRoleName(JToken token)
//        {
//            return token.Type == JTokenType.Array ? token[0]["FUNCLINK"].ToString() : token["FUNCLINK"].ToString();
//        }
//    }
//}