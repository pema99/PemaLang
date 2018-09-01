using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Interpreter
{
    public class Runtime
    {
        public Dictionary<string, object> Globals { get; private set; }
        public Stack<Scope> Stack { get; private set; }
        public object Register { get; set; }
        public bool Returning { get; set; }

        public Runtime()
        {
            this.Returning = false;
            this.Stack = new Stack<Scope>();
            this.Globals = new Dictionary<string, object>();

            //Expose stdlib
            foreach (MethodInfo Method in typeof(StdLib).GetMethods())
            {
                if (Method.DeclaringType == typeof(StdLib))
                {
                    ExposeMethod(Method);
                }
            }
            ExposeMethod(GetType().GetMethod("Require", BindingFlags.NonPublic | BindingFlags.Instance), "require", this);
        }

        public void Process(Dictionary<string, Expression> Globals)
        {
            Scope TempScope = new Scope(new ScopeInfo());
            foreach (var E in Globals)
            {
                if (this.Globals.ContainsKey(E.Key))
                {
                    throw new Exception("Global \'" + E.Key + "\' already declared.");
                }

                E.Value.Evaluate(this);
                this.Globals.Add(E.Key, Register);
            }
        }

        public void ExposeMethod(MethodInfo Method, string Identifier = "", object Target = null)
        {
            var Params = Method.GetParameters();
            List<string> Parameters = new List<string>();
            foreach (var Param in Params)
            {
                Parameters.Add(Param.Name);
            }
            ScopeInfo Scope = new ScopeInfo();
            FuncInfo Func = new FuncInfo(Identifier == "" ? Method.Name : Identifier, Scope, Parameters);
            Scope.Expressions.Add
            (
                new ReturnExpr
                (
                    new LambdaExpr(delegate
                    {
                        List<object> Values = new List<object>();
                        foreach (string Param in Parameters)
                        {
                            Values.Add(GetVariable(Param));
                        }
                        Register = Method.Invoke(Target, Values.ToArray());
                    })
                )
            );
            Globals[Identifier == "" ? Method.Name : Identifier] = Func;
        }

        public object Call(string FuncName, params object[] Parameters)
        {
            if (Globals[FuncName] is FuncInfo)
            {
                RunScopeConstant((Globals[FuncName] as FuncInfo).Body, Parameters);
                return Register;
            }
            else
            {
                throw new Exception("\'" + FuncName + "\' is not a function.");
            }
        }

        public object Call(FuncInfo Func, params object[] Parameters)
        {
            RunScopeConstant(Func.Body, Parameters);
            return Register;
        }

        private void Require(string Path)
        {
            Lexer Lexer = new Lexer();
            var Tokens = Lexer.ScanTokens(File.ReadAllText(Path));
            Parser Parser = new Parser();
            var ParseData = Parser.Parse(Tokens);

            Process(ParseData);
        }

        #region Stack Operations
        public void RunScope(ScopeInfo Info, List<Expression> Parameters = null)
        {
            Scope Scope = new Scope(Info);
            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Count(); i++)
                {
                    Parameters[i].Evaluate(this);
                    Scope.Variables[Scope.Info.OwnerFunc.Parameters[i]] = Register;
                }
            }
            Scope.Run(this);
        }

        //Run a scope with parameters that have already been evaluated (constants)
        public void RunScopeConstant(ScopeInfo Info, object[] Parameters)
        {
            Scope Scope = new Scope(Info);
            if (Parameters != null)
            {
                if (Parameters.Length == Scope.Info.OwnerFunc.Parameters.Count)
                {
                    for (int i = 0; i < Parameters.Count(); i++)
                    {
                        Scope.Variables[Scope.Info.OwnerFunc.Parameters[i]] = Parameters[i];
                    }
                }
                else
                {
                    throw new Exception("Invalid amount of parameters, expected " + Scope.Info.OwnerFunc.Parameters.Count + ", found " + Parameters.Length + ".");
                }
            }
            Scope.Run(this);
        }

        public void SetVariable(string Identifier, object Value)
        {
            if (Globals.ContainsKey(Identifier))
            {
                Globals[Identifier] = Value;
                return;
            }
            for (int i = 0; i < Stack.Count; i++)
            {
                Scope Scope = Stack.ElementAt(i);
                if (Scope.Variables.ContainsKey(Identifier))
                {
                    Scope.Variables[Identifier] = Value;
                    return;
                }
                if (Scope.IsFunction)
                {
                    break;
                }
            }
            throw new Exception("Runtime error. Variable " + Identifier + " has not been declared.");
        }

        public object GetVariable(string Identifier)
        {
            if (Globals.ContainsKey(Identifier))
            {
                return Globals[Identifier];
            }
            for (int i = 0; i < Stack.Count; i++)
            {
                Scope Scope = Stack.ElementAt(i);
                if (Scope.Variables.ContainsKey(Identifier))
                {
                    return Scope.Variables[Identifier];
                }
                if (Scope.IsFunction)
                {
                    break;
                }
            }
            throw new Exception("Runtime error. Variable " + Identifier + " has not been declared.");
        }

        public bool VariableExists(string Identifier)
        {
            if (Globals.ContainsKey(Identifier))
            {
                return true;
            }
            for (int i = 0; i < Stack.Count; i++)
            {
                Scope Scope = Stack.ElementAt(i);
                if (Scope.Variables.ContainsKey(Identifier))
                {
                    return true;
                }
                if (Scope.IsFunction)
                {
                    break;
                }
            }
            return false;
        }

        public void AddVariable(string Identifier, object Value)
        {
            if (Globals.ContainsKey(Identifier))
            {
                throw new Exception("Runtime error. Variable " + Identifier + " has already been declared.");
            }
            for (int i = 0; i < Stack.Count; i++)
            {
                Scope Scope = Stack.ElementAt(i);
                if (Scope.Variables.ContainsKey(Identifier))
                {
                    throw new Exception("Runtime error. Variable " + Identifier + " has already been declared.");
                }
                if (Scope.IsFunction)
                {
                    break;
                }
            }
            Stack.Peek().Variables.Add(Identifier, Value);
        }
        #endregion
    }
}
