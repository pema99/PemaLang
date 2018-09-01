using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Scope
    {
        public Dictionary<string, object> Variables { get; set; }
        public ScopeInfo Info { get; set; }
        public bool IsFunction { get { return Info.OwnerFunc != null; } }

        public Scope(ScopeInfo Info)
        {
            this.Variables = new Dictionary<string, object>();
            this.Info = Info;
        }

        public void Run(Runtime Runtime)
        {
            Runtime.Stack.Push(this);
            foreach (Expression E in Info.Expressions)
            {
                if (Runtime.Returning)
                {
                    return;
                }
                E.Evaluate(Runtime);
            }
            Runtime.Stack.Pop();
        }
    }
}
