
namespace NALSTEST.Models.LogUserAction
{
    public class LogUserActionCreateModel
    {
        public decimal UserId { get; set; }
        public int FuncId { get; set; }
        public string ActionType { get; set; }
        public string ActionDesc { get; set; }
    }
}