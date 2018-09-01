using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ArrayExpr : Expression
    {
        public List<Expression> Initializer { get; set; }

        public ArrayExpr(List<Expression> Initializer)
        {
            this.Initializer = Initializer;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Dictionary<int, object> Result = new Dictionary<int, object>();
            for (int i = 0; i < Initializer.Count; i++)
            {
                Initializer[i].Evaluate(Runtime);
                Result.Add(i, Runtime.Register);
            }
            Runtime.Register = Result;
        }
    }
}
