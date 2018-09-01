using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ScopeInfo
    {
        public List<Expression> Expressions { get; set; }
        public FuncInfo OwnerFunc { get; set; }

        public ScopeInfo()
        {
            Expressions = new List<Expression>();
        }
    }
}
