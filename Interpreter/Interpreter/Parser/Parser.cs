using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Parser
    {
        private int Start { get; set; }
        private int Current { get; set; }
        public List<Token> Tokens { get; private set; }
        public Dictionary<string, Expression> Globals { get; private set; }

        public Parser()
        {
        }

        public Dictionary<string, Expression> Parse(List<Token> Tokens)
        {
            this.Tokens = Tokens;
            this.Start = 0;
            this.Current = 0;
            this.Globals = new Dictionary<string, Expression>();

            while (!IsAtEnd())
            {
                if (Match(TokenType.Func))
                {
                    FuncDecl();
                }
                else if (Match(TokenType.Var))
                {
                    GlobalDecl();
                }
                else
                {
                    throw new Exception("Invalid token \'" + Peek().Lexeme + "\' at line " + Peek().Line + ", only variables and functions are allowed in global scope.");
                }
            }
            return Globals;
        }

        private Expression Statement()
        {
            switch (Peek().Type)
            {
                case TokenType.Var:
                    return VarDecl();
                    break;

                case TokenType.Print:
                case TokenType.PrintLine:
                    return Print();
                    break;

                case TokenType.Identifier:
                    return Identifier();
                    break;

                case TokenType.If:
                    return Conditional();
                    break;

                case TokenType.While:
                    return While();
                    break;

                case TokenType.Left_Brace:
                    return new ScopeExpr(Scope());
                    break;

                case TokenType.Clear:
                    return ClearConsole();
                    break;

                case TokenType.Return:
                    return Return();
                    break;

                case TokenType.For:
                    return For();
                    break;

                default:
                    throw new Exception("Unexpected token: \'" + Peek().Type + "\' at line " + Peek().Line + ".");
                    break;
            }
        }

        private ScopeInfo Scope()
        {
            ScopeInfo Result = new ScopeInfo();

            Advance(TokenType.Left_Brace);
            while (!IsAtEnd() && !Match(TokenType.Right_Brace))
            {
                Result.Expressions.Add(Statement());
            }
            Advance(TokenType.Right_Brace);

            return Result;
        }

        #region Statement parsing
        private void FuncDecl()
        {
            FuncInfo Func = FuncParse(false);
            
            if (Globals.ContainsKey(Func.Identifier))
            {
                throw new Exception("Variable \'" + Func.Identifier + "\' already declared");
            }

            Globals.Add(Func.Identifier, new ConstantExpr(Func));
        }

        private void GlobalDecl()
        {
            Advance(TokenType.Var);
            Token Identifier = Advance(TokenType.Identifier);
            Advance(TokenType.Equal);

            if (Globals.ContainsKey(Identifier.Lexeme))
            {
                throw new Exception("Variable \'" + Identifier.Lexeme + "\' already declared");
            }

            Globals.Add(Identifier.Lexeme, Expression());
        }

        private FuncInfo FuncParse(bool Anonymous)
        {
            Advance(TokenType.Func);
            Token Identifier = null;
            if (!Anonymous)
            {
                Identifier = Advance(TokenType.Identifier);
            }

            Advance(TokenType.Left_Paren);
            List<string> Parameters = new List<string>();
            if (Match(TokenType.Identifier))
            {
                while (!Match(TokenType.Right_Paren))
                {
                    Token Token = Advance();
                    if (Token.Type == TokenType.Identifier)
                    {
                        if (Parameters.Contains(Token.Lexeme))
                        {
                            throw new Exception("Error at line " + Token.Line + ". Two parameters with the same name are not allowed in a function declaration.");
                        }
                        Parameters.Add(Token.Lexeme);
                    }
                    else if (Token.Type == TokenType.Comma) ;
                    else
                    {
                        throw new Exception("Error at line " + Token.Line + ". Only identifiers are allowed in function declarations.");
                    }
                }
            }
            Advance(TokenType.Right_Paren);

            return new FuncInfo(Identifier?.Lexeme ?? "", Scope(), Parameters);
        }

        private VarDeclExpr VarDecl()
        {
            Advance(TokenType.Var);
            Token Identifier = Advance(TokenType.Identifier);
            Advance(TokenType.Equal);
            Expression Intializer = Expression();

            return new VarDeclExpr(Identifier.Lexeme, Intializer);
        }

        private FuncCallExpr FuncCall()
        {
            Token Identifier = Advance(TokenType.Identifier);

            Advance(TokenType.Left_Paren);
            List<Expression> Params = new List<Expression>();
            while (!Match(TokenType.Right_Paren))
            {
                Params.Add(Expression());
                if (Match(TokenType.Comma))
                {
                    Advance(TokenType.Comma);
                }
            }
            Advance(TokenType.Right_Paren);

            return new FuncCallExpr(new VarExpr(Identifier.Lexeme), Params);
        }

        private Expression Identifier()
        {
            Token Identifier = null;

            switch (PeekNext().Type)
            {
                case TokenType.Plus_Equal:
                case TokenType.Plus_Plus:
                case TokenType.Minus_Equal:
                case TokenType.Minus_Minus:
                case TokenType.Star_Equal:
                case TokenType.Slash_Equal:
                    Identifier = Advance(TokenType.Identifier);
                    Token Operator = Advance();

                    Expression Delta = null;
                    switch (Operator.Type)
                    {
                        case TokenType.Plus_Equal:
                            Operator.Type = TokenType.Plus;
                            Delta = Expression();
                            break;

                        case TokenType.Plus_Plus:
                            Operator.Type = TokenType.Plus;
                            Delta = new ConstantExpr(1.0);
                            break;

                        case TokenType.Minus_Equal:
                            Operator.Type = TokenType.Minus;
                            Delta = Expression();
                            break;

                        case TokenType.Minus_Minus:
                            Operator.Type = TokenType.Minus;
                            Delta = new ConstantExpr(1.0);
                            break;

                        case TokenType.Slash_Equal:
                            Operator.Type = TokenType.Slash;
                            Delta = Expression();
                            break;

                        case TokenType.Star_Equal:
                            Operator.Type = TokenType.Star;
                            Delta = Expression();
                            break;

                        default:
                            throw new Exception("Unexpected operator " + Operator.Type + " at line " + Operator.Line);
                            break;
                    }
                    return
                        new VarAssignExpr
                        (
                            Identifier.Lexeme,
                            new BinaryExpr
                            (
                                new VarExpr(Identifier.Lexeme),
                                Operator,
                                Delta
                            )
                        );
                    break;

                case TokenType.Equal:
                    Identifier = Advance(TokenType.Identifier);
                    Advance(TokenType.Equal);
                    return new VarAssignExpr(Identifier.Lexeme, Expression());
                    break;

                case TokenType.Left_Square:
                    Identifier = Advance(TokenType.Identifier);
                    Advance(TokenType.Left_Square);
                    Expression Index = Expression();
                    Advance(TokenType.Right_Square);
                    if (Match(TokenType.Left_Paren))
                    {
                        Advance(TokenType.Left_Paren);
                        List<Expression> Params = new List<Expression>();
                        while (!Match(TokenType.Right_Paren))
                        {
                            Params.Add(Expression());
                            if (Match(TokenType.Comma))
                            {
                                Advance(TokenType.Comma);
                            }
                        }
                        Advance(TokenType.Right_Paren);
                        return new FuncCallExpr(new ArrayGetExpr(Identifier.Lexeme, Index), Params);
                    }
                    Advance(TokenType.Equal);
                    Expression Value = Expression();
                    return new ArraySetExpr(Identifier.Lexeme, Index, Value);
                    break;

                case TokenType.Left_Paren:
                    return FuncCall();
                    break;

                default:
                    throw new Exception("Invalid token \'" + Tokens[Current + 1].Type + "\' after Identifier on line " + Identifier.Line);
                    break;
            }
        }

        private PrintExpr Print()
        {
            Token Token = Advance();
            return new PrintExpr(Expression(), Token.Type == TokenType.PrintLine);
        }

        private ReturnExpr Return()
        {
            Advance(TokenType.Return);
            return new ReturnExpr(Expression());
        }

        private ClearConsoleExpr ClearConsole()
        {
            Advance(TokenType.Clear);
            return new ClearConsoleExpr();
        }

        private WhileExpr While()
        {
            Advance(TokenType.While);
            Advance(TokenType.Left_Paren);

            Expression Condition = Expression();

            Advance(TokenType.Right_Paren);

            ScopeInfo Body = Scope();

            return new WhileExpr(Condition, Body);
        }

        private ForExpr For()
        {
            Advance(TokenType.For);
            Advance(TokenType.Left_Paren);

            Expression Initial = Statement();

            Advance(TokenType.Comma);

            Expression Condition = Expression();

            Advance(TokenType.Comma);

            Expression Iteration = Statement();

            Advance(TokenType.Right_Paren);

            ScopeInfo Body = Scope();

            return new ForExpr(Initial, Condition, Iteration, Body);
        }

        private ConditionalExpr Conditional()
        {
            Advance(TokenType.If);
            Advance(TokenType.Left_Paren);

            Expression Condition = Expression();

            Advance(TokenType.Right_Paren);

            ScopeInfo Body = Scope();

            Expression Alternate = null;
            if (Match(TokenType.Else))
            {
                Advance(TokenType.Else);

                //Else if
                if (Match(TokenType.If))
                {
                    Alternate = Conditional();
                }
                //Else
                else
                {
                    Alternate = new ScopeExpr(Scope());
                }
            }

            return new ConditionalExpr(Condition, Body, Alternate);
        }
        #endregion

        #region Expression parsing
        private Expression Expression()
        {
            Expression Higher = Term();

            //+ | - | == | != | <= | >= | < | > | && | || | ^^
            while (Match(TokenType.Plus) || Match(TokenType.Minus) || Match(TokenType.Equal_Equal) || Match(TokenType.Bang_Equal)
               || Match(TokenType.Less) || Match(TokenType.Less_Equal) || Match(TokenType.Greater) || Match(TokenType.Greater_Equal)
               || Match(TokenType.And) || Match(TokenType.Or) || Match(TokenType.Xor))
            {
                Token Token = Advance();
                Higher = new BinaryExpr(Higher, Token, Term());
                if (IsAtEnd()) return Higher;
            }

            return Higher;
        }

        private Expression Term()
        {
            Expression Higher = Factor();

            //* | /
            while (Match(TokenType.Star) || Match(TokenType.Slash))
            {
                Token Token = Advance();
                Higher = new BinaryExpr(Higher, Token, Factor());
                if (IsAtEnd()) return Higher;
            }

            return Higher;
        }

        private Expression Factor()
        {
            switch (Peek().Type)
            {
                case TokenType.Number:
                    return new ConstantExpr((double)Advance().Literal);
                    break;

                case TokenType.String:
                    return new ConstantExpr((string)Advance().Literal);
                    break;

                case TokenType.Left_Square:
                    Advance(TokenType.Left_Square);
                    if (Match(TokenType.Right_Square))
                    {
                        return new ArrayExpr(new List<Expression>());
                    }
                    List<Expression> Initializer = new List<Expression>();
                    while (!Match(TokenType.Right_Square))
                    {
                        Initializer.Add(Expression());
                        if (Match(TokenType.Comma))
                        {
                            Advance(TokenType.Comma);
                        }
                    }
                    Advance(TokenType.Right_Square);
                    return new ArrayExpr(Initializer);
                    break;

                case TokenType.True:
                case TokenType.False:
                    return new ConstantExpr(Advance().Type == TokenType.True ? true : false);
                    break;

                case TokenType.Null:
                    Advance(TokenType.Null);
                    return new ConstantExpr(null);
                    break;

                case TokenType.Left_Paren:
                    Advance(TokenType.Left_Paren);
                    Expression Higher = Expression();
                    Advance(TokenType.Right_Paren);
                    return Higher;
                    break;

                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Bang:
                    return new UnaryExpr(Advance(), Factor());
                    break;

                case TokenType.Identifier:
                    if (PeekNext().Type == TokenType.Left_Paren)
                    {
                        return FuncCall();
                    }
                    else if (PeekNext().Type == TokenType.Left_Square)
                    {
                        Token Identifier = Advance(TokenType.Identifier);
                        Advance(TokenType.Left_Square);
                        Expression Index = Expression();
                        Advance(TokenType.Right_Square);

                        if (Match(TokenType.Left_Paren))
                        {
                            Advance(TokenType.Left_Paren);
                            List<Expression> Params = new List<Expression>();
                            while (!Match(TokenType.Right_Paren))
                            {
                                Params.Add(Expression());
                                if (Match(TokenType.Comma))
                                {
                                    Advance(TokenType.Comma);
                                }
                            }
                            Advance(TokenType.Right_Paren);
                            return new FuncCallExpr(new ArrayGetExpr(Identifier.Lexeme, Index), Params);
                        }
                        else
                        {
                            return new ArrayGetExpr(Identifier.Lexeme, Index);
                        }
                    }
                    else
                    {
                        return new VarExpr((string)Advance().Lexeme);
                    }
                    break;

                case TokenType.Func:
                    return new ConstantExpr(FuncParse(true));
                    break;

                case TokenType.Line:
                case TokenType.Key:
                    return new InputExpr(Advance());
                    break;

                default:
                    throw new Exception("Unexpected token: \'" + Peek().Type + "\' at line " + Peek().Line + ".");
                    break;
            }
        }
        #endregion

        #region Parser operations
        private Token Peek()
        {
            return Tokens[Current];
        }

        private Token PeekNext()
        {
            return Tokens[Current + 1];
        }

        private bool Match(TokenType Type)
        {
            return Type == Peek().Type;
        }

        private Token Advance(TokenType Type)
        {
            Current++;
            if (Tokens[Current - 1].Type == Type)
            {
                return Tokens[Current - 1];
            }
            else
            {
                throw new Exception("Unexpected token: \'" + Tokens[Current - 1].Type + "\' at line " + Tokens[Current - 1].Line + ".");
            }
        }

        private Token Advance()
        {
            Current++;
            return Tokens[Current - 1];
        }

        private bool IsAtEnd()
        {
            return Current >= Tokens.Count || Peek().Type == TokenType.EOF;
        }
        #endregion
    }
}
