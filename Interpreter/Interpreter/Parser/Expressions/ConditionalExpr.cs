using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ConditionalExpr : Expression
    {
        public Expression Condition { get; set; }
        public ScopeInfo Body { get; set; } 
        public Expression Alternate { get; set; }

        public ConditionalExpr(Expression Condition, ScopeInfo Body, Expression Alternate)
        {
            this.Condition = Condition;
            this.Body = Body;
            this.Alternate = Alternate;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Condition.Evaluate(Runtime);
            if ((bool)Runtime.Register == true)
            {
                Runtime.RunScope(Body);
            }
            else if (Alternate != null)
            {
                Alternate.Evaluate(Runtime);
            }
        }
    }
}
