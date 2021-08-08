using NALSTEST.Models.LogUserAction;

namespace NALSTEST.Repository.LogUserAction
{
    public interface ILogUserAction
    {
        LogUserActionCreateModel GetLogModel(decimal userId, int funcId, LogActionTypeModel.ActionTypeEnum actionType, string actionDesc);
        void Insert(LogUserActionCreateModel model);
    }
}