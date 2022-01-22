using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasiliskLang.Interpreter.Abstracts;

namespace BasiliskLang.Interpreter
{
    public class StandardLibrary : Library
    {
        public StandardLibrary() : base() { }

        public override void GenerateFunctions()
        {
            throw new NotImplementedException();
        }
    }
}
