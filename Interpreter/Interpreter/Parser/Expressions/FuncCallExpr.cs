using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class FuncCallExpr : Expression
    {
        public Expression FuncExpr { get; set; }
        public List<Expression> Parameters { get; set; }

        public FuncCallExpr(Expression FuncExpr, List<Expression> Parameters)
        {
            this.FuncExpr = FuncExpr;
            this.Parameters = Parameters;
        }

        public override void Evaluate(Runtime Runtime)
        {
            FuncExpr.Evaluate(Runtime);
            if (!(Runtime.Register is FuncInfo))
            {
                if (FuncExpr is VarExpr)
                {
                    throw new Exception("Runtime error. \'" + (FuncExpr as VarExpr).Identifier + "\' is not a function.");
                }
                else
                {
                    throw new Exception("Runtime error. Attempt to call non-function.");
                }
            }
            FuncInfo Function = (FuncInfo)Runtime.Register;

            if (Function.Parameters.Count != Parameters.Count)
            {
                //Parameter mismatch
                throw new Exception("Invalid amount of parameters for function \'" + Function.Identifier + "\' Found " + Parameters.Count + ", expected " + Function.Parameters.Count);
            }

            Runtime.RunScope(Function.Body, Parameters);
            if (Runtime.Returning)
            {
                Runtime.Returning = false;
            }

            //Return null from function if no return statement was present
            else
            {
                Runtime.Register = null;
            }
        }
    }
}
