using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexeme { get; set; }
        public object Literal { get; set; }
        public int Line { get; set; }

        public Token(TokenType Type, string Lexeme, object Literal, int Line)
        {
            this.Type = Type;
            this.Lexeme = Lexeme;
            this.Literal = Literal;
            this.Line = Line;
        }
    }
}
