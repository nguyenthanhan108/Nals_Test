using System.Collections.Generic;
using NALSTEST.Models.FunctionModel;

namespace NALSTEST.Repository.FunctionStore
{
    public interface IFunctionStore
    {

      /// <summary>
        /// Lay ve danh sach cac function duoc gan cho user theo userId
      /// </summary>
      /// <param name="id">User Id</param>
      /// <param name="parentId">Function Parent Id</param>
      /// <param name="funcCode">function Code</param>
      /// <param name="funcdisplay">function display</param>
      /// <returns>List FunctionViewModel</returns>
        IList<FunctionViewModel> GetFunctionsByUserId(decimal id, decimal? parentId, string funcCode, string funcdisplay);
        /// <summary>
        /// Lay ve danh sach cac function
        /// </summary>
        /// <returns>List FunctionViewModel</returns>
        IList<FunctionViewModel> GetFunctionsList();

        /// <summary>
        /// Kiem tra xem tai khoan co quyen truy cap function nay khong
        /// </summary>
        /// <param name="userName">Ten tai khoan</param>
        /// <param name="funcId">Id function</param>
        /// <returns>true or false</returns>
        bool CheckUserAccessFunction(string userName, decimal funcId);

        IList<FunctionViewModel> GetFunctionByRoleName(string role, decimal? parentId, string funcCode,
            string funcdisplay);

    }
}