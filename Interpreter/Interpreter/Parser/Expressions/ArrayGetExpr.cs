using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ArrayGetExpr : Expression
    {
        public string Identifier { get; set; }
        public Expression Index { get; set; }

        public ArrayGetExpr(string Identifier, Expression Index)
        {
            this.Identifier = Identifier;
            this.Index = Index;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Dictionary<int, object> Array = (Dictionary<int, object>)Runtime.GetVariable(Identifier);
            Index.Evaluate(Runtime);
            int NumIndex = (int)((double)Runtime.Register);
            if (Array.ContainsKey(NumIndex))
            {
                Runtime.Register = Array[NumIndex];
            }
            else
            {
                Runtime.Register = null;
            }
        }
    }
}
