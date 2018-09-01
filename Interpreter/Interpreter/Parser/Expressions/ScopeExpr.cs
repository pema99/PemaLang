using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ScopeExpr : Expression
    {
        public ScopeInfo Body { get; set; }

        public ScopeExpr(ScopeInfo Body)
        {
            this.Body = Body;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Runtime.RunScope(Body);
        }
    }
}
