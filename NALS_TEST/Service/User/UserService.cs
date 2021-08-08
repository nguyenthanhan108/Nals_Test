using NALSTEST.Models.UserModel;
using NALSTEST.Repository.UserStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NALSTEST.Helpers;
using NALSTEST.Models.SystemAdministrator.UserModel;
using NALSTEST.Service.Config;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace NALSTEST.Service.User
{
    public class UserService
    {
        UserStoreService userStore = new UserStoreService();
        public bool Login(string userName, string password, string ip, ref string codelogin)
        {
            var dt = userStore.Login(userName, password, ip, ref codelogin);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CheckSession(string userName,  string ip)
        {
            var dt = userStore.CheckSession(userName, ip);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CheckSessionOut(string userName, string time)
        {
            var dt = userStore.CheckSessionOut(userName, time);
            if (dt)
                return true;
            else
                return false;
        }
        public bool UpdateSession(string userName, string time)
        {
            var dt = userStore.UpdateSession(userName, time);
            if (dt)
                return true;
            else
                return false;
        } 
        public UserLoginViewModel GetUserByName(string userName)
        {
            try
            {
                var model = new UserLoginViewModel();
                var dt = userStore.GetUserByName(userName);
                if (dt.Rows.Count == 0) return null;
                model.FullName = dt.Rows[0]["FULLNAME"].ToString();
                model.UserName = dt.Rows[0]["USERNAME"].ToString();
                model.BranchCode = dt.Rows[0]["BRANCH_CODE"].ToString();
                model.PosCode = dt.Rows[0]["POS_CODE"].ToString();
                model.UserId = Decimal.Parse(dt.Rows[0]["USER_ID"].ToString());
                model.DateUpdate = dt.Rows[0]["CHANGE_PASS_DATE"].ToString();
                model.Branch = dt.Rows[0]["BRANCH_NAME"].ToString();
                model.Email = dt.Rows[0]["EMAIL"].ToString();
                
                return model;
            }
            catch (Exception ex)
            {
                return new UserLoginViewModel();
            }
        }
        public UserListViewModel GetUserById(decimal userId)
        {
            try
            {
                var dt = userStore.GetUserById(userId);
                var model = new UserListViewModel()
                {
                    UserId = Int64.Parse(dt.Rows[0]["USER_ID"].ToString()),
                    UserName = dt.Rows[0]["USERNAME"].ToString(),
                    FullName = dt.Rows[0]["FULLNAME"].ToString(),
                    BranchCode = dt.Rows[0]["BRANCH_CODE"].ToString(),
                    PosCode = dt.Rows[0]["POS_CODE"].ToString(),
                    RoleId = dt.Rows[0]["ROLE_ID"].ToString(),
                    Phone = dt.Rows[0]["PHONE"].ToString(),
                    Email = dt.Rows[0]["EMAIL"].ToString(),
                    Status = dt.Rows[0]["STATUS"].ToString() == "1",
                    SupperAdmin = dt.Rows[0]["USER_ROLETYPE"].ToString().Trim().ToUpper() == "S"
                };
                return model;
            }
            catch (Exception ex)
            {
                return new UserListViewModel();
            }
        }
        public UserProfileViewModel GetProfileUser(decimal userId)
        {
            try
            {
                var model = new UserProfileViewModel();
                var dt = userStore.GetProfileUser(userId);
                model.LastLogin = CommonHelper.ToDateTimeHHMMDDMMYYYY(dt.Rows[0]["LASTLOGIN"].ToString());

                model.LastIpLogin = dt.Rows[0]["LASTIPLOGIN"].ToString();
                model.Branch = dt.Rows[0]["BRANCH"].ToString();
                model.RoleId = Convert.ToInt32(dt.Rows[0]["ROLE_ID"].ToString());
                model.RoleName = dt.Rows[0]["ROLE_NAME"].ToString();
                return model;
            }
            catch (Exception ex)
            {
                return new UserProfileViewModel();
            }
        }
        public IList<UserViewModel> GetListUser(string userName, string status, string branch, string bankPos, string role, string fromdate, string todate)
        {
            try
            {
                var datas = new List<UserViewModel>();
                var dt = userStore.GetListUser(userName, status, branch, bankPos, role,fromdate,todate);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new UserViewModel()
                    {
                        Stt = i + 1,
                        UserId = Int64.Parse(dt.Rows[i]["USER_ID"].ToString()),
                        UserName = dt.Rows[i]["USERNAME"].ToString(),
                        FullName = dt.Rows[i]["FULLNAME"].ToString(),
                        Status = dt.Rows[i]["STATUS"].ToString().Trim() == "1",
                        CreateByUser = dt.Rows[i]["CREATED_USER"].ToString(),
                        Branch = dt.Rows[i]["BRANCH_NAME"].ToString(),
                        BankPos = dt.Rows[i]["POS_NAME"].ToString(),
                        CreateDate = CommonHelper.ToDateTimeStringDDMMYYY( dt.Rows[i]["CREATED_DATE"].ToString()),
                        Email = dt.Rows[i]["EMAIL"].ToString()

                    };
                    datas.Add(model);

                }
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetUserStatus(decimal id)
        {
            var dt = userStore.GetUserStatus(id);
                return dt;
        }
        public int CreateUser(UserCreateViewModel model, string userCreate, string password, ref string loidb)
        {
            try
            {
                var config = new ConfigService();
                model.Email_Server = config.GetConfigById("EMAIL_SERVER_MAIL").Value;
                model.Pass_Email_Server = config.GetConfigById("PASS_SERVER_MAIL").Value;

                string smtpUserName = model.Email_Server;
                string smtpPassword = model.Pass_Email_Server;
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 25;

                string emailTo = model.Email;
                string body = string.Format("<b>Passworld là:</b><br/>{0}<br/>",
                   password);
                UserService service = new UserService();
                bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, body);
                var dt = userStore.CreateUser(model, userCreate, password, ref loidb);
                if (dt > 0)
                    return 1;
                else
                    return -1;
            }
            catch(Exception) {
                return -1;
            }
            
        }

        public bool Send( string smtpUserName, string smtpPassword, string smtpHost, int smtpPort,
               string toEmail, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = smtpPort;
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                    var msg = new MailMessage
                    {
                        Subject = "Pass World",
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(smtpUserName),
                        Body = body,
                        Priority = MailPriority.Normal,
                    };

                    msg.To.Add(toEmail);

                    smtpClient.Send(msg);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateUser(UserCreateViewModel model, ref string loidb)
        {
            var dt = userStore.UpdateUser(model, ref loidb);
            if (dt)
                return true;
            else
                return false;
        }        
        public bool ChangeStatus(decimal userId, string status,ref string loidb)
        {
            var dt = userStore.ChangeStatus(userId, status,ref loidb);
            if (dt)
                return true;
            else
                return false;
        }
        public string ChangePassword(decimal userId, string oldPassword, string newPassword, ref string erorr)
        {
            var dt = userStore.ChangePassword(userId, oldPassword, newPassword, ref erorr);
            return dt;
        }
        public bool ResetPassword(decimal userId, string newPassword,ref string loidb)
        {
            try {
                var config = new ConfigService();
                UserService service = new UserService();
                

                string smtpUserName = config.GetConfigById("EMAIL_SERVER_MAIL").Value;
                string smtpPassword = config.GetConfigById("PASS_SERVER_MAIL").Value;
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 25;

                string emailTo = service.GetUserById(userId).Email;
                string body = string.Format("<b>Passworld là:</b><br/>{0}<br/>",
                   newPassword);
                
                bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, body);

                var dt = userStore.ResetPassword(userId, newPassword, ref loidb);
                if (dt)
                    return true;
                else
                    return false;
            }
            catch(Exception){
                return false;
            }
            
        }
        public bool DeleteUser(decimal id)
        {
            try
            {
                var dt = userStore.DeleteUser(id);
                if (dt)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AssignRole(decimal userId, string roleIds, string userCreated, ref string loidb)
        {
            var dt = userStore.AssignRole(userId, roleIds, userCreated, ref loidb);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CheckPassChange(string userName)
        {
            var dt = userStore.CheckPassChange(userName);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CheckPassReset(string userName)
        {
            var dt = userStore.CheckPassReset(userName);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CheckBlockUser(string userName)
        {
            var dt = userStore.CheckBlockUser(userName);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CheckHistoryPassWord(decimal userID, string password, int lengtop)
        {
            var dt = userStore.CheckHistoryPassWord(userID, password, lengtop);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CreateHistoryPass(int userID, string password)
        {
            var dt = userStore.CreateHistoryPass(userID, password);
            if (dt)
                return true;
            else
                return false;
        }

        public bool CheckLifePass(string date, int slenglife) 
        {
            try
            {
                double tam;
                tam = (DateTime.Now - DateTime.Parse(date)).TotalDays;
                if (tam >= slenglife) 
                    return true;
                else
                    return false;               
            }
            catch (Exception ex )
            {
            return false;
            }
        }

       public bool CheckPolicyPassword(string password)
       {
        if (CheckNumber(password) && CheckAlphalet(password))
        {
            return true;
        }

           return false;
       }

        public bool CheckNumber(string sPass) 
        {
            try
            {
                int tam =0;
                int lengh = sPass.Length;
                    for ( int i = 0  ; i <= lengh -1 ; i++)
                    {
                        if( Char.IsNumber(sPass,i))
                            return true;
                    }
                    return false;
            }
            catch (Exception ex )
            {
            return false;
            }
        }
         public bool CheckAlphalet(string sPass) 
        {
            try
            {
                string  map  = "asdfghjklqwertyuiopzxcvbnmASDFGHJKLQWERTYUIOPZXCVBNM" ; 
                int lengh = sPass.Length;
                for (int i = 0; i <= lengh - 1; i++)
                {
                    if (map.Contains(sPass.Substring(i,1)))
                        return true;
                }
                return false;
            }
            catch (Exception ex )
            {
            return false;
            }
        }

        public bool CheckCharter(string sPass)
        {
            try
            {
                 string  map  = "!#$%&'()*+,-./:;<=>?@[\\]^_`{|}~/" ; 
                int lengh = sPass.Length;
                for (int i = 0; i <= lengh - 1; i++)
                {
                     if (map.Contains(sPass.Substring(i,1)))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }  
        //public bool CheckModifyUser(string userCreated, decimal userId)
        //{
        //    var dt = userStore.CheckModifyUser(userCreated, userId);
        //    if (dt)
        //        return true;
        //    else
        //        return false;
        //}        
    }
}