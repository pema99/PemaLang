using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class UnaryExpr : Expression
    {
        public Token Operator { get; set; }
        public Expression Operand { get; set; }

        public UnaryExpr(Token Operator, Expression Operand)
        {
            this.Operator = Operator;
            this.Operand = Operand;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Operand.Evaluate(Runtime);
            if (Operator.Type == TokenType.Bang)
            {
                Runtime.Register = !(bool)Runtime.Register;
            }
            else
            {
                double Value = (double)Runtime.Register;

                if (Operator.Type == TokenType.Minus) Value = -Value;
                else if (Operator.Type == TokenType.Plus) Value = +Value;

                Runtime.Register = Value;
            }
        }
    }
}
