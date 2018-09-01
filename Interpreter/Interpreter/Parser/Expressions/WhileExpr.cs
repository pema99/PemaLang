using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class WhileExpr : Expression
    {
        public Expression Condition { get; set; }
        public ScopeInfo Body { get; set; }

        public WhileExpr(Expression Condition, ScopeInfo Body)
        {
            this.Condition = Condition;
            this.Body = Body;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Condition.Evaluate(Runtime);
            bool Current = (bool)Runtime.Register;
            while(Current == true)
            {
                Runtime.RunScope(Body);

                Condition.Evaluate(Runtime);
                Current = (bool)Runtime.Register;
            }
        }
    }
}
