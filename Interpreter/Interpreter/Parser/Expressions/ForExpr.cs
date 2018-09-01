using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class ForExpr : Expression
    {
        public Expression Initial { get; set; }
        public Expression Condition { get; set; }
        public Expression Iteration { get; set; }
        public ScopeInfo Body { get; set; }

        public ForExpr(Expression Initial, Expression Condition, Expression Iteration, ScopeInfo Body)
        {
            this.Initial = Initial;
            this.Condition = Condition;
            this.Iteration = Iteration;
            this.Body = Body;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Scope Outer = new Scope(new ScopeInfo());
            Runtime.Stack.Push(Outer);

            Initial.Evaluate(Runtime);

            Condition.Evaluate(Runtime);
            bool Current = (bool)Runtime.Register;
            while (Current == true)
            {
                Runtime.RunScope(Body);

                Iteration.Evaluate(Runtime);
                Condition.Evaluate(Runtime);
                Current = (bool)Runtime.Register;
            }

            Runtime.Stack.Pop();
        }
    }
}
