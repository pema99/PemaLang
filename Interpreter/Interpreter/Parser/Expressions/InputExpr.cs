using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class InputExpr : Expression
    {
        public Token Token { get; set; }

        public InputExpr(Token Token)
        {
            this.Token = Token;
        }

        public override void Evaluate(Runtime Runtime)
        {
            switch (Token.Type)
            {
                case TokenType.Line:
                    Runtime.Register = Console.ReadLine();
                    break;

                case TokenType.Key:
                    Runtime.Register = Console.ReadKey().KeyChar.ToString();
                    break;
            }
        }
    }
}
