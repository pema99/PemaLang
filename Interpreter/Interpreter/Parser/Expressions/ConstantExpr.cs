using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ConstantExpr : Expression
    {
        public object Value { get; set; }

        public ConstantExpr(object Value)
        {
            this.Value = Value;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Runtime.Register = Value;
        }
    }
}
