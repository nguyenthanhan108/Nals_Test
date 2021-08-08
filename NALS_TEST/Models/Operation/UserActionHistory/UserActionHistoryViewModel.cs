namespace NALSTEST.Models.Operation.UserActionHistory
{
    public class UserActionHistoryViewModel
    {
        public decimal Ordinal { get; set; }
        public decimal Id { get; set; }
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public decimal FuncId { get; set; }
        public string FuncName { get; set; }
        public string ActionType { get; set; }
        public string ActionTime { get; set; }
    }

}