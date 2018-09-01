using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Interpreter
    {
        public Runtime Runtime { get; private set; }
        public Lexer Lexer { get; private set; }
        public Parser Parser { get; private set; }

        public Interpreter()
        {
            this.Runtime = new Runtime();
            this.Lexer = new Lexer();
            this.Parser = new Parser();
        }

        public void DoString(string Code)
        {
            DoAST(Parse(Lex(Code)));
        }

        public void DoAST(Dictionary<string, Expression> AST)
        {
            Runtime.Process(AST);
        }

        public object Call(string FuncName, params object[] Parameters)
        {
            return Runtime.Call(FuncName, Parameters);
        }

        public List<Token> Lex(string Code)
        {
            return Lexer.ScanTokens(Code);
        }

        public Dictionary<string, Expression> Parse(List<Token> Tokens)
        {
            return Parser.Parse(Tokens);
        }
    }
}
