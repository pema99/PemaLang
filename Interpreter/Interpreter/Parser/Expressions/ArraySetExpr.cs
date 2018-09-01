using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ArraySetExpr : Expression
    {
        public string Identifier { get; set; }
        public Expression Index { get; set; }
        public Expression Value { get; set; }

        public ArraySetExpr(string Identifier, Expression Index, Expression Value)
        {
            this.Identifier = Identifier;
            this.Index = Index;
            this.Value = Value;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Dictionary<int, object> Array = (Dictionary<int, object>)Runtime.GetVariable(Identifier);
            Index.Evaluate(Runtime);
            int NumIndex = (int)((double)Runtime.Register);
            Value.Evaluate(Runtime);
            Array[NumIndex] = Runtime.Register;
        }
    }
}
