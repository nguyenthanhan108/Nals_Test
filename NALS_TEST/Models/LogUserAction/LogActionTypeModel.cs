using System.Collections.Generic;

namespace NALSTEST.Models.LogUserAction
{
    public class LogActionTypeModel
    {
        public static Dictionary<string, string> ActionTypeDicts = new Dictionary<string, string>()
        {
            {"0","Login"},
            {"1","Logout"},
            {"2","Change password"},
            {"3","Insert"},
            {"4","Update"},
            {"5","Thay đổi trạng thái"},
            {"6","Reset password"},
            {"7", "Assign Rule"},
            {"8","Delete"},
            {"9","Confirm"},
            {"A", "Send Approval"}
        };
        public enum ActionTypeEnum
        {
            LOGIN,
            LOGOUT,
            CHANGEPASSWORD,
            INSERT,
            UPDATE,
            CHANGESTATUS,
            RESETPASSWORD,
            ASSIGNRULE,
            DELETE,
            CONFIRM,
            SENDAPPROVAL,
            NOTHING
        }
        private static Dictionary<ActionTypeEnum, string> _toDicts = new Dictionary<ActionTypeEnum, string>()
        {
            {ActionTypeEnum.LOGIN, "0"},
            {ActionTypeEnum.LOGOUT, "1"},
            {ActionTypeEnum.CHANGEPASSWORD, "2"},
            {ActionTypeEnum.INSERT, "3"},
            {ActionTypeEnum.UPDATE, "4"},
            {ActionTypeEnum.CHANGESTATUS, "5"},
            {ActionTypeEnum.RESETPASSWORD, "6"},
            {ActionTypeEnum.ASSIGNRULE, "7"},
            {ActionTypeEnum.DELETE, "8"},
            {ActionTypeEnum.CONFIRM, "9"},
            {ActionTypeEnum.SENDAPPROVAL, "A"}
        };

        public static string Parse(ActionTypeEnum actionType)
        {
            if (!_toDicts.ContainsKey(actionType))
                return string.Empty;
            return _toDicts[actionType];
        }
    }
}