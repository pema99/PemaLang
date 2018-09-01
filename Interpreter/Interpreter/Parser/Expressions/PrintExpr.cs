using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class PrintExpr : Expression
    {
        public Expression Value { get; set; }
        public bool NewLine { get; set; }

        public PrintExpr(Expression Value, bool NewLine)
        {
            this.Value = Value;
            this.NewLine = NewLine;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Value.Evaluate(Runtime);
            Console.Write(Runtime.Register + (NewLine ? "\n" : ""));
        }
    }
}
