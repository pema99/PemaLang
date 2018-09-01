using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Lexer
    {
        public string Source { get; private set; }
        public List<Token> Tokens { get; private set; }
        private int Start { get; set; }
        private int Current { get; set; }
        private int Line { get; set; }
        public Dictionary<string, TokenType> Keywords { get; private set; }

        public Lexer()
        {
            this.Keywords = new Dictionary<string, TokenType>(); 

            Keywords.Add("else", TokenType.Else);
            Keywords.Add("false", TokenType.False);
            Keywords.Add("for", TokenType.For);
            Keywords.Add("func", TokenType.Func);
            Keywords.Add("if", TokenType.If);
            Keywords.Add("null", TokenType.Null);
            Keywords.Add("print", TokenType.Print);
            Keywords.Add("println", TokenType.PrintLine);
            Keywords.Add("return", TokenType.Return);
            Keywords.Add("true", TokenType.True);
            Keywords.Add("var", TokenType.Var);
            Keywords.Add("while", TokenType.While);
            Keywords.Add("line", TokenType.Line);
            Keywords.Add("key", TokenType.Key);
            Keywords.Add("cls", TokenType.Clear);
        }

        public List<Token> ScanTokens(string Source)
        {
            this.Source = Source;
            this.Tokens = new List<Token>();
            this.Start = 0;
            this.Current = 0;
            this.Line = 1;

            while (Current < Source.Length)
            {
                Start = Current;
                ScanToken();
            }
            Tokens.Add(new Token(TokenType.EOF, null, null, Line));

            return Tokens;
        }

        private void ScanToken()
        {
            char C = Advance();
            switch (C)
            {
                //Tokens
                case '(': AddToken(TokenType.Left_Paren); break;
                case ')': AddToken(TokenType.Right_Paren); break;
                case '{': AddToken(TokenType.Left_Brace); break;
                case '}': AddToken(TokenType.Right_Brace); break;
                case '[': AddToken(TokenType.Left_Square); break;
                case ']': AddToken(TokenType.Right_Square); break;
                case ',': AddToken(TokenType.Comma); break;
                case '.': AddToken(TokenType.Dot); break;
                case '-': AddToken(Match('=') ? TokenType.Minus_Equal : Match('-') ? TokenType.Minus_Minus : TokenType.Minus); break;
                case '+': AddToken(Match('=') ? TokenType.Plus_Equal : Match('+') ? TokenType.Plus_Plus : TokenType.Plus); break;
                case '*': AddToken(Match('=') ? TokenType.Star_Equal : TokenType.Star); break;
                case '/': AddToken(Match('=') ? TokenType.Slash_Equal : TokenType.Slash); break;

                //Operators
                case '!': AddToken(Match('=') ? TokenType.Bang_Equal : TokenType.Bang); break;
                case '=': AddToken(Match('=') ? TokenType.Equal_Equal : TokenType.Equal); break;
                case '<': AddToken(Match('=') ? TokenType.Less_Equal : TokenType.Less); break;
                case '>': AddToken(Match('=') ? TokenType.Greater_Equal : TokenType.Greater); break;
                case '&': if (Match('&')) AddToken(TokenType.And); break;
                case '|': if (Match('|')) AddToken(TokenType.Or); break;
                case '^': if (Match('^')) AddToken(TokenType.Xor); break;

                //Literals
                case '"': String(); break;

                //Whitespace
                case ' ': break;
                case '\r': break;
                case '\t': break;
                case '\n': Line++; break;

                //Comments
                case '\'': Comment(); break;

                default:
                    if (IsDigit(C))
                    {
                        Number();
                    }
                    else if (IsAlpha(C))
                    {
                        Identifier();
                    }
                    else
                    {
                        throw new Exception("Unexpected symbol \'" + C + "\' at line " + Line);
                    }
                    break;
            }
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string Text = Source.Substring(Start, Current - Start);
            if (Keywords.ContainsKey(Text))
            {
                AddToken(Keywords[Text]);
            }

            else
            {
                AddToken(TokenType.Identifier);
            }
        }

        private bool IsAlpha(char C)
        {
            return (C >= 'a' && C <= 'z') ||
                   (C >= 'A' && C <= 'Z') ||
                    C == '_';
        }

        private bool IsAlphaNumeric(char C)
        {
            return IsAlpha(C) || IsDigit(C);
        }

        private void Number()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }
            if (Peek() == '.')
            {
                Advance();
                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }
            
            AddToken(TokenType.Number, double.Parse(Source.Substring(Start, Current - Start), CultureInfo.InvariantCulture));
        }

        private bool IsDigit(char C)
        {
            return C >= '0' && C <= '9';
        }

        private void String()
        {
            while (Peek() != '"' && Current < Source.Length)
            {
                if (Peek() == '\n')
                {
                    Line++;
                }
                Advance();
            }
            if (Current >= Source.Length)
            {
                throw new Exception("Unterminated string at line " + Line);
                return;
            }
            Advance();

            AddToken(TokenType.String, Source.Substring(Start+1, (Current-1)-(Start+1)));
        }

        private void Comment()
        {
            while (Peek() != '\n' && Current < Source.Length)
            {
                Advance();
            }
        }

        private char Peek()
        {
            return Source[Current];
        }

        private bool Match(char Expected)
        {
            if (Peek() != Expected)
            {
                return false;
            }
            Current++;
            return true;
        }

        private char Advance()
        {
            Current++;
            return Source[Current - 1];
        }

        private void AddToken(TokenType Type)
        {
            AddToken(Type, null);
        }

        private void AddToken(TokenType Type, object Literal)
        {
            String Text = Source.Substring(Start, Current-Start);
            Tokens.Add(new Token(Type, Text, Literal, Line));
        }
    }
}
