using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ClearConsoleExpr : Expression
    {
        public ClearConsoleExpr()
        {
        }

        public override void Evaluate(Runtime Runtime)
        {
            Console.Clear();
        }
    }
}
