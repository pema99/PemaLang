using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class VarExpr : Expression
    {
        public string Identifier { get; set; }

        public VarExpr(string Identifier)
        {
            this.Identifier = Identifier;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Runtime.Register = Runtime.GetVariable(Identifier);
        }
    }
}
