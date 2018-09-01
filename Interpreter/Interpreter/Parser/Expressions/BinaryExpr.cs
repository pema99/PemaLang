using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class BinaryExpr : Expression
    {
        public Expression Left { get; set; }
        public Token Operator { get; set; }
        public Expression Right { get; set; }

        public BinaryExpr(Expression Left, Token Operator, Expression Right)
        {
            this.Left = Left;
            this.Operator = Operator;
            this.Right = Right;
        }

        public override void Evaluate(Runtime Runtime)
        {
            Left.Evaluate(Runtime);
            object L = Runtime.Register;
            Right.Evaluate(Runtime);
            object R = Runtime.Register;

            switch (Operator.Type)
            {
                //TODO: Fix ugly concatting
                case TokenType.Plus:
                    if (L is double && R is double)
                    {
                        Runtime.Register = (double)L + (double)R;
                    }
                    else if (L is double && R is string)
                    {
                        Runtime.Register = (double)L + (string)R;
                    }
                    else if (L is string && R is double)
                    {
                        Runtime.Register = (string)L + (double)R;
                    }
                    else if (L is string && R is string)
                    {
                        Runtime.Register = (string)L + (string)R;
                    }
                    else if (L is bool && R is string)
                    {
                        Runtime.Register = (bool)L + (string)R;
                    }
                    else if (L is string && R is bool)
                    {
                        Runtime.Register = (string)L + (bool)R;
                    }
                    break;

                case TokenType.Minus:
                    Runtime.Register = (double)L - (double)R;
                    break;

                case TokenType.Slash:
                    Runtime.Register = (double)L / (double)R;
                    break;

                case TokenType.Star:
                    Runtime.Register = (double)L * (double)R;
                    break;

                case TokenType.Bang_Equal:
                    if (L == null || R == null)
                    {
                        Runtime.Register = L != R;
                    }
                    else
                    {
                        Runtime.Register = !L.Equals(R);
                    }
                    break;

                case TokenType.Equal_Equal:
                    if (L == null || R == null)
                    {
                        Runtime.Register = L == R;
                    }
                    else
                    {
                        Runtime.Register = L.Equals(R);
                    }
                    break;

                case TokenType.Less:
                    Runtime.Register = (double)L < (double)R;
                    break;

                case TokenType.Less_Equal:
                    Runtime.Register = (double)L <= (double)R;
                    break;

                case TokenType.Greater:
                    Runtime.Register = (double)L > (double)R;
                    break;

                case TokenType.Greater_Equal:
                    Runtime.Register = (double)L >= (double)R;
                    break;

                case TokenType.And:
                    Runtime.Register = (bool)L && (bool)R;
                    break;

                case TokenType.Or:
                    Runtime.Register = (bool)L || (bool)R;
                    break;

                case TokenType.Xor:
                    Runtime.Register = (bool)L ^ (bool)R;
                    break;

                default:
                    throw new Exception("Invalid binary token \'" + Operator.Lexeme + "\' at line " + Operator.Line);
                    break;
            }
        }
    }
}
