
using System.Collections.Generic;

namespace NALSTEST.Models.FunctionModel
{
    public class FunctionViewModel
    {
        public decimal FuncId { get; set; }
        public string FuncName { get; set; }
        public string FuncCode { get; set; }
        public int FuncOrder { get; set; }
        public string FuncDisplay { get; set; }
        public int FuncLevel { get; set; }
        public decimal? FuncParentId { get; set; }
        public string FuncUrl { get; set; }
        public string FuncControl { get; set; }
        public List<FunctionViewModel> ChildFunctions { get; set; }

        public FunctionViewModel()
        {
            ChildFunctions = new List<FunctionViewModel>();
        }
    }
}