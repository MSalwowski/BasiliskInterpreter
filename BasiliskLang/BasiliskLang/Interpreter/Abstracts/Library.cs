using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter.Abstracts
{
    public abstract class Library
    {
        Dictionary<(string, int), Identifier> functions;
        public Library() { functions = new Dictionary<(string, int), Identifier>(); }
        public abstract void GenerateFunctions();
    }
}
