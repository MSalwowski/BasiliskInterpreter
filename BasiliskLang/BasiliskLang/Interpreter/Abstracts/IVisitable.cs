using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskLang.Interpreter
{
    public interface IVisitable
    {
        void Accept(IVisitor executor);
    }
}
