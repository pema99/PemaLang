using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class VarAssignExpr : Expression
    {
        public string Identifier { get; set; }
        public Expression Initializer { get; set; }

        public VarAssignExpr(string Identifier, Expression Initializer)
        {
            //TODO: Add support for no initializer
            this.Identifier = Identifier;
            this.Initializer = Initializer;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Initializer.Evaluate(Runtime);
            Runtime.SetVariable(Identifier, Runtime.Register);
        }
    }
}
