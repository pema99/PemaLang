using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ReturnExpr : Expression
    {
        public Expression Value { get; set; }

        public ReturnExpr(Expression Value)
        {
            this.Value = Value;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Value.Evaluate(Runtime);
            Runtime.Returning = true;
        }
    }
}
