using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class LambdaExpr : Expression
    {
        public Action Lambda { get; set; }

        public LambdaExpr(Action Lambda)
        {
            this.Lambda = Lambda;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Lambda.Invoke();
        }
    }
}
