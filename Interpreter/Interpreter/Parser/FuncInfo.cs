using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class FuncInfo
    {
        public string Identifier { get; set; }
        public ScopeInfo Body { get; set; }
        public List<string> Parameters { get; set; }

        public FuncInfo(string Identifier, ScopeInfo Body, List<string> Parameters)
        {
            this.Identifier = Identifier;
            this.Body = Body;
            this.Body.OwnerFunc = this;
            this.Parameters = Parameters;
        }
    }
}