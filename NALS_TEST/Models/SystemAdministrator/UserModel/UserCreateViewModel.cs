
namespace NALSTEST.Models.UserModel
{
    public class UserCreateViewModel
    {
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string BranchCode { get; set; }
        public string PosCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public bool Status { get; set; }
        public bool SupperAdmin { get; set; }

        public string Email_Server { get; set; }
        public string Pass_Email_Server { get; set; }
        public string Email_From { get; set; }
    }
}